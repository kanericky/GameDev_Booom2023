using DG.Tweening;
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

        [Header("Bullet UI Icon")] 
        public Sprite redBullet;
        public Sprite yellowBullet;
        public Sprite blueBullet;
        public Sprite blackBullet;

        [Header("Bullet Drop UI Icon")] 
        public Sprite redBulletIcon;
        public Sprite yellowBulletIcon;
        public Sprite blueBulletIcon;
        public Sprite blackBulletIcon;

        [Header("Slow motion")]
        [SerializeField] private bool isSlowMotionEnabled = true;

        private void Awake()
        {
            instance = this;
            uiManager = FindObjectOfType<UIManager>();
            cameraController = FindObjectOfType<CameraController>();
        }

        public Sprite GetSpriteBasedOnColor(GameElementColor color)
        {
            switch (color)
            {
                case GameElementColor.Red:
                    return redBullet;
                
                case GameElementColor.Yellow:
                    return yellowBullet;
                
                case GameElementColor.Blue:
                    return blueBullet;
                
                case GameElementColor.Black:
                    return blackBullet;
            }

            return null;
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

        public PawnInventorySystem LoadCurrentInventorySystem()
        {
            return playerInventory;
        }

        public static GameElementColor GatRandomAmmoColor()
        {
            int color = Random.Range(0, (int)GameElementColor.Black);

            if (color == 0) return GameElementColor.Red;
            if (color == 1) return GameElementColor.Yellow;
            if (color == 2) return GameElementColor.Blue;
            if (color == 3) return GameElementColor.Black;

            return GameElementColor.NotDefined;
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
        
        public static Sprite GetSpriteBasedOnAmmoColor(GameElementColor color)
        {
            switch (color)
            {
                case GameElementColor.Red:
                    return GameManager.instance.redBulletIcon;

                case GameElementColor.Yellow:
                    return GameManager.instance.yellowBulletIcon;
                    
                case GameElementColor.Blue:
                    return GameManager.instance.blueBulletIcon;
                    
                case GameElementColor.Black:
                    return GameManager.instance.blackBulletIcon;
                
            }

            return null;
        }

        public static string GetTextBasedOnAmmoColor(GameElementColor color)
        {
            switch (color)
            {
                case GameElementColor.Red:
                    return "红色";

                case GameElementColor.Yellow:
                    return "黄色";
                    
                case GameElementColor.Blue:
                    return "蓝色";
                    
                case GameElementColor.Black:
                    return "黑色";
                
            }

            return null;
        }

        public static int GetReloadSlotIndexBasedOnAmmoColor(GameElementColor color)
        {
            switch (color)
            {
                case GameElementColor.Red:
                    return 0;

                case GameElementColor.Yellow:
                    return 1;
                    
                case GameElementColor.Blue:
                    return 2;
                    
                case GameElementColor.Black:
                    return 3;
            }

            return -1;
        }
    }
}
