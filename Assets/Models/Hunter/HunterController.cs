using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterController : MonoBehaviour
{
    float speed = 4;
    float rotSpeed = 80;
    float gravity = 8;

    Vector3 moveDir = Vector3.zero;

    CharacterController controller;
    Animator anim;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (controller.isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                moveDir = new Vector3(0, 0, 1);
                moveDir *= speed;
            }
        }

        moveDir.y -= gravity * Time.deltaTime;
        controller.Move(moveDir * Time.deltaTime);
    }

}
