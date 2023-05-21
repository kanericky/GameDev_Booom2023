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
        [SerializeField] private int totalEnemyNum;
        [SerializeField] private int currentAliveEnemyNum;

        private void Start()
        {
            InitReference();
            InitData();
            RegisterEvents();
        }

        private void InitReference()
        {
            gameManager = FindObjectOfType<GameManager>();
            uiManager = GameManager.instance.uiManager;
        }

        private void InitData()
        {
            allEnemiesInLevel = FindObjectsOfType<EnemyCharacterController>().ToList();
            totalEnemyNum = allEnemiesInLevel.Count;
            currentAliveEnemyNum = totalEnemyNum;
        }

        private void RegisterEvents()
        {
            GameEvents.instance.EnemyIsKilled += UpdateCurrentAliveEnemyNum;
        }
        
        private void UpdateCurrentAliveEnemyNum()
        {
            currentAliveEnemyNum -= 1;

            if (currentAliveEnemyNum > 0) return;

            HandleAllEnemyClearedAction();
        }

        private void HandleAllEnemyClearedAction()
        {
            // Open drop item panel
            uiManager.OpenDropItemCanvas();
        }
        
    }
}
