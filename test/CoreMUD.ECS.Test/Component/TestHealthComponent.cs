using System;

namespace CoreMUD.ECS.Test.Component
{
    internal class TestHealthComponent : IComponent
    {
        private double _health;

        /// <summary>
        /// Constructs new <see cref="TestHealthComponent"/> with
        /// default health value.
        /// </summary>

        public TestHealthComponent(): this(100f) { }


        /// <summary>
        /// Constructs new <see cref="TestHealthComponent"/> with
        /// specified <param name="health"/> value.
        /// </summary>

        public TestHealthComponent(double health)
        {
            MaxHealth = health;
            _health = health;
        }

        /// <summary>The entity's current health</summary>

        public double Health
        {
            get
            {
                return _health;
            }
            set
            {
                _health = Math.Min(MaxHealth, Math.Max(0, value));
            }
        }

        /// <summary>The entity's maximum health</summary>

        public double MaxHealth { get; set; }

        /// <summary>
        /// Gets the percentage of the entity's current 
        /// health to it's initial health.
        /// </summary>

        public double Percentage
        {
            get
            {
                return Math.Round(_health / MaxHealth * 100.0);
            }
        }

        /// <summary>
        /// True if the entity is alive; Otherwise, false.
        /// </summary>

        public bool IsAlive
        {
            get
            {
                return _health > 0;
            }
        }
    }
}
