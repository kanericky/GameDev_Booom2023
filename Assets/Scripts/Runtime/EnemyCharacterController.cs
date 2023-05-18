using System;
using DG.Tweening;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Runtime
{

    [RequireComponent(typeof(Pawn))]
    public class EnemyCharacterController : MonoBehaviour
    {
        [Header("Player Reference")] 
        public GameCharacterController playerCharacter;

        [Header("Enemy Data")] 
        public EnemyConfigSO enemyData;

        [Header("Weapon")] 
        public Weapon enemyWeapon;

        [Header("Reference")] 
        [SerializeField] private GameManager gameManager;
        [SerializeField] private Pawn enemyPawn;
        [SerializeField] private Animator enemyAnimator;
        [SerializeField] private EnemyHealthBar enemyHealthBar;
        [SerializeField] private AnimatorController behaviourAnimator;
        [SerializeField] private Transform aimTarget;
        [SerializeField] private Renderer characterRenderer;
        [SerializeField] private Material highLightMaterial;
        [SerializeField] private Material defaultMaterial;

        private float characterHeadOffset = 3f; 
        private float characterChestOffset = 2.2f; 
        private float characterLegOffset = 1f; 

        private void Start()
        {
            InitEnemy();

            playerCharacter = FindObjectOfType<GameCharacterController>();
        }

        private void InitEnemy()
        {
            // Get game manager
            gameManager = FindObjectOfType<GameManager>();
            
            // Get health bar UI component
            enemyHealthBar = GetComponent<EnemyHealthBar>();
            
            // Get animator
            enemyAnimator = GetComponent<Animator>();
            
            // Get weapon
            enemyWeapon = GetComponentInChildren<Weapon>();
            
            // Get Pawn
            enemyPawn = GetComponent<Pawn>();

            if (enemyData.Equals(null)) return;
            
            // Set Health
            enemyPawn.healthSystem = new HealthSystem(enemyData.health);

            enemyHealthBar.InitHealthBar(enemyPawn.healthSystem.GetHealthInPercentage());
            
            // Set Armor
            if (enemyData.helmetArmor != null && enemyData.chestArmor != null)
            {
                enemyPawn.healthSystem =
                    new HealthSystem(enemyData.health, enemyData.chestArmor, enemyData.helmetArmor);
            }else if (enemyData.helmetArmor != null)
            {
                enemyPawn.healthSystem =
                    new HealthSystem(enemyData.health, enemyData.helmetArmor);
                
            }else if (enemyData.chestArmor != null)
            {
                enemyPawn.healthSystem =
                    new HealthSystem(enemyData.health, enemyData.chestArmor);
            }
            else { enemyPawn.healthSystem = new HealthSystem(enemyData.health); }
            
            enemyHealthBar.InitHealthBar(enemyPawn.healthSystem.GetHealthInPercentage());


            // Set Material
            characterRenderer.material = enemyData.defaultMaterial;
            highLightMaterial = enemyData.highlightMaterial;
            defaultMaterial = enemyData.defaultMaterial;

            behaviourAnimator = enemyData.enemyBehaviourAnimator;
            enemyAnimator.runtimeAnimatorController = behaviourAnimator;
        }

        public void OnCharacterHit(float damage)
        {
            enemyPawn.TakeDamage(damage);
            enemyHealthBar.UpdateHealthBar(enemyPawn.healthSystem.GetHealthInPercentage());
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
            int randomCharacterTarget = Random.Range(0, 3);

            Vector3 finalShootPos = new Vector3();
            Transform playerTrans = playerCharacter.transform;

            Vector3 offset = new Vector3(
                Random.Range(-enemyData.enemyAccuracy.x, enemyData.enemyAccuracy.x),
                Random.Range(-enemyData.enemyAccuracy.y, enemyData.enemyAccuracy.y),
                Random.Range(-enemyData.enemyAccuracy.z, enemyData.enemyAccuracy.z)
                );

            switch (randomCharacterTarget)
            {
                case 0:
                    finalShootPos = playerTrans.position + new Vector3(0, characterHeadOffset, 0) + offset;
                    break;
                case 1:
                    finalShootPos = playerTrans.position + new Vector3(0, characterChestOffset, 0) + offset;
                    break;
                case 2:
                    finalShootPos = playerTrans.position + new Vector3(0, characterLegOffset, 0) + offset;
                    break;
            }
            
            aimTarget.position = playerTrans.position;
            aimTarget.rotation = playerTrans.rotation;
            
            enemyPawn.EnemyFire(finalShootPos, AmmoFactory.GetAmmoFromFactory(enemyData.ammoType));
        }
        
        public void ChangeCharacterMatToHit()
        {
            characterRenderer.material = gameManager.matHit;
        }

        public void ChangeCharacterMatToNormal()
        {
            characterRenderer.material = defaultMaterial;
        }

    }
}
