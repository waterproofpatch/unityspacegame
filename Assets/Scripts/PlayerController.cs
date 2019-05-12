/**
PlayerController controls the physics of the rigidbody as the player moves around in the game world
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Turrets;
/*
contoller notes:
JoystickButton0 - X
JoystickButton1 - A
JoystickButton2 - B
JoystickButton3 - Y
JoystickButton4  - LB
JoystickButton5  - RB
JoystickButton6  - LT
JoystickButton7  - RT
JoystickButton8 - back
JoystickButton9 - start
JoystickButton10 - left stick[not direction, button]
JoystickButton11 - right stick[not direction, button]
*/

public class PlayerController : MonoBehaviour
{
    // turrets that we aim when we click on something
    public TurretRotation turret;

    // rigidbody sensitivity/modifies to apply on inputs
    public int thrust = 10;
    public float rollSensitivity = 100.0f;
    public float yawSensitivity = 200.0f;
    public float pitchSensitivity = 200.0f;

    // the physics object we move when we thrust
    private Rigidbody rb;

    void Start()
    {
        this.rb = GetComponent<Rigidbody>();
        this.turret = GetComponentInChildren<TurretRotation>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Ray ray = cam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0)); // from camera center position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);  // from mouse position
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                print("Targeting " + hit.transform.name);
                this.SetAimPointForControlledTurrets(hit.transform);
            }
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision");

        //Stop Moving/Translating
        this.rb.velocity = Vector3.zero;

        //Stop rotating
        this.rb.angularVelocity = Vector3.zero;
    }

    void FixedUpdate()
    {
        float newAxisInput = Input.GetAxis("NewAxis");
        Debug.Log("NewAxis has " + newAxisInput);

        // thrust
        if (Input.GetKey(KeyCode.JoystickButton4))
        {
            GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * thrust);
            Debug.Log("Thrusting forward");
        }
        if (Input.GetKey(KeyCode.JoystickButton5))
        {
            GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * (thrust * -1));
            Debug.Log("Thrusting back");
        }

        // pitch
        float pitch = -Input.GetAxis("Vertical") * Time.deltaTime * pitchSensitivity;

        // check modifier, roll or yaw accordingly
        float roll = 0.0f;
        float yaw = 0.0f;
        if (Input.GetKey(KeyCode.JoystickButton0))
        {
            roll = -Input.GetAxis("Horizontal") * Time.deltaTime * rollSensitivity;
        }
        else
        {
            yaw = Input.GetAxis("Horizontal") * Time.deltaTime * yawSensitivity;
        }

        //Debug.Log("Pitch " + pitch);
        //Debug.Log("Roll " + roll);
        //Debug.Log("Yaw " + roll);
        Vector3 keyboardRot = new Vector3(pitch, yaw, roll);
        this.rb.AddRelativeTorque(keyboardRot);
    }

    void SetAimPointForControlledTurrets(Transform transform)
    {
        if (this.turret == null)
        {
            return;
        }
        turret.SetAimpoint(transform);
    }
}
