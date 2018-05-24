using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vehicle : MonoBehaviour {
    [System.Serializable] public struct Cockpits {
        public Cockpit primary;
        public Cockpit secondary;
        public Cockpit tertiary;
        public Cockpit quaternary;
    }
    [SerializeField] private Cockpits myCockpits;
    Cockpits cockpits {
        get {
            return myCockpits;
        }
    }

    [System.Serializable] public struct Stats {
        private int maxIntegrity; // total maximum health
        private int myIntegrity; // total current health
        public int Integrity {
            get {
                return myIntegrity;
            }
            set {
                myIntegrity = value;
            }
        }
        public float integrityPercent {
            get {
                return myIntegrity / maxIntegrity;
            }
        }

        private float maxPower; // maximum power
        public float power; // current power resource
    }


}
