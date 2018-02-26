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
        CalculateResultant();

        Debug.DrawLine(Vector3.zero, Resultant.transform.forward, Color.blue);
    }

    private void CalculateResultant() {
        Vector3 v3 = new Vector3(A.transform.forward.x + B.transform.forward.x, A.transform.forward.y + B.transform.forward.y, A.transform.forward.z + B.transform.forward.z);
        Resultant.transform.rotation = Quaternion.Euler(v3);
    }
}
