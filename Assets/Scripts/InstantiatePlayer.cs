using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class InstantiatePlayer : MonoBehaviourPun
{
    public PhotonView pv;

    void Awake()
    {
        var character = PhotonNetwork.Instantiate("Character", new Vector3(10, 1f, -10f), Quaternion.identity);

        if (SetID.instance.characterID == 1)
        {
            character.transform.position = new Vector3(-7f, 1f, -10f);
            Debug.Log("ENTREE XDXDXD");
        }
        else if (SetID.instance.characterID == 2)
        {
            character.transform.Rotate(0, -180, 0);
            character.transform.position = new Vector3(7f, 1f, -10f);
            Debug.Log("ENTREE XDXDXD x2");
        }
    }

}
