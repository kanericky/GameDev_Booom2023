using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Runtime.Menu
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
            //Cursor.visible = false;
            
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
        
        public static void LoadLevel(int levelIndex)
        {
            MenuUIManager.instance.TransitionOutro();
            DOTween.Sequence().SetDelay(1f).onComplete = () =>
            {
                DOTween.CompleteAll();
                DOTween.KillAll();
                SceneManager.LoadScene(levelIndex);
            };
        }

        private void OnDestroy()
        {
            inputActions.Menu.Disable();
            DOTween.KillAll();
            DOTween.CompleteAll();
        }
    }
}
