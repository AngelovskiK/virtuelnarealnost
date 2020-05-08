using Assets.Scripts.HelperInterfaces;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TraceController : MonoBehaviour, ISignalCarrier
{
    public int needed = 1;
    public List<MonoBehaviour> inputTraces;
    private List<ISignalCarrier> inputs;
    private bool isActive = false;
    public Color activeColor;
    public Color inactiveColor;

    private Renderer myRenderer;
    // Start is called before the first frame update
    void Start()
    {
        myRenderer = GetComponent<Renderer>();
        inputs = inputTraces.Select(it => (ISignalCarrier)it).ToList();
    }

    // Update is called once per frame
    void Update()
    {
        float inputActivation = inputs.Select(t => t.GetSignal()).Sum();       
        isActive = inputActivation >= needed;
        myRenderer.material.color = isActive ? activeColor : inactiveColor;
    }

    public float GetSignal()
    {
        return isActive ? 1f : 0f;
    }
}
