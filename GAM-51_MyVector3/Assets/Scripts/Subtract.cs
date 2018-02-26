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

    private void Update () {
        Debug.DrawLine(A.transform.forward, Vector3.zero, Color.red);
        Debug.DrawLine(B.transform.forward, Vector3.zero, Color.green);
        CalculateResultant();

        Debug.DrawLine(Vector3.zero, Resultant.transform.forward, Color.blue);
    }

    private void CalculateResultant() {
        Resultant.transform.rotation = Quaternion.Euler(A.transform.forward - B.transform.forward); // subtract B instead
    }
}
