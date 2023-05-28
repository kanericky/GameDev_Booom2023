using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.ShopSystemFramework
{
    public class ShopManager : MonoBehaviour
    {
        [Serializable]
        public struct ShopItemUI
        {
            public TMP_Text price;
            public TMP_Text storageAmount;
        }
        
        public ShopItem[] shopItems;
        public ShopItemUI[] shopItemUI;

        public void Start()
        {
            SetupShop();
        }

        public void SetupShop()
        {
            for (int i = 0; i < 4; i++)
            {
                shopItemUI[i].price.text = shopItems[i].singleAmmoPrice.ToString();
                shopItemUI[i].storageAmount.text = shopItems[i].storageAmount.ToString();
            }
        }

        public void BuyAmmo(int index)
        {
            if (shopItems[index].storageAmount < 0) return;
            if (ShopLevelManager.instance.playerInventory.coins - shopItems[index].singleAmmoPrice < 0) return;

            ShopLevelManager.instance.playerInventory.coins -= shopItems[index].singleAmmoPrice;

            Ammo ammo = AmmoFactory.GetAmmoFromFactory(index);

            shopItems[index].singleAmmoPrice += 2;
            shopItems[index].storageAmount -= 1;
            SetupShop();

            ShopLevelManager.instance.playerInventory.AddItemToSlot(index, ammo);
            ShopUIManager.instance.RefreshAmmoStatusUI(0);
        }
        
        public void QuitShop()
        {
            GameManager.instance.SaveCurrentStatusInfo(ShopLevelManager.instance.playerInventory, GameManager.instance.playerHealthSystem);
        }
    }

    [Serializable]
    public class ShopItem
    {
        public AmmoType ammoType;
        public int singleAmmoPrice;
        public int storageAmount;
    }
}
