#region Using

using System;
using System.Collections.Generic;
using Common._2D;
using Common.Interfaces;
using Common.Time;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace SoccerXNA
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class SupportSpotCalculator : DrawableGameComponent
    {
        protected static PrimitiveBatch primitiveBatch;

        private readonly Regulator m_pRegulator;
        //a data structure to hold the values and positions of each spot

        private readonly AbstractSoccerTeam m_pTeam;

        private readonly List<SupportSpot> m_Spots;
        private readonly ParamLoader Prm = ParamLoader.Instance;

        //a pointer to the highest valued spot from the last update
        private SupportSpot m_pBestSupportingSpot;

        //this will regulate how often the spots are calculated (default is
        //one update per second)


        public SupportSpotCalculator(Game game, int numX,
                                     int numY,
                                     AbstractSoccerTeam team) : base(game)
        {
            m_Spots = new List<SupportSpot>();
            m_pBestSupportingSpot = null;
            m_pTeam = team;
            Region PlayingField = team.Pitch().PlayingArea();

            //calculate the positions of each sweet spot, create them and 
            //store them in m_Spots
            float HeightOfSSRegion = PlayingField.Height()*0.8f;
            float WidthOfSSRegion = PlayingField.Width()*0.9f;
            float SliceX = WidthOfSSRegion/numX;
            float SliceY = HeightOfSSRegion/numY;

            float left = PlayingField.Left() + (PlayingField.Width() - WidthOfSSRegion)/2.0f + SliceX/2.0f;
            float right = PlayingField.Right() - (PlayingField.Width() - WidthOfSSRegion)/2.0f - SliceX/2.0f;
            float top = PlayingField.Top() + (PlayingField.Height() - HeightOfSSRegion)/2.0f + SliceY/2.0f;

            for (int x = 0; x < (numX/2) - 1; ++x)
            {
                for (int y = 0; y < numY; ++y)
                {
                    if (m_pTeam.Color() == AbstractSoccerTeam.team_color.blue)
                    {
                        m_Spots.Add(new SupportSpot(new Vector2(left + x*SliceX, top + y*SliceY), 0.0f));
                    }

                    else
                    {
                        m_Spots.Add(new SupportSpot(new Vector2(right - x*SliceX, top + y*SliceY), 0.0f));
                    }
                }
            }

            //create the regulator
            m_pRegulator = new Regulator(Prm.SupportSpotUpdateFreq);
        }

        protected PrimitiveBatch PrimitiveBatch
        {
            get
            {
                if (primitiveBatch == null)
                {
                    IDrawingManager manager = (IDrawingManager) Game.Services.GetService(typeof (IDrawingManager));
                    primitiveBatch = manager.GetPrimitiveBatch();
                }
                return primitiveBatch;
            }
        }

        ~SupportSpotCalculator()
        {
            m_pRegulator.Dispose();
            Dispose(false);
        }

        //draws the spots to the screen as a hollow circles. The higher the 
        //score, the bigger the circle. The best supporting spot is drawn in
        //bright green.
        public override void Draw(GameTime gameTime)
        {
            for (int spt = 0; spt < m_Spots.Count; ++spt)
            {
                Drawing.DrawCircle(PrimitiveBatch, m_Spots[spt].m_vPos, m_Spots[spt].m_dScore, Color.Gray);
            }

            if (m_pBestSupportingSpot != null)
            {
                Drawing.DrawCircle(PrimitiveBatch, m_pBestSupportingSpot.m_vPos, m_pBestSupportingSpot.m_dScore,
                                   Color.Green);
            }
        }

        //this method iterates through each possible spot and calculates its
        //score.
        public Vector2 DetermineBestSupportingPosition()
        {
            if (m_pTeam.ControllingPlayer() == null)
            {
                return Vector2.Zero;
            }

            //only update the spots every few frames                              
            if (!m_pRegulator.isReady() && m_pBestSupportingSpot != null)
            {
                return m_pBestSupportingSpot.m_vPos;
            }

            //reset the best supporting spot
            m_pBestSupportingSpot = null;

            float BestScoreSoFar = 0.0f;

            foreach (SupportSpot curSpot in m_Spots)
            {
                //first remove any previous score. (the score is set to one so that
                //the viewer can see the positions of all the spots if he has the 
                //aids turned on)
                curSpot.m_dScore = 1.0f;

                //Test 1. is it possible to make a safe pass from the ball's position 
                //to this position?
                if (m_pTeam.isPassSafeFromAllOpponents(m_pTeam.ControllingPlayer().Pos(),
                                                       curSpot.m_vPos,
                                                       null,
                                                       Prm.MaxPassingForce))
                {
                    curSpot.m_dScore += Prm.Spot_PassSafeScore;
                }


                //Test 2. Determine if a goal can be scored from this position.  
                if (m_pTeam.CanShoot(curSpot.m_vPos,
                                     Prm.MaxShootingForce))
                {
                    curSpot.m_dScore += Prm.Spot_CanScoreFromPositionScore;
                }


                //Test 3. calculate how far this spot is away from the controlling
                //player. The further away, the higher the score. Any distances further
                //away than OptimalDistance pixels do not receive a score.
                if (m_pTeam.SupportingPlayer() != null)
                {
                    const float OptimalDistance = 200.0f;

                    float dist = Vector2.Distance(m_pTeam.ControllingPlayer().Pos(),
                                                  curSpot.m_vPos);

                    float temp = Math.Abs(OptimalDistance - dist);

                    if (temp < OptimalDistance)
                    {
                        //normalize the distance and add it to the score
                        curSpot.m_dScore += Prm.Spot_DistFromControllingPlayerScore*
                                            (OptimalDistance - temp)/OptimalDistance;
                    }
                }

                //check to see if this spot has the highest score so far
                if (curSpot.m_dScore > BestScoreSoFar)
                {
                    BestScoreSoFar = curSpot.m_dScore;

                    m_pBestSupportingSpot = curSpot;
                }
            }

            if (m_pBestSupportingSpot != null)
            {
                return m_pBestSupportingSpot.m_vPos;
            }
            return new Vector2();
        }


        //returns the best supporting spot if there is one. If one hasn't been
        //calculated yet, this method calls DetermineBestSupportingPosition and
        //returns the result.
        public Vector2 GetBestSupportingSpot()
        {
            if (m_pBestSupportingSpot != null)
            {
                return m_pBestSupportingSpot.m_vPos;
            }


            return DetermineBestSupportingPosition();
        }

        #region Nested type: SupportSpot

        private class SupportSpot
        {
            internal readonly Vector2 m_vPos;

            internal float m_dScore;

            internal SupportSpot(Vector2 pos, float value)
            {
                m_vPos = pos;
                m_dScore = value;
            }
        }

        #endregion
    }
}