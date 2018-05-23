using System;
using SwinGameSDK;
using static SwinGameSDK.SwinGame;

namespace ShooterGame
{
    public class InGameMenu : Menu
    {
        private static InGameMenu _currentGame = null;

        private Entity _playerEntity;
        private Point2D _point;
        private Map _map;
        private Random _random;
        private int _enemySpawnCounter;
        private int _score;

        /// <summary>
        /// Get the current in-game menu.
        /// </summary>
        public static InGameMenu CurrentGame { get => _currentGame; }

        /// <summary>
        /// Access score value.
        /// </summary>
        public int Score
        {
            get => _score;
            set { _score = value; }
        }

        /// <summary>
        /// Game menu controller.
        /// </summary>
        public InGameMenu()
        {
            // Set the current in-game menu
            _currentGame = this;

            // Create map
            _map = new Map(4, 3);

            // Create player
            _playerEntity = new Entity
            {
                Position = new PositionComponent(_map, 100, 100),
                Drawable = new DrawableComponent(Color.Blue, 12),
                Movement = new MovementComponent(),
                Controller = new PlayerControllerComponent(),
                Collision = new CollisionComponent(12)
            };

            // Create or setup other values
            _point = new Point2D();
            _random = new Random();
            _enemySpawnCounter = 0;
            _score = 0;
        }

        /// <summary>
        /// Check for user input and run any other updates.
        /// </summary>
        public override void Update()
        {
            // Spawn enemies
            if (_enemySpawnCounter > 0)
            {
                _enemySpawnCounter -= 1;
            }
            else
            {
                _enemySpawnCounter = 60;
                SpawnEnemy();
            }

            // Update message log
            MessageLog.Current?.Update();
        }

        /// <summary>
        /// Draw the menu to the screen.
        /// </summary>
        public override void Draw()
        {
            // Set camera before drawing world
            // Also set before running update in case update methods use camera position
            PositionComponent position = _playerEntity.Position;
            if (position != null)
            {
                _point.X = position.X - (ScreenWidth() / 2);
                _point.Y = position.Y - (ScreenHeight() / 2);
                SetCameraPos(_point);
            }

            // Update entities
            // Placed here instead of in Update() so that proper camera control is setup
            Entity.UpdateAll();

            // Draw world
            _map.Draw();

            // Reset camera before drawing interface
            _point.X = 0;
            _point.Y = 0;
            SetCameraPos(_point);

            // Draw interface components
            DrawText("Score: " + _score, Color.Black, 2, 15);
            MessageLog.Current?.Draw();
        }

        /// <summary>
        /// Spawn a new enemy at a random location.
        /// </summary>
        /// <returns>The enemy entity created.</returns>
        public Entity SpawnEnemy()
        {
            // Get random position to spawn enemy
            int px = (2 * Tile.Width) + (int)(_random.NextDouble() * (_map.Width - (4 * Tile.Width)));
            int py = (2 * Tile.Height) + (int)(_random.NextDouble() * (_map.Height - (4 * Tile.Height)));

            // Get random motion to give enemy
            int mx = (int)(_random.NextDouble() * 4) - 2;
            int my = (int)(_random.NextDouble() * 4) - 2;

            // Create and return new enemy
            return new Entity
            {
                Position = new PositionComponent(_map, px, py),
                Movement = new MovementComponent(mx, my),
                Drawable = new DrawableComponent(Color.Red, 12),
                Collision = new CollisionComponent(8),
                Health = new HealthComponent(1)
            };
        }
    }
}
