using System;
using System.Collections.Generic;
using Runtime.ArmorSystem;
using UnityEngine;

namespace Runtime
{
    [Serializable]
    public class HealthSystem
    {
        [Header("Health System - Data")] 
        public float healthAmount;
        public float currentHealth;

        public float armorAmount = 0;
        public float currentArmor = 0;

        public HealthSystem(float healthAmount)
        {
            this.healthAmount = healthAmount;
            currentHealth = this.healthAmount;
        }
        
        public HealthSystem(float healthAmount, ArmorData armorA)
        {
            this.healthAmount = healthAmount;
            currentHealth = this.healthAmount;
            
            armorAmount = armorA.armorAmount;
            currentArmor = armorAmount;
        }
        
        public HealthSystem(float healthAmount, ArmorData armorA, ArmorData armorB)
        {
            this.healthAmount = healthAmount;
            currentHealth = this.healthAmount;
            
            armorAmount = armorA.armorAmount + armorB.armorAmount;
            currentArmor = armorAmount;
        }

        public float TakeDamage(float damageValue)
        {
            if (currentHealth <= 0) return 0;

            // Apply damage to armor first, if the armor is still there
            if (currentArmor > 0)
            {
                float newArmorValue = currentArmor - damageValue;
                
                // The armor takes all the damage
                if (newArmorValue >= 0)
                {
                    currentArmor = newArmorValue;
                    return currentHealth;
                }
                
                // The armor takes part of the damage (and break)
                currentArmor = 0;
                TakeDamage(-newArmorValue);
                return currentHealth;
            }

            float healthTemp = currentHealth - damageValue;

            currentHealth = healthTemp > 0 ? healthTemp : 0;

            if (currentHealth == 0) Death();

            return currentHealth;

        }

        public float GetHealthInPercentage()
        {
            return currentHealth / healthAmount;
        }

        public float GetArmorInPercentage()
        {
            return armorAmount == 0 ? 0 : currentArmor / armorAmount;
        }

        public void Death()
        {
            Debug.Log("Pawn is dead");
        }
    }
}

