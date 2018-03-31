using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Osprey : MonoBehaviour {
    public new Rigidbody rigidbody;
    public Vector3 CenterOfGravity;

    [SerializeField] private Rotors rotors;
    [SerializeField] private Wings wings;
    [SerializeField] private Rudders rudders;

    void Start() {
        if(gameObject.GetComponent<Rigidbody>() == null) {
            rigidbody = gameObject.AddComponent<Rigidbody>();
            //rigidbody.useGravity = false;
        }
    }

    void Update() {
        // Input: Direction
        float z = Input.GetAxis("Vertical");
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Jump");

        // Movement: Direction
        rigidbody.velocity = new Vector3(x, y, z);


        // Input: Rotation
        float theta = Input.GetAxis("Mouse ScrollWheel") * 10; // Pitch
        //float phi = Input.GetAxis(""); // Yaw
        //float psi = Input.GetAxis(""); // Roll

        // Movement: Rotation
        rigidbody.rotation = Quaternion.Euler(rigidbody.rotation.eulerAngles + new Vector3(theta, 0, 0));

        Debug.Log("theta: " + theta);
    }

    void FixedUpdate() {
        Debug.DrawRay(transform.position + transform.forward, transform.forward, Color.blue); // local Z
        Debug.DrawRay(transform.position + transform.right, transform.right, Color.red); // local X
        Debug.DrawRay(transform.position + transform.up, transform.up, Color.green); // local Y

        Debug.DrawRay(transform.position, rigidbody.velocity, Color.yellow); // velocity direction
        //Debug.Log("Dot Product of local forward and velocity: " + Vector3.Dot(transform.forward, rigidbody.velocity));
    }

    [System.Serializable]
    public class Rotors {
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
                return efficiency * Area *  spin;
            }
        }
        // Area of circle = pi * r^2 ~ proportional  to: volume of air moved ~ thrust produced
    }

    [System.Serializable]
    public class Wings {
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
    public class Rudders {
        public Flap VFlapL, VFlapR;

        public enum FlapTurnMode {
            Left,
            Right
        }
    }

    public class Flap {
        public GameObject gameobject;


    }
}
