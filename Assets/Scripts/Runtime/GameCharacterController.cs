using System;
using DG.Tweening;
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
        public GameManager gameManager;

        [Header("Aiming")] 
        public Transform aimingTarget;
        public Transform gunAimStartPos;

        [Header("Input")] 
        public float inputCoolDownTimeGeneral = 0.1f;
        public float inputCoolDownTimeFire = 0.2f;
        
        private InputActions _inputAction;
        
        [Header("Debug")] 
        [SerializeField] private Vector2 mouseInput;


        private void Awake()
        {
            _inputAction = new InputActions();
            _inputAction.Player.Enable();

            _inputAction.Player.ReloadPressed.performed += e => EnterReloadState();

            _inputAction.Player.AimPressed.performed += e => ToggleAimingState();
            
            _inputAction.Player.ReloadSelectionA.performed += e => HandleReloadSelection(0);
            _inputAction.Player.ReloadSelectionB.performed += e => HandleReloadSelection(1);
            _inputAction.Player.ReloadSelectionC.performed += e => HandleReloadSelection(2);
            _inputAction.Player.ReloadSelectionD.performed += e => HandleReloadSelection(3);

            _inputAction.Player.CancelReload.performed += e => ExitReloadState();

            _inputAction.Player.Fire.performed += e => Fire();

            gameManager = FindObjectOfType<GameManager>();
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

        /// <summary>
        /// "R" - Toggle Reloading State
        /// </summary>
        private void EnterReloadState()
        {
            InputCoolDown(inputCoolDownTimeGeneral);
            
            if (playerPawn.GetPawnCurrentState() == CharacterPhaseState.IdlePhase)
            {
                cameraController.ChangeCameraPosToReload();
                uIManager.ChangeDebugText("Reload Phase");
                uIManager.OpenReloadUIWidget();
                playerPawn.EnterReloadingState();
            }
            else if (playerPawn.GetPawnCurrentState() == CharacterPhaseState.AimingPhase)
            {
                cameraController.ChangeCameraPosToReload();
                uIManager.ChangeDebugText("Reload Phase");
                uIManager.OpenReloadUIWidget();
                playerPawn.EnterReloadingState();
            }
        }

        /// <summary>
        /// "Mouse Right Button" - Toggle Aiming State
        /// </summary>
        private void ToggleAimingState()
        {
            InputCoolDown(inputCoolDownTimeGeneral);
            
            if (playerPawn.GetPawnCurrentState() == CharacterPhaseState.ReloadingPhase)
            {
                EnterAimingState();
            }else if (playerPawn.GetPawnCurrentState() == CharacterPhaseState.AimingPhase)
            {
                ExitAimingState();
                GameManager.instance.EnterSlowMotion();
            }
        }

        private void EnterAimingState()
        {
            playerPawn.EnterAimingState();
            cameraController.ChangeCameraPosToAiming();
            uIManager.ChangeDebugText("Aiming Phase");
        }

        private void ExitReloadState()
        {
            playerPawn.ExitReloadingState();
            cameraController.ChangeCameraPosToIdle();
            uIManager.ChangeDebugText("Idle Phase");
        }

        /// <summary>
        /// Handle Aiming logic
        /// </summary>
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
            InputCoolDown(inputCoolDownTimeFire);
            
            if (playerPawn.GetPawnCurrentState() != CharacterPhaseState.AimingPhase) return;

            if (playerPawn.weapon.IsMagEmpty())
            {
                ExitAimingState();
                return;
            }

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


        /// <summary>
        /// Handle camera dead zone movement
        /// </summary>
        private void HandleCameraDeadZoneMovement()
        {
            if (playerPawn.GetPawnCurrentState() == CharacterPhaseState.IdlePhase)
            {
                cameraController.HandleCameraDeadZoneMovement(cameraController.GetCameraRotIdle());
            }
            else if(playerPawn.GetPawnCurrentState() == CharacterPhaseState.AimingPhase)
            {
                cameraController.HandleCameraDeadZoneMovement(cameraController.GetCameraRotAim(), isAimingPhase: true);
            }
            else
            {
                cameraController.HandleCameraDeadZoneMovement(cameraController.GetCameraRotReload());
            }
        }
        
        /// <summary>
        /// Disable player's input for a certain amount of time
        /// </summary>
        /// <param name="coolDownTime">Disable time period</param>
        private void InputCoolDown(float coolDownTime)
        {
            _inputAction.Disable();

            DOTween.Sequence().SetDelay(coolDownTime).onComplete = () =>
            {
                _inputAction.Enable();
            };
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
