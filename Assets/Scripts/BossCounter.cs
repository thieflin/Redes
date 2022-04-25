using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class BossCounter : MonoBehaviour
{
    public PhotonView pv;
    public TextMeshProUGUI bossCounterText;
    public static int timeDefeatBoss;

    private void Awake()
    {
        timeDefeatBoss = 0;
    }

    private void Update()
    {
        //pv.RPC("UpdateScore", RpcTarget.All);
        UpdateScore();
    }

    [PunRPC]
    public void UpdateScore()
    {
        bossCounterText.text = timeDefeatBoss.ToString();
    }
}
