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
            if (currentPhaseState != CharacterPhaseState.IdlePhase) return;
            
            Debug.Log("Start Reloading");
            
            pawnAnimator.SetTrigger(AnimatorTriggerReload);
            
            currentPhaseState = CharacterPhaseState.ReloadingPhase;
            
            // Reload
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

            Debug.Log("Fired!");

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
    }
}
