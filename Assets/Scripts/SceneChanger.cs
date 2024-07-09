using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneChanger : MonoBehaviour
{
    public AudioClip collisionSound; // Audio clip to play on collision
    private AudioSource audioSource;
    private bool isColliding = false;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isColliding)
        {
            isColliding = true;
            if (collisionSound != null)
            {
                audioSource.clip = collisionSound;
                audioSource.Play();
            }
            StartCoroutine(ChangeSceneAfterDelay(7f));
        }
    }

    IEnumerator ChangeSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("MainMenu");
    }
}
