using System;
using System.Collections.Generic;
using System.Linq;
using Common._2D;
using Common.Game;
using Common.Messaging;
using Common.Misc;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Raven.armory;
using Raven.Bots;
using Raven.goals;
using Raven.lua;
using Raven.navigation;

namespace Raven
{
    public class Raven_Game : DrawableGameComponent
    {
        private static readonly MessageDispatcher Dispatcher = MessageDispatcher.Instance();
        private static readonly EntityManager EntityMgr = EntityManager.Instance();
        private static readonly Raven_UserOptions UserOptions = Raven_UserOptions.Instance();
        private readonly List<Raven_Projectile> m_Projectiles;
        private readonly Raven_Scriptor script = Raven_Scriptor.Instance();


        //the current game map

        //a list of all the bots that are inhabiting the map
        private List<AbstractBot> m_Bots;

        //the user may select a bot to control manually. This is a pointer to that
        //bot


        //if true the game will be paused
        private bool m_bPaused;

        //if true a bot is removed from the game
        private bool m_bRemoveABot;

        //when a bot is killed a "grave" is displayed for a few seconds. This
        //class manages the graves
        private GraveMarkers m_pGraveMarkers;
        private Raven_Map m_pMap;
        private PathManager m_pPathManager;
        private AbstractBot m_pSelectedBot;
        private DateTime startTime;
        private int gameDuration;

        public Raven_Game(Game game)
            : base(game)
        {
            m_Bots = new List<AbstractBot>();
            m_Projectiles = new List<Raven_Projectile>();
            m_pSelectedBot = null;
            m_bPaused = false;
            m_bRemoveABot = false;
            m_pMap = null;
            m_pPathManager = null;
            m_pGraveMarkers = null;

            //load in the default map
            LoadMap(script.GetString("StartMap"));
            startTime = DateTime.Now;
            gameDuration = script.GetInt("GameDuration");
        }

        //this iterates through each trigger, testing each one against each bot
        // KJB: THIS MAY NOT BE USED OR NEEDED CAN'T FIND A METHOD IMPLEMENETATION private void  UpdateTriggers();

        //deletes all entities, empties all containers and creates a new navgraph 
        private void Clear()
        {
            Debug.WriteLine("------------------------------ Clearup -------------------------------");

            //delete the bots
            foreach (AbstractBot it in m_Bots)
            {
                Debug.WriteLine("deleting entity id: " + it.ID() + " of type " +
                                Raven_ObjectEnumerations.GetNameOfType(it.EntityType()) + "(" + it.EntityType() + ")");
                it.Dispose();
            }

            foreach (Raven_Projectile curW in m_Projectiles)
            {
                Debug.WriteLine("deleting projectile id: " + curW.ID());
                curW.Dispose();
            }

            //clear the containers
            m_Projectiles.Clear();
            m_Bots.Clear();

            m_pSelectedBot = null;
        }

        //attempts to position a spawning bot at a free spawn point. returns false
        //if unsuccessful 
        private bool AttemptToAddBot(AbstractBot pBot)
        {
            //make sure there are some spawn points available
            if (m_pMap.GetSpawnPoints().Count() <= 0)
            {
                throw new Exception("Map has no spawn points!"); return false;
            }

            //we'll make the same number of attempts to spawn a bot this update as
            //there are spawn points
            int attempts = m_pMap.GetSpawnPoints().Count();

            while (--attempts >= 0)
            {
                //select a random spawn point
                Vector2 pos = m_pMap.GetRandomSpawnPoint();

                bool bAvailable = true;
                foreach (AbstractBot curBot in m_Bots)
                {
                    //if the spawn point is unoccupied spawn a bot
                    if (Vector2.Distance(pos, curBot.Pos()) < curBot.BRadius())
                    {
                        bAvailable = false;
                    }
                }

                if (bAvailable)
                {
                    pBot.Spawn(pos);

                    return true;
                }
            }

            return false;
        }

        //when a bot is removed from the game by a user all remaining bots
        //must be notified so that they can remove any references to that bot from
        //their memory
        private void NotifyAllBotsOfRemoval(AbstractBot pRemovedBot)
        {
            foreach (AbstractBot curBot in m_Bots)
            {
                Dispatcher.DispatchMsg(MessageDispatcher.SEND_MSG_IMMEDIATELY,
                                       MessageDispatcher.SENDER_ID_IRRELEVANT, curBot.ID(),
                                       (int)message_type.Msg_UserHasRemovedBot,
                                       pRemovedBot);
            }
        }

        ~Raven_Game()
        {
            Clear();
            m_pPathManager.Dispose();
            m_pMap.Dispose();
            m_pGraveMarkers.Dispose();
        }

        //the usual suspects
        public override void Draw(GameTime gameTime)
        {


            Game1.SpriteBatch.Begin();
            Game1.PrimitiveBatch.Begin(PrimitiveType.LineList);

            int minutes = GameTimeMinutes();
            string seconds = GameTimeSeconds().ToString();
            if(GameTimeSeconds() < 10)
            {
                seconds = "0" + seconds;
            }
            string timeString = minutes + ":" + seconds;

            if (IsTimeUp())
            {
                Game1.SpriteBatch.DrawString(Game1.GameFont, "Game Over!", new Vector2(360, 12), Color.Black);
            }
            else
            {
                Game1.SpriteBatch.DrawString(Game1.GameFont, timeString, new Vector2(360, 12), Color.Black);
            }
            m_pGraveMarkers.Render(Game1.PrimitiveBatch, Game1.SpriteBatch, Game1.GameFont);

            //render the map
            m_pMap.Render(Game1.PrimitiveBatch, Game1.SpriteBatch, Game1.GameFont);

            //render all the bots unless the user has selected the option to only 
            //render those bots that are in the fov of the selected bot
            if (m_pSelectedBot != null && UserOptions.m_bOnlyShowBotsInTargetsFOV)
            {
                List<AbstractBot> VisibleBots = GetAllBotsInFOV(m_pSelectedBot);

                foreach (AbstractBot it in VisibleBots)
                {
                    it.Render(Game1.PrimitiveBatch, Game1.SpriteBatch, Game1.GameFont);
                }

                if (m_pSelectedBot != null)
                    m_pSelectedBot.Render(Game1.PrimitiveBatch, Game1.SpriteBatch, Game1.GameFont);
            }

            else
            {
                //render all the entities
                foreach (AbstractBot curBot in m_Bots)
                {
                    if (curBot.isAlive())
                    {
                        curBot.Render(Game1.PrimitiveBatch, Game1.SpriteBatch, Game1.GameFont);
                    }
                }
            }

            //render any projectiles
            foreach (Raven_Projectile curW in m_Projectiles)
            {
                curW.Render(Game1.PrimitiveBatch);
            }

            //Game1.SpriteBatch.DrawString(Game1.GameFont, "Num Current Searches: "+m_pPathManager.GetNumActiveSearches(), new Vector2(300, Game.GraphicsDevice.Viewport.Height - 70), Color.White );


            //render a red circle around the selected bot (blue if possessed)
            if (m_pSelectedBot != null)
            {
                if (m_pSelectedBot.isPossessed())
                {
                    Drawing.DrawCircle(Game1.PrimitiveBatch, m_pSelectedBot.Pos(), m_pSelectedBot.BRadius() + 1,
                                       Color.Blue);
                }
                else
                {
                    Drawing.DrawCircle(Game1.PrimitiveBatch, m_pSelectedBot.Pos(), m_pSelectedBot.BRadius() + 1,
                                       Color.Red);
                }


                if (UserOptions.m_bShowOpponentsSensedBySelectedBot)
                {
                    m_pSelectedBot.GetSensoryMem().RenderBoxesAroundRecentlySensed(Game1.PrimitiveBatch);
                }

                //render a square around the bot's target
                if (UserOptions.m_bShowTargetOfSelectedBot && m_pSelectedBot.GetTargetBot() != null)
                {
                    Vector2 p = m_pSelectedBot.GetTargetBot().Pos();
                    float b = m_pSelectedBot.GetTargetBot().BRadius();
                    Drawing.DrawLine(Game1.PrimitiveBatch, new Vector2(p.X - b, p.Y - b), new Vector2(p.X + b, p.Y - b),
                                     Color.DarkRed);
                    Drawing.DrawLine(Game1.PrimitiveBatch, new Vector2(p.X + b, p.Y - b), new Vector2(p.X + b, p.Y + b),
                                     Color.DarkRed);
                    Drawing.DrawLine(Game1.PrimitiveBatch, new Vector2(p.X + b, p.Y + b), new Vector2(p.X - b, p.Y + b),
                                     Color.DarkRed);
                    Drawing.DrawLine(Game1.PrimitiveBatch, new Vector2(p.X - b, p.Y + b), new Vector2(p.X - b, p.Y - b),
                                     Color.DarkRed);
                }


                //render the path of the bot
                if (UserOptions.m_bShowPathOfSelectedBot)
                {
                    m_pSelectedBot.GetBrain().Render(Game1.PrimitiveBatch);
                }

                //display the bot's goal stack
                if (UserOptions.m_bShowGoalsOfSelectedBot)
                {
                    var p = new Vector2(m_pSelectedBot.Pos().X - 50, m_pSelectedBot.Pos().Y);


                    m_pSelectedBot.GetBrain().RenderAtPos(Game1.SpriteBatch, Game1.GameFont, ref p,
                                                          GoalTypeToString.Convert);
                }

                if (UserOptions.m_bShowGoalAppraisals)
                {
                    m_pSelectedBot.GetBrain().RenderEvaluations(Game1.SpriteBatch, Game1.GameFont, 5, 415);
                }

                if (UserOptions.m_bShowWeaponAppraisals)
                {
                    m_pSelectedBot.GetWeaponSys().RenderDesirabilities(Game1.SpriteBatch, Game1.GameFont, Color.Black);
                }

                if (Game1.WasKeyPressed(Keys.Q) && m_pSelectedBot.isPossessed())
                {
                    Game1.SpriteBatch.DrawString(Game1.GameFont, "Queuing", Game1.GetClientCursorPosition(),
                                                 new Color(255, 0, 0));
                }
            }
            Game1.PrimitiveBatch.End();
            Game1.SpriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            //don't update if the user has paused the game
            if (m_bPaused) return;

            CheckRightMouseButtonClicked();
            CheckLeftMouseButtonClicked();
            m_pGraveMarkers.Update();

            //get any player keyboard input
            GetPlayerInput();

            //update all the queued searches in the path manager
            m_pPathManager.UpdateSearches();

            //update any doors
            foreach (Raven_Door curDoor in m_pMap.GetDoors())
            {
                curDoor.Update();
            }


            //update any current projectiles
            int projectileNumber = 0;
            while (projectileNumber < m_Projectiles.Count)
            {
                Raven_Projectile curW = m_Projectiles[projectileNumber];
                if (!curW.isDead())
                {
                    curW.Update();
                    projectileNumber++;
                }
                else
                {
                    curW.Dispose();
                    m_Projectiles.RemoveAt(projectileNumber);
                }
            }


            //update the bots
            bool bSpawnPossible = true;

            foreach (AbstractBot curBot in m_Bots)
            {
                if (curBot.isSpawning() && bSpawnPossible)
                {
                    bSpawnPossible = AttemptToAddBot(curBot);
                }

                    //if this bot's status is 'dead' add a grave at its current location 
                //then change its status to 'respawning'
                else if (curBot.isDead())
                {
                    //create a grave
                    m_pGraveMarkers.AddGrave(curBot.Pos());

                    //change its status to spawning
                    curBot.SetSpawning();
                }

                    //if this bot is alive update it.
                else if (curBot.isAlive())
                {
                    curBot.Update();
                }
            }

            //update the triggers
            m_pMap.UpdateTriggerSystem(ref m_Bots);

            //if the user has requested that the number of bots be decreased, remove
            //one
            if (m_bRemoveABot)
            {
                if (m_Bots.Count != 0)
                {
                    AbstractBot pBot = m_Bots[m_Bots.Count - 1];
                    if (pBot == m_pSelectedBot)
                        m_pSelectedBot = null;
                    NotifyAllBotsOfRemoval(pBot);
                    m_Bots.Remove(pBot);
                    pBot.Dispose();
                    pBot = null;
                }

                m_bRemoveABot = false;
            }
        }

        //loads an environment from a file
        public bool LoadMap(string filename)
        {
            //clear any current bots and projectiles
            Clear();

            //out with the old
            if (m_pMap != null)
            {
                m_pMap.Dispose();
            }
            m_pMap = null;
            if (m_pGraveMarkers != null)
            {
                m_pGraveMarkers.Dispose();
            }
            m_pGraveMarkers = null;
            if (m_pPathManager != null)
            {
                m_pPathManager.Dispose();
            }
            m_pPathManager = null;

            //in with the new
            m_pGraveMarkers = new GraveMarkers(script.GetDouble("GraveLifetime"));
            m_pPathManager = new PathManager(script.GetInt("MaxSearchCyclesPerUpdateStep"));
            m_pMap = new Raven_Map();

            //make sure the entity manager is reset
            EntityMgr.Reset();


            //load the new map data
            if (m_pMap.LoadMap(filename))
            {
                int NumBots = script.GetInt("NumBots");
                for (int i = 1; i <= NumBots; i++) 
                {
                    string botClassName = script.GetString("BotNames" + i);
                    if (string.IsNullOrEmpty(botClassName))
                    {
                        Debug.WriteLine("Invalid lua table: BotNames");
                    }
                    else
                    {
                        AddBot(botClassName);
                    }
                }

                return true;
            }

            return false;
        }

        public void AddBot(string className)
        {
            //create a bot. (its position is irrelevant at this point because it will
            //not be rendered until it is spawned)
            //var rb = new AbstractRaven_Bot(this, new Vector2());
            AbstractBot bot = BotFactory.Instance().CreateTeam(className, this, new Vector2());

            //switch the default steering behaviors on
            bot.GetSteering().WallAvoidanceOn();
            bot.GetSteering().SeparationOn();

            m_Bots.Add(bot);

            //register the bot with the entity manager
            EntityMgr.RegisterEntity(bot);


            Debug.WriteLine("Adding Bot of ClassName: " + className + " with ID " + bot.ID());

        }

        public void AddRocket(AbstractBot shooter, Vector2 target)
        {
            Raven_Projectile rp = new Rocket(shooter, target);

            m_Projectiles.Add(rp);

            Debug.WriteLine("Adding a rocket " + rp.ID() + " at pos " + rp.Pos());
        }

        public void AddRailGunSlug(AbstractBot shooter, Vector2 target)
        {
            Raven_Projectile rp = new Slug(shooter, target);

            m_Projectiles.Add(rp);

            Debug.WriteLine("Adding a rail gun slug " + rp.ID() + " at pos " + rp.Pos());
        }

        public void AddShotGunPellet(AbstractBot shooter, Vector2 target)
        {
            Raven_Projectile rp = new Pellet(shooter, target);

            m_Projectiles.Add(rp);


            Debug.WriteLine("Adding a shotgun shell " + rp.ID() + " at pos " + rp.Pos());
        }

        public void AddBolt(AbstractBot shooter, Vector2 target)
        {
            Raven_Projectile rp = new Bolt(shooter, target);

            m_Projectiles.Add(rp);

            Debug.WriteLine("Adding a bolt " + rp.ID() + " at pos " + rp.Pos());
        }

        //removes the last bot to be added
        public void RemoveBot()
        {
            m_bRemoveABot = true;
        }

        //returns true if a bot of size BoundingRadius cannot move from A to B
        //without bumping into world geometry
        //------------------------- isPathObstructed ----------------------------------
        //
        //  returns true if a bot cannot move from A to B without bumping into 
        //  world geometry. It achieves this by stepping from A to B in steps of
        //  size BoundingRadius and testing for intersection with world geometry at
        //  each point.
        //-----------------------------------------------------------------------------
        public bool isPathObstructed(Vector2 A, Vector2 B, float BoundingRadius)
        {
            Vector2 ToB = Vector2.Normalize(B - A);

            Vector2 curPos = A;

            while (Vector2.DistanceSquared(curPos, B) > BoundingRadius * BoundingRadius)
            {
                //advance curPos one step
                curPos += ToB * 0.5f * BoundingRadius;

                //test all walls against the new position
                IEnumerable<Wall2D> walls = m_pMap.GetWalls();
                if (WallIntersectionTests.doWallsIntersectCircle(ref walls, curPos, BoundingRadius))
                {
                    return true;
                }
            }

            return false;
        }

        //returns a vector of pointers to bots in the FOV of the given bot
        public List<AbstractBot> GetAllBotsInFOV(AbstractBot pBot)
        {
            var VisibleBots = new List<AbstractBot>();
            IEnumerable<Wall2D> walls = m_pMap.GetWalls();

            foreach (AbstractBot curBot in m_Bots)
            {
                //make sure time is not wasted checking against the same bot or against a
                // bot that is dead or re-spawning
                if (curBot == pBot || !curBot.isAlive()) continue;

                //first of all test to see if this bot is within the FOV
                if (isSecondInFOVOfFirst(pBot.Pos(),
                                         pBot.Facing(),
                                         curBot.Pos(),
                                         pBot.FieldOfView()))
                {
                    //cast a ray from between the bots to test visibility. If the bot is
                    //visible add it to the vector
                    if (!WallIntersectionTests.doWallsObstructLineSegment(pBot.Pos(),
                                                                          curBot.Pos(),
                                                                          ref walls))
                    {
                        VisibleBots.Add(curBot);
                    }
                }
            }

            return VisibleBots;
        }

        //------------------ isSecondInFOVOfFirst -------------------------------------
        //
        //  returns true if the target position is in the field of view of the entity
        //  positioned at posFirst facing in facingFirst
        //-----------------------------------------------------------------------------
        public static bool isSecondInFOVOfFirst(Vector2 posFirst,
                                                Vector2 facingFirst,
                                                Vector2 posSecond,
                                                float fov)
        {
            Vector2 toTarget = Vector2.Normalize(posSecond - posFirst);

            return Vector2.Dot(facingFirst, toTarget) >= (float)Math.Cos(fov / 2.0);
        }

        //returns true if the second bot is unobstructed by walls and in the field
        //of view of the first.
        public bool isSecondVisibleToFirst(AbstractBot pFirst,
                                           AbstractBot pSecond)
        {
            IEnumerable<Wall2D> walls = m_pMap.GetWalls();
            //if the two bots are equal or if one of them is not alive return false
            if (!(pFirst == pSecond) && pSecond.isAlive())
            {
                //first of all test to see if this bot is within the FOV
                if (isSecondInFOVOfFirst(pFirst.Pos(),
                                         pFirst.Facing(),
                                         pSecond.Pos(),
                                         pFirst.FieldOfView()))
                {
                    //test the line segment connecting the bot's positions against the walls.
                    //If the bot is visible add it to the vector
                    if (!WallIntersectionTests.doWallsObstructLineSegment(pFirst.Pos(),
                                                                          pSecond.Pos(),
                                                                          ref walls))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        //returns true if the ray between A and B is unobstructed.
        public bool isLOSOkay(Vector2 A, Vector2 B)
        {
            IEnumerable<Wall2D> walls = m_pMap.GetWalls();
            return !WallIntersectionTests.doWallsObstructLineSegment(A, B, ref walls);
        }

        //returns the position of the closest visible switch that triggers the
        //door of the specified ID
        public Vector2 GetPosOfClosestSwitch(Vector2 botPos, int doorID)
        {
            var SwitchIDs = new List<int>();

            //first we need to get the ids of the switches attached to this door
            foreach (Raven_Door curDoor in m_pMap.GetDoors())
            {
                if (curDoor.ID() == doorID)
                {
                    SwitchIDs = curDoor.GetSwitchIDs();
                    break;
                }
            }

            var closest = new Vector2();
            float ClosestDist = float.MaxValue;

            //now test to see which one is closest and visible
            foreach (int it in SwitchIDs)
            {
                BaseGameEntity trig = EntityMgr.GetEntityFromID(it);

                if (isLOSOkay(botPos, trig.Pos()))
                {
                    float dist = Vector2.DistanceSquared(botPos, trig.Pos());

                    if (dist < ClosestDist)
                    {
                        ClosestDist = dist;
                        closest = trig.Pos();
                    }
                }
            }

            return closest;
        }

        //given a position on the map this method returns the bot found with its
        //bounding radius of that position.If there is no bot at the position the
        //method returns NULL
        public AbstractBot GetBotAtPosition(Vector2 CursorPos)
        {
            foreach (AbstractBot curBot in m_Bots)
            {
                if (Vector2.Distance(curBot.Pos(), CursorPos) < curBot.BRadius())
                {
                    if (curBot.isAlive())
                    {
                        return curBot;
                    }
                }
            }
            return null;
        }


        public void TogglePause()
        {
            m_bPaused = !m_bPaused;
        }

        //this method is called when the user clicks the right mouse button.
        //The method checks to see if a bot is beneath the cursor. If so, the bot
        //is recorded as selected.If the cursor is not over a bot then any selected
        // bot/s will attempt to move to that position.
        public void CheckRightMouseButtonClicked()
        {
            if (!Game1.GetRightMouseButtonClicked())
            {
                return;
            }
            AbstractBot pBot = GetBotAtPosition(Game1.GetClientCursorPosition());

            //if there is no selected bot just return;
            if (pBot == null && m_pSelectedBot == null) return;

            //if the cursor is over a different bot to the existing selection,
            //change selection
            if (pBot != null && pBot != m_pSelectedBot)
            {
                if (m_pSelectedBot != null) m_pSelectedBot.Exorcise();
                m_pSelectedBot = pBot;

                return;
            }

            //if the user clicks on a selected bot twice it becomes possessed(under
            //the player's control)
            if (pBot != null && pBot == m_pSelectedBot)
            {
                m_pSelectedBot.TakePossession();

                //clear any current goals
                m_pSelectedBot.GetBrain().RemoveAllSubgoals();
            }

            //if the bot is possessed then a right click moves the bot to the cursor
            //position
            if (m_pSelectedBot.isPossessed())
            {
                //if the shift key is pressed down at the same time as clicking then the
                //movement command will be queued
                if (Game1.IsKeyPressed(Keys.Q))
                {
                    m_pSelectedBot.GetBrain().QueueGoal_MoveToPosition(Game1.GetClientCursorPosition());
                }
                else
                {
                    //clear any current goals
                    m_pSelectedBot.GetBrain().RemoveAllSubgoals();

                    m_pSelectedBot.GetBrain().AddGoal_MoveToPosition(Game1.GetClientCursorPosition());
                }
            }
        }


        //this method is called when the user clicks the left mouse button. If there
        //is a possessed bot, this fires the weapon, else does nothing
        public void CheckLeftMouseButtonClicked()
        {
            if (!Game1.GetLeftMouseButtonClicked())
            {
                return;
            }
            if (m_pSelectedBot != null && m_pSelectedBot.isPossessed())
            {
                m_pSelectedBot.FireWeapon(Game1.GetClientCursorPosition());
            }
        }

        //when called will release any possessed bot from user control
        public void ExorciseAnyPossessedBot()
        {
            if (m_pSelectedBot != null) m_pSelectedBot.Exorcise();
        }

        //if a bot is possessed the keyboard is polled for user input and any 
        //relevant bot methods are called appropriately
        public void GetPlayerInput()
        {
            
            if (m_pSelectedBot != null && m_pSelectedBot.isPossessed())
            {
                m_pSelectedBot.RotateFacingTowardPosition(Game1.GetClientCursorPosition());
            }
        }

        public AbstractBot PossessedBot()
        {
            return m_pSelectedBot;
        }

        public void ChangeWeaponOfPossessedBot(int weapon)
        {
            //ensure one of the bots has been possessed
            if (m_pSelectedBot != null)
            {
                switch (weapon)
                {
                    case (int)Raven_Objects.type_blaster:

                        PossessedBot().ChangeWeapon((int)Raven_Objects.type_blaster);
                        return;

                    case (int)Raven_Objects.type_shotgun:

                        PossessedBot().ChangeWeapon((int)Raven_Objects.type_shotgun);
                        return;

                    case (int)Raven_Objects.type_rocket_launcher:

                        PossessedBot().ChangeWeapon((int)Raven_Objects.type_rocket_launcher);
                        return;

                    case (int)Raven_Objects.type_rail_gun:

                        PossessedBot().ChangeWeapon((int)Raven_Objects.type_rail_gun);
                        return;
                }
            }
        }


        // public static Raven_Map                   GetMap(){return m_pMap;}
        public Raven_Map GetMap()
        {
            return m_pMap;
        }

        public List<AbstractBot> GetAllBots()
        {
            return m_Bots;
        }

        public PathManager GetPathManager()
        {
            return m_pPathManager;
        }

        public int GetNumBots()
        {
            return m_Bots.Count();
        }


        public void TagRaven_BotsWithinViewRange(AbstractBot pRaven_Bot, float range)
        {
            EntityFunctionTemplates<AbstractBot>.TagNeighbors(pRaven_Bot, m_Bots, range);
        }

        public bool IsTimeUp()
        {
            if (startTime.AddMinutes(gameDuration) < DateTime.Now)
            {
                m_bPaused = true;
                return true;
            }
            return false;

        }

        public int GameTimeMinutes()
        {
            TimeSpan x = (startTime.AddMinutes(gameDuration) - DateTime.Now);
            return x.Minutes;

        }

        public int GameTimeSeconds()
        {
            TimeSpan x = (startTime.AddMinutes(gameDuration) - DateTime.Now);
            return x.Seconds;

        }
    }
}