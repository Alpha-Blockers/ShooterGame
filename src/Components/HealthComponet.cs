using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShooterGame
{
    class HealthComponet
    {
        public int _health;


        public int Health
        {
            get => _health;
            set { _health = value; }
        }

        public HealthComponet(int health = 100)
        {
            _health = health;
        }

    }
}
