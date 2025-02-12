using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    public GameObject door,left,right,thisobject;
    public LeftAnimControl leftanimate;
    public RightAnimControl rightanimate;
    public Rigidbody self;
    public bool freezeholdblue=false,freezeholdred=false;

    void start() {
        leftanimate=left.GetComponent<LeftAnimControl>();
        rightanimate=right.GetComponent<RightAnimControl>();
    }
    // Update is called once per frame
    void Update()
    {
        if((thisobject.name==("ButtonBlue") && leftanimate.bluefreeze && !freezeholdblue) || 
        (thisobject.name==("ButtonRed") && rightanimate.redfreeze && !freezeholdred)) {
            self.constraints=RigidbodyConstraints.FreezeAll;
        } else if((thisobject.name==("ButtonBlue") && !leftanimate.bluefreeze && freezeholdblue) || 
        (thisobject.name==("ButtonRed") && !rightanimate.redfreeze && freezeholdred)) {
            self.constraints&=~RigidbodyConstraints.FreezePositionY;
        }
        if(transform.position.y<=-0.6) {
            door.SetActive(false);
        } else {
            door.SetActive(true);
        }
            
        Vector3 clampedPosition = transform.position;
         // Now we can manipulte it to clamp the y element
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, -0.718f, -0.383f);
        // re-assigning the transform's position will clamp it
        transform.position = clampedPosition;

        freezeholdblue=leftanimate.bluefreeze;
        freezeholdred=rightanimate.redfreeze;
    }
}
