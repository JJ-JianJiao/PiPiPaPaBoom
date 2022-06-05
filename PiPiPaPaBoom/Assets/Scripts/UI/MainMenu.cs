using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    private Button newGameBtn;
    private Button continueBtn;
    private Button quiteBtn;

    private void Awake()
    {
        newGameBtn = transform.GetChild(1).GetComponent<Button>();
        continueBtn = transform.GetChild(2).GetComponent<Button>();
        quiteBtn = transform.GetChild(3).GetComponent<Button>();

        newGameBtn.onClick.AddListener(StartNewGame);
        continueBtn.onClick.AddListener(ContinueGame);
        quiteBtn.onClick.AddListener(QuitGame);

        if (!PlayerPrefs.HasKey("Level"))
        {
            continueBtn.interactable = false;
            continueBtn.transform.GetChild(0).GetComponent<TMP_Text>().color = new Color(179 / 255f, 179 / 255f, 179 / 255f, 1f);
        }
        else
        {
            continueBtn.interactable = true;
            continueBtn.transform.GetChild(0).GetComponent<TMP_Text>().color = Color.white;

        }

    }

    public void ContinueGame()
    {
        int continueLevel = PlayerPrefs.GetInt("Level");
        SceneManager.LoadScene(continueLevel);
    }


    public void StartNewGame()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }

    public void QuitGame() {

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif

    }
}

