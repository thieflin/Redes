using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Character : MonoBehaviourPun
{
    Renderer r;
    public PhotonView pv;
    public float speed = 3f;


    void Start()
    {
        r = GetComponent<Renderer>();
        pv = GetComponent<PhotonView>();

        if (PhotonNetwork.PlayerList.Length < 2)
            transform.position = new Vector3(-7.5f, 1f, -10f);
        else
        {
            transform.position = new Vector3(7.5f, 1f, -10f);
        }

        if (pv.IsMine)
            r.material.color = Color.blue;
        else
            r.material.color = Color.red;

    }

    // Update is called once per frame
    void Update()
    {
        if (!pv.IsMine)
            return;

        transform.position += transform.right * Input.GetAxis("Horizontal") * Time.deltaTime * speed;
    }
}
