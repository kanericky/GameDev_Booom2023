using System;
using DG.Tweening;
using Palmmedia.ReportGenerator.Core.Reporting.Builders;
using UnityEngine;

namespace Runtime
{
    public enum GameElementColor
    {
        Red,
        Yellow,
        Blue,
        Black,
        White,
        NotDefined
    }

    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;

        [Header("Game Managers")] 
        public UIManager uiManager;
        public CameraController cameraController;

        [Header("Player Inventory Data")] 
        public GameCharacterController playerController;
        public PawnInventorySystem playerInventory;
        
        [Header("Bullet Materials")]
        public Material matRed;
        public Material matYellow;
        public Material matBlue;
        public Material matBlack;
        public Material matWhite;

        [Header("Hit Material")] 
        public Material matHit;

        [Header("Slow motion")]
        [SerializeField] private bool isSlowMotionEnabled = true;

        private void Awake()
        {
            instance = this;
            uiManager = FindObjectOfType<UIManager>();
            cameraController = FindObjectOfType<CameraController>();
        }

        public void StopTime()
        {
            Time.timeScale = 0f;
        }

        public void ReviveTime()
        {
            Time.timeScale = 1f;
        }

        public void EnterSlowMotion(float slowFactor = 0.5f, float period = 2f)
        {
            if (!isSlowMotionEnabled) return;
            
            isSlowMotionEnabled = false;
            
            Time.timeScale = slowFactor;
            uiManager.ChangeTimeDebugText(Time.timeScale.ToString());

            DOTween.Sequence().SetDelay(period).onComplete = () =>
            {
                Time.timeScale = 1;
                isSlowMotionEnabled = true;
                uiManager.ChangeTimeDebugText(Time.timeScale.ToString());
            };
        }

        public void ResetSlowMotion()
        {
            Time.timeScale = 1;
            uiManager.ChangeTimeDebugText(Time.timeScale.ToString());
        }

        public void SaveCurrentInventory(PawnInventorySystem playerCurrentInventory)
        {
            playerInventory = playerCurrentInventory;
        }
        
        public static Material GetMaterialBasedOnAmmoColor(GameElementColor color)
        {
            switch (color)
            {
                case GameElementColor.Red:
                    return GameManager.instance.matRed;

                case GameElementColor.Yellow:
                    return GameManager.instance.matYellow;
                    
                case GameElementColor.Blue:
                    return GameManager.instance.matBlue;
                    
                case GameElementColor.Black:
                    return GameManager.instance.matBlack;
                    
                case GameElementColor.White:
                    return GameManager.instance.matWhite;
            }

            return null;
        }

        private void OnDestroy()
        {
        }
    }
}
