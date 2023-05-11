using System;
using UnityEngine;

namespace Runtime
{
    [Serializable]
    public class HealthSystem
    {
        [Header("Health System - Data")] 
        public float healthAmount;
        public float currentHealth;

        public HealthSystem(float healthAmount)
        {
            this.healthAmount = healthAmount;
            currentHealth = this.healthAmount;
        }

        public float TakeDamage(float damageValue)
        {
            if (currentHealth <= 0) return 0;

            float healthTemp = currentHealth - damageValue;
            
            Debug.Log(currentHealth);

            currentHealth = healthTemp > 0 ? healthTemp : 0;

            if (currentHealth == 0) Death();

            return currentHealth;

        }

        public void Death()
        {
            Debug.Log("Pawn is dead");
        }
    }
}
