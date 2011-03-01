#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using Common._2D;
using Common.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using SoccerXNA.Teams;

#endregion

namespace SoccerXNA
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class SoccerPitch : DrawableGameComponent
    {
        private const int NumRegionsHorizontal = 6;
        private const int NumRegionsVertical = 3;
        private static SpriteFont gameFont;
        private static PrimitiveBatch primitiveBatch;
        private static SpriteBatch spriteBatch;
        private readonly ParamLoader Prm = ParamLoader.Instance;

        //true if the game is in play. Set to false whenever the players
        //are getting ready for kickoff
        public bool m_bGameOn;
        public bool m_bGoalKeeperHasBall;

        //set true to pause the motion
        public bool m_bPaused;

        //local copy of client window dimensions
        public int m_cxClient,
                   m_cyClient;

        public SoccerBall m_pBall;
        public Goal m_pBlueGoal;
        public AbstractSoccerTeam m_pBlueTeam;
        public Region m_pPlayingArea;
        public Goal m_pRedGoal;
        public AbstractSoccerTeam m_pRedTeam;
        public Region[] m_Regions;
        public List<Wall2D> m_vecWalls;
        private Stopwatch m_gameTimer;
        private Stopwatch m_redTotalTime;
        private Stopwatch m_blueTotalTime;

        public SoccerPitch(Game game, string redId, string blueId)
            : base(game)
        {
            m_cxClient = game.GraphicsDevice.Viewport.Width;
            m_cyClient = game.GraphicsDevice.Viewport.Height; //define the playing area
            m_pPlayingArea = new Region(game, 20, 20, m_cxClient - 20, m_cyClient - 20);
            m_pPlayingArea.Initialize();
            m_bPaused = false;
            m_bGoalKeeperHasBall = false;
            //create the goals
            m_pRedGoal = new Goal(new Vector2(m_pPlayingArea.Left(), (m_cyClient - Prm.GoalWidth) / 2),
                                  new Vector2(m_pPlayingArea.Left(), m_cyClient - (m_cyClient - Prm.GoalWidth) / 2),
                                  new Vector2(1, 0));


            m_pBlueGoal = new Goal(new Vector2(m_pPlayingArea.Right(), (m_cyClient - Prm.GoalWidth) / 2),
                                   new Vector2(m_pPlayingArea.Right(), m_cyClient - (m_cyClient - Prm.GoalWidth) / 2),
                                   new Vector2(-1, 0));
            m_Regions = new Region[NumRegionsHorizontal * NumRegionsVertical];
            m_vecWalls = new List<Wall2D>();
            //create the walls
            Vector2 TopLeft = new Vector2(m_pPlayingArea.Left(), m_pPlayingArea.Top());
            Vector2 TopRight = new Vector2(m_pPlayingArea.Right(), m_pPlayingArea.Top());
            Vector2 BottomRight = new Vector2(m_pPlayingArea.Right(), m_pPlayingArea.Bottom());
            Vector2 BottomLeft = new Vector2(m_pPlayingArea.Left(), m_pPlayingArea.Bottom());
            m_vecWalls.Add(new Wall2D(game, BottomLeft, m_pRedGoal.RightPost()));
            m_vecWalls.Add(new Wall2D(game, m_pRedGoal.LeftPost(), TopLeft));
            m_vecWalls.Add(new Wall2D(game, TopLeft, TopRight));
            m_vecWalls.Add(new Wall2D(game, TopRight, m_pBlueGoal.LeftPost()));
            m_vecWalls.Add(new Wall2D(game, m_pBlueGoal.RightPost(), BottomRight));
            m_vecWalls.Add(new Wall2D(game, BottomRight, BottomLeft));
            //create the soccer ball
            m_pBall = new SoccerBall(game, new Vector2(m_cxClient / 2.0f, m_cyClient / 2.0f),
                                     Prm.BallSize,
                                     Prm.BallMass,
                                     m_vecWalls);


            //create the regions  
            CreateRegions(PlayingArea().Width() / NumRegionsHorizontal,
                          PlayingArea().Height() / NumRegionsVertical);


            //create the teams 
            m_pRedTeam = SoccerTeamFactory.Instance().CreateTeam(redId, game, m_pRedGoal, m_pBlueGoal, this,
                                               AbstractSoccerTeam.team_color.red);
            if (m_pRedTeam == null)
            {
                throw new NullReferenceException("Red Team could not be created properly");
            }
            //new SoccerTeam(game, m_pRedGoal, m_pBlueGoal, this, SoccerTeam.team_color.red);
            m_pBlueTeam = SoccerTeamFactory.Instance().CreateTeam(blueId, game, m_pBlueGoal, m_pRedGoal, this,
                                               AbstractSoccerTeam.team_color.blue);
            if (m_pBlueTeam == null)
            {
                throw new NullReferenceException("Blue Team could not be created properly");
            }

            //make sure each team knows who their opponents are
            m_pRedTeam.SetOpponents(m_pBlueTeam);
            m_pBlueTeam.SetOpponents(m_pRedTeam);

            m_bGameOn = true;
            m_gameTimer = new Stopwatch();
            m_redTotalTime = new Stopwatch();
            m_blueTotalTime = new Stopwatch();
            m_gameTimer.Start();
        }

        protected PrimitiveBatch PrimitiveBatch
        {
            get
            {
                if (primitiveBatch == null)
                {
                    IDrawingManager manager = (IDrawingManager)Game.Services.GetService(typeof(IDrawingManager));
                    primitiveBatch = manager.GetPrimitiveBatch();
                }
                return primitiveBatch;
            }
        }

        protected virtual SpriteFont GameFont
        {
            get
            {
                if (gameFont == null)
                {
                    IFontManager manager = (IFontManager)Game.Services.GetService(typeof(IFontManager));
                    gameFont = manager.GetFont("Default");
                }
                return gameFont;
            }
        }

        protected SpriteBatch SpriteBatch
        {
            get
            {
                if (spriteBatch == null)
                {
                    IDrawingManager manager = (IDrawingManager)Game.Services.GetService(typeof(IDrawingManager));
                    spriteBatch = manager.GetSpriteBatch();
                }
                return spriteBatch;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void CreateRegions(float width, float height)
        {
            //index into the vector
            int idx = NumRegionsHorizontal * NumRegionsVertical - 1;

            for (int col = 0; col < NumRegionsHorizontal; ++col)
            {
                for (int row = 0; row < NumRegionsVertical; ++row)
                {
                    m_Regions[idx] = new Region(Game, PlayingArea().Left() + col * width,
                                             PlayingArea().Top() + row * height,
                                             PlayingArea().Left() + (col + 1) * width,
                                             PlayingArea().Top() + (row + 1) * height,
                                             idx);
                    m_Regions[idx].Initialize();
                    idx--;
                }
            }
        }

        public void DestroySoccerPitch()
        {
            m_pBall.Dispose();

            m_pRedTeam.Dispose();
            m_pBlueTeam.Dispose();

            m_pRedGoal.Dispose();
            m_pBlueGoal.Dispose();

            m_pPlayingArea.Dispose();

            for (int i = 0; i < m_Regions.Length; ++i)
            {
                m_Regions[i].Dispose();
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                DestroySoccerPitch();
            }
        }

        ~SoccerPitch()
        {
            Dispose(false);
        }

        private KeyboardState previousState = Keyboard.GetState();
        private KeyboardState currentState = Keyboard.GetState();

        public override void Update(GameTime gameTime)
        {
            previousState = currentState;
            currentState = Keyboard.GetState();
            if (currentState.IsKeyUp(Keys.P) && previousState.IsKeyDown(Keys.P))
            {
                m_bPaused = !m_bPaused;
                if (Paused())
                {
                    m_gameTimer.Stop();
                }
                else
                {
                    m_gameTimer.Start();
                }
            }

            if (Paused()) return;

            //update the balls
            m_pBall.Update(gameTime);

            ////update the teams

            m_redTotalTime.Start();
            m_pRedTeam.Update(gameTime);
            m_redTotalTime.Stop();
            m_blueTotalTime.Start();
            m_pBlueTeam.Update(gameTime);
            m_blueTotalTime.Stop();

            //if a goal has been detected reset the pitch ready for kickoff
            if (m_pBlueGoal.Scored(m_pBall) || m_pRedGoal.Scored(m_pBall))
            {
                m_bGameOn = false;

                //reset the ball                                                      
                m_pBall.PlaceAtPosition(new Vector2(m_cxClient / 2.0f, m_cyClient / 2.0f));

                //get the teams ready for kickoff
                m_pRedTeam.PrepareForKickoff();
                m_pBlueTeam.PrepareForKickoff();
            }
            if(this.IsTimeUp(Prm.GameTime))
            {
                bDone = true;
                m_gameTimer.Stop();
                m_redTotalTime.Stop();
                m_blueTotalTime.Stop();
            }
        }

        private bool bDone = false;

        public override void Draw(GameTime gameTime)
        {
            //render regions
            if (Prm.bRegions)
            {
                for (int r = 0; r < m_Regions.Length; ++r)
                {
                    m_Regions[r].Draw(gameTime, true);
                }
            }
            Drawing.DrawRectangle(PrimitiveBatch, Color.Red, m_pPlayingArea.Left(), (m_cyClient - Prm.GoalWidth) / 2, 40,
                                  Prm.GoalWidth);
            Drawing.DrawRectangle(PrimitiveBatch, Color.Blue, m_pPlayingArea.Right() - 40,
                                  (m_cyClient - Prm.GoalWidth) / 2, 40,
                                  Prm.GoalWidth);

            //render the pitch markings
            Drawing.DrawCircle(PrimitiveBatch, m_pPlayingArea.Center(), m_pPlayingArea.Width() * 0.125f, Color.White);
            Drawing.DrawLine(PrimitiveBatch, new Vector2(m_pPlayingArea.Center().X, m_pPlayingArea.Top()),
                             new Vector2(m_pPlayingArea.Center().X, m_pPlayingArea.Bottom()), Color.White);

            m_pBall.Draw(gameTime);

            //Render the teams
            m_pRedTeam.Draw(gameTime);
            m_pBlueTeam.Draw(gameTime);

            //render the walls
            for (int w = 0; w < m_vecWalls.Count(); ++w)
            {
                m_vecWalls[w].Draw(gameTime, Color.White);
            }
            TimeSpan remaining = RemainingTime();

            SpriteBatch.DrawString(GameFont, string.Format("{0}:{1:00}", remaining.Minutes, remaining.Seconds) ,
                                       new Vector2((m_cxClient / 2) - 40, 2), Color.Yellow);
            if (bDone)
            {
                ShowAdjustedScore();
            }
            else
            {
                ////show the score
                SpriteBatch.DrawString(GameFont, m_pRedTeam.Name() + " (Red): " + m_pBlueGoal.NumGoalsScored(),
                                       new Vector2((m_cxClient/2) - 300, m_cyClient - 18), Color.Red);
                SpriteBatch.DrawString(GameFont, m_pBlueTeam.Name() + " (Blue): " + m_pRedGoal.NumGoalsScored(),
                                       new Vector2((m_cxClient/2) + 10, m_cyClient - 18), Color.Blue);
            }

        }

        private void ShowAdjustedScore()
        {
            int blueScore = m_pRedGoal.NumGoalsScored();
            int redScore = m_pBlueGoal.NumGoalsScored();
            long redTime = m_redTotalTime.ElapsedTicks;
            long blueTime = m_blueTotalTime.ElapsedTicks;
  double redAdjScore = redScore;
  double blueAdjScore = blueScore;
  if (redTime > blueTime)
  {
	if (redScore >= blueScore)
	{
		blueAdjScore = AdjustedScore(redTime, blueTime, blueScore);
	}
  } else if (blueTime > redTime)
  {
	if (blueScore >= redScore)
	{
        redAdjScore = AdjustedScore(blueTime, redTime, redScore);
	}
  }
            string redGoals = string.Format("{0} (Red): {1} Time: {2:mm.ss.ff} = {3:0.00} ", m_pRedTeam.Name(), redScore, new TimeSpan(redTime), redAdjScore);
            string blueGoals = string.Format("{0} (Blue): {1} Time: {2:mm.ss.ff} = {3:0.00} ", m_pBlueTeam.Name(), blueScore, new TimeSpan(blueTime), blueAdjScore);
            SpriteBatch.DrawString(GameFont, redGoals,
                                       new Vector2((m_cxClient / 2) - 300, m_cyClient - 18), Color.Red);
            SpriteBatch.DrawString(GameFont, blueGoals,
                                   new Vector2((m_cxClient / 2) + 10, m_cyClient - 18), Color.Blue);
        }
        public void TogglePause()
        {
            m_bPaused = !m_bPaused;
        }

        public bool Paused()
        {
            return m_bPaused || bDone;
        }

        public int cxClient()
        {
            return m_cxClient;
        }

        public int cyClient()
        {
            return m_cyClient;
        }

        public bool GoalKeeperHasBall()
        {
            return m_bGoalKeeperHasBall;
        }

        public void SetGoalKeeperHasBall(bool b)
        {
            m_bGoalKeeperHasBall = b;
        }

        public Region PlayingArea()
        {
            return m_pPlayingArea;
        }

        public List<Wall2D> Walls()
        {
            return m_vecWalls;
        }

        public SoccerBall Ball()
        {
            return m_pBall;
        }

        public Region GetRegionFromIndex(int idx)
        {
            if (idx < 0 || idx > m_Regions.Length)
            {
                throw new Exception("Original code assertion!");
            }
            return m_Regions[idx];
        }

        public bool GameOn()
        {
            return m_bGameOn;
        }

        public void SetGameOn()
        {
            m_bGameOn = true;
        }

        public TimeSpan RemainingTime()
        {
            /// Every timer check stops the timer
            /// So we will start and stop it ourselves everytime to know that
            /// it's been reest
            
            m_gameTimer.Stop();
            TimeSpan elapsed = m_gameTimer.Elapsed;
            TimeSpan gameTime = new TimeSpan(0,Prm.GameTime, 0);
            TimeSpan remaining = gameTime - elapsed;

            if(!Paused())
            {
                m_gameTimer.Start();
            }
            
            return remaining;
        }
        public bool IsTimeUp(int minutes)
        {
            /// Every timer check stops the timer
            /// So we will start and stop it ourselves everytime to know that
            /// it's been reest
            m_gameTimer.Stop();
            bool timeUp = false;
            if (m_gameTimer.Elapsed > new TimeSpan(0, 0, minutes, 0, 0))
            {
              timeUp = true;
            }
            if (!Paused())
            {
                m_gameTimer.Start();
            } 
            return timeUp;

        }

        public void SetRedTeamReady()
        {
            m_redReady = true;
            if (m_redReady && m_blueReady)
                SetGameOn();
        }

        public void SetBlueTeamReady()
        {
            m_blueReady = true;
            if (m_redReady && m_blueReady)
                SetGameOn();
        }

        public void SetGameOff()
        {
            m_bGameOn = false;
            m_redReady = false;
            m_blueReady = false;
        }

        private bool m_redReady;
        private bool m_blueReady;

        private double AdjustedScore(long winnersTime, long losersTime, int losersScore)
        {
            double diff = winnersTime - losersTime;
            double fraction = (double)diff / (double)winnersTime;

            return (fraction * 5) + losersScore;
        }

        public bool OneTeamReady()
        {
            return m_redReady || m_blueReady;
        }
    }
}