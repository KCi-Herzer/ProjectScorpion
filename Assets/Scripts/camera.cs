using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera : MonoBehaviour
{
    [SerializeField] int sensHori;
    [SerializeField] int sensVert;

    [SerializeField] int lockVertMin;
    [SerializeField] int lockVertMax;

    [SerializeField] bool invert;
    [Range(0, 100)] [SerializeField] float tiltDistance;

    float xRotation;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        //Get the input
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * sensHori;
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * sensVert;

        if (invert)
            xRotation += mouseY;
        else
            xRotation -= mouseY;

        //clamp rotation
        xRotation = Mathf.Clamp(xRotation, lockVertMin, lockVertMax);

        //rotate the camera on the xAxis
        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        //rotate the player
        transform.parent.parent.Rotate(Vector3.up * mouseX);

        if (!gameManager.instance.isPaused)
        {
            Tilt();
        }

        

    }

    void Tilt()
    {
        if (Input.GetButtonDown("TiltR"))
        {
            transform.parent.localRotation = Quaternion.Euler(0, 0, -tiltDistance);
        }
        else if (Input.GetButtonUp("TiltR"))
        {
            transform.parent.localRotation = Quaternion.Euler(0, 0, 0);
        }

        if (Input.GetButtonDown("TiltL"))
        {
            transform.parent.localRotation = Quaternion.Euler(0, 0, tiltDistance);
        }
        else if (Input.GetButtonUp("TiltL"))
        {
            transform.parent.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }

    
}
