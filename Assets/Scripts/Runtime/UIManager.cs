using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime
{
    public class UIManager : MonoBehaviour
    {
        [Header("HUD - Canvas")] 
        public Canvas debugMenu;
        public Canvas hudMenu;

        [Header("HUD - Elements")]
        public Image playerHealth;
        
        [Header("HUD - Elements - Debug")]
        public TMP_Text debugText;
        public TMP_Text debugTextTime;

        [Header("In-game Reload Phase UI")] 
        public Image widgetImage;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                debugMenu.enabled = !debugMenu.enabled;
            }
        }

        private void Start()
        {
            GameEvents.instance.PlayerHealthChanged += UpdatePlayerHealthHUDBar;
        }

        public void UpdatePlayerHealthHUDBar(float ratio)
        {
            playerHealth.transform.DOScaleX(ratio, .4f);
        }

        public void OpenReloadUIWidget()
        {
            widgetImage.enabled = true;
        }

        public void CloseReloadUIWidget()
        {
            widgetImage.enabled = false;
        }
        
        // ------------ Debug ------------ //
        public void ChangeDebugText(string text)
        {
            debugText.text = text;
        }

        public void ChangeTimeDebugText(string text)
        {
            debugTextTime.text = text;
        }
        
    }
}
