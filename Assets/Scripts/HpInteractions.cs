using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpInteractions : MonoBehaviour
{
    public PhotonView pv;

    void Start()
    {
        pv = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
