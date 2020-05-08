using Assets.Scripts.HelperInterfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour, ISignalCarrier
{
    public GameObject contactPoint;
    public float weightOnMe = 0;
    public float requiredWeight = 1;
    
    private Rigidbody myRigidBody;
    public bool hasContact = false;

    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        hasContact = weightOnMe >= requiredWeight;
    }

    private void FixedUpdate()
    {
        if (weightOnMe <= 0.2f)
            myRigidBody.AddForce(new Vector3(0, 490f * Time.fixedDeltaTime, 0));
    }

    void OnCollisionEnter(Collision collision)
    {
        Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
        if (rb == null)
            return;
        weightOnMe += rb.mass;
    }

    private void OnCollisionExit(Collision collision)
    {
        Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
        if (rb == null)
            return;
        weightOnMe -= rb.mass;
    }

    public float GetSignal()
    {
        return hasContact ? 1f : 0f;
    }
}
