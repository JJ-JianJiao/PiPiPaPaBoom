using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public PilipalaController playerController;

    public bool isGameOver;

    public List<Enemy> enemies;

    public List<Door> exitDoors;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
        }
        else
            Instance = this;

        DontDestroyOnLoad(gameObject);

        enemies = new List<Enemy>();
        exitDoors = new List<Door>();
    }

    public void GameOver() {
        isGameOver = true;
        UIManager.instance.ActiveGameOverPanel();
    }

    public void RegistEnemy(Enemy enemy) {
        enemies.Add(enemy);
    }

    public void RegistExitDoor(Door door) {
        exitDoors.Add(door);
    }

    public void RemoveEnemy(Enemy enemy) {
        enemies.Remove(enemy);
        if (enemies.Count == 0 && exitDoors.Count != 0) {
            foreach (var door in exitDoors)
            {
                door.OpenDoor();
                door.EnableColi();
#if UNITY_ANDROID ||UNITY_IOS
                door.doorSign.SetActive(true);
#endif
            }
            exitDoors.Clear();
        }
    }

    public void SavePlayerData() {
        if(playerController.isdead)
            PlayerPrefs.SetInt("PlayerHealth", playerController.fullHealth);
        else
            PlayerPrefs.SetInt("PlayerHealth", playerController.currentHealth);
        PlayerPrefs.SetInt("Level", SceneManager.GetActiveScene().buildIndex);
        PlayerPrefs.Save();
    }

    public int LoadPlayerData() {
        if (!PlayerPrefs.HasKey("PlayerHealth")) {
            PlayerPrefs.SetInt("PlayerHealth", playerController.fullHealth);
        }

        int getPlayerHealth = PlayerPrefs.GetInt("PlayerHealth");
        return getPlayerHealth;
    }

    public void ResetGameState() {
        exitDoors.Clear();
        enemies.Clear();
        playerController.currentHealth = playerController.fullHealth;
        SavePlayerData();
        UIManager.instance.UpdatePlayerHealth(playerController.currentHealth);
    }



}