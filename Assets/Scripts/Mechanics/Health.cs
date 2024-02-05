using System;
using Platformer.Gameplay;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Platformer.Core.Simulation;

namespace Platformer.Mechanics
{
    /// <summary>
    /// Represebts the current vital statistics of some game entity.
    /// </summary>
    public class Health : MonoBehaviour
    {
        /// <summary>
        /// The maximum hit points for the entity.
        /// </summary>
        [Range(0, 5)]
        public int maxHP = 3;
        public TextMeshProUGUI healthText;
        public Slider healthSlider;
        public ParticleSystem deathParticleSystem;
        /// <summary>
        /// Indicates if the entity should be considered 'alive'.
        /// </summary>
        public bool IsAlive => currentHP > 0;

        int currentHP;

        /// <summary>
        /// Increment the HP of the entity.
        /// </summary>
        public void Increment()
        {
            currentHP = Mathf.Clamp(currentHP + 1, 0, maxHP);

            SetHealthToText();
        }

        private void Update()
        {
            healthSlider.value = currentHP;
        }

        /// <summary>
        /// Decrement the HP of the entity. Will trigger a HealthIsZero event when
        /// current HP reaches 0.
        /// </summary>
        public void Decrement()
        {
            currentHP = Mathf.Clamp(currentHP - 1, 0, maxHP);

            SetHealthToText();

            if (currentHP == 0)
            {
                var ev = Schedule<HealthIsZero>();
                ev.health = this;

                deathParticleSystem.Play();
            }
        }

        public void Reset()
        {
            currentHP = maxHP;
            SetHealthToText();
        }

        /// <summary>
        /// Decrement the HP of the entitiy until HP reaches 0.
        /// </summary>
        public void Die()
        {
            currentHP = 0;
        }

        void Awake()
        {
            currentHP = maxHP;

            SetHealthToText();
        }

        private void SetHealthToText()
        {
            // Update the UI text
            healthText.text = currentHP.ToString();
        }
    }
}
