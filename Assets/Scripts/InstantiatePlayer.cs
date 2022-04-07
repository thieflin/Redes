using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class InstantiatePlayer : MonoBehaviourPun
{
    
    public static int count = 0;
    public PhotonView pv;
    bool havePlayer;

    void Awake()
    {
        pv.RPC("UpdateCount", RpcTarget.AllBuffered);
    }

    private void Update()
    {
        if(havePlayer == false)
        {
            if (count == 1)
            {
                PhotonNetwork.Instantiate("Player", new Vector3(0, 1f, -10f), Quaternion.identity);
                count++;
            } 
            else
            {
                PhotonNetwork.Instantiate("Player", new Vector3(7.5f, 1f, -10f), Quaternion.identity);
                count++;
                Debug.Log(count);
            }
            havePlayer = true;
        }
    }

    [PunRPC]
    public void UpdateCount()
    {
        Debug.Log("sume");
        count++;
    }
}
