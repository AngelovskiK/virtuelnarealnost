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

    public AudioClip openClip;
    public AudioClip closeClip;

    private AudioSource audioSource;

    void Start()
    {
        inputs = inputTraces.Select(it => (ISignalCarrier)it).ToList();
        animator = GetComponent<Animator>();
        animator.SetBool("isOpen", isOpen);
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        audioSource = GetComponent<AudioSource>();

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
        bool prev = isOpen;
        isOpen = !isOpen;
        if (!prev && isOpen)
            audioSource.clip = openClip;
        else if (prev && !isOpen)
            audioSource.clip = closeClip;
        if (prev != isOpen)
            audioSource.Play();
        animator.SetBool("isOpen", isOpen);
    }

    // Update is called once per frame
    void Update()
    {
        if (activationNeeded == 0)
            return;
        float inputActivation = inputs.Select(t => t.GetSignal()).Sum();

        bool prev = isOpen;
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
        if (!prev && isOpen)
            audioSource.clip = openClip;
        else if (prev && !isOpen)
            audioSource.clip = closeClip;
        if (prev != isOpen)
            audioSource.Play();
        animator.SetBool("isOpen", isOpen);
    }

    public void TryUnlock()
    {
        GameObject playerGameObject = GameObject.FindGameObjectWithTag("Player");
        if ((playerGameObject.transform.position - Camera.main.transform.position).magnitude > 2.5)
            return;

        if (!isLocked)
        {
            gameController.Notify("Вратата не е заклучена");
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
