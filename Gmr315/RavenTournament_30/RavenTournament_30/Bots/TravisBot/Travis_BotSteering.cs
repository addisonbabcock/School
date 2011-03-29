﻿using System;
using System.Collections.Generic;
using System.Linq;
using Common._2D;
using Common.Misc;
using Microsoft.Xna.Framework;
using Raven.lua;

namespace Raven.Bots.TravisBot
{
    public class Travis_BotSteering : AbstractSteering
    {


        public Travis_BotSteering(Raven_Game world, AbstractBot agent)
            : base(world, agent)
        {
            Travis_BotScriptor myScript = Travis_BotScriptor.Instance();
            m_dWeightSeparation = myScript.GetDouble("SeparationWeight");
            m_dWeightWander = myScript.GetDouble("WanderWeight");
            m_dWeightWallAvoidance = myScript.GetDouble("WallAvoidanceWeight");
            m_dViewDistance = myScript.GetDouble("ViewDistance");
            m_dWallDetectionFeelerLength = myScript.GetDouble("WallDetectionFeelerLength");
            m_Deceleration = Deceleration.normal;
            m_dWanderDistance = myScript.GetDouble("WanderDist");
            m_dWanderJitter = myScript.GetDouble("WanderJitterPerSec");
            m_dWanderRadius = myScript.GetDouble("WanderRad");
            m_dWeightSeek = myScript.GetDouble("SeekWeight");
            m_dWeightArrive = myScript.GetDouble("ArriveWeight");
            m_bCellSpaceOn = false;
            m_SummingMethod = summing_method.prioritized;

            double theta = Utils.RandFloat() * MathHelper.TwoPi;

            m_vWanderTarget = new Vector2(m_dWanderRadius * (float)Math.Cos(theta), m_dWanderRadius * (float) Math.Sin(theta));
        }
    }
}