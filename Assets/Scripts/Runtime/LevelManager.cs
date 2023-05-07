using UnityEngine;

namespace Runtime
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private GamePhaseBase levelStartPhase;

        private void Start()
        {
            levelStartPhase.Execute();
        }
    }
}
