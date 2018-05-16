using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShooterGame
{
    class HealthComponet : IComponent
    {
        private int _health;
        private Entity _entity;

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
                    if (_entity != null) _entity.Health = null;
                    _entity = value;
                    if (_entity != null) _entity.Health = this;
                }
            }
        }

        /// <summary>
        ///  get or set HEalth Value
        /// </summary>
        public int Health
        {
            get => _health;
            set { _health = value; }
        }


        /// <summary>
        /// Health component cunstructer
        /// </summary>
        /// <param name="health">initial health value(optional)</param>
        public HealthComponet(int health = 100)
        {
            _health = health;
        }

        /// <summary>
        ///  Clear all component data and attempt to unlink from any external data.
        /// </summary>
        public void Destroy()
        {
            //empyty
        }

    }
}
