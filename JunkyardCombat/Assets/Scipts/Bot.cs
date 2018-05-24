using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bot : MonoBehaviour {
    [System.Serializable] private class Inputs {
        public float speed = 1;
        public Vector3 translation = Vector3.zero;
        public Vector3 rotation = Vector3.zero;
        public bool LMB = false;
        public bool RMB = false;
    }
    [SerializeField] Inputs inputs;

    void Update() {
        
    }

    void GetInputs() {
        inputs.translation = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Jump"), Input.GetAxis("Vertical")) *
            inputs.speed;
        inputs.rotation = new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0) * inputs.speed;

        if(Input.GetKeyDown(KeyCode.Mouse0)) {
            inputs.LMB = true;
        }
        if(Input.GetKeyDown(KeyCode.Mouse1)) {
            inputs.RMB = true;
        }
    }

    void DoInputs() {

    }

    void ClearInputs() {
        inputs = null;
        inputs = new Inputs();
    }


}
