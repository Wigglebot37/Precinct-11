using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftAnimControl : MonoBehaviour
{   
    public Animator animleft;
    public CanvasGroup bluecanvas;
    public ParticleSystem particleleft;
    public float maxlength, timestamp;
    public bool bluefreeze=false,bluetrigger=false,keepholdleft=false;

    // Use this for initialization
    void Start () {
        animleft = GetComponent<Animator>();
        particleleft = GetComponent<ParticleSystem>();
        bluecanvas.alpha=0f;
        maxlength=1.0f;
        timestamp=1.0f;
    }
    
    // Update is called once per frame
    void Update () {
        if(Input.GetMouseButtonDown(0) && animleft.GetCurrentAnimatorStateInfo(0).IsName("Default State") && !keepholdleft) {
            animleft.Play("Arm-Left-Lift");
        } if(Input.GetMouseButtonUp(0) && animleft.GetCurrentAnimatorStateInfo(0).IsName("Arm-Left-Lift")) {
            if(animleft.GetCurrentAnimatorStateInfo(0).normalizedTime>=maxlength) {
                animleft.Play("Arm-Left-Lower");
                particleleft.Play();
                bluefreeze=!bluefreeze;
                if(bluefreeze) FindObjectOfType<AudioManager>().Play("Freeze");
                else FindObjectOfType<AudioManager>().Play("Unfreeze");
                bluetrigger=true;
            } else {
                timestamp=animleft.GetCurrentAnimatorStateInfo(0).normalizedTime % 1;
                animleft.Play("Arm-Left-Lift-Rev", -1, 1-timestamp);
            }
        }
        if(!Input.GetMouseButtonDown(0) && animleft.GetCurrentAnimatorStateInfo(0).normalizedTime>=maxlength && animleft.GetCurrentAnimatorStateInfo(0).IsName("Arm-Left-Lower")) {
            animleft.Play("Default State");
        }
        if(!Input.GetMouseButtonDown(0) && animleft.GetCurrentAnimatorStateInfo(0).normalizedTime>=maxlength && animleft.GetCurrentAnimatorStateInfo(0).IsName("Arm-Left-Lift-Rev")) {
            animleft.Play("Default State");
        }

        if(bluetrigger) {
            if(bluefreeze) {
                bluecanvas.alpha+=0.01f;
                if(bluecanvas.alpha==1) bluetrigger=false;
            } else {
                bluecanvas.alpha-=0.01f;
                if(bluecanvas.alpha==0) bluetrigger=false;
            }
        }
    }
}