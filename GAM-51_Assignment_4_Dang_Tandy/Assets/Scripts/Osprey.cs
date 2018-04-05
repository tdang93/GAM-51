using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Osprey : MonoBehaviour {
    public new Rigidbody rigidbody;
    public Vector3 CenterOfGravity;
    public float sensitivity = 1;

    [SerializeField] private FlyingMode flyingMode;
    [SerializeField] private Rotors rotors; // RotorL, RotorR
    [SerializeField] private Wings wings; // WingL, WingR
    [SerializeField] private Rudders rudders; // RudderL, RudderR

    void Start() {
        if(gameObject.GetComponent<Rigidbody>() == null) {
            rigidbody = gameObject.AddComponent<Rigidbody>();
            rigidbody.useGravity = false;
        }

        flyingMode = FlyingMode.Helicopter; // start in Helicopter FlyingMode
        rotors.radius = 1;
    }

    void Update() {

    }

    void FixedUpdate() {
        Debug.DrawRay(transform.position + transform.forward, transform.forward, Color.blue); // local Z
        Debug.DrawRay(transform.position + transform.right, transform.right, Color.red); // local X
        Debug.DrawRay(transform.position + transform.up, transform.up, Color.green); // local Y

        Debug.DrawRay(transform.position, rigidbody.velocity, Color.yellow); // velocity direction
        //Debug.Log("Dot Product of local forward and velocity: " + Vector3.Dot(transform.forward, rigidbody.velocity));


        Move(flyingMode);
    }

    private void Move(FlyingMode FM) {
        if(FM == FlyingMode.Helicopter) {
            Vector3 pendingMove = Input_Positional;
            if(pendingMove.z != 0) {
                float RotorLRotationX = rotors.RotorL.transform.rotation.eulerAngles.x;
                float RotorRRotationX = rotors.RotorR.transform.rotation.eulerAngles.x;
                Vector3 rotateAmount = Vector3.zero;
                if(Mathf.Approximately(RotorLRotationX, RotorRRotationX)) {
                    rotateAmount = new Vector3(1, 0, 0) * Sensitivity;
                }

                float test = rotors.RotorL.transform.rotation.eulerAngles.x + sensitivity;
                if(test >= 270) {
                    test -= 360;
                }
                if(pendingMove.z > 0) { // tilt Rotors forward for Z-axis movement
                    if(test <= 90) {
                        //Debug.Log("x: " + rotors.RotorL.transform.rotation.eulerAngles.x);
                        rotors.RotorL.transform.Rotate(rotateAmount); // set RotorL rotation, (+)
                        rotors.RotorR.transform.Rotate(rotateAmount); // set RotorR rotation, (+)
                        //Debug.Log("z > 0: " + Input_Positional.z);
                    }
                }
                else if(pendingMove.z < 0) { // tilt Rotors backward for Z-axis movement
                    if(test >= -45) {
                        //Debug.Log("x: " + rotors.RotorL.transform.rotation.eulerAngles.x);
                        rotors.RotorL.transform.Rotate(-rotateAmount); // set RotorL rotation, (-)
                        rotors.RotorR.transform.Rotate(-rotateAmount); // set RotorR rotation, (-)
                        //Debug.Log("z < 0: " + Input_Positional.z);
                    }
                }
                Debug.Log("test: " + test);
            }

            Vector3 rotation = Vector3.zero;
            float newRotation = 0;
            if(pendingMove.y != 0) {
                if(pendingMove.y > 0) {
                    newRotation = 0.1f * Sensitivity;
                }
                else if(pendingMove.y < 0) {
                    newRotation = -0.1f * Sensitivity;
                }
                rotors.spin += newRotation;
                if(rotors.spin > 100) {
                    rotors.spin = 100;
                }
                else if(rotors.spin < 0 || Mathf.Approximately(rotors.spin, 0)) {
                    rotors.spin = 0;
                }
            }
            rotation += new Vector3(0, rotors.spin, 0);
            rotors.RotorL_Propeller.transform.Rotate(rotation);
            rotors.RotorR_Propeller.transform.Rotate(-rotation);
            //Debug.Log("rotors.spin: " + rotors.spin);
            //Debug.Log("rotation.magnitude: " + rotation.magnitude);

            rigidbody.AddForceAtPosition(100 * rotors.thrust, rotors.RotorL_Propeller.transform.position);
            rigidbody.AddForceAtPosition(100 * rotors.thrust, rotors.RotorR_Propeller.transform.position);
        }
    }

    private float Sensitivity {
        get {
            float input = Input.GetAxis("Mouse ScrollWheel");
            //Debug.Log("scrollwheel: " + input);
            if(input > 0 && sensitivity <= 1.9f) { // upper limit is 2.0
                sensitivity += 0.1f;
            }
            else if(input < 0 && sensitivity >= 0.2f) { // lower limit is 0.1
                sensitivity -= 0.1f;
            }
            return sensitivity;
        }
    }

    private Vector3 Input_Positional {
        get {
            float z = Input.GetAxis("Vertical");
            float x = Input.GetAxis("Horizontal");
            float y = Input.GetAxis("Jump");

            return new Vector3(x, y, z);
        }
    }
    private Quaternion Input_Rotational {
        get {
            float theta = Input.GetAxis("Mouse ScrollWheel") * 10; // Pitch
            //float phi = Input.GetAxis(""); // Yaw
            //float psi = Input.GetAxis(""); // Roll

            return Quaternion.Euler(rigidbody.rotation.eulerAngles + new Vector3(theta, 0, 0));
        }
    }

    private enum FlyingMode {
        Helicopter,
        Transition,
        Airplane
    }

    [System.Serializable]
    private class Rotors {
        public GameObject RotorL;
        public GameObject RotorL_Propeller;

        public GameObject RotorR;
        public GameObject RotorR_Propeller;

        public float radius;
        public float Area { // Area of circle = pi * r^2
            get {
                return (3.14159f * radius * radius);
            }
        }
        public float spin; // speed: cycles per second
        public float efficiency; // thrust generated per cycle
        public Vector3 thrust { // thrust = efficiency * area * speed
            get {
                return new Vector3(0, efficiency * Area *  spin, 0);
            }
        }
        // Area of circle = pi * r^2 ~ proportional  to: volume of air moved ~ thrust produced
    }

    [System.Serializable]
    private class Wings {
        public GameObject WingL;
        public Flap HFlapL;

        public GameObject WingR;
        public Flap HFlapR;

        public enum FlapTurnMode {
            Up,
            Down
        }


    }

    [System.Serializable]
    private class Rudders {
        public Flap VFlapL, VFlapR;

        public enum FlapTurnMode {
            Left,
            Right
        }
    }

    private class Flap {
        public GameObject gameobject;


    }
}
