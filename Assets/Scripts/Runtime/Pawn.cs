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
        
        [Header("Debug")]
        [SerializeField] private CharacterPhaseState _currentPhaseState;
        [SerializeField] private float _currentHealth;

        private void Start()
        {
            _currentPhaseState = CharacterPhaseState.IdlePhase;
            _currentHealth = health;
        }

        public void EnterReloadingState()
        {
            if (_currentPhaseState != CharacterPhaseState.IdlePhase) return;
            
            Debug.Log("Start Reloading");
            _currentPhaseState = CharacterPhaseState.ReloadingPhase;
            
            // Reload
        }

        public void ExitReloadingState()
        {
            if (_currentPhaseState != CharacterPhaseState.ReloadingPhase) return;
            
            _currentPhaseState = CharacterPhaseState.IdlePhase;
        }
        

        public void EnterAimingState()
        {
            if (_currentPhaseState != CharacterPhaseState.ReloadingPhase) return;
            
            Debug.Log("Start Aiming");
            _currentPhaseState = CharacterPhaseState.AimingPhase;
            
            // Aiming
        }

        public void ExitAimingState()
        {
            if (_currentPhaseState != CharacterPhaseState.AimingPhase) return;
            
            _currentPhaseState = CharacterPhaseState.IdlePhase;
        }

        public void Fire()
        {
            if (_currentPhaseState != CharacterPhaseState.IdlePhase) return;

            Debug.Log("Fired!");
            _currentPhaseState = CharacterPhaseState.FiringPhase;
            
            // FIRE!!!
            weapon.Fire();

            _currentPhaseState = CharacterPhaseState.IdlePhase;
        }

        public CharacterPhaseState GetPawnCurrentState()
        {
            return _currentPhaseState;
        }
    }
}
