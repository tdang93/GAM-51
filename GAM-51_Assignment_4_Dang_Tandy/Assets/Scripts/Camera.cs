using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour {
    public GameObject cameraX;
    public GameObject cameraDock;
    public GameObject Osprey;
    public float moveX;
    public float moveY;
    public float zoom;

    public void Start() {
        cameraDock = new GameObject();
        cameraDock.transform.position = cameraX.transform.position;
        cameraDock.transform.parent = Osprey.transform;
    }

    public void Update() {
        moveX = Input.GetAxis("Mouse X");
        if(Mathf.Abs(moveX) < 0.25) { // dead zone for camera stability
            moveX = 0;
        }
        moveY = Input.GetAxis("Mouse Y");
        zoom = Input.GetAxis("Mouse ScrollWheel");
    }

    public void FixedUpdate() {
        //cameraX.transform.Rotate(new Vector3(0, moveX, 0), Space.World);
        //cameraY.transform.Rotate(new Vector3(-moveY, 0, 0), Space.World);
        Vector3 look = Osprey.transform.position - cameraDock.transform.position;
        gameObject.transform.Translate(look.normalized * zoom);
    }
}
