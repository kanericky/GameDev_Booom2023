using System;
using DG.Tweening;
using UnityEngine;

namespace Runtime.Menu
{
    public class MenuUIManager : MonoBehaviour
    {
        public static MenuUIManager instance;
        
        [Header("Reference")] 
        public CanvasGroup menuCanvasGroup;
        public Transform mask;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            if(menuCanvasGroup != null) menuCanvasGroup.alpha = 0;
        }
        
        public void TransitionOutro()
        {
            mask.DOScale(Vector3.zero, .5f);
        }

        public void TransitionIntro()
        {
            mask.DOScale(new Vector3(2,2, 2), .5f);
        }

        public void MenuUIEnterReady()
        {
            menuCanvasGroup.DOFade(1, .2f);
        }

        public void QuitGame()
        {
            Debug.Log("Player quit game");
            Application.Quit();
        }
    }
}
