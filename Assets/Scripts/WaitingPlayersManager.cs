using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WaitingPlayersManager : MonoBehaviour
{
    public GameObject textWaitingPlayers;

    public Animator startTextAnimation;

    public static bool canStart;

    public TextMeshProUGUI startMessageText, threeText, twoText, oneText;

    public SpawnManager sm;

    void Update()
    {
        if (PhotonNetwork.PlayerList.Length < 2)
        {
            textWaitingPlayers.SetActive(true);
            canStart = false;
        }
        else
        {
            textWaitingPlayers.SetActive(false);
            CountDown();
        }
    }

    void CountDown()
    {
        startTextAnimation.SetTrigger("StartGame");
        canStart = true;
    }
}
