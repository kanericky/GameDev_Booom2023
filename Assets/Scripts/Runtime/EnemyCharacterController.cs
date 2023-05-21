using System;
using Runtime.ArmorSystem;
using UnityEditor;
using UnityEngine;
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

        [Header("Reference")] 
        [SerializeField] private GameManager gameManager;
        
        [SerializeField] private Pawn enemyPawn;
        [SerializeField] private Animator enemyAnimator;
        [SerializeField] private EnemyHealthBar enemyHealthBar;
        [SerializeField] private RuntimeAnimatorController behaviourAnimator;
        
        [SerializeField] private Transform aimTarget;
        [SerializeField] private Transform helmetArmorSlot;
        [SerializeField] private Transform chestArmorSlot;
        
        [SerializeField] private Renderer characterRenderer;
        [SerializeField] private Material highLightMaterial;
        [SerializeField] private Material defaultMaterial;

        private float characterHeadOffset = 3f; 
        private float characterChestOffset = 2.2f; 
        private float characterLegOffset = 1f;

        private Armor helmetArmor;
        private Armor chestArmor;

        private bool isHighlighted;

        private void Start()
        {
            InitEnemy();
            InitEnemyHealthSystem();

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

            // Get Pawn
            enemyPawn = GetComponent<Pawn>();

            if (enemyData.Equals(null)) return;

            // Set Material
            characterRenderer.material = enemyData.defaultMaterial;
            highLightMaterial = enemyData.highlightMaterial;
            defaultMaterial = enemyData.defaultMaterial;

            behaviourAnimator = enemyData.enemyBehaviourAnimator;
            enemyAnimator.runtimeAnimatorController = behaviourAnimator;

            isHighlighted = false;
        }

        private void InitEnemyHealthSystem()
        {
            if (enemyData == null) return;
            
            ArmorData chestArmorData = enemyData.chestArmorData;
            ArmorData helmetArmorData = enemyData.helmetArmorData;
            
            if (helmetArmorData != null && chestArmorData != null)
            {
                enemyPawn.healthSystem =
                    new HealthSystem(enemyData.health, chestArmorData, helmetArmorData);
                
                helmetArmor = Instantiate(helmetArmorData.armorPrefab, helmetArmorSlot);
                helmetArmor.SetupArmor(helmetArmorData);
                
                chestArmor = Instantiate(chestArmorData.armorPrefab, chestArmorSlot);
                chestArmor.SetupArmor(chestArmorData);

            }else if (helmetArmorData != null)
            {
                enemyPawn.healthSystem =
                    new HealthSystem(enemyData.health, enemyData.helmetArmorData);
                
                helmetArmor = Instantiate(helmetArmorData.armorPrefab, helmetArmorSlot);
                helmetArmor.SetupArmor(helmetArmorData);
                
            }else if (chestArmorData != null)
            {
                enemyPawn.healthSystem =
                    new HealthSystem(enemyData.health, chestArmorData);
                
                chestArmor = Instantiate(chestArmorData.armorPrefab, chestArmorSlot);
                chestArmor.SetupArmor(chestArmorData);
            }
            else { enemyPawn.healthSystem = new HealthSystem(enemyData.health); }
            
            // Setup enemy UI
            HealthSystem enemyHealthSystem = enemyPawn.healthSystem;
            enemyHealthBar.InitHealthBar(enemyHealthSystem.GetHealthInPercentage(), enemyHealthSystem.currentHealth, enemyHealthSystem.healthAmount);
            enemyHealthBar.InitArmorBar(enemyPawn.healthSystem.GetArmorInPercentage());
        }

        public void OnCharacterHit(float damage)
        {
            HealthSystem enemyHealthSystem = enemyPawn.healthSystem;
            enemyPawn.TakeDamage(damage, characterRenderer, gameManager.matHit, defaultMaterial, false);
            enemyHealthBar.UpdateHealthBar(enemyHealthSystem.GetHealthInPercentage(), enemyHealthSystem.currentHealth, enemyHealthSystem.healthAmount);
            enemyHealthBar.UpdateArmorBar(enemyPawn.healthSystem.GetArmorInPercentage());
        }

        public void EnableHighlight()
        {
            if (isHighlighted) return;
            
            if(characterRenderer.material != highLightMaterial) characterRenderer.material = highLightMaterial;
            isHighlighted = true;
        }

        public void DisableHighlight()
        {
            if (characterRenderer.material != defaultMaterial) characterRenderer.material = defaultMaterial;
            isHighlighted = false;
        }

        // Used in animation to trigger fire
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
            
            aimTarget.position = finalShootPos;

            enemyPawn.EnemyFire(finalShootPos, AmmoFactory.GetAmmoFromFactory(enemyData.ammoType));
        }
    }
}
