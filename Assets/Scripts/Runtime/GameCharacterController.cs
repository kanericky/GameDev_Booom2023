using System;
using DG.Tweening;
using UnityEngine;

namespace Runtime
{
    [RequireComponent(typeof(Pawn))]
    public class GameCharacterController : MonoBehaviour
    {
        public static GameCharacterController instance;
        
        [Header("Character Config")] 
        public CharacterConfigSO playerData;
        
        [Header("References")] 
        public Pawn playerPawn;
        public CameraController cameraController;
        public UIManager uIManager;
        public GameManager gameManager;
        public SkinnedMeshRenderer characterMeshRenderer;

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
        private int playerPawnPositionIndex = 1;
        private Material defaultMat;
        private int numCorrectImpact;


        private void Awake()
        {
            RegisterInput();

            GameEvents.instance.CorrectImpact += HandleCorrectImpact;
            
            instance = this;
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
            InitHealthSystem();

            playerPawnPositionIndex = 1;
        }

        private void InitReference()
        {
            // Get Game Managers
            gameManager = FindObjectOfType<GameManager>();
            uIManager = UIManager.instance;
            characterMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();

            // Get Pawn
            playerPawn = GetComponent<Pawn>();
        }

        private void InitData()
        {
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
            
            // Init material
            defaultMat = characterMeshRenderer.material;
            numCorrectImpact = 0;
        }

        private void InitHealthSystem()
        {
            uIManager.InitPlayerHUDHealthBar(
                healthRatio: playerPawn.healthSystem.GetHealthInPercentage(),
                armorRatio: playerPawn.healthSystem.GetArmorInPercentage());
        }

        private void HandleCorrectImpact()
        {
            numCorrectImpact += 1;

            if (numCorrectImpact == 3)
            {
                Ammo blackAmmo = AmmoFactory.GetAmmoFromFactory(AmmoType.BlackAmmo);
                int slotIndex = GameManager.GetReloadSlotIndexBasedOnAmmoColor(blackAmmo.gameElementColor);
                
                playerPawn.pawnInventory.AddItemToSlot(slotIndex, blackAmmo);
                numCorrectImpact = 0;
            }
        }

        private void FixedUpdate()
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
                AudioManager.instance.PlayByName("Reload Start");
                
                // Handle Camera
                cameraController.ChangeCameraPosToReload();
                
                // Handle UI
                uIManager.OpenReloadUIWidget();
                
                // Pawn action
                playerPawn.EnterReloadingState();
            }
            
            // Go to reloading state from aiming state
            else if (playerPawn.GetPawnCurrentState() == CharacterPhaseState.AimingPhase)
            {
                // Handle Camera
                cameraController.ChangeCameraPosToReload();
                
                uIManager.OpenReloadUIWidget();
                
                // Pawn action
                playerPawn.EnterReloadingState();
                
                GameManager.instance.EnterSlowMotion(timeScale, slowMotionDuration);
            }
            
            else if (playerPawn.GetPawnCurrentState() == CharacterPhaseState.ReloadingPhase)
            {
                EnterAimingState();
            }
            
            UIManager.instance.ClearBulletUI();
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
                AudioManager.instance.PlayByName("Reload End");
                uIManager.CloseReloadUIWidget();
                EnterAimingState();
                GameManager.instance.ResetSlowMotion();
                
            // Go back to idle state from aiming state    
            }else if (playerPawn.GetPawnCurrentState() == CharacterPhaseState.AimingPhase)
            {
                ExitAimingState();
                GameManager.instance.EnterSlowMotion(timeScale, slowMotionDuration);
                
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
        }

        public void ExitReloadState()
        {
            playerPawn.ExitReloadingState();
            uIManager.CloseReloadUIWidget();
            cameraController.ChangeCameraPosToIdle();
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

        public void ExitAimingState()
        {
            // Pawn Action
            playerPawn.ExitAimingState();
            
            // Handle Camera
            cameraController.ChangeCameraPosToIdle();
            
            // Handle UI
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
            
            AudioManager.instance.PlayByName("Fire A");

            playerPawn.Fire(cameraController.GetMousePosInWorld());
            
            cameraController.HandleCameraShake();
        }

        private void HandleReloadSelection(int index)
        {
            if (playerPawn.GetPawnCurrentState() != CharacterPhaseState.ReloadingPhase) return;
            
            AudioManager.instance.PlayByName("Reload");

            playerPawn.HandleReloadSelection(index);
            uIManager.ReloadButtonPressedAnimation(index);
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
            playerPawn.TakeDamage(damage, characterMeshRenderer, gameManager.matHit, defaultMat, isPlayer: true);

            // Update UI
            GameEvents.instance.OnPlayerHealthChanged(playerPawn.healthSystem.GetHealthInPercentage());
            GameEvents.instance.OnPlayerArmorChanged(playerPawn.healthSystem.GetArmorInPercentage());
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

        public void EnablePlayerControllerInput()
        {
            _inputAction.Player.Enable();
        }

        public void DisablePlayerControllerInput()
        {
            _inputAction.Player.Disable();
        }

        public void RetractLoadedAmmo()
        {
            foreach (var ammo in playerPawn.weapon.ammoInMag)
            {
                playerPawn.pawnInventory.AddItemToSlot(GameManager.GetReloadSlotIndexBasedOnAmmoColor(ammo.gameElementColor), ammo);
            }
        }

        private void OnDestroy()
        {
            _inputAction.Player.Disable();
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
