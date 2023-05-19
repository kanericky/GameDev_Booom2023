using DG.Tweening;
using UnityEngine;

namespace Runtime.Menu
{
    public class MenuUIManager : MonoBehaviour
    {
        [Header("Reference")] public CanvasGroup menuCanvasGroup;

        private void Start()
        {
            menuCanvasGroup.alpha = 0;
        }

        public void MenuUIEnterReady()
        {
            menuCanvasGroup.DOFade(1, .2f);
        }

        public void QuitGame()
        {
            Debug.Log("Player quit game");
            Application.Quit();
        }
    }
}
