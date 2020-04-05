using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody myRigidbody;
    public float moveSpeed = 10;
    public Transform cameraTransform;
    public GvrReticlePointer reticlePointer;
    private bool moving;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update() 
    {        
        moving = Input.GetButton("Fire1") && !Input.GetKey(KeyCode.LeftAlt) && !Input.GetKey(KeyCode.LeftControl) && !reticlePointer.isOnInteractive;
    }

    void FixedUpdate()
    {
        if (moving)
        {
            myRigidbody.velocity = cameraTransform.forward * Time.deltaTime * moveSpeed;
        } 
    }
}
