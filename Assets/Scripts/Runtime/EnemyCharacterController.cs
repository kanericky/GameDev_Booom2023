using System;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Runtime
{

    [RequireComponent(typeof(Pawn))]
    public class EnemyCharacterController : MonoBehaviour
    {
        [Header("Player Reference")] 
        public GameCharacterController playerCharacter;

        [Header("Weapon")] public Weapon enemyWeapon;

        [Header("Reference")] 
        [SerializeField] private Pawn enemyPawn;

        [Header("Collision")] 
        public Collider bodyCollider;
        public Collider headCollider;
        public Collider leftArmCollider;
        public Collider rightArmCollider;

        private void Start()
        {
            playerCharacter = FindObjectOfType<GameCharacterController>();
            enemyPawn = GetComponent<Pawn>();
            
            Attack();
        }

        public void OnCharacterHit(Ammo ammo)
        {
            enemyPawn.TakeDamage(ammo);
        }

        private void Attack()
        {

            DOTween.Sequence().SetDelay(Random.Range(2f, 4f)).onComplete = () =>
            {
                enemyPawn.EnemyEnterReloadingState();
                DOTween.Sequence().SetDelay(2f).onComplete = () =>
                {
                    enemyPawn.EnterAimingState();
                    DOTween.Sequence().SetDelay(1f).onComplete = () =>
                    {
                        enemyPawn.EnemyFire(playerCharacter.transform.position);
                    };
                };
            };
        }

    }
}
