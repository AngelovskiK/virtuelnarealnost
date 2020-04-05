using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCube : MonoBehaviour
{
    private double rotateSpeed = 25;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rotateSpeed -= 20 *  Time.deltaTime;
        if (rotateSpeed < 0)
            rotateSpeed = 0;
        transform.Rotate(0, (float) (rotateSpeed * Time.deltaTime), 0);
    }

    public void IncreaseSpeed()
    {
        rotateSpeed += 35;
        if(rotateSpeed > 100)
        {
            rotateSpeed = 100;
        }
    }
}
