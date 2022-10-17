using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(CharacterController))]
public class CharacterController3D : MonoBehaviour
{
    //Unity units per second
    public float Speed = 5f;

    public float JumpVelocity = 5f;

    public Vector3 currentVelocity;

    public bool ShowOnGround;

    public Material pigMaterial;
    public Material pinkMaterial;

    //reference to the character Controller attached to our game object
    CharacterController controller;

    public Transform cameraTarget;

    public float cameraDistance = 5;
    public float defaultPitch = 30;
    private float cameraPitch;
    private float cameraYaw = 0;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        cameraTarget = transform;
        cameraPitch = defaultPitch;
        setMaterial(pigMaterial);
    }

    public void setMaterial(Material material)
    {
        GetComponent<MeshRenderer>().material = material;
    }

    // Update is called once per frame
    void Update()
    {
        updateController();
        updateCamera();
    }

    private void updateCamera()
    {
        if (Input.GetMouseButton(1) || Input.GetMouseButton(0))
        {
            cameraPitch += Input.GetAxis("Mouse Y") * -2.0f;
            cameraPitch = Mathf.Clamp(cameraPitch, -10.0f, 80f);
            cameraYaw += Input.GetAxis("Mouse X") * 5.0f;
            cameraYaw = cameraYaw % 360.0f;
        }
        else
        {
            cameraYaw = Mathf.LerpAngle(cameraYaw, cameraTarget.eulerAngles.y, 5.0f * Time.deltaTime);
            cameraPitch = Mathf.LerpAngle(cameraPitch, cameraTarget.eulerAngles.x + defaultPitch, 5.0f * Time.deltaTime);
        }

        if(Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            cameraDistance -= Input.GetAxis("Mouse ScrollWheel") * 5.0f;
            cameraDistance = Mathf.Clamp(cameraDistance, 2.0f, 12.0f);
        }


        Vector3 newCameraPosition = cameraTarget.position + (Quaternion.Euler(cameraPitch, cameraYaw, 0) * Vector3.back * cameraDistance);
        Camera.main.transform.position = newCameraPosition;
        Camera.main.transform.LookAt(cameraTarget.position);
    }

    private void updateController()
    {
        Vector2 RawInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if (RawInput.magnitude > 1)
        {
            RawInput.Normalize();
        }

        ShowOnGround = controller.isGrounded;

        if (controller.isGrounded && Input.GetButtonDown("Jump"))
        {
            currentVelocity.y += JumpVelocity;
        }
        else if (controller.isGrounded)
        {
            currentVelocity.y = Physics.gravity.y * Time.deltaTime;
        }
        else if (!controller.isGrounded)
        {
            currentVelocity.y += Physics.gravity.y * Time.deltaTime;
        }

        if (Input.GetMouseButton(1))
        {
            transform.rotation = Quaternion.Euler(0, cameraYaw, 0); // face the same way as the camera
        }

        Vector3 RelativeInput = new Vector3(RawInput.x, 0, RawInput.y);
        Vector3 moveDirection = transform.TransformDirection(RelativeInput);


        controller.Move(moveDirection * Speed * Time.deltaTime + currentVelocity * Time.deltaTime);
    }
}
