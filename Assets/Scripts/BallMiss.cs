using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BallMiss : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        MissManager.Instance.BallMissed();
    }
}
