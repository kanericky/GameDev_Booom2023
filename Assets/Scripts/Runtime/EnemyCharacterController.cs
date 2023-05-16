using System;
using DG.Tweening;
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

        [Header("Weapon")] public Weapon enemyWeapon;

        [Header("Reference")]
        [SerializeField] private Pawn enemyPawn;
        [SerializeField] private Transform aimTarget;
        [SerializeField] private Renderer characterRenderer;
        [SerializeField] private Material highLightMaterial;
        [SerializeField] private Material defaultMaterial;

        private void Start()
        {
            InitEnemy();

            playerCharacter = FindObjectOfType<GameCharacterController>();
        }

        private void InitEnemy()
        {
            // Get weapon
            enemyWeapon = GetComponentInChildren<Weapon>();
            
            // Get Pawn
            enemyPawn = GetComponent<Pawn>();
            
            // Set Health
            enemyPawn.healthSystem = new HealthSystem(enemyData.health);
            
            // Set Material
            characterRenderer.material = enemyData.defaultMaterial;
            highLightMaterial = enemyData.highlightMaterial;
            defaultMaterial = enemyData.defaultMaterial;
        }

        public void OnCharacterHit(float damage)
        {
            enemyPawn.TakeDamage(damage);
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
            // TODO AI Accuracy
            
            aimTarget.position = playerCharacter.transform.position;
            aimTarget.rotation = playerCharacter.transform.rotation;
            
            enemyPawn.EnemyFire(playerCharacter.transform.position);
        }

    }
}
