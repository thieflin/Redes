using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class WinLossCondition : MonoBehaviour
{
    public PhotonView pv;

    public static int playersDead;
    public int killsToWin = 5;

    public GameObject winScreen, loseScreen;

    public static Character player1, player2;

    public int playersReady;

    public TextMeshProUGUI textPlayersReadyOnLoss;
    public TextMeshProUGUI textPlayersReadyOnWin;

    public Enemy enemy;

    public WaitingPlayersManager waitingPlayers;

    public Button playAgainButtonLoss;
    public Button playAgainButtonWin;

    public static bool gameIsOver;


    private void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        gameIsOver = false;

        playersDead = 0;

    }

    private void Update()
    {
        //LOSSING
        if (playersDead == 2)
        {
            loseScreen.SetActive(true);
            gameIsOver = true;
        }

        Debug.Log(playersDead + " Player Dead");
        Debug.Log(playersReady + " Player Ready");

        //WINNING
        if (BossCounter.timeDefeatBoss >= killsToWin)
        {
            winScreen.SetActive(true);
            gameIsOver = true;
        }
        else
        {
            winScreen.SetActive(false);
            gameIsOver = false;
        }
    }

    [PunRPC]
    public void BTN_PlayAgain()
    {
        playAgainButtonLoss.interactable = false;
        playAgainButtonWin.interactable = false;
        pv.RPC("OnPlayAgain", RpcTarget.All);
    }

    [PunRPC]
    public void OnPlayAgain()
    {
        playersReady++;

        textPlayersReadyOnLoss.text = playersReady.ToString();
        textPlayersReadyOnWin.text = playersReady.ToString();

        if (playersReady == 2)
        {
            BossCounter.timeDefeatBoss = 0;
            PhotonNetwork.LoadLevel(1);
            gameIsOver = false;
        }
        else
            return;
    }

    
}
