using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class GoldManager : MonoBehaviourPun
{
    public int totalGold;

    PhotonView pv;

    [SerializeField]
    Text goldText;
    // Start is called before the first frame update
    void Start()
    {
        pv = GetComponent<PhotonView>();
    }

    [PunRPC]
    void UpdateGoldText(int newTotalGold)
    {
        goldText.text = "GOLD: " + newTotalGold;
        totalGold = newTotalGold;
    }

    public void AddGold(int ammount)
    {
        totalGold += ammount;
        pv.RPC("UpdateGoldText", RpcTarget.All, totalGold);
    }

    public void RemoveGold(int ammount)
    {
        totalGold -= ammount;
        pv.RPC("UpdateGoldText", RpcTarget.All, totalGold);

    }

}
