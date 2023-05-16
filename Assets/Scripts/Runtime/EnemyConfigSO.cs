using UnityEditor.Animations;
using UnityEngine;

namespace Runtime
{
    [CreateAssetMenu(fileName = "Enemy Data", menuName = "GameData/Enemy")]
    public class EnemyConfigSO : ScriptableObject
    {
        [Header("Health")]
        public float health;
        public GameElementColor color;

        [Header("Material")] 
        public Material defaultMaterial;
        public Material highlightMaterial;

        [Header("Behaviour")] 
        public AnimatorController enemyBehaviourAnimator;

        [Header("AI")] 
        public float ammoDamage;

    }
}
