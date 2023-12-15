using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public AudioMixer mixer;
    public static AudioManager instance;

    public AudioSource audioSource;
    public AudioClip[] tracks;
    public AudioClip inGameSound;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(instance.gameObject);
            instance = this;
        }
    }

    void Start()
    {

        StartCoroutine(PlayMusicTracks());

        if (PlayerPrefs.HasKey("MasterVolume"))
        {
            mixer.SetFloat("MasterVol", PlayerPrefs.GetFloat("MasterVolume"));
        }
        else
        {
            mixer.SetFloat("MasterVol", -80 + 13 * (100 / 13));
            PlayerPrefs.SetFloat("MasterVolume", -80 + 13 * (100 / 13));
            PlayerPrefs.SetFloat("MasterVolumeTier", 13);
            PlayerPrefs.Save();
        }

        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            mixer.SetFloat("MusicVol", PlayerPrefs.GetFloat("MusicVolume"));
        }
        else
        {
            mixer.SetFloat("MusicVol", -80 + 7 * (100 / 13));
            PlayerPrefs.SetFloat("MusicVolume", -80 + 7 * (100 / 13));
            PlayerPrefs.SetFloat("MusicVolumeTier", 7);
            PlayerPrefs.Save();
        }

        if (PlayerPrefs.HasKey("EffectsVolume"))
        {
            mixer.SetFloat("EffectsVol", PlayerPrefs.GetFloat("EffectsVolume"));
        }
        else
        {
            mixer.SetFloat("EffectsVol", -80 + 7 * (100 / 13));
            PlayerPrefs.SetFloat("EffectsVolume", -80 + 7 * (100 / 13));
            PlayerPrefs.SetFloat("EffectsVolumeTier", 7);
            PlayerPrefs.Save();
        }
    }

    public IEnumerator PlayMusicTracks()
    {
        List<AudioClip> playlist = new List<AudioClip>(tracks); // Create a list from the array for easy manipulation

        while (true) // Loop indefinitely
        {
            Shuffle(playlist); // Shuffle the playlist before replaying

            foreach (AudioClip track in playlist)
            {
                audioSource.clip = track; // Set the current track
                audioSource.Play(); // Play the current track

                yield return new WaitForSeconds(audioSource.clip.length); // Wait for the track to finish
            }
        }
    }

    public IEnumerator PlayInGameMusic()
    {
        Debug.Log(" In game music called " +inGameSound.name);
        while (true)
        {
            audioSource.clip = inGameSound; // Set the current track
            audioSource.Play(); // Play the current track

            yield return new WaitForSeconds(audioSource.clip.length); // Wait for the track to finish
        }
    }

    // Method to shuffle the playlist
    private void Shuffle(List<AudioClip> playlist)
    {
        for (int i = 0; i < playlist.Count; i++)
        {
            AudioClip temp = playlist[i];
            int randomIndex = Random.Range(i, playlist.Count);
            playlist[i] = playlist[randomIndex];
            playlist[randomIndex] = temp;
        }
    }


}
