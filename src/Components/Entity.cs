using System.Collections.Generic;

namespace ShooterGame
{
    public class Entity
    {
        private static List<Entity> _updateList = new List<Entity>();
        private static List<Entity> _updateListAdd = new List<Entity>();
        private static List<Entity> _updateListRemove = new List<Entity>();

        private bool _inUpdateList;
        private PositionComponent _position;
        private MovementComponent _movement;
        private DrawableComponent _drawable;
        private ControllerComponent _controller;
        private CollisionComponent _collision;
        private HealthComponent _health;
        private AttackComponent _attack;

        /// <summary>
        /// Entity constructor.
        /// </summary>
        public Entity()
        {
            _inUpdateList = false;
            QueueForUpdate();
        }

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
        /// Check for collision data.
        /// </summary>
        /// <returns>True if entity has a collision component.</returns>
        public bool HasCollision() { return _collision != null; }

        /// <summary>
        /// Check for health data.
        /// </summary>
        /// <returns>True if entity has a health component.</returns>
        public bool HasHealth() { return _health != null; }

        /// <summary>
        /// Check for attack damage data.
        /// </summary>
        /// <returns>True if entity has a attack component.</returns>
        public bool CanAttack() { return _attack != null; }

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

        /// <summary>
        /// Access health data, if it exists.
        /// </summary>
        public HealthComponent Health
        {
            set
            {
                if (_health != value)
                {
                    if (_health != null) _health.Entity = null;
                    _health = value;
                    if (_health != null) _health.Entity = this;
                }
            }
            get
            {
                return _health;
            }
        }

        /// <summary>
        /// Access attack data, if it exists.
        /// </summary>
        public AttackComponent Attack
        {
            set
            {
                if (_attack != value)
                {
                    if (_attack != null) _attack.Entity = null;
                    _attack = value;
                    if (_attack != null) _attack.Entity = this;
                }
            }
            get
            {
                return _attack;
            }
        }

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
        /// Check if this entity needs to be updated.
        /// </summary>
        public bool ShouldBeUpdated
        {
            get
            {
                return
                    (true == _movement?.Active) ||
                    (true == _controller?.Enabled) ||
                    (true == _collision?.Enabled);
            }
        }

        /// <summary>
        /// Queue this entity to be added to the update list.
        /// </summary>
        public void QueueForUpdate()
        {
            if (!_inUpdateList)
            {
                _inUpdateList = true;
                _updateListAdd.Add(this);
            }
        }

        /// <summary>
        /// Update all entities within the update list.
        /// </summary>
        public static void UpdateAll()
        {
            // Add new entities to the update list
            // No need to check if already in update list, because QueueForUpdate() guards against it
            foreach (Entity e in _updateListAdd) _updateList.Add(e);
            _updateListAdd.Clear();

            // Loop through update list
            foreach (Entity e in _updateList)
            {
                // Check if entity should continue to be updated
                if (e.ShouldBeUpdated)
                {
                    // Update entity
                    if ((e._controller != null) && e._controller.Enabled) e._controller.Update();
                    if ((e._collision != null) && e._collision.Enabled) e._collision.Update();
                    if ((e._movement != null) && e._movement.Active) e._movement.Update();
                }
                else if (e._inUpdateList)
                {
                    // Mark entity to be removed from update list
                    // If another entity re-adds this entity to the list it'll get removed and re-added (no problem)
                    e._inUpdateList = false;
                    _updateListRemove.Add(e);
                }
            }

            // Remove entities which no longer need updating from update list 
            // No need to check if already being removed from update list
            // The entity might get removed while it is queued to be re-added, but that is fine
            foreach (Entity e in _updateListRemove) _updateList.Remove(e);
            _updateListRemove.Clear();

            // List number of entities in the update list
            // This is for debug only (can safely be removed)
            SwinGameSDK.SwinGame.DrawText("Entities in update list: " + _updateList.Count, SwinGameSDK.Color.Black, 0, 20);
        }
    }
}
