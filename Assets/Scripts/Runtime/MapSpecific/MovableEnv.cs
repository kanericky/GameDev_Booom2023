using DG.Tweening;
using UnityEngine;

namespace Runtime.MapSpecific
{
    public class MovableEnv : MonoBehaviour
    {
        public float delayTime;
        public float movementTime;
        public float moveDistance;
    
        private void Start()
        {
            DOTween.Sequence().SetDelay(delayTime).onComplete = () => { transform.DOMoveX(moveDistance, movementTime).SetLoops(-1); };
        }
    }
}
