using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime
{
    public class UIManager : MonoBehaviour
    {
        [Header("HUD")] 
        public Canvas debugMenu;
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

        public void ChangeDebugText(string text)
        {
            debugText.text = text;
        }

        public void ChangeTimeDebugText(string text)
        {
            debugTextTime.text = text;
        }

        public void OpenReloadUIWidget()
        {
            widgetImage.enabled = true;
        }

        public void CloseReloadUIWidget()
        {
            widgetImage.enabled = false;
        }
        
    }
}
