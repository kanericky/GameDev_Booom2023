using System;
using UnityEngine;

namespace Runtime
{
    public class GameEvents : MonoBehaviour
    {
        public static GameEvents instance;
    
        private void Awake()
        {
            if(instance == null) instance = this;
            else
            {
                Destroy(gameObject);
                return;
            }
        
            DontDestroyOnLoad(this);
        }
    
        public event Action<float> PlayerHealthChanged;
        public event Action<float> PlayerArmorChanged;
        public event Action<int> PlayerInventoryChanged;

        public event Action EnemyIsKilled;

        public void OnPlayerHealthChanged(float ratio)
        {
            PlayerHealthChanged?.Invoke(ratio);
        }
        
        public void OnPlayerArmorChanged(float ratio)
        {
            PlayerArmorChanged?.Invoke(ratio);
        }

        public void OnPlayerInventoryChanged(int slotIndex)
        {
            PlayerInventoryChanged?.Invoke(slotIndex);
        }

        public void OnEnemyBeKilled()
        {
            EnemyIsKilled?.Invoke();
        }
    }
}
