using System.Collections.Generic;

namespace ShooterGame
{
    public class Entity
    {
        private static List<Entity> _updateList = new List<Entity>();
        private static List<Entity> _updateListAdd = new List<Entity>();
        private static List<Entity> _updateListRemove = new List<Entity>();
        private static List<Entity> _destroyList = new List<Entity>();

        private bool _inUpdateList;
        private bool _destroyed;
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
        /// Check if this entity has been destroyed.
        /// </summary>
        public bool Destroyed { get => _destroyed; }

        /// <summary>
        /// Check if entity has position data.
        /// </summary>
        public bool HasPosition { get { return _position != null; } }

        /// <summary>
        /// Check if entity has movement data.
        /// </summary>
        public bool HasMovement { get { return _movement != null; } }

        /// <summary>
        /// Check if entity has a controller.
        /// </summary>
        public bool HasController { get { return _controller != null; } }

        /// <summary>
        /// Check if entity is drawable.
        /// </summary>
        public bool IsDrawable { get { return _drawable != null; } }

        /// <summary>
        /// Check if entity has collision data.
        /// </summary>
        public bool HasCollision { get { return _collision != null; } }

        /// <summary>
        /// Check if entity has health data.
        /// </summary>
        public bool HasHealth { get { return _health != null; } }

        /// <summary>
        /// Check if entity has attack data.
        /// </summary>
        public bool HasAttack { get { return _attack != null; } }

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
        /// Clear all entity data. This method is called from within the update-all method.
        /// </summary>
        private void InternalDestroy()
        {
            _position?.InternalDestroy();
            _movement?.InternalDestroy();
            _drawable?.InternalDestroy();
            _controller?.InternalDestroy();
            _collision?.InternalDestroy();
        }

        /// <summary>
        /// Clear all entity data.
        /// </summary>
        public void Destroy()
        {
            if (!_destroyed)
            {
                _destroyed = true;
                _destroyList.Add(this);
            }
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

            // Loop through destroy-list and call destroy for each entity listed
            foreach (Entity e in _destroyList) e.InternalDestroy();
            _destroyList.Clear();

            // List number of entities in the update list
            // This is for debug only (can safely be removed)
            SwinGameSDK.SwinGame.DrawText("Entities in update list: " + _updateList.Count, SwinGameSDK.Color.Black, 0, 20);
        }
    }
}
