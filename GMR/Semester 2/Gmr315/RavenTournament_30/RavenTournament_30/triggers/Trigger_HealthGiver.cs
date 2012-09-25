using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Common._2D;
using Common.Game;
using Common.Misc;
using Common.Triggers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Raven.lua;

namespace Raven.triggers
{
    public class Trigger_HealthGiver : Trigger_Respawning<AbstractBot>
    {
        private static Raven_Scriptor script = Raven_Scriptor.Instance();

        public override void Dispose()
        {
            
        }

        public override void Try(AbstractBot entity)
        {
            if (isActive() && isTouchingTrigger(entity.Pos(), entity.BRadius()))
            {
                entity.IncreaseHealth(m_iHealthGiven);

                Deactivate();
            }
        }

        //the amount of health an entity receives when it runs over this trigger
  private int   m_iHealthGiven;
  


  public Trigger_HealthGiver(StreamReader datafile) : base(StreamUtilities.GetIntFromStream(datafile))
  {
      Read(datafile);
  }
        public Trigger_HealthGiver(string[] tokens) : base(int.Parse(tokens[1]))
        {
            LoadData(tokens);
        }

        private void LoadData(string[] tokens)
        {
  
            float x = float.Parse(tokens[3]);
            float y = float.Parse(tokens[4]);
            float r = float.Parse(tokens[5]);
            m_iHealthGiven = int.Parse(tokens[6]);
            int GraphNodeIndex = int.Parse(tokens[7]);

 
  SetPos(new Vector2(x,y)); 
  SetBRadius(r);
  SetGraphNodeIndex(GraphNodeIndex);

  //create this trigger's region of fluence
  AddCircularTriggerRegion(Pos(), script.GetDouble("DefaultGiverTriggerRange"));

  SetRespawnDelay((int)(script.GetDouble("Health_RespawnDelay") * Constants.FrameRate));
  SetEntityType((int) Raven_Objects.type_health);
        }

  
  //draws a box with a red cross at the trigger's location
        public override void Render(PrimitiveBatch batch)
        {
            if (isActive())
            {
                const int sz = 5;
                Drawing.DrawRectangle(batch, Color.Black, Pos().X - sz, Pos().Y-sz, sz*2+1, sz*2+1);
                Drawing.DrawLine(batch, new Vector2(Pos().X, Pos().Y - sz), new Vector2(Pos().X, Pos().Y + sz + 1), Color.Red);
                Drawing.DrawLine(batch, new Vector2(Pos().X - sz, Pos().Y), new Vector2(Pos().X + sz + 1, Pos().Y), Color.Red);
            }
        }

  public override void Read(StreamReader stream){
      string line = stream.ReadLine();
      string[] tokens = line.Split(' ');
  LoadData(tokens);
}
    }
}
