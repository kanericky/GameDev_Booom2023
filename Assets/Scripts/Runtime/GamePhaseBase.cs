using System;
using UnityEngine;

namespace Runtime
{

    [Serializable]
    public class GamePhaseBase : MonoBehaviour, IExecutable
    {
        protected GamePhaseBase subStep;

        protected bool isPhaseTerminated;
        
        public void Execute()
        {
            Debug.Log("Phase execution is in progress...");
        }

        public void OnExecuted()
        {
            isPhaseTerminated = true;
            Debug.Log("Current phase execution has completed...");

            if (subStep != null)
            {
                subStep.Execute();
            }
            else
            {
                Debug.Log("All phases have been executed...");
            }
        }
    }

    public interface IExecutable
    {
        public void Execute();

        public void OnExecuted();
    }
}
