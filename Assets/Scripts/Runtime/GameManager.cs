using System;
using DG.Tweening;
using UnityEngine;

namespace Runtime
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;
        
        [Header("Materials")]
        public Material matRed;
        public Material matYellow;
        public Material matBlue;
        public Material matPurple;

        [Header("Slow motion")]
        [SerializeField] private bool isSlowMotionEnabled = true;

        private void Start()
        {
            instance = this;
        }

        public void EnterSlowMotion(float slowFactor = 0.8f, float period = 3f)
        {
            if (!isSlowMotionEnabled) return;
            
            isSlowMotionEnabled = false;
            
            Time.timeScale = slowFactor;

            DOTween.Sequence().SetDelay(period).onComplete = () =>
            {
                Time.timeScale = 1;
                isSlowMotionEnabled = true;
            };
        }

        private void OnDestroy()
        {
        }
    }
}
