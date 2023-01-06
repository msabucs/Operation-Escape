using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

public class AudioManager : MonoBehaviour {
    [HideInInspector] public bool isMusicMute, isSFXMute;
    public Sound[] sounds;
    public static AudioManager instance;
    MuteManager muteManager;
    
    void Start() {

        muteManager = GameObject.Find("Mute Manager").GetComponent<MuteManager>();
        
        isMusicMute = muteManager.isMusicMute;
        isSFXMute = muteManager.isSFXMute;

        Play("GameBGM");
    }

    void Update() {

        if(GameObject.Find("MuteManager") != null) {
            muteManager = GameObject.Find("Mute Manager").GetComponent<MuteManager>();
            isMusicMute = muteManager.isMusicMute;
            isSFXMute = muteManager.isSFXMute;
        } 
        else {
            return;
        }
    }

    void Awake() {

        // Check if there is only one AudioManager on the scene
        if(instance == null) {
            instance = this;
        }
        else {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach(Sound s in sounds) {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.loop = s.loop;
        }
    }

    // Play music
    public void Play(string name) {

        Sound s = Array.Find(sounds, sound => sound.name == name);

        if(name != "GameBGM") {

            if(isSFXMute == false) {
                s.source.Play();
            }
            else {
                return;
            }
        }
        else {
            s.source.volume = 0.5f;
            s.source.Play();
        }
    }

    // Mute music
    public void Mute(string name) {

        Sound s = Array.Find(sounds, sound => sound.name == name);

        if(name == "GameBGM") {
            s.source.volume = 0f;
        }
    }
}
