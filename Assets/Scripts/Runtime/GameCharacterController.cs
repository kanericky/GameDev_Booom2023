using DG.Tweening;
using UnityEngine;

namespace Runtime
{
    [RequireComponent(typeof(Pawn))]
    public class GameCharacterController : MonoBehaviour
    {
        [Header("Character Config")] 
        public CharacterConfigSO playerData;
        
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
        private EnemyCharacterController enemy;
        
        [Header("Debug")] 
        [SerializeField] private int playerPawnPositionIndex = 1;


        private void Awake()
        {
            RegisterInput();
        }

        private void RegisterInput()
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
        }

        private void Start()
        {
            InitReference();
            InitData();
            
            // Misc
            uIManager.ChangeDebugText("Idle Phase");
            playerPawnPositionIndex = 1;
        }

        private void InitReference()
        {
            // Get Game Managers
            gameManager = FindObjectOfType<GameManager>();
            uIManager = GameManager.instance.uiManager;
            
            // Get Pawn
            playerPawn = GetComponent<Pawn>();
        }

        private void InitData()
        {
            // Health System
            playerPawn.healthSystem = new HealthSystem(playerData.health);

            if (playerData.Equals(null)) return;
            
            // Init Roll System
            rollCoolDownCD = playerData.rollCoolDownCd;
            canRoll = playerData.canRoll;
            playerPawnPositionIndex = 1;
            
            // Init Slow Motion System
            timeScale = playerData.timeScale;
            slowMotionDuration = playerData.slowMotionDuration;
            
            // Init Input Data
            inputCoolDownTimeGeneral = playerData.inputCoolDownTimeGeneral;
            inputCoolDownTimeFire = playerData.inputCoolDownTimeFire;
            inputCoolDownTimeRoll = playerData.inputCoolDownTimeRoll;
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
            
            // Go to reloading state from idle state
            if (playerPawn.GetPawnCurrentState() == CharacterPhaseState.IdlePhase)
            {
                // Handle Camera
                cameraController.ChangeCameraPosToReload(playerPawnPositionIndex);
                
                // Handle UI
                uIManager.ChangeDebugText("Reload Phase");
                uIManager.OpenReloadUIWidget();
                
                // Pawn action
                playerPawn.EnterReloadingState();
                
                GameManager.instance.EnterSlowMotion(timeScale, slowMotionDuration);
            }
            
            // Go to reloading state from aiming state
            else if (playerPawn.GetPawnCurrentState() == CharacterPhaseState.AimingPhase)
            {
                // Handle Camera
                cameraController.ChangeCameraPosToReload(playerPawnPositionIndex);
                
                // Handle UI
                uIManager.ChangeDebugText("Reload Phase");
                uIManager.OpenReloadUIWidget();
                
                // Pawn action
                playerPawn.EnterReloadingState();
                
                GameManager.instance.EnterSlowMotion(timeScale, slowMotionDuration);
            }
        }

        /// <summary>
        /// "Mouse Right Button" - Toggle Aiming State
        /// </summary>
        private void ToggleAimingState()
        {
            InputCoolDown(inputCoolDownTimeGeneral);
            
            // Go to aim state from reloading state
            if (playerPawn.GetPawnCurrentState() == CharacterPhaseState.ReloadingPhase)
            {
                EnterAimingState();
                GameManager.instance.ResetSlowMotion();
                
            // Go back to idle state from aiming state    
            }else if (playerPawn.GetPawnCurrentState() == CharacterPhaseState.AimingPhase)
            {
                ExitAimingState();
                GameManager.instance.ResetSlowMotion();
                
            // Go to aim state from idle state    
            }else if (playerPawn.GetPawnCurrentState() == CharacterPhaseState.IdlePhase)
            {
                EnterAimingState();
                GameManager.instance.ResetSlowMotion();
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
                    out RaycastHit raycastHit, 100f))
            {
                
                aimingTarget.transform.position = raycastHit.point;
                
                EnemyCharacterController tempEnemy = raycastHit.transform.GetComponentInParent<EnemyCharacterController>();

                if (tempEnemy != null)
                {
                    tempEnemy.EnableHighlight();
                    enemy = tempEnemy;
                }
                else if(enemy != null)
                {
                    enemy.DisableHighlight();
                }
                
            }
            
        }

        private void ExitAimingState()
        {
            // Pawn Action
            playerPawn.ExitAimingState();
            
            // Handle Camera
            cameraController.ChangeCameraPosToIdle();
            
            // Handle UI
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

            playerPawn.Fire(cameraController.GetMousePosInWorld());
            
            cameraController.HandleCameraShake();
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
            cameraController.ResetCameraPos();
        }
        
        #endregion ------ Handle Input End ------

        public void HandleHit(float damage)
        {
            canRoll = false;

            DOTween.Clear();
            
            playerPawn.TakeDamage(damage);
            
            // Update UI
            GameEvents.instance.OnPlayerHealthChanged(playerPawn.healthSystem.GetHealthInPercentage());
        }

        public void ResetCanRollStatus()
        {
            canRoll = true;
        }


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

    }

    public enum CharacterPhaseState 
    {
        IdlePhase,
        ReloadingPhase,
        AimingPhase,
        FiringPhase
    }
}
