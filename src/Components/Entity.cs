using System.Collections.Generic;

namespace ShooterGame
{
    class Entity
    {
        private PositionComponent _position;
        private MovementComponent _movement;
        private DrawableComponent _drawable;
        private ControllerComponent _controller;
        private CollisionComponent _collision;

        /// <summary>
        /// Check for position.
        /// </summary>
        /// <returns>True if entity has a position component.</returns>
        public bool HasPosition() { return _position != null; }

        /// <summary>
        /// Check for movement.
        /// </summary>
        /// <returns>True if entity has a movement component.</returns>
        public bool HasMovement() { return _movement != null; }

        /// <summary>
        /// Check for controller.
        /// </summary>
        /// <returns>True if entity has a controller component.</returns>
        public bool HasController() { return _controller != null; }

        /// <summary>
        /// Check if entity is drawable.
        /// </summary>
        /// <returns>True if entity has drawable component.</returns>
        public bool IsDrawable() { return _drawable != null; }

        /// <summary>
        /// Check for for collision data.
        /// </summary>
        /// <returns>True if entity has a collision component.</returns>
        public bool HasCollision() { return _collision != null; }

        /// <summary>
        /// Clear all entity data.
        /// </summary>
        public void Destroy()
        {
            _position?.Destroy();
            _movement?.Destroy();
            _drawable?.Destroy();
            _controller?.Destroy();
            _collision?.Destroy();
        }

        /// <summary>
        /// Access position data, if it exists.
        /// </summary>
        public PositionComponent Position
        {
            set
            {
                if (_position != value)
                {
                    if (_position != null) _position.Entity = null;
                    _position = value;
                    if (_position != null) _position.Entity = this;
                }
            }
            get
            {
                return _position;
            }
        }

        /// <summary>
        /// Access movement data, if it exists.
        /// </summary>
        public MovementComponent Movement
        {
            set
            {
                if (_movement != value)
                {
                    if (_movement != null) _movement.Entity = null;
                    _movement = value;
                    if (_movement != null) _movement.Entity = this;
                }
            }
            get
            {
                return _movement;
            }
        }

        /// <summary>
        /// Access drawable data, if it exists.
        /// </summary>
        public DrawableComponent Drawable
        {
            set
            {
                if (_drawable != value)
                {
                    if (_drawable != null) _drawable.Entity = null;
                    _drawable = value;
                    if (_drawable != null) _drawable.Entity = this;
                }
            }
            get
            {
                return _drawable;
            }
        }

        /// <summary>
        /// Access controller data, if it exists.
        /// </summary>
        public ControllerComponent Controller
        {
            set
            {
                if (_controller != value)
                {
                    if (_controller != null) _controller.Entity = null;
                    _controller = value;
                    if (_controller != null) _controller.Entity = this;
                }
            }
            get
            {
                return _controller;
            }
        }

        /// <summary>
        /// Access controller data, if it exists.
        /// </summary>
        public CollisionComponent Collision
        {
            set
            {
                if (_collision != value)
                {
                    if (_collision != null) _collision.Entity = null;
                    _collision = value;
                    if (_collision != null) _collision.Entity = this;
                }
            }
            get
            {
                return _collision;
            }
        }
    }
}
