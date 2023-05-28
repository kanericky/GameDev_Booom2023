using System;
using UnityEngine;

namespace Runtime.ShopSystemFramework
{
    public class ShopLevelManager : MonoBehaviour
    {
        public static ShopLevelManager instance;

        public PawnInventorySystem playerInventory;
        
        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            playerInventory = GameManager.instance.playerInventory;
            ShopUIManager.instance.TransitionIntro();
        }
    }
}
