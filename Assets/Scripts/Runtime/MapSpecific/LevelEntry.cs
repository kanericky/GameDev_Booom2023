using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Runtime.MapSpecific
{
    public class LevelEntry : MonoBehaviour
    {
        public int levelIndex;

        private void OnMouseEnter()
        {
            
        }

        private void OnMouseDown()
        {
            SceneManager.LoadScene(levelIndex);
        }

        private void OnMouseOver()
        {
            Debug.LogAssertion("1");
        }
    }
}
