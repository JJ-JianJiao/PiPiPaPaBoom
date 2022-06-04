using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
using System;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public List<GameObject> hearts = new List<GameObject>();

    [Header("UI items")]
    public Button pauseBtn;
    public GameObject pauseMenuPanel;
    public Button resumeBtn;
    public Button playAgainBtn;
    public Button backToMainBtn;
    public Slider bossHealthSlider;
    public GameObject gameOverPanel;

    //[SerializeField]
    //public PilipalaInputSys uiControls;
    //private bool pauseGame;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
        DontDestroyOnLoad(this);

        //uiControls = new PilipalaInputSys();
        //uiControls.UIController.PauseGame.performed += ctx => pauseGame = true;
    }

    private void Start()
    {
        pauseBtn.onClick.AddListener(PauseBtn_OnClick);
        resumeBtn.onClick.AddListener(ResumeBtn_OnClick);
        playAgainBtn.onClick.AddListener(PlayerAgainBtn_OnClick);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {

            if (pauseMenuPanel.activeInHierarchy)
                ResumeBtn_OnClick();
            else
                PauseBtn_OnClick();


        }

        //if (pauseGame)
        //{
        //    if (pauseMenuPanel.activeInHierarchy)
        //        ResumeBtn_OnClick();
        //    else
        //        PauseBtn_OnClick();
        //}
    }
    //public void TriggerPasueBtn(InputAction.CallbackContext value) {

    //    if (pauseMenuPanel.activeInHierarchy)
    //        ResumeBtn_OnClick();
    //    else
    //        PauseBtn_OnClick();

    //}

    public void UpdatePlayerHealth(int currentHealth) {
        for (; currentHealth < hearts.Count; currentHealth++)
        {
            hearts[currentHealth].SetActive(false);
        }
    }

    public void ResetPlayerHealth() {
        foreach (var heart in hearts)
        {
            heart.SetActive(true);
        }
    }

    private void PauseBtn_OnClick()
    {
        pauseMenuPanel.SetActive(true);
        pauseBtn.gameObject.SetActive(false);
        Time.timeScale = 0;
    }

    private void ResumeBtn_OnClick()
    {
        pauseMenuPanel.SetActive(false);
        pauseBtn.gameObject.SetActive(true);
        Time.timeScale = 1;
    }

    public void SetBossHealth(float value) {
        bossHealthSlider.transform.parent.gameObject.SetActive(true);
        bossHealthSlider.maxValue = value;
        UpdateBossHealth(value);
    }

    public void UpdateBossHealth(float currentHealth) {
        bossHealthSlider.value = currentHealth;
    }

    public void ActiveGameOverPanel() {
        gameOverPanel.SetActive(true);
        pauseBtn.gameObject.SetActive(false);
    }

    public void PlayerAgainBtn_OnClick() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        GameManager.Instance.ResetGameState();
        ResetPlayerHealth();
        if(gameOverPanel.activeInHierarchy) gameOverPanel.SetActive(false);
        if(pauseMenuPanel.activeInHierarchy) pauseMenuPanel.SetActive(false);
        if (Time.timeScale == 0) Time.timeScale = 1;

    }
}
