using System;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

namespace Runtime
{

    public class CameraController : MonoBehaviour
    {
        [Header("Camera Reference")]
        [SerializeField] private Transform mainCamera;

        [Header("Camera Positions")] 
        [SerializeField] private Transform cameraPosIdlePhase;
        [SerializeField] private Transform cameraPosReloadingPhase;
        [SerializeField] private Transform cameraPosAimingPhase;
        [SerializeField] private Transform cameraPosFiringPhase;

        [Header("Camera Interp Speed")] 
        [SerializeField] private float cameraMovementTime = 1f;
        [SerializeField] private float cameraDeadZoneSmoothTime = 0.2f;

        [Header("Debug")] 
        [SerializeField] private Vector2 mouseInput;

        private void Start()
        {
        }

        public void ChangeCameraPosToIdle()
        {
            transform.DOMove(cameraPosIdlePhase.localPosition, cameraMovementTime);
            transform.DORotate(cameraPosIdlePhase.rotation.eulerAngles, cameraMovementTime);
        }

        public void ChangeCameraPosToReload()
        {
            transform.DOMove(cameraPosReloadingPhase.localPosition, cameraMovementTime).SetEase(Ease.OutQuad);
            transform.DORotate(cameraPosReloadingPhase.rotation.eulerAngles, cameraMovementTime).SetEase(Ease.OutQuad);
        }

        public void ChangeCameraPosToAiming()
        {
            transform.DOMove(cameraPosAimingPhase.localPosition, cameraMovementTime).SetEase(Ease.OutQuad);
            transform.DORotate(cameraPosAimingPhase.rotation.eulerAngles, cameraMovementTime).SetEase(Ease.OutQuad);
        }

        public Vector3 GetCameraRotIdle()
        {
            return cameraPosIdlePhase.rotation.eulerAngles;
        }

        public void HandleCameraDeadZoneMovement(Vector3 cameraCurrentRotation)
        {

            Vector3 currentMousePos = Input.mousePosition;
            Vector3 mousePivot = new Vector3(960, 540, 0);

            float x = (currentMousePos.x - mousePivot.x) * 0.005f;
            float y = (mousePivot.y - currentMousePos.y) * 0.005f;

            float targetCameraRotationX = Mathf.Clamp(
                cameraCurrentRotation.x + y,
                cameraCurrentRotation.x -1,
                cameraCurrentRotation.x + 1);
            
            float targetCameraRotationY = Mathf.Clamp(
                cameraCurrentRotation.y + x, 
                cameraCurrentRotation.y - 2, 
                cameraCurrentRotation.y + 2);

            Vector3 targetCameraRotation = new Vector3(targetCameraRotationX, targetCameraRotationY, cameraCurrentRotation.z);

            transform.DORotate(targetCameraRotation, cameraDeadZoneSmoothTime).SetEase(Ease.OutQuad);
            
            
            //transform.eulerAngles = new Vector3(
                //Mathf.Lerp(cameraCurrentRotation.x, targetCameraRotationX, cameraDeadZoneSmoothTime), 
            //Mathf.Lerp(cameraCurrentRotation.y, targetCameraRotationY, cameraDeadZoneSmoothTime),
              //  GetCameraRotIdle().z);

        }
    }
}
