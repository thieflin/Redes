using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSpawner : MonoBehaviour
{
    public void PlaySpawn()
    {
        if (FindObjectOfType<EnemySpawner>())
        {
            FindObjectOfType<EnemySpawner>().isSpawning = true;
        }
    }
}
