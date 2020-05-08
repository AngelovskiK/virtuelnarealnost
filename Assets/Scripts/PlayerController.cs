using Assets.Scripts.HelperClasses;
using System;
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
    private GameController gameController;
    private GameObject isCarryingObject = null;

    public Dictionary<GameItem, float> items;

    // Start is called before the first frame update
    void Start()
    {
        items = new Dictionary<GameItem, float>();
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    public void TryPickUp(Pickupable pickupable)
    {
        if (isCarryingObject != null && pickupable.gameObject == isCarryingObject)
        {
            isCarryingObject.GetComponent<Rigidbody>().useGravity = true;
            isCarryingObject.GetComponent<BoxCollider>().isTrigger = false;
            isCarryingObject.transform.position += Camera.main.transform.forward * 0.1f;
            isCarryingObject = null;
        }
        else if (isCarryingObject == null)
        {
            isCarryingObject = pickupable.gameObject;
            isCarryingObject.GetComponent<BoxCollider>().isTrigger = true;
            isCarryingObject.GetComponent<Rigidbody>().useGravity = false;
        }
    }

    // Update is called once per frame
    void Update() 
    {        
        moving = Input.GetButton("Fire1") && !Input.GetKey(KeyCode.LeftAlt) && !Input.GetKey(KeyCode.LeftControl) && !reticlePointer.isOnInteractive;
        if (isCarryingObject)
        {
            Vector3 v3 = Camera.main.transform.forward;
            v3.y = 0;
            Vector3 p = gameObject.transform.position;
            p.y = 0.5f;
            isCarryingObject.transform.position = p + v3 * 0.5f * isCarryingObject.GetComponent<BoxCollider>().size.magnitude;
            isCarryingObject.transform.eulerAngles = new Vector3(0, Camera.main.transform.eulerAngles.y, 0);

        }
    }

    public void CollectItem(ItemScript itemScript)
    {
        if (items.ContainsKey(itemScript.Item))
        {
            items[itemScript.Item] += itemScript.quantity;
        }
        else
        {
            items[itemScript.Item] = itemScript.quantity;
        }
        Destroy(itemScript.gameObject);
    }

    void FixedUpdate()
    {
        if (moving)
        {
            Vector3 v3 = cameraTransform.forward;
            v3.y = 0;
            myRigidbody.velocity = v3 * Time.deltaTime * moveSpeed;
        } 
    }
}
