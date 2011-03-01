#region Using

using System;
using System.Collections.Generic;
using BucklandXNA2;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XELibrary;

#endregion

namespace Assignment_3
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Vehicle : DrawableGameComponent
    {
        // Adjustable compontents that are the same for all vehicles


        private const int MaxPathPoints = 20;

        private static readonly List<Vehicle> allVehicles = new List<Vehicle>();
        private static PrimitiveBatch _batch;
        private static Vector2[] _vehicleVertexBuffer;
        public static float AlignmentWeight = 2;
        public static float CohesionWeight = 5;
        public static int MaxForce;
        public static float SeparationWeight = 150;
        public static float WanderDistance = 75;
        public static float WanderJitter = 2;
        public static float WanderRadius = 20;
        public static int WaypointSeekDistSq = 20;
        private Vector2 _currentPosition;
        public Vector2 _wanderTarget;
        public Vehicle Evader;
        public Vehicle Leader;
        public int MaxSpeed;
        public Path Path;
        public Vector2 TargetPosition;

        public Vehicle(Game game, Vector2 currentPosition, BehaviorMode mode, float scale)
            : base(game)
        {
            // TODO: Construct any child components here
            CurrentPosition = currentPosition;
            Scale = scale;
            SteeringBehavior = mode;

            PerformDefaultInitialization();
            allVehicles.Add(this);
        }

        public BehaviorMode SteeringBehavior { get; set; }

        private Vector2 Acceleration { get; set; }
        public float Mass { get; set; }

        public Vector2 CurrentPosition
        {
            get { return _currentPosition; }
            set { _currentPosition = value; }
        }

        private float Rotation { get; set; }
        private float Scale { get; set; }

        public Vector2 Heading
        {
            get
            {
                if (CurrentVelocity != Vector2.Zero)
                {
                    return Vector2.Normalize(CurrentVelocity);
                }
                return Vector2.Zero;
            }
        }

        public Vector2 CurrentVelocity { get; set; }
        public Color CarColor { get; set; }
        public Vector2 TargetVelocity { get; set; }

        private PrimitiveBatch BatchFor2DDrawing
        {
            get
            {
                if (_batch == null)
                {
                    _batch = new PrimitiveBatch(Game.GraphicsDevice);
                }
                return _batch;
            }
        }

        private bool Tag { get; set; }

        private Vector2[] VehicleVertexBuffer
        {
            get
            {
                if (_vehicleVertexBuffer == null)
                {
                    _vehicleVertexBuffer = new[]
                                               {
                                                   new Vector2(-1.0f, 0.6f),
                                                   new Vector2(1.0f, 0.0f),
                                                   new Vector2(-1.0f, -0.6f)
                                               };
                }
                return _vehicleVertexBuffer;
            }
        }

        private float BoundingRadius
        {
            get { return Scale; }
        }

        public bool IsTagged
        {
            get { return Tag; }
        }

        private void PerformDefaultInitialization()
        {
            Rotation = 0;
            CurrentVelocity = new Vector2(0, 0);
            CarColor = Color.Red;
            MaxSpeed = 20;
            MaxForce = 150;
            Mass = 20f;
            Acceleration = Vector2.Zero;
            _wanderTarget = Vector2.Zero;
            CreatePath();
        }

        public void TagNeighbors(float radius)
        {
            foreach (Vehicle v in allVehicles)
            {
                if (v != this && v.Enabled)
                {
                    // Unset any tags
                    Tag = false;

                    Vector2 to = v.CurrentPosition - CurrentPosition;

                    //the bounding radius of the other is taken into account by adding it
                    //to the range
                    float range = radius + v.BoundingRadius;

                    //if entity within range, tag for further consideration. (working in
                    //distance-squared space to avoid sqrts)
                    if (to.LengthSquared() < range*range)
                    {
                        v.Tag = true;
                    }
                }
            }
        }

        private void CreatePath()
        {
            Path = new Path(MaxPathPoints, 100, 100, Game.GraphicsDevice.Viewport.Width - 100,
                            Game.GraphicsDevice.Viewport.Height - 100, true);
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            float timeDelta = (float) gameTime.ElapsedGameTime.TotalSeconds;
            Vector2 steerForce = Vector2.Zero;

            switch (SteeringBehavior)
            {
                case BehaviorMode.None:
                    steerForce = Vector2.Zero;
                    break;
                case BehaviorMode.Arrive:
                    steerForce = SteeringBehaviors.Arrive(this, TargetPosition, SteeringBehaviors.Deceleration.normal);
                    break;
                case BehaviorMode.Flee:
                    steerForce = SteeringBehaviors.Flee(this, TargetPosition);
                    break;
                case BehaviorMode.Flocking:
                    break;
                case BehaviorMode.Hide:
                    break;
                case BehaviorMode.Interpose:
                    break;
                case BehaviorMode.NonPenetrationConstraint:
                    break;
                case BehaviorMode.ObstacleAvoidance:
                    break;
                case BehaviorMode.OffsetPursuit:
                    TagNeighbors(50.0f);
                    steerForce = SteeringBehaviors.OffsetPursuit(this, Leader, new Vector2(20f, 20f));
                    steerForce += SteeringBehaviors.Separation(this, allVehicles)*SeparationWeight;
                    steerForce += SteeringBehaviors.Alignment(this, allVehicles)*AlignmentWeight;
                    steerForce += SteeringBehaviors.Cohesion(this, allVehicles)*CohesionWeight;

                    break;
                case BehaviorMode.PathFollowing:
                    steerForce = SteeringBehaviors.PathFollowing(this);
                    break;
                case BehaviorMode.Pursuit:
                    steerForce = SteeringBehaviors.Pursuit(this, Evader);
                    break;
                case BehaviorMode.Seek:
                    steerForce = SteeringBehaviors.Seek(this, TargetPosition);
                    break;
                case BehaviorMode.Wander:
                    steerForce = SteeringBehaviors.Wander(this);
                    break;
                default:
                    break;
            }
            UpdatePosition(timeDelta, steerForce);
            base.Update(gameTime);
        }

        private void UpdatePosition(float timeDelta, Vector2 steerForce)
        {
            if (steerForce == Vector2.Zero)
            {
                return;
            }

			timeDelta *= 8f;

            steerForce = Truncate(steerForce, MaxForce);
            Acceleration = steerForce/Mass;
			CurrentVelocity = CurrentVelocity + Acceleration * timeDelta;
            CurrentVelocity = Truncate(CurrentVelocity, MaxSpeed);
			CurrentPosition = Vector2.Add (CurrentVelocity * timeDelta, CurrentPosition);

            MakeSurePositionIsInBounds();
        }

        private void MakeSurePositionIsInBounds()
        {
            if (CurrentPosition.X > Game.GraphicsDevice.Viewport.Width)
                _currentPosition.X = 0;
            if (CurrentPosition.Y > Game.GraphicsDevice.Viewport.Height)
                _currentPosition.Y = 0;
            if (CurrentPosition.X < 0)
                _currentPosition.X = Game.GraphicsDevice.Viewport.Width;
            if (CurrentPosition.Y < 0)
                _currentPosition.Y = Game.GraphicsDevice.Viewport.Height;
        }

        private static Vector2 Truncate(Vector2 vec, float max_value)
        {
            if (vec.Length() > max_value)
            {
                return Vector2.Multiply(Vector2.Normalize(vec), max_value);
            }
            return vec;
        }

        public override void Draw(GameTime gameTime)
        {
            if (!Enabled)
            {
                return;
            }
            Vector2[] vehicleVertex = new Vector2[VehicleVertexBuffer.Length];
            Rotation = CalculateRotation();

            Matrix matrix =
                Matrix.CreateScale(Scale)*
                Matrix.CreateRotationZ(Rotation)*
                Matrix.CreateTranslation(CurrentPosition.X, CurrentPosition.Y, 0);
            Vector2.Transform(VehicleVertexBuffer, ref matrix, vehicleVertex);

            DrawVehicle(vehicleVertex);
            DrawWander();
            DrawPath();
            base.Draw(gameTime);
        }

        private void DrawVehicle(Vector2[] vehicleVertex)
        {
            BatchFor2DDrawing.Begin(PrimitiveType.LineList);
            for (int i = 0; i < vehicleVertex.Length - 1; i++)
            {
                BatchFor2DDrawing.AddVertex(vehicleVertex[i], CarColor);
                BatchFor2DDrawing.AddVertex(vehicleVertex[i + 1], CarColor);
            }
            BatchFor2DDrawing.AddVertex(vehicleVertex[vehicleVertex.Length - 1], CarColor);
            BatchFor2DDrawing.AddVertex(vehicleVertex[0], CarColor);
            BatchFor2DDrawing.End();
        }

        private void DrawPath()
        {
            if (SteeringBehavior != BehaviorMode.PathFollowing)
            {
                return;
            }

            BatchFor2DDrawing.Begin(PrimitiveType.LineList);
            List<Vector2> path = Path.GetPath();
            for (int i = 0; i < path.Count - 1; i++)
            {
                BatchFor2DDrawing.AddVertex(path[i], Color.Pink);
                BatchFor2DDrawing.AddVertex(path[i + 1], Color.Pink);
            }
            BatchFor2DDrawing.AddVertex(path[MaxPathPoints - 1], Color.Pink);
            BatchFor2DDrawing.AddVertex(path[0], Color.Pink);
            BatchFor2DDrawing.End();
        }

        private void DrawWander()
        {
            if (SteeringBehavior != BehaviorMode.Wander)
            {
                return;
            }
            Vector2 circleCenterG = new Vector2((Heading.X*WanderDistance + CurrentPosition.X),
                                                (Heading.Y*WanderDistance + CurrentPosition.Y));
            BatchFor2DDrawing.Begin(PrimitiveType.LineList);

            List<Vector2> circleVectors = CreateEllipseVectorsFor2DDrawing(circleCenterG, WanderRadius, WanderRadius, 64);
            for (int i = 0; i < circleVectors.Count - 1; i++)
            {
                BatchFor2DDrawing.AddVertex(circleVectors[i], Color.LightGreen);
                BatchFor2DDrawing.AddVertex(circleVectors[i + 1], Color.LightGreen);
            }

            Vector2 circleCenterM = new Vector2((Heading.X*WanderDistance) + CurrentPosition.X,
                                                (Heading.Y*WanderDistance) + CurrentPosition.Y);
            Vector2 pointOnCircle = new Vector2(circleCenterG.X + (_wanderTarget.X),
                                                circleCenterG.Y + (_wanderTarget.Y));

            circleVectors = CreateEllipseVectorsFor2DDrawing(pointOnCircle, 2, 2, 64);
            for (int i = 0; i < circleVectors.Count - 1; i++)
            {
                BatchFor2DDrawing.AddVertex(circleVectors[i], Color.Yellow);
                BatchFor2DDrawing.AddVertex(circleVectors[i + 1], Color.Yellow);
            }
            BatchFor2DDrawing.End();
        }

        private float CalculateRotation()
        {
            if (CurrentVelocity == Vector2.Zero)
            {
                return 0;
            }

            float angle = (float) Math.Atan(Heading.Y/Heading.X);
            if (Heading.X < 0)
            {
                angle -= MathHelper.Pi;
            }
            if (Heading.X == 0)
            {
                if (Heading.Y > 0)
                {
                    angle = MathHelper.PiOver2;
                }
                else if (Heading.Y < 0)
                {
                    angle = -MathHelper.PiOver2;
                }
                else
                {
                    angle = 0;
                }
            }

            return angle;
        }

        public List<Vector2> CreateEllipseVectorsFor2DDrawing(Vector2 position, float semimajor_axis,
                                                              float semiminor_axis, int sides)
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