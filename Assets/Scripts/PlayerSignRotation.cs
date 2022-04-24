using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSignRotation : MonoBehaviour
{
    [SerializeField] float animationSpeed;


    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, -animationSpeed, 0);
    }
}
