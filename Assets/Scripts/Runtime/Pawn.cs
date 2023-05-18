using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Runtime
{

    public class Pawn : MonoBehaviour
    {
        [Header("Pawn Attributes")] 
        public HealthSystem healthSystem;

        [Header("Weapon")]
        public Weapon weapon;

        [Header("Inventory")] 
        public PawnInventorySystem pawnInventory;

        [Header("Animator")] 
        public Animator pawnAnimator;
        
        [Header("Debug")]
        [SerializeField] private CharacterPhaseState currentPhaseState;

        // Animation Triggers
        private static readonly string AnimatorTriggerReload = "Trigger Reload";
        private static readonly string AnimatorTriggerReset = "Reset Reload";
        private static readonly string AnimatorTriggerAim = "Trigger Aiming";
        private static readonly string AnimatorTriggerFire = "Fire";
        private static readonly string AnimatorTriggerResetToIdle = "Reset to Idle";
        private static readonly string AnimatorTriggerHitReaction = "Take Hit";
        private static readonly string AnimatorTriggerRollLeft = "Roll Left";
        private static readonly string AnimatorTriggerRollRight = "Roll Right";
        private static readonly string AnimatorTriggerDeath = "Death";
        private static readonly string AnimatorDeadBool = "Is Dead";

        [SerializeField] private bool isPawnDead;

        private void Start()
        {
            InitPawnSystem();
            InitWeaponSystem();
            InitInventory();
        }

        private void InitWeaponSystem()
        {
            weapon = GetComponentInChildren<Weapon>();
            if (weapon != null) weapon.WeaponInit();
        }

        private void InitPawnSystem()
        {
            pawnAnimator = GetComponent<Animator>();
            currentPhaseState = CharacterPhaseState.IdlePhase;
            isPawnDead = false;
        }

        private void InitInventory()
        {
            pawnInventory = new PawnInventorySystem();
            
            pawnInventory.AddItemToSlot(0, AmmoFactory.GetAmmoFromFactory(AmmoType.RedAmmo));
            pawnInventory.AddItemToSlot(0, AmmoFactory.GetAmmoFromFactory(AmmoType.RedAmmo));
            pawnInventory.AddItemToSlot(0, AmmoFactory.GetAmmoFromFactory(AmmoType.RedAmmo));
        }

        public void EnterReloadingState()
        {
            if (isPawnDead) return;
            
            if (!(currentPhaseState is CharacterPhaseState.IdlePhase or CharacterPhaseState.AimingPhase)) return;

            Debug.Log("Start Reloading");
            
            pawnAnimator.SetTrigger(AnimatorTriggerReload);
            
            currentPhaseState = CharacterPhaseState.ReloadingPhase;
            
            // Step 1: Clear all the bullets in the mag.
            weapon.ClearMag();
            
            // Step 2: Handle reload input
        }

        public void EnemyEnterReloadingState()
        {
            if (isPawnDead) return;
            
            if (!(currentPhaseState is CharacterPhaseState.IdlePhase or CharacterPhaseState.AimingPhase)) return;

            Debug.Log("Enemy Start Reloading");
            
            pawnAnimator.SetTrigger(AnimatorTriggerReload);
            
            currentPhaseState = CharacterPhaseState.ReloadingPhase;
        }

        public void ExitReloadingState()
        {
            if (isPawnDead) return;
            
            if (currentPhaseState != CharacterPhaseState.ReloadingPhase) return;
            
            pawnAnimator.SetTrigger(AnimatorTriggerReset);
            
            currentPhaseState = CharacterPhaseState.IdlePhase;
        }
        

        public void EnterAimingState()
        {
            if (isPawnDead) return;
            
            Debug.Log("Start Aiming");
            
            pawnAnimator.SetTrigger(AnimatorTriggerAim);
            
            currentPhaseState = CharacterPhaseState.AimingPhase;
            
            // Aiming
        }

        public void ExitAimingState()
        {
            if (isPawnDead) return;
            
            if (currentPhaseState != CharacterPhaseState.AimingPhase) return;
            
            pawnAnimator.SetTrigger(AnimatorTriggerResetToIdle);
            
            currentPhaseState = CharacterPhaseState.IdlePhase;
        }

        public void Fire(Vector3 targetPos)
        {
            if (isPawnDead) return;
            
            if (currentPhaseState != CharacterPhaseState.AimingPhase) return;

            // FIRE!!!
            weapon.Fire(targetPos);
            
            // Trigger Animation
            pawnAnimator.ResetTrigger(AnimatorTriggerFire);
            pawnAnimator.SetTrigger(AnimatorTriggerFire);
        }

        public void EnemyFire(Vector3 target, Ammo ammo)
        {
            // FIRE!!!
            weapon.EnemyWeaponFire(target, ammo);
        }

        public CharacterPhaseState GetPawnCurrentState()
        {
            return currentPhaseState;
        }
        
        public void TakeDamage(float damage)
        {
            if (isPawnDead) return;
            
            // Handle animation
            pawnAnimator.SetTrigger(AnimatorTriggerHitReaction);

            // Apply damage
            if (healthSystem.TakeDamage(damage) <= 0)
            {
                if (isPawnDead) return;
                isPawnDead = true;
                HandlePawnDeath();
            }
        }

        public void TakeDamage(float damage, Renderer characterMeshRenderer, Material matHit, Material defaultMat)
        {
            if (isPawnDead) return;
            
            // Handle animation
            // pawnAnimator.SetTrigger(AnimatorTriggerHitReaction);
            
            characterMeshRenderer.material = matHit;

            DOTween.Sequence().SetDelay(.4f).onComplete = () => { characterMeshRenderer.material = defaultMat; };
            
            // Apply damage
            if (healthSystem.TakeDamage(damage) <= 0)
            {
                if (isPawnDead) return;
                isPawnDead = true;
                HandlePawnDeath();
            }
        }

        public void HandlePawnDeath()
        {
            isPawnDead = true;
            // Handle animation
            pawnAnimator.SetBool(AnimatorDeadBool, true);
        }

        public void HandleReloadSelection(int slotIndex)
        {
            PawnInventorySlot targetSlot = pawnInventory.inventorySlots[slotIndex];

            Ammo ammo = targetSlot.GetAmmoFromSlot();

            if (ammo == null) return;
            
            GameEvents.instance.OnPlayerInventoryChanged(slotIndex);

            bool result = weapon.ReloadAmmo(ammo);
            
            if(!result) targetSlot.AddAmmoToSlot(ammo);
        }

        public void RollLeft()
        {
            if (isPawnDead) return;

            pawnAnimator.SetTrigger(AnimatorTriggerRollLeft);
        }

        public void RollRight()
        {
            if (isPawnDead) return;
            
            pawnAnimator.SetTrigger(AnimatorTriggerRollRight);
        }
        
        public void RollLeftAnimation()
        {
            Transform playerParent = transform.parent; 
            playerParent.DOMoveX(playerParent.position.x - 5f, .5f).SetEase(Ease.OutQuad);
        }

        public void RollRightAnimation()
        {
            Transform playerParent = transform.parent; 
            playerParent.DOMoveX(playerParent.position.x + 5f, 0.4f).SetEase(Ease.OutQuad);
        }
    }

    
    [Serializable]
    public class PawnInventorySystem
    {
        public PawnInventorySlot[] inventorySlots;
        public int maxBulletsAllowed;
        public int coins;
        
        public PawnInventorySystem()
        {
            inventorySlots = new PawnInventorySlot[4];

            // Setup slots
            inventorySlots[0] = new PawnInventorySlot(GameElementColor.Red);
            inventorySlots[1] = new PawnInventorySlot(GameElementColor.Yellow);
            inventorySlots[2] = new PawnInventorySlot(GameElementColor.Blue);
            inventorySlots[3] = new PawnInventorySlot(GameElementColor.Black);
            
            // Setup data
            maxBulletsAllowed = 6;
            coins = 0;
        }

        public void AddItemToSlot(int slotIndex, Ammo ammo)
        {
            if (GetTotalAmmoAmount() >= maxBulletsAllowed)
            {
                Debug.LogWarning("You have reached the max amount of bullets you can carry!");
                return;
            }
            
            inventorySlots[slotIndex].AddAmmoToSlot(ammo);
            GameEvents.instance.OnPlayerInventoryChanged(slotIndex);
        }

        public int GetTotalAmmoAmount()
        {
            int counter = 0;
            
            foreach (var slot in inventorySlots)
            {
                counter += slot.GetCurrentBulletAmount();
            }

            return counter;
        }

        public void IncreaseMaxSize(int increaseAmount)
        {
            maxBulletsAllowed += increaseAmount;
        }
    }

    [Serializable]
    public class PawnInventorySlot
    {
        public GameElementColor slotColor;
        public Queue<Ammo> ammoInSlot;

        public PawnInventorySlot()
        {
            slotColor = GameElementColor.NotDefined;
            ammoInSlot = new Queue<Ammo>();
        }

        public PawnInventorySlot(GameElementColor color)
        {
            slotColor = color;
            ammoInSlot = new Queue<Ammo>();
        }

        public int GetCurrentBulletAmount()
        {
            return ammoInSlot.Count;
        }

        public void AddAmmoToSlot(Ammo ammo)
        {
            ammoInSlot.Enqueue(ammo);
        }

        public Ammo GetAmmoFromSlot()
        {
            if (GetCurrentBulletAmount() <= 0)
            {
                Debug.LogWarning("There is no ammo left in this slot");
                return null;
            }

            return ammoInSlot.Dequeue();
        }
    }
}
