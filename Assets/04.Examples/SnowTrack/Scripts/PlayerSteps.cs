using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SnowTrack
{
    public class PlayerSteps : MonoBehaviour
    {
        public Transform playerTransform;
        public Camera cam;
        public int RTSize;
        public float moveThreshold = 0.1f;
        public string cameraParamsName = "_CameraParams";
        public string deltaPosName = "_DeltaPos";
        public string rtSizeName = "_RTSize";
        public bool moveAlongPlayer;

        Vector3 lastPos;

        private void OnEnable()
        {
            UpdateCameraParams();
            
            lastPos = playerTransform.position;

            Shader.SetGlobalFloat(rtSizeName, RTSize);

            Shader.SetGlobalVector(deltaPosName, deltaPlanar / (cam.orthographicSize * 2f));
        }

        Vector3 delta;

        Vector2 deltaPlanar
        {
            get
            {
                return new Vector2(delta.x, delta.z);
            }
        }


        private void LateUpdate()
        {
            Shader.SetGlobalFloat(rtSizeName, RTSize);

            Vector3 currentPos = playerTransform.position;

            delta = Vector3.zero;

            if(moveAlongPlayer && Vector3.Distance(currentPos, lastPos) > moveThreshold)
            {

                delta = currentPos - lastPos;

                UpdateCameraParams();
                UpdateCameraPosition();
               
                lastPos = playerTransform.position;
            }

            Shader.SetGlobalVector(deltaPosName, deltaPlanar / (cam.orthographicSize * 2f));

        }

        void UpdateCameraPosition()
        {
            Vector3 camPos = cam.transform.position;

            camPos.x = playerTransform.position.x;
            camPos.z = playerTransform.position.z;
            cam.transform.position = camPos;
        }

        void UpdateCameraParams()
        {
            float width = cam.orthographicSize * 2;

            float xMin = playerTransform.position.x - width * 0.5f;
            float zMin = playerTransform.position.z - width * 0.5f;
            float xMax = playerTransform.position.x + width * 0.5f;
            float zMax = playerTransform.position.z + width * 0.5f;
            Vector4 cameraParams = new Vector4(xMin, zMin, xMax, zMax);

            Shader.SetGlobalVector(cameraParamsName, cameraParams);
        }

    }

}
