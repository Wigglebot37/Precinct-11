using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject : MonoBehaviour
{
    public GameObject left,right,item,tempParent;
    Animator leftanimator,rightanimator;
    LeftAnimControl leftanimate;
    RightAnimControl rightanimate;
    Material material;
    Vector3 objectPos;
    float distance,maxdist=2.5f,throwForce=600;
    public Rigidbody test;
    public bool isHolding=false,redon=false,blueon=false;
    public static bool alreadyhold=false,wait=false,keephold=false;
    public static GameObject itemheld;
    Renderer rend;
    Color redval,greenval;

    void Start() {
        rend=GetComponent<Renderer>();
        leftanimator=left.GetComponent<Animator>();
        rightanimator=right.GetComponent<Animator>();
        leftanimate=left.GetComponent<LeftAnimControl>();
        rightanimate=right.GetComponent<RightAnimControl>();
        material=GetComponent<Material>();
        redval=new Color(1f,0f,0f,0.5f);
        rend.material.SetColor("_OutlineColor", redval);
    }

    // Update is called once per frame
    void Update() {
        distance = Vector3.Distance (item.transform.position, tempParent.transform.position);
        if (distance > maxdist) {
            isHolding = false;
            if(item==itemheld) { itemheld=null; alreadyhold=false; test.constraints=RigidbodyConstraints.None; }
        }
        //Check if isholding
        if(isHolding) {
            test.velocity = Vector3.zero;
            test.angularVelocity = Vector3.zero;
            item.transform.SetParent(tempParent.transform);

            if(Input.GetKeyDown(KeyCode.Q) && !keephold) {
                test.AddForce(tempParent.transform.forward*throwForce);
                test.AddTorque(tempParent.transform.right*throwForce);
                isHolding=false;
                if(item==itemheld) { itemheld=null; alreadyhold=false; test.constraints=RigidbodyConstraints.None; }
            } else if(Input.GetKeyDown(KeyCode.E) && !wait && !keephold) {
                isHolding=false;
                if(item==itemheld) { itemheld=null; alreadyhold=false; test.constraints=RigidbodyConstraints.None; }
            } else if(!leftanimator.GetCurrentAnimatorStateInfo(0).IsName("Default State") || !rightanimator.GetCurrentAnimatorStateInfo(0).IsName("Default State")) {
                isHolding=false;
                if(item==itemheld) { itemheld=null; alreadyhold=false; test.constraints=RigidbodyConstraints.None; }
            }
        } else {
            objectPos=item.transform.position;
            item.transform.SetParent(null);
            test.useGravity=true;
            item.transform.position=objectPos;
        }
        if(!isHolding) {
            if(rightanimate.redfreeze) {
                if(item.tag=="Red_Cubes") test.constraints=RigidbodyConstraints.FreezeAll;
                redon=true;
            } else {
                if(item.tag=="Red_Cubes" || item.tag=="Pink_Cubes") test.constraints=RigidbodyConstraints.None;
                redon=false;
            }
            if(leftanimate.bluefreeze) {
                if(item.tag=="Blue_Cubes") test.constraints=RigidbodyConstraints.FreezeAll;
                blueon=true;
            } else {
                if(item.tag=="Blue_Cubes" || item.tag=="Pink_Cubes") test.constraints=RigidbodyConstraints.None;
                blueon=false;
            }
            if(blueon && redon) {
                if(item.tag=="Pink_Cubes") test.constraints=RigidbodyConstraints.FreezeAll;
            }
        }
        if(item==itemheld && wait) wait=false;
        if(isHolding) item.transform.Rotate(Vector3.right*Input.GetAxis("Mouse ScrollWheel")*80);
        if(item==itemheld && keephold) rend.material.SetFloat("_Outline", 0.1f);
        else if(item==itemheld && !keephold) rend.material.SetFloat("_Outline", 0f);
    }

    void FixedUpdate() {
        if(alreadyhold && item==itemheld && keephold) {
            keephold=false; leftanimate.keepholdleft=false; rightanimate.keepholdright=false;
        }
    }

    void OnMouseOver() {
        if(Input.GetKeyDown(KeyCode.E)) {
            if(!isHolding && !alreadyhold) { 
                isHolding=true; alreadyhold=true; itemheld=item; test.constraints=RigidbodyConstraints.FreezeAll; wait=true;
            }
            if(distance<=maxdist && isHolding && !keephold && wait) {
                if(leftanimator.GetCurrentAnimatorStateInfo(0).IsName("Default State") && rightanimator.GetCurrentAnimatorStateInfo(0).IsName("Default State")) {
                    test.useGravity = false;
                    item.transform.position = tempParent.transform.position;
                    item.transform.rotation = tempParent.transform.rotation;
                }
            }
        }
    }

    void OnCollisionStay(Collision collision) {
        if(alreadyhold && item==itemheld && !keephold) {
            keephold=true; leftanimate.keepholdleft=true; rightanimate.keepholdright=true;
        }
    }
}