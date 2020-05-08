using Assets.Scripts.HelperClasses;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemScript : MonoBehaviour
{
    public string itemName = "";
    public float quantity = 1;
    public int id = 0;
    public GameItem Item { get; private set;  }

    // Start is called before the first frame update
    void Start()
    {
        Item = new GameItem(itemName, id);
    }

    private void Update()
    {
    }

    public void PickUp()
    {
        if ((gameObject.transform.position - Camera.main.transform.position).magnitude > 1.2)
            return;
        PlayerController player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        player.CollectItem(this);
    }
}
