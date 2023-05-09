using System;
using UnityEngine;

namespace Runtime
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;
        
        [Header("Materials")]
        public Material matRed;
        public Material matYellow;
        public Material matBlue;
        public Material matPurple;

        private void Start()
        {
            instance = this; 
        }
    }
}
