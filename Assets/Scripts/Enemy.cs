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

    [SerializeField]
    private int _dmgBullet;


    [SerializeField]
    private GameObject _ebSpawner;

    [SerializeField]
    private float _shootTime;
    private float _time;
    private bool _canShoot;


    public PhotonView pv;

    public EnemyBullet _ebulletPref;

    private void Start()
    {
        _hp = _maxHp;
        _canShoot = true;
    }
    private void Update()
    {
        if (!pv.IsMine) return;
        if(_canShoot)
            StartCoroutine(shootingTime());
        
    }

    //eSTA FUNCION ES LA QUE UPDATE EL SLIDER EN BASE A LA VIDA CON UN RPC PARA NOTIFICARLE A LOS OTROS LA VIDA QUE PIERDO
    [PunRPC]
    public void UpdateHp()
    {
            slider.value = _hp;
    }

    //Este take dmg lo que hace es updatear mi estado actual en MI pantalla
    //Si no soy yo, lo que hace es decirle al otro, bueno maestro perdiste vida, y acordate de que
    //tenes que cambiar el slider
    public void TakeDamage(int dmg)
    {
        if (!pv.IsMine)
        {
            _hp -= dmg;
            photonView.RPC("UpdateHp", RpcTarget.All);
            //Bueno aca consulta si muere o no para hacer el pedidito de bicho
            if (_hp <= 0)
                StartCoroutine(WaitTillSpawnCoroutine());
        }
        else
        {
            //Si soy yo, entonces updateame a mi sin problemas
            _hp -= dmg;
            slider.value = _hp;
            if (_hp <= 0)
                StartCoroutine(WaitTillSpawnCoroutine());
        }

    }

    //Bichito spawner hehe
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
        Instantiate(_ebulletPref, _ebSpawner.transform.position, Quaternion.identity);
        _ebulletPref.SetBullet(_dmgBullet);
    }

    public IEnumerator shootingTime()
    {
        _canShoot = false;
        yield return new WaitForSeconds(0.2f);
        pv.RPC("EnemyShooting", RpcTarget.All);
        _canShoot = true;
    }


}
