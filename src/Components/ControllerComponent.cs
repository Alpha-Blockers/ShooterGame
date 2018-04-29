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
                    if (_parent != null) _parent.Controller = null;
                    _parent = value;
                    if (_parent != null) _parent.Controller = this;
                    UpdateManager.Changed(this);
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
                    UpdateManager.Changed(this);
                }
            }
        }

        /// <summary>
        /// Check if this class should be updated.
        /// </summary>
        public bool ShouldUpdate { get { return _controllerActive && (Parent != null); } }

        /// <summary>
        /// Runs whatever updates the controller needs to perform every game tick.
        /// </summary>
        public abstract void Update();
    }
}
