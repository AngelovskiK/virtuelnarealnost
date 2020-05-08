using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    enum Status
    {
        StartingMenu = 0,
        HelpMenu,
        Ingame,
        Paused
    }

    public PlayerController playerControllerScript;
    public Transform canvasesRotator;
    public GameObject startCanvas;
    public GameObject helpCanvas;
    public GameObject pauseCanvas;
    public GameObject ingameCanvas;
    private Status status = Status.StartingMenu;
    public Text notificationText;
    public float notificationCounter = -1;

    internal void Notify(string m, float duration = 3)
    {
        notificationText.text = m;
        notificationCounter = duration;
    }

    void Start()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("GameController");
        if(objects.Length == 1)
        {
            playerControllerScript.enabled = false;
            startCanvas.SetActive(true);
            helpCanvas.SetActive(false);
            pauseCanvas.SetActive(false);
            ingameCanvas.SetActive(false);
        }
        else
        {
            playerControllerScript.enabled = true;
            startCanvas.SetActive(false);
            helpCanvas.SetActive(false);
            pauseCanvas.SetActive(false);
            ingameCanvas.SetActive(true);
        }
        foreach(GameObject obj in objects)
        {
            if(!ReferenceEquals(gameObject, obj))
            {
                Destroy(obj);
            }
        }
        DontDestroyOnLoad(this);
    }

    public void StartGame()
    {
        playerControllerScript.enabled = true;
        startCanvas.SetActive(false);
        helpCanvas.SetActive(false);
        pauseCanvas.SetActive(false);
        ingameCanvas.SetActive(true);
        status = Status.Ingame;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ShowHelp()
    {
        startCanvas.SetActive(false);
        helpCanvas.SetActive(true);
        pauseCanvas.SetActive(false);
        ingameCanvas.SetActive(false);
        status = Status.HelpMenu;
    }

    public void PauseGame()
    {
        playerControllerScript.enabled = false;
        startCanvas.SetActive(false);
        helpCanvas.SetActive(false);
        pauseCanvas.SetActive(true);
        ingameCanvas.SetActive(false);
        status = Status.Paused;
    }
    public void UnpauseGame()
    {
        playerControllerScript.enabled = true;
        startCanvas.SetActive(false);
        helpCanvas.SetActive(false);
        pauseCanvas.SetActive(false);
        ingameCanvas.SetActive(true);
        status = Status.Ingame;
    }

    public void ExitHelp()
    {
        startCanvas.SetActive(true);
        helpCanvas.SetActive(false);
        pauseCanvas.SetActive(false);
        ingameCanvas.SetActive(false);
        status = Status.StartingMenu;
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    // Update is called once per frame
    void Update()
    {
        if (status == Status.Ingame)
            canvasesRotator.eulerAngles = new Vector3(0, Camera.main.transform.eulerAngles.y, 0);
        if (notificationCounter > 0)
        {
            notificationCounter -= Time.deltaTime;
            if(notificationCounter <= 0)
            {
                notificationText.text = "";
            }
        }
    }
}
