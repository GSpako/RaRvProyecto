using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using UnityEngine;

public class BallScript : MonoBehaviour
{
    public GrabInteractable grabbable;
    public Rigidbody rb;


    // Update is called once per frame
    void Update()
    {
        if (grabbable.State == InteractableState.Select)
        {
            rb.useGravity = true;
        }
    }
}
