using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Runtime
{
    public class GameCharacterController : MonoBehaviour
    {

        [Header("References")] 
        public Pawn playerPawn;
        public CameraController cameraController;
        public UIManager uIManager;
        
        private InputActions _inputAction;
        
        [Header("Debug")] 
        [SerializeField] private Vector2 mouseInput;


        private void Awake()
        {
            _inputAction = new InputActions();
            _inputAction.Player.Enable();

            _inputAction.Player.ReloadPressed.performed += e => ToggleReloadingState();

            _inputAction.Player.AimPressed.performed += e => EnterAimingState();
            _inputAction.Player.AimReleased.performed += e => ExitAimingState();

            _inputAction.Player.Fire.performed += e => Fire();
        }

        private void Start()
        {
            playerPawn = GetComponent<Pawn>();
            uIManager.ChangeDebugText("Idle Phase");
        }

        private void Update()
        {
            HandleCameraDeadZoneMovement();

        }


        #region ------ Handle Input ------

        private void ToggleReloadingState()
        {
            if (playerPawn.GetPawnCurrentState() == CharacterPhaseState.IdlePhase)
            {
                playerPawn.EnterReloadingState();
                cameraController.ChangeCameraPosToReload();
                uIManager.ChangeDebugText("Reloading Phase");
            }
            else if (playerPawn.GetPawnCurrentState() == CharacterPhaseState.ReloadingPhase)
            {
                cameraController.ChangeCameraPosToIdle();
                playerPawn.ExitReloadingState();
                uIManager.ChangeDebugText("Idle Phase");
                //HandleCameraBreath();
            }
        }

        private void EnterAimingState()
        {
            playerPawn.EnterAimingState();
            cameraController.ChangeCameraPosToAiming();
            uIManager.ChangeDebugText("Aiming Phase");
        }

        private void ExitAimingState()
        {
            playerPawn.ExitAimingState();
        }

        private void Fire()
        {
            playerPawn.Fire();
        }
        
        #endregion ------ Handle Input End ------


        private void HandleCameraDeadZoneMovement()
        {
            if (playerPawn.GetPawnCurrentState() != CharacterPhaseState.IdlePhase) return;
            
            cameraController.HandleCameraDeadZoneMovement(cameraController.GetCameraRotIdle());
        }

        private void HandleCameraBreath()
        {
            if (playerPawn.GetPawnCurrentState() != CharacterPhaseState.IdlePhase) return;
            
            cameraController.HandleCameraBreath(cameraController.GetCameraPosIdle());
        }
        

        private void TargetShootDestination()
        {
            
        }
        
    }

    public enum CharacterPhaseState 
    {
        IdlePhase,
        ReloadingPhase,
        AimingPhase,
        FiringPhase
    }
}
