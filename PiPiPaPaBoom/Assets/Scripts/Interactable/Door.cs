using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;


public enum DoorType { Entrance, Exit }
public enum LevelType {level1 = 1,level2 =2 ,level3 = 3,level4 = 4,level5 = 5, empty =6}

public class Door : MonoBehaviour
{

    public DoorType type = DoorType.Entrance;

    Animator doorAnim;

    Collider2D coli;

    public GameObject doorSign;

    public LevelType nextlevel = LevelType.empty;

    //PilipalaInputSys controls;

    private void Awake()
    {
        doorAnim = GetComponent<Animator>();
        coli = GetComponent<Collider2D>();
        DisableColi();
        doorSign = transform.GetChild(0).gameObject;
        //controls = new PilipalaInputSys();
        //controls.Player.Interact.started += ctx => NextLevel();
    }

    private void OnEnable()
    {
        //controls.Player.Enable();
    }

    private void OnDisable()
    {
        //controls.Player.Disable();
    }

    private void Update()
    {
#if !UNITY_ANDROID ||UNITY_IOS
        if (doorSign.activeSelf) {
            if (Input.GetKeyDown(KeyCode.J)) {
                NextLevel();

            }
        }
#endif
    }

    private void Start()
    {
        if (type == DoorType.Entrance) {
            doorAnim.Play("Close");
        }
        if (type == DoorType.Exit) {
            GameManager.Instance.RegistExitDoor(this);
        }
    }

    public void OpenDoor() {
        doorAnim.Play("Open");
    }

    public void EnableColi() {
        coli.enabled = true;
    }

    public void DisableColi() {
        coli.enabled = false;
    }



    private void NextLevel() {

        if (nextlevel != LevelType.empty)
        {
            SceneManager.LoadScene((int)nextlevel);
            GameManager.Instance.SavePlayerData();
        }
        else {
            UIManager.instance.ActiveVictoryPanel();
        }
    }
#if UNITY_ANDROID ||UNITY_IOS
    private void OnTriggerEnter2D(Collider2D collision)
    {
        NextLevel();
    }
#else
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            doorSign.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            doorSign.SetActive(false);
        }
    }
#endif

}
