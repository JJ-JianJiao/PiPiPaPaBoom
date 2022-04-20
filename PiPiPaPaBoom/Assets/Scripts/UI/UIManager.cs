using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public List<GameObject> hearts = new List<GameObject>();

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
        DontDestroyOnLoad(this);
    }

    public void UpdatePlayerHealth(int currentHealth) {
        for (; currentHealth < hearts.Count; currentHealth++)
        {
            hearts[currentHealth].SetActive(false);
        }
    }
}
