using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        Paused,
        GameOver
    }

    public PlayerController playerControllerScript;
    public Transform canvasesRotator;
    public GameObject startCanvas;
    public GameObject helpCanvas;
    public GameObject pauseCanvas;
    public GameObject ingameCanvas;
    private Status status = Status.StartingMenu;
    public Text notificationText;
    public Text levelTimerText;
    public Text totalTimerText;
    public float notificationCounter = -1;
    public int noLevels;
    public int currentLevel = 1;

    private AudioSource cameraAudioSource;
    public List<AudioClip> BGMusicAudioClips;
    private int currentBGMusicAudioClipIndx;
    private static System.Random rng = new System.Random();
    private float currentLevelTime;
    private DateTime currentLevelStartTime;
    private float totalRunTime;
    private DateTime totalRunStartTime;

    private float[] levelTimes;

    private float endLevelHelpTimer = -1;

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
        cameraAudioSource = Camera.main.gameObject.GetComponent<AudioSource>();
        BGMusicAudioClips = BGMusicAudioClips.OrderBy(a => rng.Next()).ToList();
        currentBGMusicAudioClipIndx = 0;

        totalRunStartTime = DateTime.Now;
        currentLevelStartTime = DateTime.Now;

        levelTimes = new float[noLevels];
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

    public void EndLevel()
    {
        if (endLevelHelpTimer > 0)
            return;

        if (currentLevel == noLevels)
        {
            // game over
            playerControllerScript.enabled = false;
            startCanvas.SetActive(false);
            helpCanvas.SetActive(false);
            pauseCanvas.SetActive(false);
            ingameCanvas.SetActive(false);
            status = Status.GameOver;
        }
        else
        {
            levelTimes[currentLevel - 1] = currentLevelTime;
        }
        currentLevel++;
        currentLevelStartTime = DateTime.Now;
        currentLevelTime = 0f;

        endLevelHelpTimer = 1f;
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
        {
            canvasesRotator.eulerAngles = new Vector3(0, Camera.main.transform.eulerAngles.y, 0);
            if (endLevelHelpTimer > 0)
                endLevelHelpTimer -= Time.deltaTime;
        }
        if (status == Status.Ingame || status == Status.Paused)
        {
            totalRunTime += Time.deltaTime;
            currentLevelTime += Time.deltaTime;
            int runTimeInSeconds = (int)totalRunTime;
            int levelTimeInSeconds = (int)currentLevelTime;
            levelTimerText.text = string.Format("{0}:{1:D2}", (levelTimeInSeconds / 60), (levelTimeInSeconds % 60));
            totalTimerText.text = string.Format("Total: {0}:{1:D2}", (runTimeInSeconds / 60), (runTimeInSeconds % 60));
        }
        if (notificationCounter > 0)
        {
            notificationCounter -= Time.deltaTime;
            if(notificationCounter <= 0)
            {
                notificationText.text = "";
            }
        }
        if (!cameraAudioSource.isPlaying)
        {
            currentBGMusicAudioClipIndx++;
            currentBGMusicAudioClipIndx %= BGMusicAudioClips.Count;
            cameraAudioSource.clip = BGMusicAudioClips[currentBGMusicAudioClipIndx];
            cameraAudioSource.Play();
        }
    }

}
