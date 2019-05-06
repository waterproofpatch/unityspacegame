using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Turrets;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    public Camera cam;
    public List<GameObject> turrets;

    public int thrust = 10;

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
                print("I clicked " + hit.transform.name);
                this.SetAimPointForControlledTurrets(hit.transform);
            }
        }
    }
    void FixedUpdate() {
        // thrust
        if (Input.GetKey(KeyCode.W)) {
            GetComponent<Rigidbody>().AddRelativeForce(transform.forward * thrust);
        }
        if (Input.GetKey(KeyCode.S)) {
            GetComponent<Rigidbody>().AddRelativeForce(transform.forward * (thrust * -1));
        }

        float pitch = -Input.GetAxis("Vertical") * Time.deltaTime * 20.0f;
        float roll = 0.0f;
        float yaw = 0.0f;

        // check modifier
        if (Input.GetKey(KeyCode.R)) {
            roll = -Input.GetAxis("Horizontal") * Time.deltaTime * 20.0f;
        } else  {
            yaw = Input.GetAxis("Horizontal") * Time.deltaTime * 20.0f;
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
            Debug.Log("Setting aimpoint...");
            tr.SetAimpoint(transform.position);
        }
    }
}
