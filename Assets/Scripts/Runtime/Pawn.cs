using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Runtime
{

    public class Pawn : MonoBehaviour
    {
        [Header("Pawn Attributes")] 
        public float health;
        
        [Header("Weapon")]
        public Weapon weapon;

        [Header("Inventory")] 
        public PawnInventory pawnInventory;

        [Header("Animator")] 
        public Animator pawnAnimator;
        
        [Header("Debug")]
        [SerializeField] private CharacterPhaseState currentPhaseState;
        [SerializeField] private float currentHealth;
        
        private static readonly string AnimatorTriggerReload = "Trigger Reload";
        private static readonly string AnimatorTriggerReset = "Reset Reload";
        private static readonly string AnimatorTriggerAim = "Trigger Aiming";
        private static readonly string AnimatorTriggerFire = "Fire";
        private static readonly string AnimatorTriggerResetToIdle = "Reset to Idle";
        private static readonly string AnimatorTriggerHitReaction = "Take Hit";

        private void Start()
        {
            pawnAnimator = GetComponent<Animator>();
            
            weapon = GetComponentInChildren<Weapon>();
            if (weapon != null) weapon.WeaponInit();
            
            currentPhaseState = CharacterPhaseState.IdlePhase;
            currentHealth = health;
        }

        private void ValidateData()
        {
            if (pawnAnimator == null)
            {
                Debug.LogAssertion("There is no animator on the pawn...");
            }
        }

        public void EnterReloadingState()
        {
            if (!(currentPhaseState is CharacterPhaseState.IdlePhase or CharacterPhaseState.AimingPhase)) return;

            Debug.Log("Start Reloading");
            
            pawnAnimator.SetTrigger(AnimatorTriggerReload);
            
            currentPhaseState = CharacterPhaseState.ReloadingPhase;
            
            // Step 1: Clear all the bullets in the mag.
            weapon.ClearMag();
            
            // Step 2: Handle reload input
        }

        public void ExitReloadingState()
        {
            if (currentPhaseState != CharacterPhaseState.ReloadingPhase) return;
            
            pawnAnimator.SetTrigger(AnimatorTriggerReset);
            
            currentPhaseState = CharacterPhaseState.IdlePhase;
        }
        

        public void EnterAimingState()
        {
            if (currentPhaseState != CharacterPhaseState.ReloadingPhase) return;
            
            Debug.Log("Start Aiming");
            
            pawnAnimator.SetTrigger(AnimatorTriggerAim);
            
            currentPhaseState = CharacterPhaseState.AimingPhase;
            
            // Aiming
        }

        public void ExitAimingState()
        {
            if (currentPhaseState != CharacterPhaseState.AimingPhase) return;
            
            pawnAnimator.SetTrigger(AnimatorTriggerResetToIdle);
            
            currentPhaseState = CharacterPhaseState.IdlePhase;
        }

        public void Fire()
        {
            if (currentPhaseState != CharacterPhaseState.AimingPhase) return;

            // FIRE!!!
            weapon.Fire();
            pawnAnimator.ResetTrigger(AnimatorTriggerFire);
            pawnAnimator.SetTrigger(AnimatorTriggerFire);
            //pawnAnimator.ResetTrigger(AnimatorTriggerFire);
        }

        public CharacterPhaseState GetPawnCurrentState()
        {
            return currentPhaseState;
        }

        public void TakeDamage()
        {
            pawnAnimator.SetTrigger(AnimatorTriggerHitReaction);
        }

        public void HandleReloadSelection(int slotIndex)
        {
            Ammo ammo;
            PawnInventoryItem item;
            
            switch (slotIndex)
            {
                case 0:
                    item = pawnInventory.itemSlotA;
                    ammo = new Ammo(item.itemColor);
                    if (item.amount > 0 && weapon.ReloadAmmo(ammo))
                    {
                        pawnInventory.itemSlotA.amount--;
                    }
                    
                    break;
                
                case 1:
                    item = pawnInventory.itemSlotB;
                    ammo = new Ammo(item.itemColor);
                    if (item.amount > 0 && weapon.ReloadAmmo(ammo))
                    {
                        pawnInventory.itemSlotB.amount--;
                    }
                    break;
                
                case 2:
                    item = pawnInventory.itemSlotC;
                    ammo = new Ammo(item.itemColor);
                    if (item.amount > 0 && weapon.ReloadAmmo(ammo))
                    {
                        pawnInventory.itemSlotC.amount--;
                    }
                    break;
                
                case 3:
                    item = pawnInventory.itemSlotD;
                    ammo = new Ammo(item.itemColor);
                    if (item.amount > 0 && weapon.ReloadAmmo(ammo))
                    {
                        pawnInventory.itemSlotD.amount--;
                    }
                    break;
                
                default:
                    return;
            }
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
