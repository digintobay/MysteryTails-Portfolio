using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioMixer mixer;
    public AudioSource bgm;
    public AudioClip[] bglist;
    public static SoundManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
            //SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        for (int i = 0; i < bglist.Length; i++)
        {
            if (arg0.name == bglist[i].name)
                BGMPlay(bglist[i]);
        }

    }

    public void BGMChgnaer(string bgmName)
    {

        for (int i =0; i < instance.bglist.Length; i++)
        {
            if (bgmName == instance.bglist[i].name)
                instance.BGMPlay(instance.bglist[i]);

        }
    }



    public static void BGMVolumeChanger(float val)
    {
        if (val <= 0.0001f) val = 0.0001f; 
        SoundManager.instance.mixer.SetFloat("BGMParam", Mathf.Log10(val) * 20);
        PlayerPrefs.SetFloat("BGMVolume", val);

    }

    public static void SFXVolumeChanger(float val)
    {
        if (val <= 0.0001f) val = 0.0001f;
        SoundManager.instance.mixer.SetFloat("SFXParam", Mathf.Log10(val) * 20);
        PlayerPrefs.SetFloat("SFXVolume", val);

    }


    public void SFXPlay(string sfxName, AudioClip clip)
    {
        GameObject go = new GameObject(sfxName + "Sound");
        AudioSource audioSource = go.AddComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = mixer.FindMatchingGroups("SFX")[0];
        audioSource.clip = clip;
        audioSource.volume = 0.9f;
        audioSource.Play();

        Destroy(go, clip.length);
    }
    public void BGMPlay(AudioClip clip)
    {
        bgm.outputAudioMixerGroup = mixer.FindMatchingGroups("BGM")[0];
        bgm.clip = clip;
        bgm.loop = true;
        bgm.volume = 0.6f;
        bgm.Play();
    }

    public void BGMMuteOnOff(bool changer)
    {
        bgm.mute = changer;
    }

  
}

