using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    public static SoundManager instance;

    private AudioSource source;

    void Awake() {
        if (instance == null)
            instance = this;

        source = GetComponent<AudioSource>();

        DontDestroyOnLoad(this);
    }

}
