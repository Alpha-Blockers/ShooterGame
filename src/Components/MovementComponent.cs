
namespace ShooterGame
{
    class MovementComponent : IComponent, IUpdate
    {
        private Entity _parent;
        private int _speedX;
        private int _speedY;
        
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
                    if (_parent != null) _parent.Movement = null;
                    _parent = value;
                    if (_parent != null) _parent.Movement = this;
                }
            }
        }

        /// <summary>
        /// Get or set speed in x-direction
        /// </summary>
        public int SpeedX
        {
            get => _speedX;
            set
            {
                bool shouldUpdatePrevious = ShouldUpdate;
                _speedX = value;
                if (!shouldUpdatePrevious && ShouldUpdate) UpdateController.Add(this);
            }
        }

        /// <summary>
        /// Get or set speed in y-direction.
        /// </summary>
        public int SpeedY
        {
            get => _speedY;
            set
            {
                bool shouldUpdatePrevious = ShouldUpdate;
                _speedY = value;
                if (!shouldUpdatePrevious && ShouldUpdate) UpdateController.Add(this);
            }
        }

        /// <summary>
        /// Check if this class should be updated.
        /// </summary>
        public bool ShouldUpdate
        {
            get
            {
                return (_speedX != 0) || (_speedY != 0);
            }
        }

        /// <summary>
        /// Update movement data.
        /// </summary>
        public void Update()
        {
            PositionComponent p = Parent?.Position;
            if (p != null)
            {
                p.X = p.X + _speedX;
                p.Y = p.Y + _speedY;
            }
        }

        /// <summary>
        /// Clear all component data and attempt to unlink from any external data.
        /// </summary>
        public void Destroy()
        {
            _speedX = 0;
            _speedY = 0;
        }
    }
}
