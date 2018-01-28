using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour {
	public Slider playerHealthSlider;
	public Slider opponentHealthSlider;
	public GameObject player;
	public GameObject opponent;

	void Awake(){
		playerHealthSlider.value = 100;
		opponentHealthSlider.value = 100;
	}

	// Update is called once per frame
	void Update () {
		playerHealthSlider.value = player.GetComponent<PlayerController> ().currentHealth / player.GetComponent<PlayerController> ().maxHealth;
		opponentHealthSlider.value = opponent.GetComponent<OpponentAI> ().currentHealth / opponent.GetComponent<OpponentAI> ().maxHealth;

	}
}
