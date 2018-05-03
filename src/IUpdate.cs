
namespace ShooterGame
{
    interface IUpdate
    {
        /// <summary>
        /// Check if this class should continue to be updated.
        /// </summary>
        bool ShouldUpdate { get; }

        /// <summary>
        /// Run any updates for this class.
        /// </summary>
        void Update();
    }
}
