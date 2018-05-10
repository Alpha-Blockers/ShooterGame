using SwinGameSDK;
using static SwinGameSDK.SwinGame;

namespace ShooterGame
{
    class BulletControllerComponent : ControllerComponent
    {
        private int _life;

        /// <summary>
        /// Player controller component constructor.
        /// </summary>
        public BulletControllerComponent()
        {
            ControllerActive = true;
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
                Parent.Destroy();
            }
        }
    }
}
