using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Enemy : MonoBehaviourPun
{
    [SerializeField]
    private float _hp;
    [SerializeField]
    private float _maxHp;
    public Slider slider;


    public PhotonView pv;

    public EnemyBullet _ebulletPref;

    private void Start()
    {
        _hp = _maxHp;
        slider.value = _maxHp;
        slider.maxValue = _maxHp;
    }



    public void OnTriggerEnter(Collider other)
    {
        //if (!pv.IsMine) return;
        //if(other.gameObject.tag == "bullet")
        //{
        //    Destroy(other.gameObject);
        //    pv.RPC("TakeDamage", RpcTarget.All, 20);
        //}
    }



    [PunRPC]
    public void UpdateHp()
    {
        if (!pv.IsMine)
            slider.value = _hp;
        else return;
    }

    public void TakeDamage(int dmg)
    {
        if (pv.IsMine)
        {
            _hp -= dmg;
            slider.value = _hp;

            pv.RPC("UpdateHp", RpcTarget.All);
            if (_hp <= 0)
                StartCoroutine(WaitTillSpawnCoroutine());
        }
        
    }


    IEnumerator WaitTillSpawnCoroutine()
    {
        GetComponent<Renderer>().enabled = false;
        yield return new WaitForSeconds(1);
        IDManager.instance.canSpawn = true;
        Destroy(gameObject);
    }

    [PunRPC]
    public void EnemyShooting()
    {

    }



}
