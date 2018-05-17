using SwinGameSDK;

namespace ShooterGame
{
    /// <summary>
    /// A tick-rate controller is used to control the tick rate of the game.
    /// </summary>
    public class TickRateController
    {
        private const uint TICK_REBASE_LIMIT = 2000; // Measured in milliseconds

        private readonly uint _tickDuration;
        private Timer _timer;
        private uint _nextTick;

        /// <summary>
        /// Tick-rate controller constructor.
        /// </summary>
        /// <param name="ticksPerSecond">Target number of ticks per second.</param>
        public TickRateController(uint ticksPerSecond)
        {
            _tickDuration = 1000 / ticksPerSecond;
            _timer = new Timer();
            _nextTick = _timer.Ticks + _tickDuration;
        }

        /// <summary>
        /// Pause tick rate controller.
        /// </summary>
        public void Pause()
        {
            _timer.Pause();
        }

        /// <summary>
        /// Resume tick rate controller, after having been paused.
        /// </summary>
        public void Resume()
        {
            _timer.Resume();
        }

        /// <summary>
        /// Reset controller.
        /// </summary>
        public void Reset()
        {
            _timer.Stop();
            _timer.Reset();
            _timer.Start();
            _nextTick = _timer.Ticks + _tickDuration;
        }

        /// <summary>
        /// Check if a game tick should be run.
        /// </summary>
        /// <returns>True if time for game to tick.</returns>
        public bool Ticked()
        {
            if (_nextTick <= _timer.Ticks)
            {
                _nextTick += _tickDuration;
                if (_nextTick > TICK_REBASE_LIMIT) RebaseTimer();
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Wait here until next game tick is due.
        /// </summary>
        public void Wait()
        {
            // Record the current time to make sure it doesn't change
            uint current = _timer.Ticks;

            // Check method should wait
            if (_nextTick > current)
                SwinGame.Delay(_nextTick - current);

            // Update next tick time
            _nextTick += _tickDuration;

            // Check if timer needs to be rebased
            if (_nextTick > TICK_REBASE_LIMIT) RebaseTimer();
        }

        /// <summary>
        /// Quietly reset the internal timer but keep the timer running.
        /// </summary>
        private void RebaseTimer()
        {
            uint currentTime = _timer.Ticks;
            if (currentTime < _nextTick)
            {
                _nextTick -= currentTime;
                _timer.Reset();
            }
        }
    }
}
