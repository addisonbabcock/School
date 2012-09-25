#region Using

using Common.Misc;

#endregion

namespace SoccerXNA
{
    public class ParamLoader : IniFileLoaderBase
    {
        private static ParamLoader _paramLoader;

        public float BallMass;
        public float BallSize;
        public float BallWithinReceivingRange;
        public float BallWithinReceivingRangeSq;
        public bool bHighlightIfThreatened;
        public bool bIDs;
        public bool bNonPenetrationConstraint;
        public bool bRegions;
        public bool bShowControllingTeam;
        public bool bStates;
        public bool bSupportSpots;
        public bool bViewTargets;
        public float ChanceOfUsingArriveTypeReceiveBehavior;
        public float ChancePlayerAttemptsPotShot;
        public int FrameRate;
        public float Friction;
        public float GoalKeeperInterceptRange;
        public float GoalKeeperInterceptRangeSq;
        public float GoalkeeperMinPassDist;

        //this is the distance the keeper puts between the back of the net
        //and the ball when using the interpose steering behavior
        public float GoalKeeperTendingDistance;
        public float GoalWidth;

        public float KeeperInBallRange;
        public float KeeperInBallRangeSq;
        public float MaxDribbleForce;
        public float MaxPassingForce;
        public float MaxShootingForce;
        public float MinPassDist;
        public int NumAttemptsToFindValidStrike;
        public int NumSupportSpotsX;
        public int NumSupportSpotsY;
        public float PlayerComfortZone;
        public float PlayerComfortZoneSq;

        public float PlayerInTargetRange;
        public float PlayerInTargetRangeSq;
        public float PlayerKickFrequency;
        public float PlayerKickingAccuracy;
        public float PlayerKickingDistance;
        public float PlayerKickingDistanceSq;

        public float PlayerMass;

        //max steering force
        public float PlayerMaxForce;
        public float PlayerMaxSpeedWithBall;
        public float PlayerMaxSpeedWithoutBall;
        public float PlayerMaxTurnRate;
        public float PlayerScale;
        public float SeparationCoefficient;
        public float Spot_AheadOfAttackerScore;
        public float Spot_CanScoreFromPositionScore;
        public float Spot_ClosenessToSupportingPlayerScore;
        public float Spot_DistFromControllingPlayerScore;
        public float Spot_PassSafeScore;
        public float SupportSpotUpdateFreq;
        public float ViewDistance;

        //the distance away from the center of its home region a player
        //must be to be considered at home
        public float WithinRangeOfHome;

        //how close a player must get to a sweet spot before he can change state
        public float WithinRangeOfSupportSpot;
        public float WithinRangeOfSupportSpotSq;
        public bool bShowGoalKeeperTendingDistance;
        public bool bShowGoalKeeperInterceptRange;
        public bool bShowGoalkeeperMinPassDist;
        public bool bShowTeamStates;

        public string RedTeamId;
        public string BlueTeamId;
        public int GameTime;

        private ParamLoader()
            : base("params.ini")
        {
            GoalWidth = GetFloat("GoalWidth");

            NumSupportSpotsX = GetInt("NumSweetSpotsX");
            NumSupportSpotsY = GetInt("NumSweetSpotsY");

            Spot_PassSafeScore = GetFloat("Spot_CanPassScore");
            Spot_CanScoreFromPositionScore = GetFloat("Spot_CanScoreFromPositionScore");
            Spot_DistFromControllingPlayerScore = GetFloat("Spot_DistFromControllingPlayerScore");
            Spot_ClosenessToSupportingPlayerScore = GetFloat("Spot_ClosenessToSupportingPlayerScore");
            Spot_AheadOfAttackerScore = GetFloat("Spot_AheadOfAttackerScore");

            SupportSpotUpdateFreq = GetFloat("SupportSpotUpdateFreq");

            ChancePlayerAttemptsPotShot = GetFloat("ChancePlayerAttemptsPotShot");
            ChanceOfUsingArriveTypeReceiveBehavior = GetFloat("ChanceOfUsingArriveTypeReceiveBehavior");

            BallSize = GetFloat("BallSize");
            BallMass = GetFloat("BallMass");
            Friction = GetFloat("Friction");

            KeeperInBallRange = GetFloat("KeeperInBallRange");
            PlayerInTargetRange = GetFloat("PlayerInTargetRange");
            PlayerKickingDistance = GetFloat("PlayerKickingDistance");
            PlayerKickFrequency = GetFloat("PlayerKickFrequency");


            PlayerMass = GetFloat("PlayerMass");
            PlayerMaxForce = GetFloat("PlayerMaxForce");
            PlayerMaxSpeedWithBall = GetFloat("PlayerMaxSpeedWithBall");
            PlayerMaxSpeedWithoutBall = GetFloat("PlayerMaxSpeedWithoutBall");
            PlayerMaxTurnRate = GetFloat("PlayerMaxTurnRate");
            PlayerScale = GetFloat("PlayerScale");
            PlayerComfortZone = GetFloat("PlayerComfortZone");
            PlayerKickingAccuracy = GetFloat("PlayerKickingAccuracy");

            NumAttemptsToFindValidStrike = GetInt("NumAttemptsToFindValidStrike");


            MaxDribbleForce = GetFloat("MaxDribbleForce");
            MaxShootingForce = GetFloat("MaxShootingForce");
            MaxPassingForce = GetFloat("MaxPassingForce");

            WithinRangeOfHome = GetFloat("WithinRangeOfHome");
            WithinRangeOfSupportSpot = GetFloat("WithinRangeOfSweetSpot");

            MinPassDist = GetFloat("MinPassDistance");
            GoalkeeperMinPassDist = GetFloat("GoalkeeperMinPassDistance");

            GoalKeeperTendingDistance = GetFloat("GoalKeeperTendingDistance");
            GoalKeeperInterceptRange = GetFloat("GoalKeeperInterceptRange");
            BallWithinReceivingRange = GetFloat("BallWithinReceivingRange");

            bStates = GetBoolean("ViewStates");
            bIDs = GetBoolean("ViewIDs");
            bSupportSpots = GetBoolean("ViewSupportSpots");
            bRegions = GetBoolean("ViewRegions");
            bShowControllingTeam = GetBoolean("bShowControllingTeam");
            bViewTargets = GetBoolean("ViewTargets");
            bHighlightIfThreatened = GetBoolean("HighlightIfThreatened");

            FrameRate = GetInt("FrameRate");

            SeparationCoefficient = GetFloat("SeparationCoefficient");
            ViewDistance = GetFloat("ViewDistance");
            bNonPenetrationConstraint = GetBoolean("bNonPenetrationConstraint");
            bShowGoalKeeperTendingDistance = GetBoolean("bShowGoalKeeperTendingDistance");
            bShowGoalKeeperInterceptRange = GetBoolean("bShowGoalKeeperInterceptRange");
            bShowGoalkeeperMinPassDist = GetBoolean("bShowGoalkeeperMinPassDist");
            bShowTeamStates = GetBoolean("bShowTeamStates");
            RedTeamId = GetParameter("RedTeam");
            BlueTeamId = GetParameter("BlueTeam");
            GameTime = GetInt("TimeLimit");

            BallWithinReceivingRangeSq = BallWithinReceivingRange*BallWithinReceivingRange;
            KeeperInBallRangeSq = KeeperInBallRange*KeeperInBallRange;
            PlayerInTargetRangeSq = PlayerInTargetRange*PlayerInTargetRange;
            PlayerKickingDistance += BallSize;
            PlayerKickingDistanceSq = PlayerKickingDistance*PlayerKickingDistance;
            PlayerComfortZoneSq = PlayerComfortZone*PlayerComfortZone;
            GoalKeeperInterceptRangeSq = GoalKeeperInterceptRange*GoalKeeperInterceptRange;
            WithinRangeOfSupportSpotSq = WithinRangeOfSupportSpot*WithinRangeOfSupportSpot;
        }

        private string GetParameter(string parameterName)
        {
            if(!parameters.ContainsKey(parameterName))
            {
                return null;
            }
            return parameters[parameterName];
        }
        public static ParamLoader Instance
        {
            get
            {
                if (_paramLoader == null)
                {
                    _paramLoader = new ParamLoader();
                }

                return _paramLoader;
            }
        }

        private float GetFloat(string parameterName)
        {
            string parameter = GetParameter(parameterName);
            if(parameter == null)
            {
                return 0f;
            }
            float possibleFloat;
            float.TryParse(parameter, out possibleFloat);
            return possibleFloat;
        }

        private int GetInt(string parameterName)
        {
            string parameter = GetParameter(parameterName);
            if (parameter == null)
            {
                return 0;
            }
            int possibleFloat;
            int.TryParse(parameter, out possibleFloat);
            return possibleFloat;
        }

        private bool GetBoolean(string parameterName)
        {
            string parameter = GetParameter(parameterName);
            if(parameter == null || parameter.Equals("0"))
            {
                return false;
            }
            return true;
        }


        //the minimum distance a receiving player must be from the passing player
    }
}