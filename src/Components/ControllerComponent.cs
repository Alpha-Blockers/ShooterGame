using SwinGameSDK;

namespace ShooterGame
{
    abstract class ControllerComponent : IComponent
    {
        private Entity _entity;
        private bool _enabled;

        /// <summary>
        /// Get or set parent entity.
        /// </summary>
        public Entity Entity
        {
            get => _entity;
            set
            {
                if (_entity != value)
                {
                    _entity = value;
                    if (_enabled) _entity?.QueueForUpdate();
                }
            }
        }

        /// <summary>
        /// Get or set the enabled status of this collision component.
        /// </summary>
        public bool Enabled
        {
            get => _enabled;
            set
            {
                if (_enabled != value)
                {
                    _enabled = value;
                    if (_enabled) Entity?.QueueForUpdate();
                }
            }
        }

        /// <summary>
        /// Runs whatever updates the controller needs to perform every game tick.
        /// </summary>
        public abstract void Update();

        /// <summary>
        /// Clear all component data and attempt to unlink from any external data.
        /// This method should only be called by the parent entity.
        /// </summary>
        public void InternalDestroy()
        {
            _enabled = false;
        }
    }
}
