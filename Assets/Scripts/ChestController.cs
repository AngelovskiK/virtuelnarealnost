using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChestController : MonoBehaviour
{
    public bool isLocked = false;
    public bool isOpen = false;
    public int keyId = 0;
    private Animator animator;
    private GameController gameController;
    void Start()
    {
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
