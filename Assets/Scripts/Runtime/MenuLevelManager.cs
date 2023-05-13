using System;
using UnityEngine;

namespace Runtime
{
    public class MenuLevelManager : MonoBehaviour
    {
        [Header("Manager Reference")] 
        public MenuCameraController cameraController;
        public MenuUIManager uiManager;
        public Animator menuCharacterAnimator;

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
            
            Debug.Log("Start Game");
            cameraController.EnterMenuStartCameraPos();
            uiManager.MenuUIEnterReady();
            menuCharacterAnimator.SetTrigger("Enter Ready Pos");
        }
    }
}
