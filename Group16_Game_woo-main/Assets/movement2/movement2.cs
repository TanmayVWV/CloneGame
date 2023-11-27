using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class movement2 : MonoBehaviour
{

    [SerializeField] float maxSpeed;
    [SerializeField] float acceleration;
    [SerializeField] float turningSpeed;
    [SerializeField] float turningMaxSpeed;
    [SerializeField] float traction;
    [SerializeField] float rotationalDampener;
    [SerializeField] float knockbackForce;
    [SerializeField] float MaxKnockBackForce;
    [SerializeField] float BackwardAcceleration;

    public Rigidbody rb;

    private InputSystem controls;
    private Boat BoatScript;
    private StatManager StatScript;


    private void Awake()
    {
        //Find Boat Script
        BoatScript = GetComponentInChildren<Boat>();
        if (!BoatScript)
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
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        float speed = StatScript.GetSpeed();
        float handling = StatScript.GetHandling();

        // read input system values
        float forwardInput = controls.BoatControls.ForwardInput.ReadValue<float>();
        float turnInput = controls.BoatControls.TurnInput.ReadValue<float>();

        float adjAcceleration = acceleration * speed/100;
        float adjMaxSpeed = maxSpeed * speed/100;

        float adjTraction = traction * handling/100;

        // Movement
        rb.AddRelativeForce(Vector3.forward * adjAcceleration * Time.fixedDeltaTime * forwardInput * (forwardInput >= 0 ? 1.0f : BackwardAcceleration), ForceMode.VelocityChange);

        // Rotate
        //transform.Rotate(new Vector3(0, turnInput * turningSpeed * Time.fixedDeltaTime, 0));
        rb.AddTorque(Vector3.up * turningSpeed * Time.fixedDeltaTime * turnInput, ForceMode.Force);
        rb.angularVelocity = Vector3.ClampMagnitude(rb.angularVelocity, turningMaxSpeed);

        // Rotate Damperner
        rb.angularVelocity *= Mathf.Pow(1.0f - rotationalDampener * Time.fixedDeltaTime, Time.fixedDeltaTime);

        // Traction
        rb.velocity = Vector3.Lerp(rb.velocity.normalized, transform.forward, adjTraction * Time.fixedDeltaTime) * rb.velocity.magnitude;

        // Max Speed
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, adjMaxSpeed);

    }

    public void ApplyKnockBack(Vector3 knockBack)
    {
        rb.AddForce(knockBack, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //This is for a weird bug where we hit enemies when there is no enemy there
        Rigidbody rbc = collision.gameObject.GetComponent<Rigidbody>();
        if(rbc != null) {
            Debug.Log(collision.gameObject.name +
                "Collider Location: " + collision.gameObject.transform.position
                + "Player Location" + gameObject.transform.position
                + " RB Velocity: " + rbc.velocity.magnitude +
                " RelativeVelocity: " + collision.relativeVelocity);
        }
        else
        {
            Debug.Log(collision.gameObject.name +
                "Collider Location: " + collision.gameObject.transform.position
                + "Player Location" + gameObject.transform.position
                + " No Rigid body found on other collider" +
                " RelativeVelocity: " + collision.relativeVelocity);
        }

        Vector3 bounceDirection = collision.contacts[0].normal;
        bounceDirection.y = 0f;
        Vector3 knockback = bounceDirection * knockbackForce * rb.velocity.magnitude;
        knockback = Vector3.ClampMagnitude(knockback, MaxKnockBackForce);

        ApplyKnockBack(knockback);
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
