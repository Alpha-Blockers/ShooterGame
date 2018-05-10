using SwinGameSDK;
using static SwinGameSDK.SwinGame;

namespace ShooterGame
{
    class PlayerControllerComponent : ControllerComponent
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
            PositionComponent p = Entity.Position;
            MovementComponent m = Entity.Movement;
            if ((p == null) || (m == null))
            {
                Enabled = false;
                return;
            }

            // Check for fire command
            if (_shootCooldown > 0)
            {
                _shootCooldown -= 1;
            }
            else if (KeyDown(KeyCode.SpaceKey))
            {
                _shootCooldown = 3;
                new Entity
                {
                    Position = new PositionComponent(p.ParentChunk.Map, p.X, p.Y),
                    Drawable = new DrawableComponent(Color.Red, 2),
                    Movement = new MovementComponent { SpeedX = m.SpeedX * 2, SpeedY = m.SpeedY * 2 },
                    Controller = new BulletControllerComponent(),
                    Collision = new CollisionComponent(2)
                };
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
                if (m.SpeedX < MAX_SPEED)
                    m.SpeedX = m.SpeedX + 1;
            }
            else if (x < 0)
            {
                // Speed up in negative direction if not already at negative max speed
                if (m.SpeedX > -MAX_SPEED)
                    m.SpeedX = m.SpeedX - 1;
            }
            else
            {
                // Slow down movement regardless of direction
                if (m.SpeedX > 0)
                    m.SpeedX = m.SpeedX - 1;
                else if (m.SpeedX < 0)
                    m.SpeedX = m.SpeedX + 1;
            }

            // Update vertical movement
            if (y > 0)
            {
                // Speed up in positive direction if not already at max speed
                if (m.SpeedY < MAX_SPEED)
                    m.SpeedY = m.SpeedY + 1;
            }
            else if (y < 0)
            {
                // Speed up in negative direction if not already at negative max speed
                if (m.SpeedY > -MAX_SPEED)
                    m.SpeedY = m.SpeedY - 1;
            }
            else
            {
                // Slow down movement regardless of direction
                if (m.SpeedY > 0)
                    m.SpeedY = m.SpeedY - 1;
                else if (m.SpeedY < 0)
                    m.SpeedY = m.SpeedY + 1;
            }
        }
    }
}
