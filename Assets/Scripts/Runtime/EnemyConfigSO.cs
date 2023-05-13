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
        
    }
}
