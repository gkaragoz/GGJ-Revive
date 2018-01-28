using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public static UIManager instance;

    public GameObject objGameOver;
    public GameObject objGamePlay;

    public GameObject imgYouWin;
    public GameObject imgGameOver;

    public Button btnReplay;
    public Button btnPlay;

	void Awake () {
        if (instance == null) {
            instance = this;
        }

        objGamePlay = GameObject.Find("Canvas").transform.GetChild(0).gameObject;
        objGameOver = GameObject.Find("Canvas").transform.GetChild(1).gameObject;

        btnReplay = objGameOver.GetComponentInChildren<Button>(true);
        btnPlay = objGamePlay.GetComponentInChildren<Button>(true);

        imgYouWin = objGameOver.transform.GetChild(2).gameObject;
        imgGameOver = objGameOver.transform.GetChild(1).gameObject;

        imgYouWin.SetActive(false);
        imgGameOver.SetActive(false);

        btnPlay.onClick.AddListener(delegate
        {
            StartGame();    
        });

        btnReplay.onClick.AddListener(delegate
        {
            SceneManager.LoadScene(0);
        });
	}

    public void PlayerWin() {
        imgYouWin.SetActive(true);
        imgGameOver.SetActive(false);
    }

    public void OpponentWin() {
        imgYouWin.SetActive(false);
        imgGameOver.SetActive(true);
    }

    public void StartGame()
    {
        objGameOver.SetActive(false);
        objGamePlay.SetActive(false);
        GameManager.instance.isGameStarted = true;
        GameManager.instance.GameFinished = false;
    }

    public void OpenGameOver(string who)
    {
        if (who == "Player") {
            PlayerWin();
        } else if (who == "Opponent") {
            OpponentWin();
        }

        objGamePlay.SetActive(false);
        objGameOver.SetActive(true);
        GameManager.instance.isGameStarted = false;
    }

}
