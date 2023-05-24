using DG.Tweening;
using UnityEngine;

namespace Runtime
{
    public class MapCameraController : MonoBehaviour
    {
        public Transform mapOverviewCameraPos;
        public Transform mapLevelDetailPos;

        public float cameraMovementSpeed = .4f;

        
        void Start()
        {
            transform.position = mapOverviewCameraPos.position;
            transform.rotation = mapOverviewCameraPos.rotation;

            transform.DOMove(mapLevelDetailPos.position, cameraMovementSpeed);
            transform.DORotate(mapLevelDetailPos.rotation.eulerAngles, cameraMovementSpeed);
        }
        
    }
}
