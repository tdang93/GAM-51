using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Player : MonoBehaviour {
    [SerializeField] new private Camera camera;
    [SerializeField] private Text infoText;
    private RaycastHit hit;
    [SerializeField] float rayRange = 10;
    [SerializeField] float speed = 0.2f;

    [System.Serializable] private struct Inputs {
        public Vector3 translation;
        public Vector3 rotation;
    }
    [SerializeField] private Inputs inputs;

    [System.Serializable] private struct Target {
        public Part currentPart;
        public Vector3 currentPartSurface;
        public Part previousPart;
        public Vector3 previousPartSurface;

        public GameObject testPartGO;
        public GameObject targetGO;
        public Block testBlock;
        public Block baseBlock;

        public void updateTargetPart(RaycastHit hit) {
            if(currentPart) {
                previousPart = currentPart;
                previousPartSurface = currentPartSurface;
            }
            currentPart = hit.transform.gameObject.GetComponent<Part>();
            currentPartSurface = hit.normal;
        }
        public void clearPart() {
            currentPart = null; // previousPart not set to null; kept for Remove()
            currentPartSurface = previousPartSurface = Vector3.zero;

            Destroy(testPartGO);
            testPartGO = null;
        }

        public void clearTestBlock() {
            if(testBlock) {
                Destroy(testBlock.gameObject);
                testBlock = null;
            }
        }
    }
    [SerializeField] private Target target;

    [System.Serializable] private class PartIndex {
        public Part part;
        public int amount;
        public int sellPrice;
    }
    [SerializeField] private List<PartIndex> inventory = new List<PartIndex>();
    [SerializeField] private Part selectedPart;
    //[SerializeField] private bool SPsocketAligned;
    //[SerializeField] private Part targetedPart;
    //[SerializeField] private TargetedPart targetedPart; 
    //[SerializeField] private bool TPsocketAligned;
    //[SerializeField] private Part testPart;
    //[SerializeField] private GameObject testPartGO;
    //[SerializeField] private Part previousPart;

    void Awake() {
        //SPsocketAligned = TPsocketAligned = false;
        //selectedPart = testPart = null;
        //testPartGO = null;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update() {
        RayCast();
        ProcessInputs();
    }
    void RayCast() {
        Ray ray = new Ray(camera.transform.position, camera.transform.forward);
        Color rayColor = Color.yellow;
        //Vector3 targetedSurface;
        if(Physics.Raycast(ray, out hit, rayRange)) { // RayCast hit something
            if(hit.transform.GetComponent<Part>() != null) { // RayCast hit Part
                target.clearTestBlock();
                target.updateTargetPart(hit);

                infoText.text = target.currentPart.name;
                infoText.color = Color.blue;
                rayColor = Color.blue;

                Debug.DrawLine(hit.transform.position, hit.transform.position + hit.normal, Color.red);
                if(selectedPart && target.currentPart) {
                    if(!target.testPartGO) { // no testPartGO Instantiated
                        PreviewPart();
                    }
                    else { // testPartGO already Instantiated
                        if(target.currentPartSurface != target.previousPartSurface) {
                            Destroy(target.testPartGO);
                            target.testPartGO = null;

                            PreviewPart();
                        }
                    }   
                }
            }
            else { // RayCast hit non-Part GameObject
                target.clearPart();
                target.targetGO = hit.transform.gameObject;
                if(!target.currentPart) { // only if no current targeted Part, so that target is only a GameObject
                    PreviewBaseBlock();
                }

                infoText.text = target.targetGO.name;
                infoText.color = Color.black;
            }
        }
        else { // RayCast hit nothing
            //targetedPart = null;
            //target = null;
            target.clearPart();
            target.targetGO = null;
            target.clearTestBlock();

            infoText.text = "";
            infoText.color = Color.black;
        }
        Debug.DrawLine(camera.transform.position, camera.transform.position + camera.transform.forward * rayRange, rayColor);

    }
    void PreviewPart() {
        if(target.baseBlock) {
            Vector3 offset = target.currentPart.transform.position + target.currentPartSurface;
            target.testPartGO = GameObject.Instantiate(selectedPart, offset, Quaternion.identity).gameObject;
            target.testPartGO.GetComponent<Collider>().enabled = false;
            //Destroy(testPart);

            //testPartGO = GameObject.Instantiate(selectedPart, offset, Quaternion.identity).gameObject;
            /*
            Mesh mesh = selectedPart.gameObject.GetComponent<MeshFilter>().mesh;
            Vector3 offset = targetedPart.transform.position + hit.normal;
            Graphics.DrawMeshNow(mesh, offset, Quaternion.identity);
            */
        
            Debug.Log("TestPlace()");
        }
    }

    void PreviewBaseBlock() {
        if(!target.baseBlock && !target.testPartGO && selectedPart is Block && target.targetGO) {
            Vector3 placement = hit.transform.gameObject.transform.position + hit.normal; // intercept offset by normal
            if(!target.testBlock) {
                target.testBlock = GameObject.Instantiate(selectedPart, placement, Quaternion.identity) as Block;
                target.testBlock.GetComponent<Collider>().enabled = false;
            }
            else {
                target.testBlock.transform.position = placement;
            }
        }
    }
    void DropBaseBlock() {
        if(target.testBlock) {
            target.baseBlock = target.testBlock;
            target.baseBlock.GetComponent<Collider>().enabled = true;
            target.baseBlock.IsBaseBlock = true;
            // rest of logic continued in Block.cs
        }
        else {
            Debug.LogError("DropBaseBlock(): cannot place; target.testPartGO not null");
            if(!(selectedPart is Block)) {
                Debug.LogError("DropBaseBlock(): selectedPart is not a Block");
            }
        }
    }

    void ProcessInputs() {
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Jump"), Input.GetAxis("Vertical"));
        transform.Translate(move * speed);
        inputs.translation = move;

        Vector3 turn = new Vector3(0, Input.GetAxis("Mouse X"), 0);
        transform.Rotate(turn);
        inputs.rotation = turn;

        Vector3 look = new Vector3(-Input.GetAxis("Mouse Y"), 0, 0);
        camera.transform.Rotate(look);
        inputs.rotation += look;

        if(Input.GetKeyDown(KeyCode.Alpha1)) {
            if(inventory[0].amount > 0) {
                selectedPart = inventory[0].part;
            }
        }

        if(Input.GetKeyDown(KeyCode.Mouse0)) {
            Add(selectedPart);
        }
        else if(Input.GetKeyDown(KeyCode.Mouse1)) {
            Remove(target.previousPart);
        }

        if(Input.GetKeyDown(KeyCode.Escape)) {
            Cursor.visible = !Cursor.visible;
        }
    }
    void Add(Part addPart) {
        Debug.Log("Add()");
        if(target.testPartGO && selectedPart) { // if a Part is selected, and a Part is targeted
            Debug.Log("Add(): targetedpart && selectedPart");
            //testPart = null;
            //testPartGO.AddComponent<Part>();
            target.testPartGO.GetComponent<Collider>().enabled = true;
            target.testPartGO = null;

            /*
            foreach(Socket TPsocket in targetedPart.sockets) { // search through targetedPart Sockets first
                Debug.Log("Add(): DotProduct(Camera, Socket) = " + Vector3.Dot(camera.transform.forward, TPsocket.transform.up));
                if(Vector3.Dot(camera.transform.forward, TPsocket.transform.up) < 0) { // if Camera faces a TPsocket
                    Debug.Log("Add(): Camera facing TPsocket");
                    foreach(Socket APsocket in addPart.sockets) {
                        if(Vector3.Dot(TPsocket.transform.up, APsocket.transform.up) == -1) { // if sockets align
                            Debug.Log("Add(): APsocket facing TPsocket");
                            inventory[addPart.Index].amount--;
                        }
                        break; // end search
                    }
                }
            }
            */
        }
        else { // neither selected nor targeted Part
            if(target.testPartGO == null) {
                Debug.LogError("Add(): testPartGO is null!");
            }
            if(selectedPart == null) {
                Debug.LogError("Add(): selectedPart is null!");
            }
        }
    }
    void Remove(Part removePart) {
        Destroy(removePart.gameObject);
        if(removePart is Block && (removePart as Block).IsBaseBlock) {
            target.baseBlock = null;
        }

        //targetedPart = null;
        /*
        foreach(Socket socket in removePart.sockets) {
            socket.next = null;
        }
        inventory[(int)removePart.partType].amount++;
        */
    }

}

/*

*/