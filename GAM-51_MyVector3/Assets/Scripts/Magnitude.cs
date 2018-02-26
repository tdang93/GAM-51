using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnitude : MonoBehaviour {
    public GameObject A;
    public float magnitude;
    private Scale MyScale;

    private void Start() {
        A = GetComponent<Scale>().A;
    }

    private void Update() {
        MyScale = GetComponent<Scale>();
        magnitude = MyScale.scale;
        Debug.Log("The Magnitude of " + A.name + " is: " + MyVector3.Magnitude(A.transform.forward * magnitude) + "!");
    }
}
