using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    CharacterController characterController;

    public float speed = 6.0f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public float rotateSpeed = 35f;

    private Vector3 moveVector = Vector3.zero;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void FixedUpdate()
    {

        //ROTATION
        float mouseInput = Input.GetAxis("Mouse X");
        Vector3 lookhere = new Vector3(0, mouseInput * rotateSpeed, 0);
        transform.Rotate(lookhere);



        if (characterController.isGrounded)
        {
            moveVector = transform.rotation * new Vector3(Input.GetAxisRaw("Horizontal"), 0.0f, Input.GetAxisRaw("Vertical"));

            moveVector *= speed;

            if (Input.GetButton("Jump"))
            {
                moveVector.y = jumpSpeed;
            }
        }

        moveVector.y -= Time.deltaTime;

        characterController.Move(moveVector * Time.deltaTime);
    }

}