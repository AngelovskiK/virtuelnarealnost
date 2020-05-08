using Assets.Scripts.HelperInterfaces;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TrapDoorController : MonoBehaviour, ISignalCarrier
{
    public bool switchBehaivour = false;
    public int activationNeeded = 1;
    public List<MonoBehaviour> inputTraces;
    private List<ISignalCarrier> inputs;
    public bool isOpen = false;

    private Animator animator;
    private float counter = -1;

    public float GetSignal()
    {
        return isOpen ? 1f : 0f;
    }

    // Start is called before the first frame update
    void Start()
    {
        inputs = inputTraces.Select(it => (ISignalCarrier)it).ToList();
        animator = GetComponent<Animator>();
        animator.SetBool("isOpen", isOpen);
    }

    // Update is called once per frame
    void Update()
    {
        float inputActivation = inputs.Select(t => t.GetSignal()).Sum();
        if(switchBehaivour)
        {
            if (counter > 0)
                counter -= Time.deltaTime;
            if (inputActivation >= activationNeeded && counter <= 0)
            {
                isOpen = !isOpen;
                counter = 1f;
            }
        }else
        {
            isOpen = inputActivation >= activationNeeded;
        }
        animator.SetBool("isOpen", isOpen);
    }
}
