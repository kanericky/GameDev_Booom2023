using UnityEngine;

namespace Runtime.AmmoSystem
{
    public class Bullet : MonoBehaviour
    {
        private TrailRenderer traceLineRenderer;
        private MeshRenderer bulletMeshRenderer;
        private MeshCollider bulletMeshCollider;

        private Ammo bulletData;

        private float initDamage;
        private float currentDamage;
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
                currentDamage = initDamage;
                bulletColor = bulletData.gameElementColor;
                bulletSpeed = bulletData.ammoProjectileSpeed;
                
                // Trail color 
                traceLineRenderer.material = GameManager.GetMaterialBasedOnAmmoColor(bulletColor);

                // Set material
                if (bulletMeshRenderer != null) { bulletMeshRenderer.material = GameManager.GetMaterialBasedOnAmmoColor(bulletColor); }
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

        private float CalculateDamage(GameElementColor bulletColor, GameElementColor targetColor)
        {
            if (bulletColor.Equals(targetColor))
            {
                GameEvents.instance.OnCorrectImpact();
                return initDamage * 1.5f;
            }

            return initDamage;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                EnemyCharacterController enemyController = collision.transform.GetComponentInParent<EnemyCharacterController>();

                GameElementColor targetColor;
                
                if (enemyController.enemyData.helmetArmorData != null)
                {
                    targetColor = enemyController.enemyData.helmetArmorData.armorColor;
                }else if (enemyController.enemyData.chestArmorData != null)
                {
                    targetColor = enemyController.enemyData.chestArmorData.armorColor;
                }
                else
                {
                    targetColor = enemyController.enemyData.color;
                }

                // Hit enemy
                finalDamage = CalculateDamage(bulletColor, targetColor);
                
                // Apply damage
                enemyController.OnCharacterHit(finalDamage);
                
                Debug.Log("Hit enemy");
            }

            if (collision.gameObject.CompareTag("Player"))
            {
                GameCharacterController playerController = collision.transform.GetComponentInParent<GameCharacterController>();
                
                // Hit player
                finalDamage = CalculateDamage(bulletColor, playerController.playerData.color);
                
                // Apply damage
                playerController.HandleHit(finalDamage);
                
                Debug.Log("Hit player");
            }
            
            Destroy(transform.gameObject);
        }
    
    }
}
