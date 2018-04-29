using SwinGameSDK;

namespace ShooterGame
{
    class DrawableComponent : IComponent
    {

        private Entity _parent;

        /// <summary>
        /// Get or set parent entity.
        /// </summary>
        public Entity Parent
        {
            get => _parent;
            set
            {
                if (_parent != value)
                {
                    if (_parent != null) _parent.Drawable = null;
                    _parent = value;
                    if (_parent != null) _parent.Drawable = this;
                }
            }
        }

        /// <summary>
        /// Draw entity.
        /// </summary>
        public void Draw()
        {
            PositionComponent p = Parent?.Position;
            if (p != null)
            {
                SwinGame.FillCircle(Color.Blue, p.X, p.Y, 10);
            }
        }
    }
}
