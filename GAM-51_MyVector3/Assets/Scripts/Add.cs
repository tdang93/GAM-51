using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Add : MonoBehaviour {
    public GameObject A, B, Resultant;

    private void Start() {
        A = new GameObject("Vector A");
        B = new GameObject("Vector B");
        Resultant = new GameObject("Resultant");
    }

    private void Update() {
        Debug.DrawLine(A.transform.forward, Vector3.zero, Color.red);
        Debug.DrawLine(B.transform.forward, Vector3.zero, Color.green);
        Resultant.transform.rotation = Vector3.(A.transform.forward + B.transform.forward);

        Debug.DrawLine(A.transform.forward + B.transform.forward, Vector3.zero, Color.blue);
    }
}
