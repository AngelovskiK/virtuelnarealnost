using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickupable : MonoBehaviour
{
    // Update is called once per frame
    public void Interact()
    {
        if ((gameObject.transform.position - Camera.main.transform.position).magnitude > 1.5)
            return;
        GameObject playerGameObject = GameObject.FindGameObjectWithTag("Player");
        playerGameObject.GetComponent<PlayerController>().TryPickUp(this);
    }
}
