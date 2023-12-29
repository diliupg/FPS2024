using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMovement : MonoBehaviour
{
    public float mouseSensitivity = 600f;

    float XRotation = 0f;
    float YRotation = 0f;

    public float topClamp = -90f;
    public float bottomClamp = 90f;

    void Start()
    {
        // locking the cusor to the middle of the screen and making it invisible
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        // getting the mouse inputs
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // rotation around the x axis (look up and down)
        XRotation -= mouseY;

        // clamp the rotation
        XRotation = Mathf.Clamp(XRotation, topClamp, bottomClamp);

        // rotation around the y axis (look left and right)
        YRotation += mouseX;

        // apply rotations to the transform
        transform.localRotation = Quaternion.Euler(XRotation, YRotation, 0f);
    }
}
