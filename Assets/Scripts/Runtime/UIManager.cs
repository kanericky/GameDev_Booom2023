using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime
{
    public class UIManager : MonoBehaviour
    {
        [Header("HUD")]
        public TMP_Text debugText;

        [Header("In-game Reload Phase UI")] 
        public Image widgetImage;
        
        public void ChangeDebugText(string text)
        {
            debugText.text = text;
        }

        public void HandleReloadPhaseUI()
        {
            
        }
        
    }
}
