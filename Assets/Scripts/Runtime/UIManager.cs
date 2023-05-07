using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime
{
    public class UIManager : MonoBehaviour
    {
        public TMP_Text debugText;

        public void ChangeDebugText(string text)
        {
            debugText.text = text;
        }
        
    }
}
