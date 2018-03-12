using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour {
    public int minimumObjects = 2;
    public int maximumObjects = 10;
    public float minimumMass = 0.5f;
    public float maximumMass = 10;
    [SerializeField] private GravityObject prefab;
    [SerializeField] private List<GravityObject> AllObjects;

    private void Start() {
        AllObjects = PopulateObjects();
        //Debug.Log("AllObjects.Capacity: " + AllObjects.Capacity);
        GenerateObjects(AllObjects);
        //Debug.Log("AllObjects.Count: " + AllObjects.Count);
    }

    private List<GravityObject> PopulateObjects() {
        int objectsCount = Random.Range(minimumObjects, maximumObjects);
        //Debug.Log("objectsCount: " + objectsCount);
        List<GravityObject> populatedGOs = new List<GravityObject>(objectsCount);
        //Debug.Log("populatedGOs.Capacity: " + populatedGOs.Capacity);
        for(int i = 0; i < objectsCount; i++) {
            //Debug.Log("i: " + i);
            populatedGOs.Add(Instantiate(prefab));
            populatedGOs[i].gameObject.SetActive(false); // to save processing while setting up
        }
        Debug.Log("PopulateObjects(): populated " + populatedGOs.Count + " GravityObjects!");
        return populatedGOs;
    }

    private void GenerateObjects(List<GravityObject> L_GOs) {
        for(int i = 0; i < L_GOs.Capacity; i++) {
            //Debug.Log("GenerateObjects()#1: L_GOs.Capacity = " + L_GOs.Capacity);
            //Debug.Log("GenerateObjects()#2: for(): i = " + i);
            L_GOs[i].mass = Random.Range(minimumMass, maximumMass); // randomize radius via mass
            L_GOs[i].Refresh(); // update radius and localScale

            float maximumEndToEnd = GravityObject.calcRadius(maximumMass) * L_GOs.Capacity;
            Vector3 location = new Vector3(Random.Range(-maximumEndToEnd, maximumEndToEnd), 
                Random.Range(-maximumEndToEnd, maximumEndToEnd),
                Random.Range(-maximumEndToEnd, maximumEndToEnd));
            L_GOs[i].transform.position = location;
            //L_GOs[i].transform.position = new Vector3(0,0,0);
            
            /*
            // check for any overlaps so far, and resolve if so
            for(int h = 0; h < i; h++) {
                if(GravityObject.verifyOverlap(L_GOs[h], L_GOs[i])) {
                    D6Relocate(L_GOs[h], L_GOs[i]);
                    Debug.Log("Relocated a GravityObject that generated inside another one!");
                }
            }
            */
            
            L_GOs[i].gameObject.SetActive(true); // reactivate after positioning
        }
    }
    
    private bool D6Relocate(GravityObject GO1, GravityObject GO2) {
        // move GO2 around 6 faces of a die measuring 2 * GO1.radius per side, centered on GO1
        Vector3 center = GO1.gameObject.transform.position;
        List<Vector3> faces = new List<Vector3>(6) {
            center - new Vector3(-1.5f * GO1.radius, 0, 0), // -X, +0, +0
            center - new Vector3(+1.5f * GO1.radius, 0, 0), // +X, +0, +0
            center - new Vector3(0, -1.5f * GO1.radius, 0), // +0, -Y, +0
            center - new Vector3(0, +1.5f * GO1.radius, 0), // +0, +Y, +0
            center - new Vector3(0, 0, -1.5f * GO1.radius), // +0, +0, -Z
            center - new Vector3(0, 0, +1.5f * GO1.radius)  // +0, +0, +Z
        };
        
        // step through possible faces and attempt to reposition GO2 to an open face
        for(int i = 0; i < 6; i++) {
            GO2.transform.position = faces[i];
            if(GravityObject.verifyOverlap(GO1, GO2) == false) { // if no overlap
                GO2.gameObject.transform.position = faces[i];
                return true; // success flag
            }
        }
        return false; // failure flag; not able to relocate on any of 6 faces
    }
}
