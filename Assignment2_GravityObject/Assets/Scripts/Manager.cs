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
        Debug.Log("AllObjects.Capacity: " + AllObjects.Capacity);
        GenerateObjects(AllObjects);
        Debug.Log("AllObjects.Count: " + AllObjects.Count);
    }

    private List<GravityObject> PopulateObjects() {
        int objectsCount = Random.Range(minimumObjects, maximumObjects);
        Debug.Log("objectsCount: " + objectsCount);
        List<GravityObject> populatedGOs = new List<GravityObject>(objectsCount);
        Debug.Log("populatedGOs.Capacity: " + populatedGOs.Capacity);
        for(int i = 0; i < objectsCount; i++) {
            Debug.Log("i: " + i);
            populatedGOs.Add(Instantiate(prefab));
            populatedGOs[i].gameObject.SetActive(false); // to save processing while setting up
        }
        Debug.Log("PopulateObjects(): populated " + populatedGOs.Count + "GravityObjects!");
        return populatedGOs;
    }

    private void GenerateObjects(List<GravityObject> L_GOs) {
        for(int i = 0; i < L_GOs.Capacity; i++) {
            Debug.Log("GenerateObjects()#1: L_GOs.Capacity = " + L_GOs.Capacity);
            Debug.Log("GenerateObjects()#2: for(): i = " + i);
            L_GOs[i].mass = Random.Range(minimumMass, maximumMass); // randomize radius

            float minimumLocation = -maximumMass * L_GOs.Capacity;
            float maximumLocation = maximumMass * L_GOs.Capacity;
            Vector3 location = new Vector3(Random.Range(minimumLocation, maximumLocation), 
                Random.Range(minimumLocation, maximumLocation),
                Random.Range(minimumLocation, maximumLocation));
            L_GOs[i].transform.position = location;
            /*
            for(int h = 0; h < i; h++) {
                if(GravityObject.verifyOverlap(L_GOs[h], L_GOs[i])) {
                    D6Relocate(L_GOs[h], L_GOs[i]);
                }
            }
            Instantiate(L_GOs[i].gameObject);
            Debug.Log("Instantiated: " + L_GOs[i].gameObject.name);
            */
            //Debug.Log("GenerateObjects():#4 for(): i = " + i);
            L_GOs[i].gameObject.SetActive(true); // reactivate after positioning
        }
    }

    private bool D6Relocate(GravityObject GO1, GravityObject GO2) {
        // move GO2 around 6 faces of a die measuring 2 * GO1.radius per side, centered on GO1
        Vector3 center = GO1.gameObject.transform.position;
        Vector3[] faces = new Vector3[6];
        faces[0] = center - new Vector3(-GO1.radius, 0, 0); // -X, +0, +0
        faces[1] = center - new Vector3(+GO1.radius, 0, 0); // +X, +0, +0
        faces[2] = center - new Vector3(0, -GO1.radius, 0); // +0, -Y, +0
        faces[3] = center - new Vector3(0, +GO1.radius, 0); // +0, +Y, +0
        faces[4] = center - new Vector3(0, 0, -GO1.radius); // +0, +0, -Z
        faces[5] = center - new Vector3(0, 0, +GO1.radius); // +0, +0, +Z
        
        // step through possible faces and attempt to reposition GO2 to an open face
        GravityObject testGO = new GravityObject();
        testGO.radius = GO1.radius;
        for(int i = 0; i < 6; i++) {
            testGO.transform.position = faces[i];
            if(GravityObject.verifyOverlap(GO1, testGO) == false) { // if no overlap
                GO2.gameObject.transform.position = faces[i];
                return true; // success flag
            }
        }
        return false; // failure flag; not able to relocate on any of 6 faces
    }
}
