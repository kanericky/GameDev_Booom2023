using UnityEditor.Animations;
using UnityEngine;

namespace Runtime
{
    [CreateAssetMenu(fileName = "Enemy Data", menuName = "GameData/Enemy")]
    public class EnemyConfigSO : ScriptableObject
    {
        [Header("Data")]
        public float health;
        public GameElementColor color;
        public AmmoType ammoType;
        public Vector3 enemyAccuracy;

        [Header("Armor")] 
        public Armor chestArmor;
        public Armor helmetArmor;

        [Header("Material")] 
        public Material defaultMaterial;
        public Material highlightMaterial;

        [Header("Behaviour")] 
        public AnimatorController enemyBehaviourAnimator;
    }
}
