using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class BossCounter : MonoBehaviour
{
    public TextMeshProUGUI bossCounterText;
    public static int timeDefeatBoss;

    private void Update()
    {
        UpdateScore();
    }

    [PunRPC]
    public void UpdateScore()
    {
        bossCounterText.text = timeDefeatBoss.ToString();
    }
}
