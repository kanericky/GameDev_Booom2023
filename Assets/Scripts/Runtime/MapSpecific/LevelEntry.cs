using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Runtime.MapSpecific
{
    public class LevelEntry : MonoBehaviour
    {
        public void LoadScene(int levelIndex)
        {
            SceneManager.LoadScene(levelIndex);
        }
    }
}
