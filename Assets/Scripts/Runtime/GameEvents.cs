using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public event Action<int> PlayerInventoryChanged;

    public void OnPlayerHealthChanged(float ratio)
    {
        PlayerHealthChanged?.Invoke(ratio);
    }

    public void OnPlayerInventoryChanged(int slotIndex)
    {
        PlayerInventoryChanged?.Invoke(slotIndex);
    }
}
