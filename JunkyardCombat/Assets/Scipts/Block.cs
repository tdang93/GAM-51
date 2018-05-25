using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : Part {
    [SerializeField] private bool isBaseBlock;
    public bool IsBaseBlock {
        get {
            return isBaseBlock;
        }
        set {
            if(value == true) { // never settable to false
                isBaseBlock = value;
            }
        }
    }

    void Awake() {
        isBaseBlock = false;    
    }

    void OnCollisionEnter(Collision collision) {
            
    }
}
