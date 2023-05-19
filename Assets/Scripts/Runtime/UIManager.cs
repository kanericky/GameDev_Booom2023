using System;
using DG.Tweening;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager instance;
     
        [Header("Game Manager Reference")] 
        [SerializeField] private GameManager gameManager;

        [Header("HUD - Canvas")] 
        public Canvas debugMenuCanvas;
        public Canvas hudCanvas;
        public Canvas dropMenuCanvas;

        [Header("HUD - Elements")] 
        public Image playerHealth;
        public Image playerArmor;

        public Transform magUI;
        public Image[] uiReloadSlots;

        [Header("HUD - Elements - Debug")] 
        public TMP_Text debugText;
        public TMP_Text debugTextTime;

        [Header("In-game Reload Phase UI")] 
        public GameObject reloadUI;
        public Image[] reloadButtons;

        private void Awake()
        {
            instance = this;
        }

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
            GameEvents.instance.PlayerArmorChanged += UpdatePlayerArmorHUDBar;
            
            GameEvents.instance.PlayerInventoryChanged += RefreshReloadUI;

            SetupReloadUI();
        }

        public void AddBullet(int slotIndex, Sprite bulletIcon)
        {
            magUI.DORotate(magUI.rotation.eulerAngles + new Vector3(0, 0, -65f), .1f).onComplete = () =>
            {
                magUI.DORotate(magUI.rotation.eulerAngles + new Vector3(0, 0, 5f), .1f);
            };
            
            uiReloadSlots[slotIndex].enabled = true;
            uiReloadSlots[slotIndex].sprite = bulletIcon;
            
            // Rotation
        }

        public void FireBullet(int slotIndex)
        {
            uiReloadSlots[slotIndex].sprite = null;
            uiReloadSlots[slotIndex].enabled = false;

            magUI.DORotate(magUI.rotation.eulerAngles + new Vector3(0, 0, 65f), .1f).onComplete = () =>
            {
                magUI.DORotate(magUI.rotation.eulerAngles + new Vector3(0, 0, -5f), .1f);
            };
        }

        public void ClearBulletUI()
        {
            for (int slotIndex = 0; slotIndex < 6; slotIndex++)
            {
                uiReloadSlots[slotIndex].sprite = null;
                uiReloadSlots[slotIndex].enabled = false;
            }
            magUI.rotation = Quaternion.Euler(0, 0, 0);
        }
    

        public void SetupReloadUI()
        {
            for (int i = 0; i < 4; i++)
            {
                if (gameManager.playerController.playerPawn.pawnInventory.inventorySlots[i].GetCurrentBulletAmount() ==
                    0)
                {
                    reloadButtons[i].color = Color.HSVToRGB(0, 0, .7f);
                }
            }

        }

        private void RefreshReloadUI(int index)
        {
            if (gameManager.playerController.playerPawn.pawnInventory.inventorySlots[index].GetCurrentBulletAmount() ==
                0)
            {
                reloadButtons[index].color = Color.HSVToRGB(0, 0, .7f);
            }
            else
            {
                reloadButtons[index].color = Color.HSVToRGB(0, 0, 1f);
            }
        }

        public void InitPlayerHUDHealthBar(float healthRatio, float armorRatio)
        {
            playerHealth.transform.localScale = new Vector3(healthRatio, 1f, 1f);
            playerArmor.transform.localScale = new Vector3(armorRatio, 1f, 1f);
        }

        private void UpdatePlayerHealthHUDBar(float ratio)
        {
            playerHealth.transform.DOScaleX(ratio, .4f);
        }

        private void UpdatePlayerArmorHUDBar(float ratio)
        {
            playerArmor.transform.DOScaleX(ratio, .4f);
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

        public void ReloadButtonPressedAnimation(int index)
        {
            Color currentColor = reloadButtons[index].color;
            Color targetColor = Color.HSVToRGB(0f, 0f, 0.7f);

            reloadButtons[index].DOColor(targetColor, .1f).onComplete = () =>
            {
                reloadButtons[index].DOColor(currentColor, .1f);
            };

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
