using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    public float speed=12f;
    public float gravity=-9.81f;
    public float jumpHeight=3f;

    public Transform groundCheck;
    public float groundDistance=0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    bool isGrounded, startplay=false;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R)) SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        if(Input.GetKeyDown(KeyCode.Escape)) Application.Quit();

        isGrounded=Physics.CheckSphere(groundCheck.position,groundDistance,groundMask);

        if(isGrounded && velocity.y < 0) {
            controller.slopeLimit=45.0f;
            velocity.y=-2f;
        }

        float x=Input.GetAxis("Horizontal");
        float z=Input.GetAxis("Vertical");

        Vector3 move=transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

        if(Input.GetButtonDown("Jump") && isGrounded) {
            controller.slopeLimit = 100.0f;
            velocity.y=Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y+=gravity*Time.deltaTime;
        controller.Move(velocity*Time.deltaTime);

        if((z!=0 || x!=0) && !startplay && isGrounded) {
            FindObjectOfType<AudioManager>().Play("Walking");
            startplay=true;
        } else if((z==0 && x==0) || !isGrounded) {
            FindObjectOfType<AudioManager>().Stop("Walking");
            startplay=false;
        }
    }
}
