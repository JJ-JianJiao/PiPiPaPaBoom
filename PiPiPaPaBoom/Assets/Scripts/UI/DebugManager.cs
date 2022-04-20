using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DebugManager : MonoBehaviour
{

    public Button reloadBtn;
    public Button generateEnemyBtn;
    public Button clearEnemyBtn;
    public Button exitBtn;

    public PilipalaController PilipalaController;

    public List<GameObject> enemyPrefabList = new List<GameObject>();
    public List<Transform> enemyGenratePositions = new List<Transform>();

    public List<GameObject> enemies = new List<GameObject>();

    public int currentIndex;

    private void Awake()
    {
        reloadBtn.onClick.AddListener(ReloadBtn_OnClick);
        generateEnemyBtn.onClick.AddListener(GenerateEnemy);
        clearEnemyBtn.onClick.AddListener(ClearEnemies);
        exitBtn.onClick.AddListener(QuitGame);
    }

    private void QuitGame()
    {
        Application.Quit();
    }

    private void Start()
    {
        currentIndex = 0;
#if UNITY_STANDALONE_OSX
        Screen.SetResolution(3840, 2160, true);
#elif UNITY_STANDALONE_WIN
        Screen.SetResolution(1920, 1080, true);
#endif
    }

    private void ReloadBtn_OnClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void PlayerDropDown_OnChange(int value) {

        switch (value)
        {
            case 0:
                Debug.Log("Normal");
                PilipalaController.currentHealth = PilipalaController.fullHealth;
                PilipalaController.attackRate = 1;
                break;
            case 1:
                Debug.Log("Invincible");
                PilipalaController.currentHealth = 99999;
                PilipalaController.attackRate = 0.1f;
                break;

            default:
                break;
        }
    }

    public void EnemyDropDown_OnChange(int value)
    {
        currentIndex = value;
    }

    public void GenerateEnemy() {
        var enemy = Instantiate(enemyPrefabList[currentIndex], enemyGenratePositions[currentIndex].position, Quaternion.identity);
        enemies.Add(enemy);
    }

    public void ClearEnemies() {
        for (int i = 0; i < enemies.Count; i++)
        {
            Destroy(enemies[i]);
        }

    }

}
