using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Subtract : MonoBehaviour {
    public GameObject A, B, Resultant;

    private void Start() {
        A = new GameObject("Vector A");
        B = new GameObject("Vector B");
        Resultant = new GameObject("Resultant");
    }

    private void Update() {
        Debug.DrawRay(Vector3.zero, A.transform.forward, Color.red);
        Debug.DrawRay(Vector3.zero, B.transform.forward, Color.green);

        Vector3 target = A.transform.forward - B.transform.forward;
        Resultant.transform.LookAt(target, Vector3.up);
        Debug.DrawLine(Vector3.zero, Resultant.transform.forward * target.magnitude, Color.blue);
    }
}
