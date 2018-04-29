using System.Collections.Generic;

namespace ShooterGame
{
    class Entity
    {
        private PositionComponent _position;
        private MovementComponent _movement;
        private DrawableComponent _drawable;
        private ControllerComponent _controller;

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
        /// Access position data, if it exists.
        /// </summary>
        public PositionComponent Position
        {
            set
            {
                if (_position != value)
                {
                    if (_position != null) _position.Parent = null;
                    _position = value;
                    if (_position != null) _position.Parent = this;
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
                    if (_movement != null) _movement.Parent = null;
                    _movement = value;
                    if (_movement != null) _movement.Parent = this;
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
                    if (_drawable != null) _drawable.Parent = null;
                    _drawable = value;
                    if (_drawable != null) _drawable.Parent = this;
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
                    if (_controller != null) _controller.Parent = null;
                    _controller = value;
                    if (_controller != null) _controller.Parent = this;
                }
            }
            get
            {
                return _controller;
            }
        }
    }
}
