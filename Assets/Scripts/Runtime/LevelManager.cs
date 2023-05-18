using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Runtime
{
    public class LevelManager : MonoBehaviour
    {
        [Header("Game Manager Reference")] 
        [SerializeField] private GameManager gameManager;
        [SerializeField] private UIManager uiManager;
        
        
        [Header("Enemy")] 
        [SerializeField] private List<EnemyCharacterController> allEnemiesInLevel;

        private void Start()
        {
            InitDataReference();
            InitLevel();
        }

        private void InitDataReference()
        {
            gameManager = FindObjectOfType<GameManager>();
            uiManager = GameManager.instance.uiManager;
            
            allEnemiesInLevel = FindObjectsOfType<EnemyCharacterController>().ToList();
        }

        private void InitLevel()
        {
            gameManager.StopTime();
            uiManager.ShowDropItemInterface();
        }
        
    }
}
