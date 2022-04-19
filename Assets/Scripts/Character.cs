using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class Character : MonoBehaviourPun
{
    Renderer r;
    public PhotonView pv;
    [SerializeField] Bullet _bulletPref;
    [SerializeField] GameObject _bSpawner;

    [Header("Physics")]
    public float speed;

    public float bulletDmg;
    [SerializeField]
    float jumpForce;
    Rigidbody _rb;

    public float currentHp, maxHp;
    public float percent;
    public Slider hpSlider;

    void Start()
    {
        currentHp = maxHp;
        r = GetComponent<Renderer>();
        pv = GetComponent<PhotonView>();

        hpSlider.maxValue = maxHp;
        hpSlider.value = maxHp;

        //Los posiciono a uno en cada lado
        if (PhotonNetwork.PlayerList.Length < 2)
            transform.position = new Vector3(-7.5f, 1f, -10f);
        else
            transform.position = new Vector3(7.5f, 1f, -10f);


        if (pv.IsMine)
        {
            r.material.color = Color.blue;
            _rb = GetComponent<Rigidbody>();
            //Esto es para que la vida sea del color que le corresponde
            hpSlider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = Color.blue;
        }
        else
        {
            r.material.color = Color.red;
            hpSlider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = Color.red;
        }

    }

    private void Update()
    {
        if (!photonView.IsMine)
            return;

        if (WaitingPlayersManager.canStart)
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {

                Jump();
            }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            photonView.RPC("Shoot", RpcTarget.All);
        }
    }

    private void FixedUpdate()
    {
        if (!pv.IsMine)
            return;

        if (WaitingPlayersManager.canStart)
            _rb.MovePosition(_rb.position + transform.right * speed * Input.GetAxis("Horizontal") * Time.fixedDeltaTime);
    }

    void Jump()
    {
        _rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
    }

    [PunRPC]
    void Shoot()
    {
        Instantiate(_bulletPref, transform.position, Quaternion.identity);
        //Creo la bala con las caracteristicas que quiera
        _bulletPref.SetBullet(bulletDmg, this/*, Color.blue*/);

    }

    [PunRPC]
    public void TakeDmg(float dmg)
    {
        if (!pv.IsMine)
        {
            photonView.RPC("UpdateLifebar", RpcTarget.All);
            currentHp -= dmg;
        }
        else { 
            currentHp -= dmg;
             hpSlider.value = currentHp;
        }
        //Hago un rpc que se encargue de updatear la lifebar



    }

    [PunRPC]
    public void UpdateLifebar()
    {
        hpSlider.value = currentHp;
    }

    [PunRPC]
    public void Die()
    {
        Debug.Log("a casitaaaaaaaaaaaaa");
    }

}

