using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Runtime
{
    [RequireComponent(typeof(Pawn))]
    public class GameCharacterController : MonoBehaviour
    {

        [Header("References")] 
        public Pawn playerPawn;
        public CameraController cameraController;
        public UIManager uIManager;

        [Header("Aiming")] 
        public Transform aimingTarget;
        public Transform gunAimStartPos;
        
        private InputActions _inputAction;
        
        [Header("Debug")] 
        [SerializeField] private Vector2 mouseInput;


        private void Awake()
        {
            _inputAction = new InputActions();
            _inputAction.Player.Enable();

            _inputAction.Player.ReloadPressed.performed += e => ToggleReloadingState();

            _inputAction.Player.AimPressed.performed += e => ToggleAimingState();
            
            _inputAction.Player.ReloadSelectionA.performed += e => HandleReloadSelection(0);
            _inputAction.Player.ReloadSelectionB.performed += e => HandleReloadSelection(1);
            _inputAction.Player.ReloadSelectionC.performed += e => HandleReloadSelection(2);
            _inputAction.Player.ReloadSelectionD.performed += e => HandleReloadSelection(3);

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
            HandleAiming();

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

        private void ToggleAimingState()
        {
            if (playerPawn.GetPawnCurrentState() == CharacterPhaseState.ReloadingPhase)
            {
                EnterAimingState();
            }else if (playerPawn.GetPawnCurrentState() == CharacterPhaseState.AimingPhase)
            {
                ExitAimingState();
            }
        }

        private void EnterAimingState()
        {
            playerPawn.EnterAimingState();
            cameraController.ChangeCameraPosToAiming();
            uIManager.ChangeDebugText("Aiming Phase");
        }

        private void HandleAiming()
        {
            if (playerPawn.GetPawnCurrentState() != CharacterPhaseState.AimingPhase) return;
            
            if (Physics.Raycast(gunAimStartPos.position, cameraController.GetMousePosInWorld()-gunAimStartPos.position,
                    out RaycastHit raycastHit, 900f))
            {
                aimingTarget.transform.position = raycastHit.point;
            }
            
        }

        private void ExitAimingState()
        {
            playerPawn.ExitAimingState();
            cameraController.ChangeCameraPosToIdle();
            uIManager.ChangeDebugText("Idle phase");
        }

        private void Fire()
        {
            if (playerPawn.GetPawnCurrentState() != CharacterPhaseState.AimingPhase) return;

            if (playerPawn.weapon.IsMagEmpty()) return;

            playerPawn.Fire();
            cameraController.HandleCameraShake();
            
            if (Physics.Raycast(gunAimStartPos.position, cameraController.GetMousePosInWorld()-gunAimStartPos.position,
                    out RaycastHit raycastHit, 900f) && raycastHit.transform.CompareTag("Enemy"))
            {
                Debug.Log("Enemy Hit!");
                raycastHit.transform.GetComponentInParent<EnemyCharacterController>().OnCharacterHit();
            }
        }

        private void HandleReloadSelection(int index)
        {
            if (playerPawn.GetPawnCurrentState() != CharacterPhaseState.ReloadingPhase) return;

            playerPawn.HandleReloadSelection(index);
        }
        
        #endregion ------ Handle Input End ------


        private void HandleCameraDeadZoneMovement()
        {
            if (playerPawn.GetPawnCurrentState() == CharacterPhaseState.IdlePhase)
            {
                cameraController.HandleCameraDeadZoneMovement(cameraController.GetCameraRotIdle());
            }else if(playerPawn.GetPawnCurrentState() == CharacterPhaseState.AimingPhase)
            {
                cameraController.HandleCameraDeadZoneMovement(cameraController.GetCameraRotAim());
            }
            else
            {
                cameraController.HandleCameraDeadZoneMovement(cameraController.GetCameraRotReload());
            }
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
