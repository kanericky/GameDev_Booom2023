using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace Runtime.MapSpecific
{
    public class LevelEntry : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public GameObject description;

        private void Start()
        {
            description.SetActive(false);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            description.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            description.SetActive(false);
        }
    }
}
