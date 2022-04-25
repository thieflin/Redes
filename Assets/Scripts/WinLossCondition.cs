using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;

public class WinLossCondition : MonoBehaviour
{
    public PhotonView pv;

    public static int playersDead;
    public int killsToWin = 5;

    public GameObject winScreen, loseScreen;

    public static Character player1, player2;

    public int playersReady;

    public TextMeshProUGUI textPlayersReady;

    public Enemy enemy;

    public WaitingPlayersManager waitingPlayers;

    public Button playAgainButtonLoss;
    public Button playAgainButtonWin;

    public static bool gameIsOver;


    private void Start()
    {
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
        if (playersDead < 2)
        {
            loseScreen.SetActive(false);
            playersReady = 0;
            playAgainButtonLoss.interactable = true;
            gameIsOver = false;
        }

        Debug.Log(playersDead + " Player Dead");
        Debug.Log(playersReady + " Player Ready");

        //WINNING
        if (BossCounter.timeDefeatBoss >= killsToWin)
        {
            winScreen.SetActive(true);
            gameIsOver = true;
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

        textPlayersReady.text = playersReady.ToString();

        if (playersReady == 2)
        {
            playersDead = 0;
            player1.RespawnWithRPC();
            player2.RespawnWithRPC();
            playersReady = 0;
            BossCounter.timeDefeatBoss = 0;
            IDManager.instance.canSpawn = true;
            textPlayersReady.text = "0";
        }
        else
            return;
    }
}
