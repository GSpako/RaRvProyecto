using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;
using TMPro;
using Unity.VisualScripting;

public class GoalDetection : MonoBehaviour
{
    public AudioSource sound;
    public Transform particlesPosition;
    public ParticleSystem particles;
    public GameObject Menu;

    string nextSceneName = "";
    int sceneCount = 0;
    private void Awake()
    {
        sceneCount = SceneManager.sceneCountInBuildSettings;
    }


    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Score");

        sound.Play();

        particlesPosition.position = other.transform.position;
        particles.Play();


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
            if(nextNumber > sceneCount)
            {
                Menu.SetActive(true);
            }
            else
            {
                // Construct the next scene name
                nextSceneName = baseName + nextNumber;

                StartCoroutine(LoadNextScene());
            }
            
        }
        else
        {
            Menu.SetActive(true);
        }

        Destroy(other.gameObject);
    }

    IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(2);

            // Load the next scene
            SceneManager.LoadScene(nextSceneName);
    }
}
