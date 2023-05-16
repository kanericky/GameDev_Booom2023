using System;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

namespace Runtime
{
    public class Bullet : MonoBehaviour
    {
        private TrailRenderer traceLineRenderer;
        private MeshRenderer bulletMeshRenderer;
        private MeshCollider bulletMeshCollider;

        private Ammo bulletData;

        private float initDamage;
        private float finalDamage;
        private float bulletSpeed;

        private GameElementColor bulletColor;

        private bool startProjectile = false;
        private Vector3 direction;
        
        private void Awake()
        {
            InitReference();
            InitBulletData();
        }

        private void InitReference()
        {
            traceLineRenderer = GetComponent<TrailRenderer>();
            bulletMeshRenderer = GetComponentInChildren<MeshRenderer>();
            bulletMeshCollider = GetComponentInChildren<MeshCollider>();
        }

        public void InitBulletData(Ammo ammo = null)
        {
            if (!traceLineRenderer.Equals(null)) { traceLineRenderer.enabled = false; }

            if (!bulletMeshCollider.Equals(null)) { bulletMeshCollider.enabled = false; }

            if (ammo != null)
            {

                bulletData = ammo;
                
                // Set data
                initDamage = bulletData.initDamage;
                bulletColor = bulletData.gameElementColor;
                bulletSpeed = bulletData.ammoProjectileSpeed;

                // Set material
                if (bulletMeshRenderer != null) { bulletMeshRenderer.material = ammo.GetMaterialBasedOnAmmoColor(bulletColor); }
            }

            startProjectile = false;
        }

        public void BulletFire(Vector3 startPos, Vector3 direction)
        {
            // Move the bullet to the muzzle
            transform.position = startPos;
            transform.parent = null;
            
            // Enable collider for hit detection
            bulletMeshCollider.enabled = true;
            
            // Enable Trace Renderer
            traceLineRenderer.enabled = true;
            
            // Handle bullet movements
            this.direction = direction;
            this.startProjectile = true;
        }

        private void Update()
        {
            if (!startProjectile) return;

            if (direction.Equals(null)) return;
            
            transform.position += direction * Time.deltaTime * bulletSpeed;
        }

        private float CalculateDamage()
        {
            // TODO
            return 50f;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                EnemyCharacterController enemy = collision.transform.GetComponentInParent<EnemyCharacterController>();
                
                // Hit enemy
                finalDamage = CalculateDamage();
                
                // Apply damage
                enemy.OnCharacterHit(finalDamage);
                
                Debug.Log("Hit enemy");
            }

            if (collision.gameObject.CompareTag("Player"))
            {
                GameCharacterController player = collision.transform.GetComponentInParent<GameCharacterController>();
                
                // Hit player
                finalDamage = CalculateDamage();
                
                // Apply damage
                player.HandleHit(finalDamage);
                
                Debug.Log("Hit player");
            }
            
            Destroy(transform.gameObject);
        }
    
    }
}
