using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    private Button newGameBtn;
    private Button continueBtn;
    private Button quiteBtn;

    private Animator anim;

    public PlayableDirector director;

    private void Awake()
    {
        newGameBtn = transform.GetChild(1).GetComponent<Button>();
        continueBtn = transform.GetChild(2).GetComponent<Button>();
        quiteBtn = transform.GetChild(3).GetComponent<Button>();
        anim = GetComponent<Animator>();

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

    private void Start()
    {
        AudioManager.Instance.Play(Musicname.Main);
    }

    public void ContinueGame()
    {
        int continueLevel = PlayerPrefs.GetInt("Level");
        SceneManager.LoadScene(continueLevel);
    }


    public void StartNewGame()
    {
        PlayerPrefs.DeleteAll();
        StartCoroutine(RunStartGameAnim());

        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }


    IEnumerator RunStartGameAnim() {
        director.Play();
        Debug.Log("PlayerOnce");

        while (director.state == PlayState.Playing) {
            yield return null;

        }
        //Animator
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

