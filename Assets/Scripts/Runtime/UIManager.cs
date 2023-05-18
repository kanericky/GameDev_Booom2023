using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime
{
    public class UIManager : MonoBehaviour
    {
        [Header("Game Manager Reference")] 
        [SerializeField] private GameManager gameManager;
        
        [Header("HUD - Canvas")] 
        public Canvas debugMenuCanvas;
        public Canvas hudCanvas;
        public Canvas dropMenuCanvas;

        [Header("HUD - Elements")]
        public Image playerHealth;
        
        [Header("HUD - Elements - Debug")]
        public TMP_Text debugText;
        public TMP_Text debugTextTime;

        [Header("In-game Reload Phase UI")] 
        public GameObject reloadUI;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                debugMenuCanvas.enabled = !debugMenuCanvas.enabled;
            }
        }

        private void Start()
        {
            gameManager = FindObjectOfType<GameManager>();

            dropMenuCanvas.enabled = false;
            
            // Register events
            GameEvents.instance.PlayerHealthChanged += UpdatePlayerHealthHUDBar;
        }

        private void UpdatePlayerHealthHUDBar(float ratio)
        {
            playerHealth.transform.DOScaleX(ratio, .4f);
        }

        public void OpenReloadUIWidget()
        {
            reloadUI.SetActive(true);
        }

        public void CloseReloadUIWidget()
        {
            reloadUI.SetActive(false);
        }

        public void ShowDropItemInterface()
        {
            dropMenuCanvas.enabled = true;
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
