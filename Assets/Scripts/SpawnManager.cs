using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviourPun
{
    [SerializeField]
    GameObject _enemyPrefab;
    public PhotonView thisPv;
    public int enemyHp;


    private void Update()
    {

        //Reviso que pueda spawnear, que el pv sea suyo, y revise el mastercliente, tambien se spawnee cuando los pjs estan conectados
        if (IDManager.instance.canSpawn && thisPv.IsMine && PhotonNetwork.IsMasterClient && WaitingPlayersManager.canStart)
        {
            IDManager.instance.canSpawn = false;
            //Spawneo de a uno con photonnetwork
            PhotonNetwork.Instantiate(_enemyPrefab.name, transform.position, Quaternion.identity);
        }
    } 


    //ESTA FUNCION SPAWNEA AL ENEMIGO
    [PunRPC]
    public void SpawnEnemy()
    {
        Instantiate(_enemyPrefab, transform.position, Quaternion.identity);
        IDManager.instance.canSpawn = false;
    }


}
