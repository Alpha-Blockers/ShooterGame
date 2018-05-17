using SwinGameSDK;
using static SwinGameSDK.SwinGame;

namespace ShooterGame
{
    public class PlayerControllerComponent : ControllerComponent
    {
        const int MAX_SPEED = 5;

        private int _shootCooldown;

        /// <summary>
        /// Player controller component constructor.
        /// </summary>
        public PlayerControllerComponent()
        {
            Enabled = true;
            _shootCooldown = 0;
        }

        /// <summary>
        /// Check for input from the player
        /// </summary>
        public override void Update()
        {
            // Get required components
            PositionComponent position = Entity.Position;
            MovementComponent movement = Entity.Movement;
            if ((position == null) || (movement == null))
                throw new System.FormatException("Entities with a player controller component must also have a position and movement component");

            // Check shoot cooldown
            if (_shootCooldown > 0)
            {
                _shootCooldown -= 1;
            }
            else if (KeyDown(KeyCode.SpaceKey))
            {
                // Update shoot cooldown
                _shootCooldown = 3;

                // Get offset of mouse from current position
                float offsetX = MouseX() - position.X + CameraX();
                float offsetY = MouseY() - position.Y + CameraY();
                double offsetMultiplier = 8.0 / System.Math.Sqrt((offsetX * offsetX) + (offsetY * offsetY));

                // Get shoot direction
                int shootX = (int)(offsetX * offsetMultiplier) + movement.X;
                int shootY = (int)(offsetY * offsetMultiplier) + movement.Y;

                // Check if shoot direction is non-zero
                if ((shootX != 0) || (shootY != 0))
                {
                    // Create new bullet entity
                    new Entity
                    {
                        Position = new PositionComponent(position.Chunk.Map, position.X, position.Y),
                        Drawable = new DrawableComponent(Color.Red, 2),
                        Movement = new MovementComponent { X = shootX, Y = shootY },
                        Controller = new BulletControllerComponent(),
                        Collision = new CollisionComponent(2),
                        Attack = new AttackComponent(1)
                    };
                }
            }

            // Check for horizontal movement commands
            int x = 0;
            if (KeyDown(KeyCode.AKey) || KeyDown(KeyCode.LeftKey)) x -= 1;
            if (KeyDown(KeyCode.DKey) || KeyDown(KeyCode.RightKey)) x += 1;

            // Check for vertical movement commands
            int y = 0;
            if (KeyDown(KeyCode.WKey) || KeyDown(KeyCode.UpKey)) y -= 1;
            if (KeyDown(KeyCode.SKey) || KeyDown(KeyCode.DownKey)) y += 1;

            // Update horizontal movement
            if (x > 0)
            {
                // Speed up in positive direction if not already at max speed
                if (movement.X < MAX_SPEED)
                    movement.X = movement.X + 1;
            }
            else if (x < 0)
            {
                // Speed up in negative direction if not already at negative max speed
                if (movement.X > -MAX_SPEED)
                    movement.X = movement.X - 1;
            }
            else
            {
                // Slow down movement regardless of direction
                if (movement.X > 0)
                    movement.X = movement.X - 1;
                else if (movement.X < 0)
                    movement.X = movement.X + 1;
            }

            // Update vertical movement
            if (y > 0)
            {
                // Speed up in positive direction if not already at max speed
                if (movement.Y < MAX_SPEED)
                    movement.Y = movement.Y + 1;
            }
            else if (y < 0)
            {
                // Speed up in negative direction if not already at negative max speed
                if (movement.Y > -MAX_SPEED)
                    movement.Y = movement.Y - 1;
            }
            else
            {
                // Slow down movement regardless of direction
                if (movement.Y > 0)
                    movement.Y = movement.Y - 1;
                else if (movement.Y < 0)
                    movement.Y = movement.Y + 1;
            }
        }
    }
}
