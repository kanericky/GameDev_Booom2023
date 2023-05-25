using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using Runtime.DropItemSystemFramework;
using Random = UnityEngine.Random;

namespace Runtime
{
    [RequireComponent(typeof(LevelManager))]
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager instance;
        
        [Header("Game Manager Reference")] 
        [SerializeField] private GameManager gameManager;
        [SerializeField] private UIManager uiManager;

        [Header("Enemy")] 
        [SerializeField] private List<EnemyCharacterController> allEnemiesInLevel;
        [SerializeField] private int totalEnemyNum;
        [SerializeField] private int currentAliveEnemyNum;

        [SerializeField] private EnemyConfigSO[] enemyDataList;

        [Header("Level")] [SerializeField] private int loadLevelIndex;

        [Header("Drop Item System")] 
        [SerializeField] private DropItemSystem dropItemSystem;

        private void Awake()
        {
            instance = this;
            DOTween.CompleteAll();
            DOTween.KillAll();
        }

        private void OnValidate()
        {
            SetupEnemy();
        }

        private void Start()
        {
            InitReference();
            InitData();
            RegisterEvents();
            
            DOTween.Sequence().SetDelay(1f).onComplete = () =>
            {
                uiManager.TransitionIntro();
            };
        }

        private void InitReference()
        {
            gameManager = FindObjectOfType<GameManager>();
            uiManager = UIManager.instance;

            dropItemSystem.GetComponent<DropItemSystem>();
        }

        private void InitData()
        {
            // Init enemy info
            allEnemiesInLevel = FindObjectsOfType<EnemyCharacterController>().ToList();
            totalEnemyNum = allEnemiesInLevel.Count;
            currentAliveEnemyNum = totalEnemyNum;
            
            // Init systems
            dropItemSystem.SetupDataToUI();
            
            SetupEnemy();
        }

        private void SetupEnemy()
        {
            foreach (var enemy in allEnemiesInLevel)
            {
                int index = Random.Range(0, enemyDataList.Length);
                EnemyConfigSO enemyData = enemyDataList[index];
                enemy.InitEnemy(enemyData);
            }
        }

        private void RegisterEvents()
        {
            GameEvents.instance.EnemyIsKilled += UpdateCurrentAliveEnemyNum;
        }
        
        private void UpdateCurrentAliveEnemyNum()
        {
            currentAliveEnemyNum -= 1;

            if (currentAliveEnemyNum > 0) return;

            if (currentAliveEnemyNum == 0)
            {
                GameCharacterController.instance.ExitAimingState();
                GameManager.instance.EnterSlowMotion(period: .5f);

                DOTween.Sequence().SetDelay(1f).onComplete = () =>
                {
                    GameCharacterController.instance.DisablePlayerControllerInput();
                    
                    HandleAllEnemyClearedAction();
                };
            }
        }

        private void HandleAllEnemyClearedAction()
        {
            // Open drop item panel
            DOTween.Sequence().SetDelay(.5f).onComplete = () => { uiManager.OpenDropItemCanvas(); };
        }

        public void ExitLevel()
        {
            DOTween.CompleteAll();
            DOTween.KillAll();
            GameCharacterController.instance.RetractLoadedAmmo();
            Pawn playerPawn = GameCharacterController.instance.playerPawn;
            gameManager.SaveCurrentStatusInfo(playerPawn.pawnInventory, playerPawn.healthSystem);
            GameManager.LoadLevel(loadLevelIndex);
        }
        
    }
}
