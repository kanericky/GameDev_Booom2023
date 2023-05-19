using Runtime.ArmorSystem;
using UnityEditor.Animations;
using UnityEngine;

namespace Runtime
{
    [CreateAssetMenu(fileName = "Enemy Data", menuName = "GameData/Enemy")]
    public class EnemyConfigSO : ScriptableObject
    {
        [Header("Data - Health")]
        public float health;
        public GameElementColor color;
        
        [Header("Data - Armor")] 
        public ArmorData chestArmorData;
        public ArmorData helmetArmorData;
        
        [Header("Data - Weapon")]
        public AmmoType ammoType;
        public Vector3 enemyAccuracy;

        [Header("Material")] 
        public Material defaultMaterial;
        public Material highlightMaterial;

        [Header("Behaviour")] 
        public AnimatorController enemyBehaviourAnimator;
    }
}
