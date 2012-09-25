using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Common._2D;
using Common.Misc;
using Common.Triggers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Raven.lua;

namespace Raven.triggers
{
    public class Trigger_WeaponGiver : Trigger_Respawning<AbstractBot>
    {
        private static Raven_Scriptor script = Raven_Scriptor.Instance();
        public override void Dispose()
        {
            if (m_vecRLVB != null)
            {
                m_vecRLVB.Clear();
            }
            m_vecRLVB = null;
            if (m_vecRLVBTrans != null)
            {
                m_vecRLVBTrans.Clear();
            }
            m_vecRLVBTrans = null;
        }

        //vrtex buffers for rocket shape
        private static List<Vector2> m_vecRLVB;
        private List<Vector2> m_vecRLVBTrans;



        //this type of trigger is created when reading a map file
        public Trigger_WeaponGiver(StreamReader datafile)
            : base(StreamUtilities.GetIntFromStream(datafile))
        {
            Read(datafile);
            InitializeVertices();
        }

        //this type of trigger is created when reading a map file
        public Trigger_WeaponGiver(string[] tokens)
            : base(int.Parse(tokens[1]))
        {
            LoadData(tokens);
            InitializeVertices();
        }

        private void LoadData(string[] tokens)
        {
            float x, y, r;
            int GraphNodeIndex;

            x = float.Parse(tokens[3]);
            y = float.Parse(tokens[4]);
            r = float.Parse(tokens[5]);
            GraphNodeIndex = int.Parse(tokens[7]);

            SetPos(new Vector2(x, y));
            SetBRadius(r);
            SetGraphNodeIndex(GraphNodeIndex);

            //create this trigger's region of fluence
            AddCircularTriggerRegion(Pos(), script.GetDouble("DefaultGiverTriggerRange"));


            SetRespawnDelay((int)(script.GetDouble("Weapon_RespawnDelay") * Constants.FrameRate));
        }

        private void InitializeVertices()
        {
            if (m_vecRLVB == null)
            {
                m_vecRLVB = new List<Vector2>();
                m_vecRLVB.Add(new Vector2(0, 3));
                m_vecRLVB.Add(new Vector2(1, 2));
                m_vecRLVB.Add(new Vector2(1, 0));
                m_vecRLVB.Add(new Vector2(2, -2));
                m_vecRLVB.Add(new Vector2(-2, -2));
                m_vecRLVB.Add(new Vector2(-1, 0));
                m_vecRLVB.Add(new Vector2(-1, 2));
                m_vecRLVB.Add(new Vector2(0, 3));
            }


        }


       
        //if triggered, this trigger will call the PickupWeapon method of the
        //bot. PickupWeapon will instantiate a weapon of the appropriate type.
        public override void Try(AbstractBot pBot)
        {
            if (isActive() && isTouchingTrigger(pBot.Pos(), pBot.BRadius()))
            {
                pBot.GetWeaponSys().AddWeapon(EntityType());

                Deactivate();
            }
        }

        //draws a symbol representing the weapon type at the trigger's location
        public override void Render(PrimitiveBatch batch)
        {

            if (isActive())
            {
              switch (EntityType())
              {
                case (int) Raven_Objects.type_rail_gun:
                  {
                      Drawing.DrawCircle(batch, Pos(), 3, Color.Blue);

                      Drawing.DrawLine(batch, Pos(), new Vector2(Pos().X, Pos().Y-9), Color.DarkBlue);
                  }

                  break;

                case (int) Raven_Objects.type_shotgun:
                  {
                      Drawing.DrawCircle(batch, new Vector2(Pos().X-3, Pos().Y), 3, Color.Brown);
                      Drawing.DrawCircle(batch, new Vector2(Pos().X+3, Pos().Y), 3, Color.Brown);
                  }

                  break;

                case (int) Raven_Objects.type_rocket_launcher:
                  {
                      Vector2 facing = new Vector2(-1,0);
                      Vector2 facingPerp = facing.Perp();
                      Vector2 vector2point5 = new Vector2(2.5f, 2.5f);
                     m_vecRLVBTrans = Transformations.WorldTransform(m_vecRLVB,
                                                    ref m_vPosition,
                                                    ref facing,
                                                    ref facingPerp,
                                                    ref vector2point5);

                      Drawing.DrawClosedShaped(batch, m_vecRLVBTrans, Color.Red);
                  }
      
                  break;

              }//end switch
            }
        }

        public override void Read(StreamReader reader)
        {
            string line = reader.ReadLine();
            string[] tokens = line.Split(' ');
            LoadData(tokens);

        }
    }
}
