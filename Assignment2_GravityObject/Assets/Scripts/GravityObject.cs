using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityObject : MonoBehaviour {
    new public Rigidbody rigidbody;
    public float radius;
    public float mass;
    const float EARTH_KG_CUBICMETER_RATIO = 5510.0f;
    public static int total = 0;
    /*
        Earth density = 5.51g/cm^3
        = .00551kg/cm^3
        = .00551kg/.000001m^3
        = 5510kg/m^3
    */
    List<GravityObject> otherObjects;
    List<GravityObject> overlappingObjects;

    void Awake() {
        total++;
        Debug.Log("total = " + total);
    }

    public void Refresh() {
        setRadius(mass);
        gameObject.transform.localScale = new Vector3(radius * 2, radius * 2, radius * 2);
        //Debug.Log(name + "'s diameter : " + radius * 2);
        //rigidbody = GetComponent<Rigidbody>();
        //rigidbody.velocity /= mass;
    }

    public static float calcRadius(float mass)
    {
        // mass scaled up by 10^24; mass in units of 10^24 kg
        float volume = mass * Mathf.Pow(10, 24) / EARTH_KG_CUBICMETER_RATIO;

        //Debug.Log("volume: " + volume);
        // Volume = (4/3) * pi * r^3
        // r = ((3/4) * volume / pi) ^ (1/3)

        // scaled down by 1 million; radius in units of MegaMeters (10^6 m)
        float newRadius = Mathf.Pow((0.75f * volume / 3.14159f), (1f / 3f)) / 1000000f;
        
        /*
        float newRadius = volume * 0.75f / 3.14159f;
        newRadius = Mathf.Pow(newRadius, (1.0f / 3.0f));
       */
        //Debug.Log("new radius: " + newRadius );
        return newRadius;
    }

    public void setRadius(float setMass)
    {
        radius = calcRadius(setMass);
    }

    public void consume(GravityObject smallerObject) {
        if (mass > smallerObject.mass) {
            float half = smallerObject.mass / 2;
            smallerObject.mass -= half;
            mass += half;
            Refresh();
            if (smallerObject.mass < 0.1f) {
                smallerObject.gameObject.SetActive(false);
                total--;
                Debug.Log(total + " GravityObjects left!");
            }
            else {
                smallerObject.Refresh();
                smallerObject.rigidbody.velocity /= half; // slow down to prevent slingshotting
            }
            if(half > 1) {
                rigidbody.velocity /= half; // slow down based on mass consumed, because now heavier
            }
        }
    }

    private List<GravityObject> checkForSphereOverlap() {
        List<GravityObject> overlappingObjects = new List<GravityObject>();
        foreach(GravityObject GO in otherObjects) {
            float separation = (GO.transform.position - this.transform.position).magnitude;
            if(verifyOverlap(this, GO)) {
                overlappingObjects.Add(GO);
            }
        }
        return overlappingObjects;
    }

    // since this function is static, Manager can call it for utility without instantiating it
    public static bool verifyOverlap(GravityObject GO1, GravityObject GO2) {
        if(!GO1.gameObject.activeSelf || !GO2.gameObject.activeSelf || GO1 == GO2) {
            return false; // weed out cases where a GO is not active, or if it is verifying overlap on itself
        }
        //Debug.Log("verifyOverlap()");
        float separation = (GO2.transform.position - GO1.transform.position).magnitude;
        //Debug.Log("separation = " + separation);
        //Debug.Log("sum of radii = " + (GO1.radius + GO2.radius));
        if(separation < (GO1.radius + GO2.radius)) {
            return true;
        }
        return false; // else return false
    }
}
