using UnityEngine;

namespace Runtime
{
    [CreateAssetMenu(fileName = "Player Data", menuName = "GameData/Player")]
    public class CharacterConfigSO : ScriptableObject
    {
        [Header("Health")]
        public float health;
        public GameElementColor color;
        
        [Header("Roll")]
        public float rollCoolDownCd = 5f;
        public bool canRoll = true;
        
        [Header("Slow Motion")] 
        public float timeScale = 0.5f;
        public float slowMotionDuration = 1f;
        
        [Header("Input")] 
        public float inputCoolDownTimeGeneral = 0.1f;
        public float inputCoolDownTimeFire = 0.2f;
        public float inputCoolDownTimeRoll = 1.4f;
    }
}
