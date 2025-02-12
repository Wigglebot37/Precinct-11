using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightAnimControl : MonoBehaviour
{   
    public Animator animright;
    public CanvasGroup redcanvas;
    public ParticleSystem particleright;
    public float maxlength, timestamp;
    public bool redfreeze=false,redtrigger=false,keepholdright=false;

    // Use this for initialization
    void Start () {
        animright = GetComponent<Animator>();
        particleright = GetComponent<ParticleSystem>();
        redcanvas.alpha=0f;
        maxlength=1.0f;
        timestamp=1.0f;
    }
    
    // Update is called once per frame
    void Update () {   
        if(Input.GetMouseButtonDown(1) && animright.GetCurrentAnimatorStateInfo(0).IsName("Default State") && !keepholdright) {
            animright.Play("Arm-Right-Lift");
        } if(Input.GetMouseButtonUp(1) && animright.GetCurrentAnimatorStateInfo(0).IsName("Arm-Right-Lift")) {
            if(animright.GetCurrentAnimatorStateInfo(0).normalizedTime>=maxlength) {
                animright.Play("Arm-Right-Lower");
                particleright.Play();
                redfreeze=!redfreeze;
                if(redfreeze) FindObjectOfType<AudioManager>().Play("Freeze");
                else FindObjectOfType<AudioManager>().Play("Unfreeze");
                redtrigger=true;
            } else {
                timestamp=animright.GetCurrentAnimatorStateInfo(0).normalizedTime % 1;
                animright.Play("Arm-Right-Lift-Rev", -1, 1-timestamp);
            }
        }
        if(!Input.GetMouseButtonDown(1) && animright.GetCurrentAnimatorStateInfo(0).normalizedTime>=maxlength && animright.GetCurrentAnimatorStateInfo(0).IsName("Arm-Right-Lower")) {
            animright.Play("Default State");
        }
        if(!Input.GetMouseButtonDown(1) && animright.GetCurrentAnimatorStateInfo(0).normalizedTime>=maxlength && animright.GetCurrentAnimatorStateInfo(0).IsName("Arm-Right-Lift-Rev")) {
            animright.Play("Default State");
        }

        if(redtrigger) {
            if(redfreeze) {
                redcanvas.alpha+=0.01f;
                if(redcanvas.alpha==1) redtrigger=false;
            } else {
                redcanvas.alpha-=0.01f;
                if(redcanvas.alpha==0) redtrigger=false;
            }
        }
    }
}