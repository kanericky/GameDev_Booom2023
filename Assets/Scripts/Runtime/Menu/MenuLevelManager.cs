using System;
using DG.Tweening;
using UnityEngine;

namespace Runtime
{
    public class MenuLevelManager : MonoBehaviour
    {
        [Header("Manager Reference")] 
        public MenuCameraController cameraController;
        public MenuUIManager uiManager;
        public Animator menuCharacterAnimator;
        public Animator menuUIAnimator;

        private InputActions inputActions;
        private bool _isTriggered = false;

        private void Start()
        {
            inputActions = new InputActions();
            inputActions.Menu.Enable();

            inputActions.Menu.StartGame.performed += e => { StartGame(); };
        }

        private void StartGame()
        {
            if (_isTriggered) return;
            _isTriggered = true;

            menuUIAnimator.SetTrigger("Start");
            
            Debug.Log("Start Game");
            DOTween.Sequence().SetDelay(.8f).onComplete = () =>
            {
                cameraController.EnterMenuStartCameraPos();
                menuCharacterAnimator.SetTrigger("Enter Ready Pos");
                uiManager.MenuUIEnterReady();
            };
        }
    }
}
