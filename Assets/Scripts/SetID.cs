using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetID : MonoBehaviour
{
    public int characterID = 0;

    public static SetID instance;

    private void Awake()
    {
        DontDestroyOnLoad(this);

        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

}
