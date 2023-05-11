using System;
using DG.Tweening;
using UnityEngine;

namespace Runtime
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;

        [Header("Game Managers")] 
        public UIManager uiManager;
        public CameraController cameraController;
        
        [Header("Materials")]
        public Material matRed;
        public Material matYellow;
        public Material matBlue;
        public Material matPurple;

        [Header("Slow motion")]
        [SerializeField] private bool isSlowMotionEnabled = true;

        private void Awake()
        {
            instance = this;
            uiManager = FindObjectOfType<UIManager>();
            cameraController = FindObjectOfType<CameraController>();
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

        private void OnDestroy()
        {
        }
    }
}
