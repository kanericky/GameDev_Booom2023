using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Runtime.DropItemSystemFramework
{
    public class DropItemUIModel : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("Data")] 
        public DropItemConfigSO dropItemConfig;

        [Header("UI Reference")] 
        public TMP_Text coinAmount;
        public TMP_Text ammoAmount;
        public TMP_Text ammoColorText;
        public Image dropItemIcon;
        public Image dropItemHighlight;

        private string staticText = "子弹 +";
        private string staticTextForRandom = "随机";

        public void InitDropItemUI(DropItemConfigSO dropItemConfigInput)
        {
            dropItemConfig = dropItemConfigInput;

            coinAmount.text = dropItemConfig.coinAmount.ToString();
            ammoAmount.text = dropItemConfig.ammoAmount.ToString();
            
            if (!dropItemConfig.isRandom) ammoColorText.text = GameManager.GetTextBasedOnAmmoColor(dropItemConfig.ammoColor) + staticText;
            else ammoColorText.text = staticTextForRandom + staticText;

            dropItemIcon.sprite = dropItemConfig.ammoIconSprite;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            GameCharacterController playerController = GameManager.instance.playerController;
            PawnInventorySystem playerInventory = playerController.playerPawn.pawnInventory;

            GameElementColor ammoColor = dropItemConfig.ammoColor;
            int ammoAmountNum = dropItemConfig.ammoAmount;
            
            int sloIndex = GameManager.GetReloadSlotIndexBasedOnAmmoColor(ammoColor);
            Ammo ammoToAdd = AmmoFactory.GetAmmoFromFactory(ammoColor);
            
            for(int i = 0; i < ammoAmountNum; i++) playerInventory.AddItemToSlot(slotIndex: sloIndex, ammoToAdd);
            
            UIManager.instance.CloseDropItemCanvas();
            UIManager.instance.TransitionOutro();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            dropItemHighlight.gameObject.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            dropItemHighlight.gameObject.SetActive(false);
        }
    }
}
