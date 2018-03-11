using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityObject : MonoBehaviour {
    public float radius;
    public float mass;
    const float EARTH_KG_CUBICMETER_RATIO = 5510.0f;
    /*
        Earth density = 5.51g/cm^3
        = .00551kg/cm^3
        = .00551kg/.000001m^3
        = 5510kg/m^3
    */

    void Start()
    {
        setRadius(mass);
        gameObject.transform.localScale = new Vector3(radius * 2, radius * 2, radius * 2);
        Debug.Log("radius * 2 : " + radius * 2);
    }

    void FixedUpdate()
    {
        //Debug.Log("Test");
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger enter!");
        GravityObject smaller;
        if(other.gameObject.GetComponent<GravityObject>())
        {
            smaller = other.gameObject.GetComponent<GravityObject>();
            setRadius(calcRadius(mass + smaller.mass));
            Destroy(other.gameObject);
        }
    }

    public float calcRadius(float mass)
    {
        float volume = mass * Mathf.Pow(10, 24) / EARTH_KG_CUBICMETER_RATIO; // mass scaled up by 10^24; mass in units of 10^24 kg
        Debug.Log("volume: " + volume);
        // Volume = (4/3) * pi * r^3
        // r = ((3/4) * volume / pi) ^ (1/3)

        float newRadius = Mathf.Pow((0.75f * volume / 3.14159f), (1f / 3f)) / 1000000f; // scaled down by 1 million; radius in units of MegaMeters (10^6 m)
        /*
        float newRadius = volume * 0.75f / 3.14159f;
        newRadius = Mathf.Pow(newRadius, (1.0f / 3.0f));
       */
        Debug.Log("new radius: " + newRadius );
        return newRadius;
    }

    public void setRadius(float setMass)
    {
        radius = calcRadius(setMass);
    }

    public void consume(float smallerMass)
    {
        if (mass > smallerMass)
        {
            mass += smallerMass;
            radius = calcRadius(mass);
        }
    }      
}
