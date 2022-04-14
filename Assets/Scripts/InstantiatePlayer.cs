using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class InstantiatePlayer : MonoBehaviourPun
{
    public PhotonView pv;

    void Awake()
    {

        PhotonNetwork.Instantiate("Character", new Vector3(0, 1f, -10f), Quaternion.identity);
        
        pv.RPC("UpdateCount", RpcTarget.AllBuffered);

    }

    private void Update()
    {

    }

    [PunRPC]
    public void UpdateCount()
    {
        Debug.Log("sume");
    }
}
