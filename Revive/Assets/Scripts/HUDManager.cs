using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour {
	public Slider playerHealthSlider;
	public Slider opponentHealthSlider;

	void Awake()
    {
        playerHealthSlider = GameObject.Find("Canvas").transform.GetChild(3).GetComponent<Slider>();
        opponentHealthSlider = GameObject.Find("Canvas").transform.GetChild(2).GetComponent<Slider>();

		playerHealthSlider.value = 100;
		opponentHealthSlider.value = 100;
	}

	void Update () {
		playerHealthSlider.value = GameManager.instance.player.GetComponent<PlayerController> ().currentHealth / GameManager.instance.player.GetComponent<PlayerController> ().maxHealth;
		opponentHealthSlider.value = GameManager.instance.opponent.GetComponent<OpponentAI> ().currentHealth / GameManager.instance.opponent.GetComponent<OpponentAI> ().maxHealth;
	}

    public void CloseHUD()
    {
        playerHealthSlider.gameObject.SetActive(false);
        opponentHealthSlider.gameObject.SetActive(false);
    }

    public void OpenHUD()
    {
        playerHealthSlider.gameObject.SetActive(true);
        opponentHealthSlider.gameObject.SetActive(true);
    }
}
