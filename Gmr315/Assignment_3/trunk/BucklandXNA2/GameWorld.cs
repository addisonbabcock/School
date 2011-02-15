#region Using

using System;
using System.Collections.Generic;
using BucklandXNA2;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using XELibrary;

#endregion

namespace Assignment_3
{
    public class GameWorld : DrawableGameComponent
    {
        private List<Vehicle> _vehicles;
        private Vehicle CrossHair;
        private PrimitiveBatch primitiveBatch;
        public GameWorld(Game game, SpriteFont font, SpriteFont font10, SpriteBatch batch)
            : base(game)
        {
            Vector2 crossHairPosition = new Vector2(game.GraphicsDevice.Viewport.Width/2.0f,
                                            game.GraphicsDevice.Viewport.Height/2.0f);
            CrossHair = new Vehicle(game, crossHairPosition, BehaviorMode.None, 1.0f);
            SetupVehicles(25);
            _vehicles[1].CarColor = Color.Blue;
            _vehicles[2].CarColor = Color.Green;
            _vehicles[3].CarColor = Color.White;
            DisableVehicles();
            OutputFont = font;
            OutputFont10 = font10;
            SpriteBatch = batch;
            Input = (IInputHandler) Game.Services.GetService(typeof (IInputHandler));
            primitiveBatch = new PrimitiveBatch(Game.GraphicsDevice);
            
        }

        private SpriteFont OutputFont { get; set; }
        private SpriteFont OutputFont10 { get; set; }
        private SpriteBatch SpriteBatch { get; set; }
        private IInputHandler Input { get; set; }
        
        public Vehicle FirstVehicle
        {
            get
            {
                if (_vehicles != null)
                {
                    return _vehicles[0];
                }
                return null;
            }
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            CheckBehaviors();
            CheckAdjustments();
            CheckChangeCrossHairPosition();
            UpdatePursuit(gameTime);

            base.Update(gameTime);
        }

        private void UpdatePursuit(GameTime gameTime)
        {
            if (FirstVehicle.SteeringBehavior != BehaviorMode.Pursuit)
            {
             
                return;
            }
            CrossHair.SteeringBehavior = BehaviorMode.Flee;

            Vehicle nearestVehicle = FirstVehicle;
            float closestDistanceToCrossHair = float.MaxValue;
            foreach (Vehicle vehicle in _vehicles)
            {
				if (vehicle.Enabled)
				{
					float distance = Vector2.Subtract (vehicle.CurrentPosition, CrossHair.CurrentPosition).LengthSquared ();
					if (distance < closestDistanceToCrossHair)
					{
						closestDistanceToCrossHair = distance;
						nearestVehicle = vehicle;
					}
				}
            }
            CrossHair.TargetPosition = nearestVehicle.CurrentPosition;

            CrossHair.Update(gameTime);
            foreach (Vehicle v in _vehicles)
            {
                v.TargetPosition = CrossHair.CurrentPosition;
                v.TargetVelocity = CrossHair.CurrentVelocity;
            }
        }

        private void CheckAdjustments()
        {
            CheckIncrease(Keys.Home, ref FirstVehicle.MaxSpeed, 1);
            CheckDecrease(Keys.End, ref FirstVehicle.MaxSpeed, 1);
            CheckIncrease(Keys.PageUp, ref Vehicle.MaxForce, 1);
            CheckDecrease(Keys.PageDown, ref Vehicle.MaxForce, 1);
            CheckIncrease(Keys.W, ref Vehicle.WanderDistance, 1.0f);
            CheckDecrease(Keys.S, ref Vehicle.WanderDistance, 1.0f);
            CheckIncrease(Keys.E, ref Vehicle.WanderRadius,1.0f);
            CheckDecrease(Keys.D, ref Vehicle.WanderRadius, 1.0f);
            CheckIncrease(Keys.R, ref Vehicle.WanderJitter, 1.0f);
            CheckDecrease(Keys.F, ref Vehicle.WanderJitter, 1.0f);
            CheckIncrease(Keys.T, ref Vehicle.SeparationWeight, 1.0f);
            CheckDecrease(Keys.G, ref Vehicle.SeparationWeight, 1.0f);
            CheckIncrease(Keys.Y, ref Vehicle.AlignmentWeight, 0.2f);
            CheckDecrease(Keys.H, ref Vehicle.AlignmentWeight, 0.2f);
            CheckIncrease(Keys.U, ref Vehicle.CohesionWeight, 0.2f);
            CheckDecrease(Keys.J, ref Vehicle.CohesionWeight, 0.2f);
        }

        private void CheckIncrease(Keys increaseKey, ref float increaseVariable, float increaseAmount)
        {
            if (Input.KeyboardState.WasKeyPressed(increaseKey))
            {
                increaseVariable+= increaseAmount;
            }
        }
        private void CheckIncrease(Keys increaseKey, ref int increaseVariable, int increaseAmount)
        {
            if (Input.KeyboardState.WasKeyPressed(increaseKey))
            {
                increaseVariable+= increaseAmount;
            }
        }

        private void CheckDecrease(Keys increaseKey, ref int decreaseVariable, int decreaseAmount)
        {
            if (Input.KeyboardState.WasKeyPressed(increaseKey))
            {
                decreaseVariable-= decreaseAmount;
                if(decreaseVariable < 0)
                {
                    decreaseVariable = 0;
                }
            }
        }

        private void CheckDecrease(Keys increaseKey, ref float decreaseVariable, float decreaseAmount)
        {
            if (Input.KeyboardState.WasKeyPressed(increaseKey))
            {
                decreaseVariable-= decreaseAmount;
                if (decreaseVariable < 0)
                {
                    decreaseVariable = 0;
                }
            }
        }

        private void CheckBehaviors()
        {
            Keys[] pressedKeys = Input.KeyboardState.GetPressedKeys();
            if(GameKeys.IsBehaviorKeyPressed(pressedKeys))
            {
                FirstVehicle.SteeringBehavior = GameKeys.GetBehavior(pressedKeys);
                DisableVehicles();
            }
            CheckBehaviorOffsetPursuit();
            CheckBehaviorPursuit();
        }

        private void CheckChangeCrossHairPosition()
        {
            if (Input.MouseState.LeftButton == ButtonState.Pressed)
            {
                CrossHair.CurrentPosition = new Vector2(Input.MouseState.X, Input.MouseState.Y);
                FirstVehicle.TargetPosition = CrossHair.CurrentPosition;
            }
        }

        private void CheckBehaviorPursuit()
        {
            if (Input.KeyboardState.IsKeyDown(GameKeys.Pursuit))
            {
                FirstVehicle.SteeringBehavior = BehaviorMode.Pursuit;
                DisableVehicles();
                _vehicles[1].SteeringBehavior = BehaviorMode.Pursuit;
                _vehicles[1].Enabled = true;
            }
        }

        private void CheckBehaviorOffsetPursuit()
        {
            if (Input.KeyboardState.IsKeyDown(GameKeys.OffsetPursuit))
            {
                FirstVehicle.SteeringBehavior = BehaviorMode.Wander;
                DisableVehicles();
                for(int i = 1; i <=6; i++)
                {
                    _vehicles[i].Enabled = true;
                    _vehicles[i].SteeringBehavior = BehaviorMode.OffsetPursuit;
                }
            }
        }

        private void DisableVehicles()
        {
            for (int i = 1; i < _vehicles.Count; i++ )
            {
                _vehicles[i].Enabled = false;
            }
        }

        private void SetupVehicles(int numVehicles)
        {
            if (_vehicles != null)
            {
                while (_vehicles.Count > 0)
                {
                    Game.Components.Remove(_vehicles[0]);
                    _vehicles[0].Dispose();
                    _vehicles.RemoveAt(0);
                }
            }

            _vehicles = new List<Vehicle>(numVehicles);

            //setup the agents
            for (int vehicleNumber = 0; vehicleNumber < numVehicles; vehicleNumber++)
            {
                int width = Game.GraphicsDevice.Viewport.Width;
                int height = Game.GraphicsDevice.Viewport.Height;

                //determine a random starting position
                Vector2 spawnPosition = new Vector2(width/2.0f + Utils.RandomClamped()*width/2.0f,
                                                    height/2.0f + Utils.RandomClamped()*height/2.0f);

                Vehicle vehicle = new Vehicle(Game, spawnPosition, BehaviorMode.None, 10);
                vehicle.TargetPosition = CrossHair.CurrentPosition;
                vehicle.Evader = CrossHair;
                if(vehicleNumber > 0)
                {
                    vehicle.Leader = FirstVehicle;
                }
                _vehicles.Add(vehicle);
                Game.Components.Add(vehicle);
            }

            FirstVehicle.CarColor = Color.Red;
        }

        private void OutputSteeringBehaviorState(string steeringBehaviorState)
        {
            SpriteBatch.DrawString(OutputFont, steeringBehaviorState,
                                   new Vector2(Game.GraphicsDevice.Viewport.Width - 190, 23), Color.Orange);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin();
            SpriteBatch.DrawString(OutputFont, "Click to move crosshair",
                                   new Vector2(5, Game.GraphicsDevice.Viewport.Height - 20), Color.White);
            SpriteBatch.DrawString(OutputFont10, "MaxSpeed(Home/End): " + FirstVehicle.MaxSpeed,
                                   new Vector2(10, 5), Color.GreenYellow);
            SpriteBatch.DrawString(OutputFont10, "MaxForce(PUp/PDn): " + Vehicle.MaxForce, new Vector2(10, 20),
                                   Color.GreenYellow);

            switch (FirstVehicle.SteeringBehavior)
            {
                case BehaviorMode.None:
                    OutputSteeringBehaviorState("None");
                    break;
                case BehaviorMode.Arrive:
                    OutputSteeringBehaviorState("Arrive");
                    break;
                case BehaviorMode.Flee:
                    OutputSteeringBehaviorState("Flee");
                    break;
                case BehaviorMode.Flocking:
                    OutputSteeringBehaviorState("Flocking");
                    break;
                case BehaviorMode.Hide:
                    OutputSteeringBehaviorState("Hide");
                    break;
                case BehaviorMode.Interpose:
                    OutputSteeringBehaviorState("Interpose");
                    break;
                case BehaviorMode.NonPenetrationConstraint:
                    OutputSteeringBehaviorState("Non Penetration Constraint");
                    break;
                case BehaviorMode.ObstacleAvoidance:
                    OutputSteeringBehaviorState("Obstacle Avoidance");
                    break;
                case BehaviorMode.OffsetPursuit:
                    OutputSteeringBehaviorState("Offset Pursuit");
                    break;
                case BehaviorMode.PathFollowing:
                    OutputSteeringBehaviorState("Path Following");
                    break;
                case BehaviorMode.Pursuit:
                    OutputSteeringBehaviorState("Pursuit");
                    break;
                case BehaviorMode.Seek:
                    OutputSteeringBehaviorState("Seek");
                    break;
                case BehaviorMode.Wander:
                    if (!_vehicles[1].Enabled)
                    {
                        SpriteBatch.DrawString(OutputFont10, "Wander Distance(W/S): " + Vehicle.WanderDistance,
                                               new Vector2(10, 55), Color.Yellow);

                        SpriteBatch.DrawString(OutputFont10, "Wander Radius(E/D): " + Vehicle.WanderRadius,
                                               new Vector2(10, 70), Color.Yellow);
                        SpriteBatch.DrawString(OutputFont10, "Wander Jitter(R/F): " + Vehicle.WanderJitter,
                                               new Vector2(10, 85), Color.Yellow);

                        OutputSteeringBehaviorState("Wander");
                    } else
                    {
                        SpriteBatch.DrawString(OutputFont10, "Separation (T/G): " + Vehicle.SeparationWeight,
                                               new Vector2(10, 55), Color.Yellow);

                        SpriteBatch.DrawString(OutputFont10, "Alignment (Y/H): " + Vehicle.AlignmentWeight,
                                               new Vector2(10, 70), Color.Yellow);
                        SpriteBatch.DrawString(OutputFont10, "Cohesion (U/J): " + Vehicle.CohesionWeight,
                                               new Vector2(10, 85), Color.Yellow);
                        OutputSteeringBehaviorState("Offset Pursuit");
                    }

                    break;
                default:
                    break;
            }

            DrawCrossHair();

            SpriteBatch.End();
            base.Draw(gameTime);
        }

        private void DrawCrossHair()
        {
            if(FirstVehicle.SteeringBehavior == BehaviorMode.Wander)
            {
                return;
            }
            primitiveBatch.Begin(PrimitiveType.LineList);
            primitiveBatch.AddVertex(new Vector2(CrossHair.CurrentPosition.X - 8, CrossHair.CurrentPosition.Y), Color.Red);
            primitiveBatch.AddVertex(new Vector2(CrossHair.CurrentPosition.X + 8, CrossHair.CurrentPosition.Y), Color.Red);
            primitiveBatch.AddVertex(new Vector2(CrossHair.CurrentPosition.X, CrossHair.CurrentPosition.Y - 8), Color.Red);
            primitiveBatch.AddVertex(new Vector2(CrossHair.CurrentPosition.X, CrossHair.CurrentPosition.Y + 8), Color.Red);
            List<Vector2> circleVectors = CreateEllipse(CrossHair.CurrentPosition, 4, 4, 64);
            for (int i = 0; i < circleVectors.Count - 1; i++)
            {
                primitiveBatch.AddVertex(circleVectors[i], Color.Red);
                primitiveBatch.AddVertex(circleVectors[i + 1], Color.Red);
            }
            primitiveBatch.End();
        }

        /// <summary>
        /// Creates an ellipse starting from 0, 0 with the given width and height.
        /// Vectors are generated using the parametric equation of an ellipse.
        /// </summary>
        /// <param name="semimajor_axis">The width of the ellipse at its center.</param>
        /// <param name="semiminor_axis">The height of the ellipse at its center.</param>
        /// <param name="sides">The number of sides on the ellipse (a higher value yields more resolution).</param>
        public List<Vector2> CreateEllipse(Vector2 position, float semimajor_axis, float semiminor_axis, int sides)
        {
            List<Vector2> vectors = new List<Vector2>();
            vectors.Clear();
            float max = 2.0f*(float) Math.PI;
            float step = max/sides;
            float h = position.X;
            float k = position.Y;

            for (float t = 0.0f; t < max; t += step)
            {
                // center point: (h,k); add as argument if you want (to circumvent modifying this.Position)
                // x = h + a*cos(t)  -- a is semimajor axis, b is semiminor axis
                // y = k + b*sin(t)
                vectors.Add(new Vector2((float) (h + semimajor_axis*Math.Cos(t)),
                                        (float) (k + semiminor_axis*Math.Sin(t))));
            }

            // then add the first vector again so it's a complete loop
            vectors.Add(new Vector2((float) (h + semimajor_axis*Math.Cos(step)),
                                    (float) (k + semiminor_axis*Math.Sin(step))));

            return vectors;
        }
    }
}