using System;
using UnityEngine;

namespace Runtime.ArmorSystem
{
    [Serializable, CreateAssetMenu(fileName = "Armor Data", menuName = "GameData/Armor")]
    public class ArmorData : ScriptableObject
    {
        public float armorAmount;

        public ArmorType armorType;
        public GameElementColor armorColor;

        public Armor armorPrefab;
        
        public Material armorMaterial;  
    }
}
