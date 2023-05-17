using System.Collections.Generic;
using UnityEngine;

namespace Runtime.DropItemSystem
{
    public class DropItemSystem
    {
        [Header("Data")]
        [SerializeField] private List<DropItemConfigSO> allDropItems;

        public DropItemSystem()
        {
            allDropItems = new List<DropItemConfigSO>();
        }

        public List<DropItemConfigSO> SelectThreeRandomDrops()
        {
            // TODO Implement function
            return null;
        }
    }
}
