using System;
using UnityEngine;

namespace Runtime.DropItemSystemFramework
{
    
    [Serializable, CreateAssetMenu(fileName = "Drop Item Data", menuName = "GameData/Drop Items")]
    public class DropItemConfigSO : ScriptableObject
    {
        [Header("Ammo")] 
        public bool isRandom = false;
        public GameElementColor ammoColor;
        public Sprite ammoIconSprite;
        
        public int ammoAmount;

        [Header("Coin")] 
        public int coinAmount;
        public int minCoinAmount;
        public int maxCoinAmount;
    }
    
}
