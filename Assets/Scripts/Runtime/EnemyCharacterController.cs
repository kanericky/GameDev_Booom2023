using System;
using UnityEngine;

namespace Runtime
{

    [RequireComponent(typeof(Pawn))]
    public class EnemyCharacterController : MonoBehaviour
    {
        [Header("Animation")] 
        [SerializeField] private Pawn characterPawn;

        [Header("Collision")] 
        public Collider bodyCollider;
        public Collider headCollider;
        public Collider leftArmCollider;
        public Collider rightArmCollider;

        private void Start()
        {
            characterPawn = GetComponent<Pawn>();
        }

        public void OnCharacterHit()
        {
            characterPawn.TakeDamage();
        }

    }
}
