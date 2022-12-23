using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Coin : MonoBehaviour
{
    PhotonView pv;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
    }

    // Start is called before the first frame update
    void Start()
    {
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            pv.RPC("AddGold", RpcTarget.All);
        }
    }

    [PunRPC]
    void AddGold()
    {
        FindObjectOfType<GoldManager>().AddGold(25);
        Destroy(gameObject);
    }
}
