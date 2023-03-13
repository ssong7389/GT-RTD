using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
    public static AudioManager Instance
    {
        get { return _instance; }
    }
    public float bgmVolume;
    public bool isPlaying;
    AudioSource bgmSource;
    Slider bgmSlider;
    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

        bgmSource = GetComponent<AudioSource>();
        bgmSource.loop = true;
        if (PlayerPrefs.HasKey("bgm"))
        {
            bgmVolume = PlayerPrefs.GetFloat("bgm");
        }
        else
        {
            bgmVolume = 0.5f;
        }
        SceneManager.sceneLoaded += OnPlaySceneLoaded;
    }

    private void OnPlaySceneLoaded(Scene scene, LoadSceneMode arg1)
    {
        if(scene.buildIndex == 1)
        {
            Transform settings = ButtonManager.Instance.settingsPopup.transform.GetChild(0);
            bgmSlider = settings.Find("Slider").GetComponent<Slider>();
            bgmSlider.onValueChanged.AddListener((value) => OnBgmSliderChanged(value));
            bgmSlider.value = bgmVolume;
        }
    }

    private void OnBgmSliderChanged(float value)
    {
        bgmSource.volume = value;
        bgmVolume = value;
    }

    public void PlayBgm(string bgm, float delay = 0)
    {
        isPlaying = true;
        bgmSource.clip = Resources.Load<AudioClip>($"sounds/{bgm}");
        bgmSource.volume = bgmVolume;
        bgmSource.PlayDelayed(delay);
    }
    public void StopBgm()
    {
        isPlaying = false;
        bgmSource.Stop();
    }


}
