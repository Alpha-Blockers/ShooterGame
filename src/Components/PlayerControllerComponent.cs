using SwinGameSDK;
using static SwinGameSDK.SwinGame;

namespace ShooterGame
{
    class PlayerControllerComponent : ControllerComponent
    {
        const int MAX_SPEED = 5;

        /// <summary>
        /// Player controller component constructor.
        /// </summary>
        public PlayerControllerComponent()
        {
            ControllerActive = true;
        }

        /// <summary>
        /// Check for input from the player
        /// </summary>
        public override void Update()
        {
            // Check for fire command
            if (KeyDown(KeyCode.SpaceKey))
            {
                PositionComponent p = Parent.Position;
                MovementComponent m = Parent.Movement;
                if ((p != null) && (m != null))
                {
                    new Entity
                    {
                        Position = new PositionComponent(p.ParentChunk.Parent, p.X, p.Y),
                        Drawable = new DrawableComponent(Color.Red, 2),
                        Movement = new MovementComponent { SpeedX = m.SpeedX * 2, SpeedY = m.SpeedY * 2 },
                        Controller = new BulletControllerComponent()
                    };
                }
            }
            
            // Check for up-movement command
            if (SwinGame.KeyDown(KeyCode.WKey) || SwinGame.KeyDown(KeyCode.UpKey)) {
                MovementComponent m = Parent.Movement;
                if (m != null)
                    if (m.SpeedY > -MAX_SPEED)
                        m.SpeedY = m.SpeedY - 1;
            }
            else if (SwinGame.KeyDown(KeyCode.SKey) || SwinGame.KeyDown(KeyCode.DownKey)) {
                MovementComponent m = Parent.Movement;
                if (m != null)
                    if (m.SpeedY < MAX_SPEED)
                        m.SpeedY = m.SpeedY + 1;
            }
            else
            {
                MovementComponent m = Parent.Movement;
                if (m != null)
                {
                    if (m.SpeedY > 0)
                        m.SpeedY = m.SpeedY - 1;
                    else if (m.SpeedY < 0)
                        m.SpeedY = m.SpeedY + 1;
                }
            }

            if (SwinGame.KeyDown(KeyCode.AKey) || SwinGame.KeyDown(KeyCode.LeftKey)) {
                MovementComponent m = Parent.Movement;
                if (m != null)
                    if (m.SpeedX > -MAX_SPEED)
                        m.SpeedX = m.SpeedX - 1;
            }
            else if (SwinGame.KeyDown(KeyCode.DKey) || SwinGame.KeyDown(KeyCode.RightKey)) {
                MovementComponent m = Parent.Movement;
                if (m != null)
                    if (m.SpeedX < MAX_SPEED)
                        m.SpeedX = m.SpeedX + 1;
            }
            else
            {
                MovementComponent m = Parent.Movement;
                if (m != null)
                {
                    if (m.SpeedX > 0)
                        m.SpeedX = m.SpeedX - 1;
                    else if (m.SpeedX < 0)
                        m.SpeedX = m.SpeedX + 1;
                }
            }
        }
    }
}
