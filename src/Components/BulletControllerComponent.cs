using SwinGameSDK;
using static SwinGameSDK.SwinGame;

namespace ShooterGame
{
    public class BulletControllerComponent : ControllerComponent
    {
        private int _life;

        /// <summary>
        /// Player controller component constructor.
        /// </summary>
        public BulletControllerComponent()
        {
            Enabled = true;
            _life = 60;
        }

        /// <summary>
        /// Check for input from the player
        /// </summary>
        public override void Update()
        {
            if (_life > 0)
            {
                _life -= 1;
            }
            else
            {
                Entity.Destroy();
            }
        }
    }
}
