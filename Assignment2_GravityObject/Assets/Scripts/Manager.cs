using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour {
    public int minimumObjects = 2;
    public int maximumObjects = 10;
    public float minimumRadius = 0.5f;
    public float maximumRadius = 10;
    [SerializeField] private List<GravityObject> AllObjects;

    private void Start() {
        AllObjects = PopulateObjects();
    }

    private List<GravityObject> PopulateObjects() {
        int objectsCount = Random.Range(minimumObjects, maximumObjects);
        return new List<GravityObject>(objectsCount);
    }

    private void GenerateObjects(List<GravityObject> L_GOs) {
        for(int i = 0; i < L_GOs.Count; i++) {
            L_GOs[i].radius = Random.Range(minimumRadius, maximumRadius); // randomize radius
            
            float minimumLocation = -maximumRadius * L_GOs.Count;
            float maximumLocation = maximumRadius * L_GOs.Count;
            Vector3 location = new Vector3(Random.Range(minimumLocation, maximumLocation), 
                Random.Range(minimumLocation, maximumLocation),
                Random.Range(minimumLocation, maximumLocation));
            L_GOs[i].transform.position = location;
            for(int h = 0; h < i; h++) {
                if(GravityObject.verifyOverlap(L_GOs[h], L_GOs[i])) {
                    D6Relocate(L_GOs[h], L_GOs[i]);
                }
            }
        }
    }

    private void D6Relocate(GravityObject GO1, GravityObject GO2) {
        // move GO2 around 6 faces of a die measuring 2 * GO1.radius per side, centered on GO1
        Vector3 center = GO1.gameObject.transform.position;
        Vector3 face1 = center - new Vector3(-GO1.radius, 0, 0); // -X, +0, +0
        Vector3 face2 = center - new Vector3(+GO1.radius, 0, 0); // +X, +0, +0
        Vector3 face3 = center - new Vector3(0, -GO1.radius, 0); // +0, -Y, +0
        Vector3 face4 = center - new Vector3(0, +GO1.radius, 0); // +0, +Y, +0
        Vector3 face5 = center - new Vector3(0, 0, -GO1.radius); // +0, +0, -Z
        Vector3 face6 = center - new Vector3(0, 0, +GO1.radius); // +0, +0, +Z
        
        // step through possible faces and attempt to reposition GO2 to an open face
    }
}
