using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Runtime
{
    public class Weapon : MonoBehaviour
    {
        [Header("Weapon Info")]
        public int magSize;
        public Queue<Ammo> ammoInMag;

        [Header("Weapon GameObject Reference")]
        public Transform weaponMag;
        public Transform weaponMuz;

        public Transform weaponMagSlotA;
        public Transform weaponMagSlotB;
        public Transform weaponMagSlotC;
        public Transform weaponMagSlotD;
        public Transform weaponMagSlotE;
        public Transform weaponMagSlotF;

        public Transform ammoHolder;

        public Queue<GameObject> bullets;

        public float ammoSpawnPosOffset = 4f;
        public float ammoReloadSpeed = .6f;
        public float ammoClearSpeed = 1f;
        
        public GameObject ammoModel;

        [Header("VFX")]
        public ParticleSystem weaponFireParticle;
        
        [Header("Debug")]
        [SerializeField] private int _numAmmoSlotFilled;
        [SerializeField] private int _currentFireIndex; 


        private void Start()
        {
            WeaponInit();
        }

        /// <summary>
        ///  Setup the init values
        /// </summary>
        public void WeaponInit()
        {
            ammoInMag = new Queue<Ammo>(magSize);
            bullets = new Queue<GameObject>(magSize);

            for (int i = 0; i < magSize; i++)
            {
                Ammo ammo = new Ammo();
                ammoInMag.Enqueue(ammo);
            }
            
            _numAmmoSlotFilled = 0;
            _currentFireIndex = 0;
        }

        /// <summary>
        /// Clear all the bullets in the mag
        /// </summary>
        public void ClearMag()
        {
            ammoInMag.Clear();

            while (bullets.Count > 0)
            {
                GameObject obj = bullets.Dequeue();
                obj.transform.DOLocalMoveX(-0.2f, ammoClearSpeed).SetDelay(.8f).onComplete = () =>
                {
                    DOTween.Sequence().SetDelay(.2f).onComplete = () =>
                    {
                        // Handle mag clear animation
                        obj.transform.parent = ammoHolder;
                        obj.GetComponent<MeshCollider>().enabled = true;
                        obj.GetComponent<Rigidbody>().useGravity = true;
                        obj.GetComponent<Rigidbody>().AddForce(new Vector3(
                            Random.Range(-10, 10),
                            -1,
                            Random.Range(-20, 20)));
                        DOTween.Sequence().SetDelay(1f).onComplete = () => { Destroy(obj); };
                    };
                };

            }

            _numAmmoSlotFilled = 0;
            bullets.Clear();
        }

        
        /// <summary>
        /// Load ammo into the empty mag slot
        /// </summary>
        /// <param name="ammo"> What is being loaded</param>
        /// <returns> Success or not</returns>
        public bool ReloadAmmo(Ammo ammo)
        {
            // Mag is full, can no longer reload
            if (_numAmmoSlotFilled >= magSize)
            {
                Debug.LogWarning("The mag is full, cannot equip ammo");
                return false;
            }
            
            // Load logic, put the ammo into the queue
            ammoInMag.Enqueue(ammo);
            HandleReloadAnimation(_numAmmoSlotFilled, ammo);
            _numAmmoSlotFilled += 1;

            return true;
        }

        /// <summary>
        /// Handle weapon reload animation
        /// </summary>
        /// <param name="slotIndex"> Which mag slot is being filled?</param>
        /// <param name="ammo"> What is going to be reloaded? </param>
        private void HandleReloadAnimation(int slotIndex, Ammo ammo)
        {

            GameObject bullet = Instantiate(ammoModel);
            Renderer bulletRender = bullet.GetComponentInChildren<Renderer>();
            bullets.Enqueue(bullet);
            
            switch (slotIndex)
            {
                case 0:
                    bullet.transform.parent = weaponMagSlotA;
                    bullet.transform.localPosition = new Vector3(-ammoSpawnPosOffset,0,0);
                    bullet.transform.localRotation = Quaternion.Euler(Vector3.zero);

                    bulletRender.material = ammo.GetMaterialBasedOnAmmoColor(ammo.gameElementColor);

                    bullet.transform.DOLocalMove(Vector3.zero, ammoReloadSpeed);
                    
                    break;
                
                case 1:
                    bullet.transform.parent = weaponMagSlotB;
                    bullet.transform.localPosition = new Vector3(-ammoSpawnPosOffset,0,0);
                    bullet.transform.localRotation = Quaternion.Euler(Vector3.zero);
                    
                    bulletRender.material = ammo.GetMaterialBasedOnAmmoColor(ammo.gameElementColor);

                    bullet.transform.DOLocalMove(Vector3.zero, ammoReloadSpeed);
                    break;
                
                case 2:
                    bullet.transform.parent = weaponMagSlotC;
                    bullet.transform.localPosition = new Vector3(-ammoSpawnPosOffset,0,0);
                    bullet.transform.localRotation = Quaternion.Euler(Vector3.zero);
                    
                    bulletRender.material = ammo.GetMaterialBasedOnAmmoColor(ammo.gameElementColor);

                    bullet.transform.DOLocalMove(Vector3.zero, ammoReloadSpeed);
                    break;
                
                case 3:
                    bullet.transform.parent = weaponMagSlotD;
                    bullet.transform.localPosition = new Vector3(-ammoSpawnPosOffset,0,0);
                    bullet.transform.localRotation = Quaternion.Euler(Vector3.zero);
                    
                    bulletRender.material = ammo.GetMaterialBasedOnAmmoColor(ammo.gameElementColor);

                    bullet.transform.DOLocalMove(Vector3.zero, ammoReloadSpeed);
                    break;
                
                case 4:
                    bullet.transform.parent = weaponMagSlotE;
                    bullet.transform.localPosition = new Vector3(-ammoSpawnPosOffset,0,0);
                    bullet.transform.localRotation = Quaternion.Euler(Vector3.zero);
                    
                    bulletRender.material = ammo.GetMaterialBasedOnAmmoColor(ammo.gameElementColor);

                    bullet.transform.DOLocalMove(Vector3.zero, ammoReloadSpeed);
                    
                    break;
                
                case 5:
                    bullet.transform.parent = weaponMagSlotF;
                    bullet.transform.localPosition = new Vector3(-ammoSpawnPosOffset,0,0);
                    bullet.transform.localRotation = Quaternion.Euler(Vector3.zero);
                    
                    bulletRender.material = ammo.GetMaterialBasedOnAmmoColor(ammo.gameElementColor);

                    bullet.transform.DOLocalMove(Vector3.zero, ammoReloadSpeed);
                    
                    break;
                
                default:
                    return;
            }
        }

        /// <summary>
        /// Weapon fire logic
        /// </summary>
        public void Fire()
        {
            if (IsMagEmpty())
            {
                Debug.LogWarning("There is no ammo left");
                return;
            }
            
            // Setup VFX
            weaponFireParticle.transform.position = weaponMuz.position;
            weaponFireParticle.transform.rotation = weaponMuz.localRotation;

            // Fire logic
            Ammo ammo = ammoInMag.Dequeue();
            Destroy(bullets.Dequeue());
            _numAmmoSlotFilled -= 1;
            
            // Play VFX
            weaponFireParticle.Stop();
            weaponFireParticle.GetComponent<ParticleSystemRenderer>().material = ammo.GetMaterialBasedOnAmmoColor(ammo.gameElementColor);
            weaponFireParticle.Play();
            
        }

        /// <summary>
        /// Check whether the mag is empty or not
        /// </summary>
        /// <returns>Is mage empty result</returns>
        public bool IsMagEmpty()
        {
            return ammoInMag.Count <= 0;
        }
        
    }
}
