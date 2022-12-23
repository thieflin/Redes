using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;


public class Baron : MonoBehaviourPun
{
    [SerializeField]
    float hp, maxHP;

    public GameObject lossScreen;

    [SerializeField] Slider sliderHP;

    public bool isDead;

    [SerializeField]
    PhotonView pv;

    private void Awake()
    {
        pv.GetComponent<PhotonView>();
        hp = maxHP;
    }


    public void TakeDamage(int dmg)
    {
        hp -= dmg;

        pv.RPC("UpdateHP", RpcTarget.All);

        if (hp <= 0)
        {
            lossScreen.SetActive(true);
            pv.RPC("Die", RpcTarget.All);
        }
    }

    [PunRPC]
    void UpdateHP()
    {
        sliderHP.value = hp;
    }


    [PunRPC]
    public void Die()
    {
        FindObjectOfType<EnemySpawner>().isSpawning = false;

        if (hp > 0)
            return;

        isDead = true;

        foreach (var enemy in FindObjectsOfType<Character>())
        {
            Destroy(enemy.gameObject);
        }

        foreach (var enemy in FindObjectsOfType<Enemy>())
        {
            Destroy(enemy.gameObject);
        }

    }
}
