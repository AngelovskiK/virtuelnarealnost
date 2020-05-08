using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PressButton : MonoBehaviour
{
   
    private Button button;
    private bool pressed = false;
    private float pressedFor = 0;

    private void Start()
    {
        button = gameObject.GetComponent<Button>();
    }

    public void Press()
    {
        pressed = true;
        button.image.color = button.colors.pressedColor;
    }

    public void Release()
    {
        pressed = false;
        pressedFor = 0;
        button.image.color = button.colors.normalColor;
    }

    void FixedUpdate()
    {
        if (pressed)
        {
            pressedFor += Time.deltaTime;
            if(pressedFor > 1.5f)
            {
                button.onClick.Invoke();
                Release();
            }
        }
    }
}
