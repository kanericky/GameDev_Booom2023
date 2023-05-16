using System;
using DG.Tweening;
using UnityEngine;

namespace Runtime
{
    public class MenuCameraController : MonoBehaviour
    {
        [Header("Menu Positions")] 
        [SerializeField] private Transform menuIdleCameraPos;
        [SerializeField] private Transform menuStartCameraPos;
        [SerializeField] private Transform currentCameraPosTarget;

        [Header("Camera Movement")] 
        [SerializeField] private float cameraMoveSpeed;
        [SerializeField] private Vector2 cameraDeadZoneRangeGeneral;
        [SerializeField] private float cameraDeadZoneSmoothTime;

        private void Start()
        {
            currentCameraPosTarget = menuIdleCameraPos;
            transform.DOMove(currentCameraPosTarget.position, cameraMoveSpeed);
            transform.DORotate(currentCameraPosTarget.rotation.eulerAngles, cameraMoveSpeed);
            HandleCameraDeadZoneMovement(currentCameraPosTarget.rotation.eulerAngles);
        }

        private void LateUpdate()
        {
            HandleCameraDeadZoneMovement(currentCameraPosTarget.rotation.eulerAngles);
        }

        public void EnterMenuStartCameraPos()
        {
            currentCameraPosTarget = menuStartCameraPos;
            transform.DOMove(menuStartCameraPos.position, cameraMoveSpeed);
            transform.DORotate(menuStartCameraPos.rotation.eulerAngles, cameraMoveSpeed);
            HandleCameraDeadZoneMovement(menuStartCameraPos.rotation.eulerAngles);
        }
        
        public void HandleCameraDeadZoneMovement(Vector3 cameraCurrentRotation)
        {
            Vector2 deadZoneRange;
            
            deadZoneRange = cameraDeadZoneRangeGeneral;

            Vector3 currentMousePos = Input.mousePosition;
            Vector3 mousePivot = new Vector3(Screen.width/2, Screen.height/2, 0);

            float x = (currentMousePos.x - mousePivot.x) * 0.005f;
            float y = (mousePivot.y - currentMousePos.y) * 0.005f;

            float targetCameraRotationX = Mathf.Clamp(
                cameraCurrentRotation.x + y,
                cameraCurrentRotation.x - deadZoneRange.x,
                cameraCurrentRotation.x + deadZoneRange.x);
            
            float targetCameraRotationY = Mathf.Clamp(
                cameraCurrentRotation.y + x, 
                cameraCurrentRotation.y - deadZoneRange.y, 
                cameraCurrentRotation.y + deadZoneRange.y);

            Vector3 targetCameraRotation = new Vector3(targetCameraRotationX, targetCameraRotationY, cameraCurrentRotation.z);

            transform.DORotate(targetCameraRotation, cameraDeadZoneSmoothTime).SetEase(Ease.OutQuad);
            
            
            //transform.eulerAngles = new Vector3(
            //Mathf.Lerp(cameraCurrentRotation.x, targetCameraRotationX, cameraDeadZoneSmoothTime), 
            //Mathf.Lerp(cameraCurrentRotation.y, targetCameraRotationY, cameraDeadZoneSmoothTime),
            //  GetCameraRotIdle().z);

        }
    }
}
