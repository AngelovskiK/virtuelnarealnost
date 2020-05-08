using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemInformation : MonoBehaviour
{
    GvrReticlePointer GRP;
    public Text itemTextLabel;
    // Start is called before the first frame update
    void Start()
    {
        GRP = GetComponent<GvrReticlePointer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GRP.CurrentRaycastResult.gameObject != null && (GRP.CurrentRaycastResult.gameObject.transform.position - Camera.main.transform.position).magnitude < 2.5)
        {
            ItemScript itemScript = getScript(GRP.CurrentRaycastResult.gameObject);
            if (itemScript == null)
                itemTextLabel.text = "";
            else
                itemTextLabel.text = $"{itemScript.itemName}{(itemScript.quantity != 1 ? itemScript.quantity.ToString("%.2f") : "")}";
        } else
            itemTextLabel.text = "";
    }

    ItemScript getScript(GameObject go)
    {
        ItemScript res = go.GetComponent<ItemScript>();
        if (res != null)
            return res;
        if (go.transform.parent == null)
            return null;
        return getScript(go.transform.parent.gameObject);
    }
}
