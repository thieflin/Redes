using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Character : MonoBehaviourPun
{
    Renderer r;
    public PhotonView pv;
    [SerializeField] GameObject _bulletPref;
    [SerializeField] GameObject _bSpawner;

    [Header("Physics")]
    public float speed;
    [SerializeField]
    float jumpForce;
    Rigidbody _rb;


    void Start()
    {
        r = GetComponent<Renderer>();
        pv = GetComponent<PhotonView>();

        //Los posiciono a uno en cada lado
        if (PhotonNetwork.PlayerList.Length < 2)
            transform.position = new Vector3(-7.5f, 1f, -10f);
        else
            transform.position = new Vector3(7.5f, 1f, -10f);


        if (pv.IsMine)
        {
            r.material.color = Color.blue;
            _rb = GetComponent<Rigidbody>();
        }
        else
            r.material.color = Color.red;

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

    }

}
