/** Contributors: Jordan Planchot **/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


namespace Player_Core
{
    public class PlayerMovement : MonoBehaviour
    {
        // set up input system and character controller
        private InputSystem controls;
        public CharacterController controller;
        public Boat BoatScript;
        public StatManager StatScript;

        [SerializeField] private float maxSpeed = 15.0f;
        [SerializeField] private float turnSpeed = 120.0f;
        [SerializeField] private float accelerationSpeed = 7.0f;
        [SerializeField] private float decelerationSpeed = 7.0f;
        [SerializeField] private float brakingSpeed = 12.0f;

        // other variables used in script
        private float currentHealth = 0.0f; // not used currently
        public float currentSpeed = 0.0f;
        private Vector3 _rotation;


        // Awake is called speedy quick, real early
        private void Awake()
        {
            //Find Boat Script
            BoatScript = GetComponentInChildren<Boat>();
            if(!BoatScript )
            {
                Debug.Log("Boat Not Found On Player");
            }
            StatScript = GetComponentInChildren<StatManager>();
            if (!StatScript)
            {
                Debug.Log("StatManager Not Found On Player");
            }

            // Set up controls for player
            controls = new InputSystem();
            controls.BoatControls.Enable();
            // this line is commented out, but if there were input actions this is how to add.
            //controls.BoatControls.~ACTION~.performed += ~ACTION~;
            controls.BoatControls.ShootLeft.performed += ShootLeft;
            controls.BoatControls.ShootRight.performed += ShootRight;

            // Initialize variables
            currentHealth = 100.0f;
            DontDestroyOnLoad(gameObject);
        }

        // Update is called once per frame
        void Update()
        {
            MovePlayer();
        }

        /** Jordan Planchot **/
        private void MovePlayer()
        {
            // read input system values
            float forwardInput = controls.BoatControls.ForwardInput.ReadValue<float>();
            float turnInput = controls.BoatControls.TurnInput.ReadValue<float>();

            float driftBonus;

            // check if turning
            if (turnInput == 1.0f) _rotation = Vector3.up;
            else if (turnInput == -1.0f) _rotation = Vector3.down;
            else _rotation = Vector3.zero;

            //forward backward movement
            // if =1 accel, if =-1 brake, if =0 no input
            if (forwardInput == 1.0f)
            {
                float tempSpeedVar = StatScript.GetAcceleration() * Time.deltaTime;
                currentSpeed = Mathf.Min(currentSpeed + tempSpeedVar, StatScript.GetSpeed());
                driftBonus = 1.1f;
            }
            else if (forwardInput == -1.0f)
            {
                float tempSpeedVar = StatScript.GetBraking() * Time.deltaTime;
                currentSpeed = Mathf.Max(currentSpeed - tempSpeedVar, 0.0f);
                driftBonus = 1.25f;
            }
            else
            {
                float tempSpeedVar = StatScript.GetDeceleration() * Time.deltaTime;
                currentSpeed = Mathf.Max(currentSpeed - tempSpeedVar, 0.0f);
                driftBonus = 1.1f;
            }

            // Rotation of ship
            if (currentSpeed == 0.0f)
            {
                transform.Rotate(_rotation * (StatScript.GetHandling() / StatScript.GetSpeed()) * Time.deltaTime);
            }
            else if (currentSpeed < 10)
            {
                transform.Rotate(_rotation * (StatScript.GetHandling() / StatScript.GetSpeed()) * currentSpeed * driftBonus * Time.deltaTime);
            }
            else
            {
                transform.Rotate(_rotation * (StatScript.GetHandling() / StatScript.GetSpeed()) * 11 * driftBonus * Time.deltaTime);
            }

            // Calculate movement direction and execute the move
            if (currentSpeed != 0)
            {
                Vector3 moveDirection = Quaternion.Euler(0.0f, transform.rotation.eulerAngles.y, 0.0f) * Vector3.forward;
                controller.Move(moveDirection.normalized * currentSpeed * Time.deltaTime);
            }
        }

        public void ShootLeft(InputAction.CallbackContext context)
        {
            BoatScript.ActivateAbilitiesByGroup(2);
        }

        public void ShootRight(InputAction.CallbackContext context) 
        {
            BoatScript.ActivateAbilitiesByGroup(1);
        }

        public void DisableControls()
        {
            controls.BoatControls.Disable();
        }

        public void EnableControls()
        {
            controls.BoatControls.Enable();
        }
    }
}
