using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IDManager : MonoBehaviour
{
    //Aca le erre y le puse nombre de interfaz #fuerzas pido disculpas publicas es lo que hay
    //Este script tiene las variables que considero necesarias para revisar en varios scripts
    public int charId;
    public bool canSpawn;

    public static IDManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else Destroy(gameObject);
    }
}
