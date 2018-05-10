using SwinGameSDK;

namespace ShooterGame
{
    abstract class ControllerComponent : IComponent, IUpdate
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
                    // Record the old value of ShouldUpdate
                    bool shouldUpdatePrevious = ShouldUpdate;

                    // Update the old parent, if any
                    if (_entity != null) _entity.Controller = null;

                    // Update value
                    _entity = value;

                    // Update the new parent, if any
                    if (_entity != null) _entity.Controller = this;

                    // Check if class needs to start being updated
                    if (!shouldUpdatePrevious && ShouldUpdate)
                        UpdateController.Add(this);
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
                    if (_enabled) UpdateController.Add(this);
                }
            }
        }

        /// <summary>
        /// Check if this class should continue to be updated.
        /// </summary>
        public bool ShouldUpdate { get { return _enabled && (Entity != null); } }

        /// <summary>
        /// Runs whatever updates the controller needs to perform every game tick.
        /// </summary>
        public abstract void Update();

        /// <summary>
        /// Clear all component data and attempt to unlink from any external data.
        /// </summary>
        public void Destroy()
        {
            Enabled = false;
        }
    }
}
