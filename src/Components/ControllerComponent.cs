using SwinGameSDK;

namespace ShooterGame
{
    abstract class ControllerComponent : IComponent, IUpdate
    {
        private Entity _parent;
        private bool _controllerActive;

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
                    // Record the old value of ShouldUpdate
                    bool shouldUpdatePrevious = ShouldUpdate;

                    // Update the old parent, if any
                    if (_parent != null) _parent.Controller = null;

                    // Update value
                    _parent = value;

                    // Update the new parent, if any
                    if (_parent != null) _parent.Controller = this;

                    // Check if class needs to start being updated
                    if (!shouldUpdatePrevious && ShouldUpdate)
                        UpdateController.Add(this);
                }
            }
        }

        /// <summary>
        /// Get or set the active-status of this controller.
        /// </summary>
        public bool ControllerActive
        {
            get => _controllerActive;
            set
            {
                if (_controllerActive != value)
                {
                    _controllerActive = value;
                    if (_controllerActive)
                        UpdateController.Add(this);
                }
            }
        }

        /// <summary>
        /// Check if this class should continue to be updated.
        /// </summary>
        public bool ShouldUpdate { get { return _controllerActive && (Parent != null); } }

        /// <summary>
        /// Runs whatever updates the controller needs to perform every game tick.
        /// </summary>
        public abstract void Update();
    }
}
