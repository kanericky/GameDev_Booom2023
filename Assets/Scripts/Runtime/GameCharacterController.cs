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

        [Header("Roll")] 
        public float rollCoolDownCD = 5f;
        public bool canRoll = true;

        [Header("Slow Motion")] 
        public float timeScale = 0.5f;
        public float slowMotionDuration = 1f;

        [Header("Input")] 
        public float inputCoolDownTimeGeneral = 0.1f;
        public float inputCoolDownTimeFire = 0.2f;
        public float inputCoolDownTimeRoll = 1.4f;
        
        private InputActions _inputAction;
        
        [Header("Debug")] 
        [SerializeField] private int playerPawnPositionIndex = 1;


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

            _inputAction.Player.RollLeft.performed += e => HandleRoll(isRollLeft: true);
            _inputAction.Player.RollRight.performed += e => HandleRoll(isRollLeft: false);

            gameManager = FindObjectOfType<GameManager>();
        }

        private void Start()
        {
            playerPawn = GetComponent<Pawn>();
            gameManager = FindObjectOfType<GameManager>();
            uIManager = GameManager.instance.uiManager;
            
            uIManager.ChangeDebugText("Idle Phase");
            playerPawnPositionIndex = 1;
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
            GameManager.instance.ResetSlowMotion();
            
            if (playerPawn.GetPawnCurrentState() == CharacterPhaseState.IdlePhase)
            {
                cameraController.ChangeCameraPosToReload(playerPawnPositionIndex);
                uIManager.ChangeDebugText("Reload Phase");
                uIManager.OpenReloadUIWidget();
                playerPawn.EnterReloadingState();
            }
            else if (playerPawn.GetPawnCurrentState() == CharacterPhaseState.AimingPhase)
            {
                cameraController.ChangeCameraPosToReload(playerPawnPositionIndex);
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
                GameManager.instance.ResetSlowMotion();
            }else if (playerPawn.GetPawnCurrentState() == CharacterPhaseState.AimingPhase)
            {
                ExitAimingState();
                GameManager.instance.EnterSlowMotion(timeScale, slowMotionDuration);
            }else if (playerPawn.GetPawnCurrentState() == CharacterPhaseState.IdlePhase)
            {
                EnterAimingState();
                GameManager.instance.ResetSlowMotion();
            }
        }

        private void EnterAimingState()
        {
            playerPawn.EnterAimingState();
            cameraController.ChangeCameraPosToAiming(playerPawnPositionIndex);
            uIManager.ChangeDebugText("Aiming Phase");
        }

        private void ExitReloadState()
        {
            playerPawn.ExitReloadingState();
            cameraController.ChangeCameraPosToIdle(playerPawnPositionIndex);
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
            cameraController.ChangeCameraPosToIdle(playerPawnPositionIndex);
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

        private void HandleRoll(bool isRollLeft = true)
        {
            if (!canRoll) return;
            
            InputCoolDown(inputCoolDownTimeRoll);

            if(playerPawn.GetPawnCurrentState() == CharacterPhaseState.ReloadingPhase) ExitReloadState();
            if(playerPawn.GetPawnCurrentState() == CharacterPhaseState.AimingPhase) ExitAimingState();

            if (isRollLeft && playerPawnPositionIndex > 0)
            {
                playerPawn.RollLeft();
                RollCoolDown(rollCoolDownCD);
                playerPawnPositionIndex -= 1;
            }
            else if(!isRollLeft && playerPawnPositionIndex < 2)
            {
                playerPawn.RollRight();
                RollCoolDown(rollCoolDownCD);
                playerPawnPositionIndex += 1;
            }
        }

        public void ResetCamera()
        {
            cameraController.ResetCameraPos(playerPawnPositionIndex);
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

        private void RollCoolDown(float coolDownTime)
        {
            canRoll = false;

            DOTween.Sequence().SetDelay(coolDownTime).onComplete = () =>
            {
                canRoll = true;
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
