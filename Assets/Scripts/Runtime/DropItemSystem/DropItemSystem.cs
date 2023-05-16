using System.Collections.Generic;
using UnityEngine;

namespace Runtime.DropItemSystem
{
    public class DropItemSystem
    {
        [Header("Data")]
        [SerializeField] public List<DropItemConfigSO> allDrops;

        public DropItemSystem()
        {
            allDrops = new List<DropItemConfigSO>();
        }

        public List<DropItemConfigSO> SelectThreeRandomDrops()
        {
            // TODO Implement function
            return null;
        }
    }
}
