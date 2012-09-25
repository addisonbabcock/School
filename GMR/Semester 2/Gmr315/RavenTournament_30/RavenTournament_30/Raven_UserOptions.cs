using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Raven
{
    public class Raven_UserOptions
    {
        private Raven_UserOptions()
        {
            m_bShowGraph = false;
            m_bShowPathOfSelectedBot = false;
            m_bSmoothPathsQuick = false;
            m_bSmoothPathsPrecise = false;
            m_bShowBotIDs = false;
            m_bShowBotHealth = false;
            m_bShowTargetOfSelectedBot = false;
            m_bOnlyShowBotsInTargetsFOV = false;
            m_bShowScore = false;
            m_bShowGoalsOfSelectedBot = false;
            m_bShowGoalAppraisals = false;
            m_bShowNodeIndices = false;
            m_bShowOpponentsSensedBySelectedBot = false;
            m_bShowWeaponAppraisals = false;
        }

        private static Raven_UserOptions _instance;

        public static Raven_UserOptions Instance()
        {
            if(_instance == null)
            {
                _instance = new Raven_UserOptions();
            }
            return _instance;
        }

        public bool m_bShowGraph;

        public bool m_bShowNodeIndices;

        public bool m_bShowPathOfSelectedBot;

        public bool m_bShowTargetOfSelectedBot;

        public bool m_bShowOpponentsSensedBySelectedBot;

        public bool m_bOnlyShowBotsInTargetsFOV;

        public bool m_bShowGoalsOfSelectedBot;

        public bool m_bShowGoalAppraisals;

        public bool m_bShowWeaponAppraisals;

        public bool m_bSmoothPathsQuick;
        public bool m_bSmoothPathsPrecise;

        public bool m_bShowBotIDs;

        public bool m_bShowBotHealth;

        public bool m_bShowScore;
    }
}
