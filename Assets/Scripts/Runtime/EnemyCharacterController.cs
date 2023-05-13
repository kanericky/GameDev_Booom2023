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
        [SerializeField] private Renderer characterRenderer;
        [SerializeField] private Material highLightMaterial;
        [SerializeField] private Material defaultMaterial;

        [Header("Collision")] 
        public Collider bodyCollider;
        public Collider headCollider;
        public Collider leftArmCollider;
        public Collider rightArmCollider;

        private void Start()
        {
            playerCharacter = FindObjectOfType<GameCharacterController>();
            enemyPawn = GetComponent<Pawn>();

            defaultMaterial = characterRenderer.material;
            
            Attack();
        }

        public void OnCharacterHit(Ammo ammo)
        {
            enemyPawn.TakeDamage(ammo);
        }

        public void EnableHighlight()
        {
            if(characterRenderer.material != highLightMaterial) characterRenderer.material = highLightMaterial;
        }

        public void DisableHighlight()
        {
            if (characterRenderer.material != defaultMaterial) characterRenderer.material = defaultMaterial;
        }

        private void Attack()
        {

            DOTween.Sequence().SetDelay(Random.Range(1f, 4f)).onComplete = () =>
            {
                enemyPawn.EnemyEnterReloadingState();
                DOTween.Sequence().SetDelay(1f).onComplete = () =>
                {
                    enemyPawn.EnterAimingState();
                    DOTween.Sequence().SetDelay(1f).onComplete = () =>
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            enemyPawn.EnemyFire(playerCharacter.transform.position);
                        }
                    };
                };
            };
        }

    }
}
