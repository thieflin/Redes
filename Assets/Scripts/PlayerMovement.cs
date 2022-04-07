using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerMovement : MonoBehaviour
{
    public Material playerMat;
    public Material enemyMat;
    Renderer r;
    public PhotonView pv;
    public float speed = 3f;
    void Start()
    {
        r = GetComponent<Renderer>();
        pv = GetComponent<PhotonView>();
        if (pv.IsMine)
            r.material = playerMat;
        else
            r.material = enemyMat;

        //if (InstantiatePlayer.count == 1)
        //{
        //    transform.position = new Vector3(-7.5f, 1f, -10f);
        //}
        //else if (InstantiatePlayer.count == 2)
        //{
        //    transform.position = new Vector3(7.5f, 1f, -10f);
        //}
    }

    // Update is called once per frame
    void Update()
    {
        if (!pv.IsMine)
            return;
        
        transform.position += transform.right * Input.GetAxis("Horizontal") * Time.deltaTime * speed;
    }
}
