using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Runtime.DropItemSystem
{
    public class DropItemUIModel : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("Data")] 
        public DropItemConfigSO dropItemConfig;

        [Header("UI Reference")] 
        public TMP_Text coinAmount;
        public TMP_Text ammoAmount;
        public Image dropItemIcon;
        public Image dropItemHighlight;

        public void InitDropItemUI(DropItemConfigSO dropItemConfigInput)
        {
            dropItemConfig = dropItemConfigInput;

            coinAmount.text = dropItemConfig.coinAmount.ToString();
            ammoAmount.text = dropItemConfig.ammoAmount.ToString();

            dropItemIcon.sprite = dropItemConfig.ammoIconSprite;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.LogAssertion("Mouse Pressed!");
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
