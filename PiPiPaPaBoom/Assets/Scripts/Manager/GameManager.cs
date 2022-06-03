using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
            }
            exitDoors.Clear();
        }
    }
}
