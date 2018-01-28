using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public static UIManager instance;

    public GameObject objGameOver;
    public GameObject objGamePlay;

    public Button btnPlay;

	void Awake () {
        if (instance == null)
            instance = this;

        btnPlay.onClick.AddListener(delegate
        {
            StartGame();    
        });

        DontDestroyOnLoad(this.gameObject);
	}

    public void StartGame()
    {
        objGameOver.SetActive(false);
        objGamePlay.SetActive(false);
        GameManager.instance.isGameStarted = true;
    }

    public void OpenGameOver()
    {
        objGamePlay.SetActive(false);
        objGameOver.SetActive(true);
        GameManager.instance.isGameStarted = false;
    }

}
