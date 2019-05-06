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
    // Start is called before the first frame update
    public Camera cam;
    public List<GameObject> turrets;

    public int thrust = 10;
    public float rollSensitivity = 10.0f;
    public float yawSensitivity = 10.0f;
    public float pitchSensitivity = 10.0f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            // Ray ray = cam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0)); // from camera center position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);  // from mouse position
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) {
                print("Targeting " + hit.transform.name);
                this.SetAimPointForControlledTurrets(hit.transform);
            }
        }
    }
    void FixedUpdate() {
        // thrust
        if (Input.GetKey(KeyCode.JoystickButton4)) {
            GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * thrust);
            Debug.Log("Thrusting forward");
        }
        if (Input.GetKey(KeyCode.JoystickButton5)) {
            GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * (thrust * -1));
            Debug.Log("Thrusting back");
        }

        // pitch
        float pitch = -Input.GetAxis("Vertical") * Time.deltaTime * pitchSensitivity;

        // check modifier, roll or yaw accordingly
        float roll = 0.0f;
        float yaw = 0.0f;
        if (Input.GetKey(KeyCode.JoystickButton0)) {
            roll = -Input.GetAxis("Horizontal") * Time.deltaTime * rollSensitivity;
        } else  {
            yaw = Input.GetAxis("Horizontal") * Time.deltaTime * yawSensitivity;
        }
 
        Vector3 keyboardRot = new Vector3(pitch, yaw, roll);
        GetComponent<Rigidbody>().AddRelativeTorque(keyboardRot);
    }

    void SetAimPointForControlledTurrets(Transform transform) {
        if (turrets == null) {
            return;
        }
        foreach (GameObject turret in turrets) {
            TurretRotation tr = turret.GetComponent<TurretRotation>();
            tr.SetAimpoint(transform.position);
        }
    }
}
