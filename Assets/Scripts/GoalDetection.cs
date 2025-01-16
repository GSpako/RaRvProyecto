using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;
using TMPro;

public class GoalDetection : MonoBehaviour
{
    public AudioSource sound;
    public Transform particlesPosition;
    public ParticleSystem particles;

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Score");

        sound.Play();

        particlesPosition.position = other.transform.position;
        particles.Play();

        StartCoroutine(LoadNextScene());
        Destroy(other.gameObject);
    }

    IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(3);

        // Get the current scene's name
        string currentSceneName = SceneManager.GetActiveScene().name;

        // Use regex to extract the name and number at the end
        Match match = Regex.Match(currentSceneName, @"^(.*?)(\d+)$");

        if (match.Success)
        {
            // Get the base name and the number
            string baseName = match.Groups[1].Value;
            int currentNumber = int.Parse(match.Groups[2].Value);

            // Increment the number
            int nextNumber = currentNumber + 1;

            // Construct the next scene name
            string nextSceneName = baseName + nextNumber;

            // Load the next scene
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogError("Scene name does not end with a number. Cannot determine the next scene.");
        }
    }
}
