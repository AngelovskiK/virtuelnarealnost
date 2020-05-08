using Assets.Scripts.HelperInterfaces;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DoorController : MonoBehaviour
{

    public bool isLocked = false;
    public bool isOpen = false;
    public bool switchBehaivour = false;
    public bool autoOpenUponActivation = true;
    public int keyId = 0;
    public List<MonoBehaviour> inputTraces;
    public int activationNeeded = 1;

    private Animator animator;
    private GameController gameController;
    private List<ISignalCarrier> inputs;
    private float counter = -1;
    
    void Start()
    {
        inputs = inputTraces.Select(it => (ISignalCarrier)it).ToList();
        animator = GetComponent<Animator>();
        animator.SetBool("isOpen", isOpen);
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    public void Interact()
    {
        if ((gameObject.transform.position - Camera.main.transform.position).magnitude > 2.5)
            return;

        if (isLocked && !isOpen)
        {
            gameController.Notify("Заклучено!");
            return;
        }
        isOpen = !isOpen;
        animator.SetBool("isOpen", isOpen);
    }

    // Update is called once per frame
    void Update()
    {
        if (activationNeeded == 0)
            return;
        float inputActivation = inputs.Select(t => t.GetSignal()).Sum();
        if (switchBehaivour)
        {
            if (counter > 0)
                counter -= Time.deltaTime;
            if (inputActivation >= activationNeeded && counter <= 0)
            {
                isOpen = !isOpen;
                counter = 1f;
            }
        }
        else
        {
            isOpen = inputActivation >= activationNeeded;
        }
        animator.SetBool("isOpen", isOpen);
    }

    public void TryUnlock()
    {
        GameObject playerGameObject = GameObject.FindGameObjectWithTag("Player");
        if ((playerGameObject.transform.position - Camera.main.transform.position).magnitude > 2.5)
            return;

        if (!isLocked)
        {
            gameController.Notify("Ковчегот не е заклучен");
            return;
        }

        PlayerController player = playerGameObject.GetComponent<PlayerController>();
        if (player.items.Keys.Any(i => i.Id == keyId))
        {
            isLocked = false;
            gameController.Notify("Отклучено!");
        }
        else
            gameController.Notify("Немате соодветен клуч!");
    }
}
