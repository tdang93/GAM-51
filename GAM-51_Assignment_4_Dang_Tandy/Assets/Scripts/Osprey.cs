using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Osprey : MonoBehaviour {
    //public GameObject body;
    public new Rigidbody rigidbody;
    public float sensitivity = 1;

    [SerializeField] private FlyingMode flyingMode;
    [SerializeField] private Rotors rotors; // RotorL, RotorR
    [SerializeField] private Wings wings; // WingL, WingR
    [SerializeField] private Rudders rudders; // RudderL, RudderR

    void Start() {
        if(gameObject.GetComponent<Rigidbody>() == null) {
            rigidbody = gameObject.AddComponent<Rigidbody>();
            //rigidbody.useGravity = false;
        }
        else {
            rigidbody = gameObject.GetComponent<Rigidbody>();
        }
        //rigidbody.position = new Vector3(transform.position.x, -10, rotors.RotorL.transform.position.z);
        rigidbody.centerOfMass = new Vector3(0, -30, 0);

        flyingMode = FlyingMode.Helicopter; // start in Helicopter FlyingMode
        //rotors.radius = 1;

    }

    void Update() {
        //Debug.Log("Mouse X" + Input.GetAxis("Mouse X"));
        if(Input.GetKeyDown(KeyCode.R)) {
            rotors.RotorL.transform.localRotation = rotors.RotorR.transform.localRotation =
                Quaternion.Euler(new Vector3(0,0,0));
        }

        if(Input.GetKey(KeyCode.Mouse0) && sensitivity + 0.05f <= 1) {
            sensitivity += 0.05f;
        }
        else if(Input.GetKey(KeyCode.Mouse1) && sensitivity - 0.05f >= 0) {
            sensitivity -= 0.05f;
        }
    }

    void FixedUpdate() {
        Debug.DrawRay(transform.position + transform.forward, transform.forward, Color.blue); // local Z
        Debug.DrawRay(transform.position + transform.right, transform.right, Color.red); // local X
        Debug.DrawRay(transform.position + transform.up, transform.up, Color.green); // local Y

        Debug.DrawRay(transform.position, rigidbody.velocity, Color.yellow); // velocity direction
        //Debug.Log("Dot Product of local forward and velocity: " + Vector3.Dot(transform.forward, rigidbody.velocity));

        Move(flyingMode);

        //rigidbody.AddRelativeTorque(new Vector3(0, 1000, 0));
    }

    private void Move(FlyingMode FM) {
        if(FM == FlyingMode.Helicopter) {
            Vector3 pendingMove = Input_Positional;
            if(pendingMove.z != 0) {
                float RotorLRotationX = rotors.RotorL.transform.localRotation.eulerAngles.x;
                float RotorRRotationX = rotors.RotorR.transform.localRotation.eulerAngles.x;
                Vector3 rotateAmount = Vector3.zero;
                if(Mathf.Approximately(RotorLRotationX, RotorRRotationX)) {
                    rotateAmount = new Vector3(1, 0, 0) * Sensitivity * 0.5f;
                }

                float testForward = rotors.RotorL.transform.localRotation.eulerAngles.x + sensitivity; // pending +Z rotation
                float testBackward = rotors.RotorL.transform.localRotation.eulerAngles.x - sensitivity; // pending -Z rotation
                if(pendingMove.z > 0) { // tilt Rotors forward for Z-axis movement
                    if((testForward < 30 || testForward > 330)) {
                        //Debug.Log("x: " + rotors.RotorL.transform.localRotation.eulerAngles.x);
                        rotors.RotorL.transform.Rotate(rotateAmount); // set RotorL rotation, (+)
                        rotors.RotorR.transform.Rotate(rotateAmount); // set RotorR rotation, (+)
                        //Debug.Log("z > 0: " + Input_Positional.z);
                    }
                }
                else if(pendingMove.z < 0) { // tilt Rotors backward for Z-axis movement
                    if((testBackward < 30 || testBackward > 330)) {
                        //Debug.Log("x: " + rotors.RotorL.transform.localRotation.eulerAngles.x);
                        rotors.RotorL.transform.Rotate(-rotateAmount); // set RotorL rotation, (-)
                        rotors.RotorR.transform.Rotate(-rotateAmount); // set RotorR rotation, (-)
                        //Debug.Log("z < 0: " + Input_Positional.z);
                    }
                }
            }

            /*
            // AUTOMATIC FORWARD/BACKWARD COMPENSATION
            float localEulerX = gameObject.transform.localRotation.eulerAngles.x;
            if(localEulerX > 15 && localEulerX < 90) { // compensate backwards from extreme forward tilt
                if(rotors.RotorL.transform.localEulerAngles.x - 5 < 45 ||
                rotors.RotorL.transform.localEulerAngles.x - 5 > 315) {
                    rotors.RotorL.transform.Rotate(new Vector3(-5, 0, 0));
                }
                if(rotors.RotorR.transform.localEulerAngles.x - 5 < 45 ||
                rotors.RotorR.transform.localEulerAngles.x - 5 > 315) {
                    rotors.RotorR.transform.Rotate(new Vector3(-5, 0, 0));
                    if (rotors.spin > 0) { // slow down rotors
                        if(rotors.spin - 10 < 0) {
                            rotors.spin = 0;
                        }
                        else {
                            rotors.spin -= 10;
                        }
                    }
                }
                else { // cannot rotate rotors backwards any further
                    rotors.spin = 80;
                }
            }
            else if(localEulerX > 270 && localEulerX < 345) { // compensate forward
                if(rotors.RotorL.transform.localEulerAngles.x + 5 < 45 ||
                rotors.RotorL.transform.localEulerAngles.x + 5 > 315) {
                    rotors.RotorL.transform.Rotate(new Vector3(5, 0, 0));
                }
                if(rotors.RotorR.transform.localEulerAngles.x + 5 < 45 ||
                rotors.RotorR.transform.localEulerAngles.x + 5 > 315) {
                    rotors.RotorR.transform.Rotate(new Vector3(5, 0, 0));
                    if (rotors.spin > 0) { // slow down rotors
                        if(rotors.spin - 10 < 0) {
                            rotors.spin = 0;
                        }
                        else {
                            rotors.spin -= 10;
                        }
                    }
                }
                else { // cannot rotate rotors backwards any further
                    rotors.spin = 80;
                }
            }
            */

            Vector3 rotation = Vector3.zero;
            float newRotation = 0;
            if(pendingMove.y != 0) {
                if(pendingMove.y > 0) {
                    newRotation = 0.2f * Sensitivity;
                }
                else if(pendingMove.y < 0) {
                    newRotation = -0.2f * Sensitivity;
                }
                rotors.spin += newRotation;
                if(rotors.spin > 100) {
                    rotors.spin = 100;
                }
                else if(rotors.spin < 0 || Mathf.Approximately(rotors.spin, 0)) {
                    rotors.spin = 0;
                }

                foreach(WheelCollider WC in GetComponentsInChildren<WheelCollider>()) {
                    WC.motorTorque = 0.000001f;
                }  
            }
            else {
                foreach(WheelCollider WC in GetComponentsInChildren<WheelCollider>()) {
                    WC.motorTorque = 0;
                }  
            }
            rotation += new Vector3(0, rotors.spin, 0);

            Vector3 rbThrustLPosition = new Vector3(rotors.RotorL_Propeller.transform.position.x,
                rotors.RotorL_Propeller.transform.position.y, rigidbody.transform.position.z);
            Vector3 rbThrustRPosition = new Vector3(rotors.RotorR_Propeller.transform.position.x,
                rotors.RotorR_Propeller.transform.position.y, rigidbody.transform.position.z);

            Vector3 thrustL = rotors.RotorL.transform.up.normalized * rotors.thrust;
            Vector3 thrustR = rotors.RotorR.transform.up.normalized * rotors.thrust;

            if(pendingMove.x != 0) { // if attempting to turn/rotate left or right
                rotors.tiltAmount = Mathf.Abs(pendingMove.x) * 0.01f;
                if(pendingMove.x < 0) {
                    rigidbody.AddRelativeTorque(new Vector3(0, -rotors.spin, 0));
                }
                else if(pendingMove.x > 0) {
                    rigidbody.AddRelativeTorque(new Vector3(0, rotors.spin, 0));
                }

                if(pendingMove.x < 0) { // -x -> Osprey rotates left, rotorL spins CW faster than RotorR
                    rotors.RotorL_Propeller.transform.Rotate(rotation * (1 + rotors.tiltAmount));
                    thrustL *= 1 + rotors.tiltAmount;
                    //rigidbody.AddTorque(new Vector3(0, -(300), 0)); // CW
                    Debug.Log("L");
                    foreach(WheelCollider WC in GetComponentsInChildren<WheelCollider>()) {
                        if(WC.transform.position.z > gameObject.transform.position.z && // if front wheels
                        (WC.steerAngle - (1 + rotors.tiltAmount)) > -45) { // and if not turning past -45 degrees
                            WC.steerAngle -= 1 + rotors.tiltAmount; // decrease to turn CW / L
                            //Debug.Log("steerAngle: " + WC.steerAngle);
                        }
                    }
                    rotors.RotorR_Propeller.transform.Rotate(-rotation * (1 - rotors.tiltAmount));
                    thrustR *= 1 - rotors.tiltAmount;
                }
                else if(pendingMove.x > 0) { // +x -> Osprey rotates right, rotorR spins CCW faster than RotorL
                    rotors.RotorL_Propeller.transform.Rotate(rotation * (1 - rotors.tiltAmount));
                    thrustL *= 1 - rotors.tiltAmount;

                    rotors.RotorR_Propeller.transform.Rotate(-rotation * (1 + rotors.tiltAmount));
                    thrustR *= 1 + rotors.tiltAmount;
                    //rigidbody.AddTorque(new Vector3(0, (300), 0)); // CCW
                    foreach(WheelCollider WC in GetComponentsInChildren<WheelCollider>()) {
                        if(WC.transform.position.z > gameObject.transform.position.z &&
                    (WC.steerAngle - (1 + rotors.tiltAmount)) < 45) { // and if not turning past 45 degrees) {
                            WC.steerAngle += 1 + rotors.tiltAmount; // increase to turn CCW / R
                            //Debug.Log("steerAngle: " + WC.steerAngle);
                        }
                    }
                }
            }
            else {
                rotors.RotorL_Propeller.transform.Rotate(rotation);
                rotors.RotorR_Propeller.transform.Rotate(-rotation);
            }

            //Debug.Log("rotors.spin: " + rotors.spin);
            //Debug.Log("rotation.magnitude: " + rotation.magnitude);

            rigidbody.AddForceAtPosition(thrustL, rbThrustLPosition);
            rigidbody.AddForceAtPosition(thrustR, rbThrustRPosition);

            Debug.DrawRay(rotors.RotorL.transform.position + new Vector3(0, 0, 0), thrustL, Color.yellow);
            Debug.DrawRay(rotors.RotorR.transform.position + new Vector3(0, 0, 0), thrustR, Color.yellow);

            //Debug.Log("force: " + rotors.thrust);
        }
    }

    private float Sensitivity {
        get {
            //float input = Input.GetAxis("Mouse Y");
            //input *= 0.25f;
            //Debug.Log("scrollwheel: " + input);
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
        public float thrust { // thrust = efficiency * area * speed
            get {
                float magnitude = efficiency * Area *  spin;
                return magnitude;
            }
        }
        // Area of circle = pi * r^2 ~ proportional  to: volume of air moved ~ thrust produced
        public float tiltAmount;
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
