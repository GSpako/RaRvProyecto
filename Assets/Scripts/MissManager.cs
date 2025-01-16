using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MissManager : MonoBehaviour
{
    public static MissManager Instance;

    int totalBalls = 0;
    int currentMisses = 0;

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    public void BallMissed() 
    {
        currentMisses++;
        if (currentMisses == totalBalls)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void AddBall()
    { 
        totalBalls++;
    }
}
