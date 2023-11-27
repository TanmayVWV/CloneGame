/** Contributors: Jordan Planchot **/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


namespace Player_Core
{
    public class CameraFollow : MonoBehaviour
    {
        public CinemachineFreeLook cam;
        private InputSystem controls;

        private float targetBias = 0.0f;


        private void Awake()
        {
            // Set up controls for player
            controls = new InputSystem();
            controls.BoatControls.Enable();
        }



        // Update is called once per frame
        void Update()
        {
            //check input
            float cameraInput = controls.BoatControls.CameraInput.ReadValue<float>();

            //set desired angle
            if (cameraInput == -1)
            {
                targetBias = -60.0f;
            }
            else if (cameraInput == 1)
            {
                targetBias = 60.0f;
            }
            else
            {
                targetBias = 0.0f;
            }

            //execute angle
            if (Mathf.Abs(cam.m_Heading.m_Bias - targetBias) < 0.1)
            {
                cam.m_Heading.m_Bias = targetBias;
            }
            else
            {
                cam.m_Heading.m_Bias = Mathf.Lerp(cam.m_Heading.m_Bias, targetBias, 0.05f);
            }
        }
    }
}
