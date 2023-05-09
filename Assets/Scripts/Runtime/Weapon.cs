using System;
using UnityEngine;

namespace Runtime
{
    public class Weapon : MonoBehaviour
    {
        public int magSize;

        public Ammo[] ammoInMag;

        public ParticleSystem particleSystem;
        
        [SerializeField] private int _numAmmoSlotFilled;


        private void Start()
        {
            ammoInMag = new Ammo[magSize];
            _numAmmoSlotFilled = 0;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                Ammo ammo = new Ammo(isRandom: true);
                ReloadAmmo(ammo);
            }
        }


        public void ReloadAmmo(int magSlotIndex, Ammo ammo)
        {
            if (_numAmmoSlotFilled >= magSize)
            {
                Debug.LogWarning("The mag is full, cannot equip ammo");
                return;
            }

            if (magSlotIndex >= magSize)
            {
                Debug.LogAssertion(" Index of mag slot index is invalid!");
                return;
            }
            
            // EquipAmmo
            ammoInMag[magSlotIndex] = ammo;
            
            _numAmmoSlotFilled += 1;
        }

        public void ReloadAmmo(Ammo ammo)
        {
            if (_numAmmoSlotFilled >= magSize)
            {
                Debug.LogWarning("The mag is full, cannot equip ammo");
                return;
            }

            for (int i = 0; i < magSize; i++)
            {
                if (!ammoInMag[i].isAmmoValid())
                {
                    ammoInMag[i] = ammo;
                    _numAmmoSlotFilled += 1;
                    break;
                }
            }
        }

        
        public void Fire()
        {
            particleSystem.Stop();
            particleSystem.Play();
            
            if (_numAmmoSlotFilled <= 0)
            {
                Debug.LogWarning("There is no ammo left");
                return;
            }
            
            // Fire logic
            ammoInMag[_numAmmoSlotFilled-1] = null;
            _numAmmoSlotFilled -= 1;
        }

        private void PlayVFX()
        {
            particleSystem.transform.SetParent(null);
            
            particleSystem.Stop();
            particleSystem.Play();
            
            particleSystem.transform.SetParent(this.transform);
        }
        
    }
}
