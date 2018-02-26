using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scale : MonoBehaviour {
    public GameObject A;
    public float scale;

    private void Awake() {
        A = new GameObject("Vector A");
        scale = 1;
    }

    private void Update() {
        Debug.DrawLine(Vector3.zero, MyVector3.Scale(A.transform.forward, scale), Color.blue);
    }
}
