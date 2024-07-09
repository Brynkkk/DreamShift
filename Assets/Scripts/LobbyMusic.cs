using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyMusic : MonoBehaviour
{
    private static LobbyMusic lobbyMusic;
    private AudioSource audioSource;

    public float fadeDuration = 2f; 

    void Awake()
    {
        if (lobbyMusic == null)
        {
            lobbyMusic = this;
            DontDestroyOnLoad(gameObject);

            audioSource = GetComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainMenu" || scene.name == "Tutorial")
        {
            PlayLobbyMusic();
        }
        else
        {
            StartCoroutine(FadeOutAndStopMusic());
        }
    }

    void PlayLobbyMusic()
    {
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.volume = 1f; 
            audioSource.Play();
        }
    }

    IEnumerator FadeOutAndStopMusic()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            float startVolume = audioSource.volume;

            for (float t = 0; t < fadeDuration; t += Time.deltaTime)
            {
                audioSource.volume = startVolume * (1 - t / fadeDuration);
                yield return null;
            }

            audioSource.Stop();
            audioSource.volume = startVolume; 
        }
    }
}
