using SwinGameSDK;

namespace ShooterGame
{
    class DrawableComponent : IComponent
    {
        private Entity _parent;
        private Color _color;
        private int _size;

        /// <summary>
        /// Drawable component constructor.
        /// </summary>
        /// <param name="color">Colour of the component when drawn.</param>
        /// <param name="size">Size of the component when drawn.</param>
        public DrawableComponent(Color color, int size)
        {
            _color = color;
            _size = size;
        }

        /// <summary>
        /// Get or set the colour of the drawable component when drawn.
        /// </summary>
        public Color Color
        {
            get => _color;
            set { _color = value; }
        }

        /// <summary>
        /// Get or set the size of the drawable component when drawn.
        /// </summary>
        public int Size
        {
            get => _size;
            set { _size = value; }
        }

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
                SwinGame.FillCircle(_color, p.X, p.Y, _size);
            }
        }

        /// <summary>
        /// Clear all component data and attempt to unlink from any external data.
        /// </summary>
        public void Destroy()
        {
            // Nothing to do here
        }
    }
}
