using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Runtime.ShopSystemFramework
{
    public class ShopUIManager : MonoBehaviour
    {
        public static ShopUIManager instance;
        
        [Header("HUD - Ammo Status")] 
        public TMP_Text[] ammoStatusText;
        public TMP_Text totalAmmo;
        public TMP_Text maxAmmo;

        public TMP_Text currentCoin;
        
        [Header("HUD - Transition")] 
        public Transform mask;


        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            RefreshAmmoStatusUI(0);
        }

        public void RefreshAmmoStatusUI(int index)
        {
            PawnInventorySystem playerInventory = GameManager.instance.playerInventory;
            
            for (int i = 0; i < 4; i++)
            {
                ammoStatusText[i].text = playerInventory.inventorySlots[i]
                    .GetCurrentBulletAmount().ToString();
            }

            maxAmmo.text = playerInventory.maxBulletsAllowed.ToString();
            totalAmmo.text = playerInventory.GetTotalAmmoAmount().ToString();

            currentCoin.text = playerInventory.coins.ToString();
        }
        
        public void TransitionOutro()                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 
        {
            mask.DOScale(Vector3.zero, .5f);
        }

        public void TransitionIntro()
        {
            mask.DOScale(new Vector3(2,2, 2), .5f);
        }
    }
}
