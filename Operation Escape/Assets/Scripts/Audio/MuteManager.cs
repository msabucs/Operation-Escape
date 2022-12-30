using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MuteManager : MonoBehaviour {
    [HideInInspector] public bool isMusicMute = false, isSFXMute = false;
    public Button[] btnAudio;
    public Sprite[] btnAudioSprites;
    Image btnMusicImg, btnSFXImg;
    AudioManager audioManager;

    void Start() {

        audioManager = GameObject.Find("Audio Manager").GetComponent<AudioManager>();

        btnMusicImg = btnAudio[0].GetComponent<Image>();
        btnSFXImg = btnAudio[1].GetComponent<Image>();

        // Check if music is mute
        if(audioManager.isMusicMute) {
            btnMusicImg.sprite = (Sprite)btnAudioSprites[2];
            audioManager.isMusicMute = true;
            isMusicMute = true;
        } else {
            btnMusicImg.sprite = (Sprite)btnAudioSprites[0];
            audioManager.isMusicMute = false;
            isMusicMute = false;
        }

        // Check if sfx is mute
        if(audioManager.isSFXMute) {
            btnSFXImg.sprite = (Sprite)btnAudioSprites[6];
            audioManager.isSFXMute = true;
            isSFXMute = true;
        } else {
            btnSFXImg.sprite = (Sprite)btnAudioSprites[4];
            audioManager.isSFXMute = false;
            isSFXMute = false;
        }
    }

    // Mute/unmute music
    public void MusicClicked() {

        if(isMusicMute == false){
            btnMusicImg.sprite = (Sprite)btnAudioSprites[2];
            isMusicMute = true;
            audioManager.isMusicMute = true;
            FindObjectOfType<AudioManager>().Mute("GameBGM");
        }
        else {
            btnMusicImg.sprite = (Sprite)btnAudioSprites[0];
            isMusicMute = false;
            audioManager.isMusicMute = false;
            FindObjectOfType<AudioManager>().Play("GameBGM");
        }
        FindObjectOfType<AudioManager>().Play("ButtonClick");  
    }

    // Mute/unmute sfx
    public void SFXClicked() {

        if(isSFXMute == false){
            btnSFXImg.sprite = (Sprite)btnAudioSprites[6];
            isSFXMute = true;
            audioManager.isSFXMute = true;
        }
        else {
            btnSFXImg.sprite = (Sprite)btnAudioSprites[4];
            isSFXMute = false;
            audioManager.isSFXMute = false;
        }
        FindObjectOfType<AudioManager>().Play("ButtonClick");
    }
}
