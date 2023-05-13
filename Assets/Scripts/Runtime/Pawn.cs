using System;
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
        public PawnInventory pawnInventory;

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

        private bool isPawnDead;

        private void Start()
        {
            InitPawnSystem();
            InitWeaponSystem();
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

        public void Fire(Vector3 target)
        {
            if (isPawnDead) return;
            
            if (currentPhaseState != CharacterPhaseState.AimingPhase) return;

            // FIRE!!!
            weapon.Fire(target);
            pawnAnimator.ResetTrigger(AnimatorTriggerFire);
            pawnAnimator.SetTrigger(AnimatorTriggerFire);
            //pawnAnimator.ResetTrigger(AnimatorTriggerFire);
        }

        public void EnemyFire(Vector3 target)
        {
            if (currentPhaseState != CharacterPhaseState.AimingPhase) return;

            // FIRE!!!
            weapon.EnemyWeaponFire(target);
            pawnAnimator.ResetTrigger(AnimatorTriggerFire);
            pawnAnimator.SetTrigger(AnimatorTriggerFire);
        }

        public CharacterPhaseState GetPawnCurrentState()
        {
            return currentPhaseState;
        }

        public void TakeDamage(Ammo ammo)
        {
            pawnAnimator.SetTrigger(AnimatorTriggerHitReaction);
            
            if (healthSystem.TakeDamage(ammo.initDamage) <= 0)
            {
                if (isPawnDead) return;
                isPawnDead = true;
                HandlePawnDeath();
            }
        }

        public void HandlePawnDeath()
        {
            pawnAnimator.SetTrigger(AnimatorTriggerDeath);
            DOTween.Sequence().SetDelay(.4f).onComplete = () => { pawnAnimator.SetBool(AnimatorDeadBool, true); };
        }

        public void HandleReloadSelection(int slotIndex)
        {
            Ammo ammo;
            PawnInventoryItem item;
            
            switch (slotIndex)
            {
                case 0:
                    item = pawnInventory.itemSlotA;
                    // TODO - Fix Ammo Data
                    ammo = new Ammo(item.itemColor, 50f);
                    if (item.amount > 0 && weapon.ReloadAmmo(ammo))
                    {
                        pawnInventory.itemSlotA.amount--;
                    }
                    
                    break;
                
                case 1:
                    item = pawnInventory.itemSlotB;
                    ammo = new Ammo(item.itemColor, 50f);
                    if (item.amount > 0 && weapon.ReloadAmmo(ammo))
                    {
                        pawnInventory.itemSlotB.amount--;
                    }
                    break;
                
                case 2:
                    item = pawnInventory.itemSlotC;
                    ammo = new Ammo(item.itemColor, 50f);
                    if (item.amount > 0 && weapon.ReloadAmmo(ammo))
                    {
                        pawnInventory.itemSlotC.amount--;
                    }
                    break;
                
                case 3:
                    item = pawnInventory.itemSlotD;
                    ammo = new Ammo(item.itemColor, 50f);
                    if (item.amount > 0 && weapon.ReloadAmmo(ammo))
                    {
                        pawnInventory.itemSlotD.amount--;
                    }
                    break;
                
                default:
                    return;
            }
        }

        public void RollLeft()
        {
            DOTween.Sequence().SetDelay(.6f).onComplete = () =>
            { 
                Transform playerParent = transform.parent; 
                playerParent.DOMoveX(playerParent.position.x - 5f, 1.1f).SetEase(Ease.OutQuad);
            };
            
            pawnAnimator.SetTrigger(AnimatorTriggerRollLeft);
        }

        public void RollRight()
        {
            DOTween.Sequence().SetDelay(.65f).onComplete = () =>
            { 
                Transform playerParent = transform.parent; 
                playerParent.DOMoveX(playerParent.position.x + 5f, 0.95f).SetEase(Ease.OutQuad);
            };
            
            pawnAnimator.SetTrigger(AnimatorTriggerRollRight);
        }
    }

    [Serializable]
    public struct PawnInventory
    {
        public PawnInventoryItem itemSlotA;
        public PawnInventoryItem itemSlotB;
        public PawnInventoryItem itemSlotC;
        public PawnInventoryItem itemSlotD;

        public void AddItemToSlot(int slotIndex)
        {
            switch (slotIndex)
            {
                case 0:
                    itemSlotA.amount += 1;
                    break;
                
                case 1:
                    itemSlotB.amount += 1;
                    break;
                
                case 2:
                    itemSlotC.amount += 1;
                    break;
                
                case 3:
                    itemSlotD.amount += 1;
                    break;
                
                default:
                    return;
            }
        }
    }

    [Serializable]
    public struct PawnInventoryItem
    {
        public GameElementColor itemColor;
        public int amount;
    }
}
