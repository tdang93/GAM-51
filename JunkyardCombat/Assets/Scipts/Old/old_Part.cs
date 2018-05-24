using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class old_Part : MonoBehaviour {
    public enum PartType {
         Block = 0,
         Cylinder = 1
    }
    public PartType partType;
    public int Index {
        get {
            return (int)partType;
        }
    }

    public float mass; // effect weight, scaled by gravity of region
    public int strength; // defense / armor
    public int integrity; // health
    public List<Socket> sockets = new List<Socket>(); // connections possible to other Parts
    
}
