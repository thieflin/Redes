using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class InstantiatePlayer : MonoBehaviourPun
{
    public PhotonView pv;

    public List<GameObject> playerList;

    [SerializeField] Transform playerPos1, playerPos2;

    public static Character player1, player2;

    void Awake()
    {
        var character = PhotonNetwork.Instantiate("Character", Vector3.zero, Quaternion.identity);

        if (SetID.instance.characterID == 0)
        {
            Debug.Log("Entre 1");
            //SetID.instance.characterID = 1;
            character.transform.position = playerPos1.position;
            character.transform.forward = playerPos1.forward;
        }

        else if (SetID.instance.characterID == 1)
        {
            Debug.Log("Entre 2");
            //SetID.instance.characterID = 2;
            character.transform.position = playerPos2.position;
            character.transform.forward = playerPos2.forward;
        }

    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.J))
            foreach (var player in playerList)
            {
                Debug.Log(player.name);
            }
    }
}
