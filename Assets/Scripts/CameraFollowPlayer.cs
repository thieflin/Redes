using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CameraFollowPlayer : MonoBehaviourPun
{
    public PhotonView pv;
    [SerializeField] Character characterToFollow;
    [SerializeField]
    Vector3 offSet;

    // Start is called before the first frame update
    void Start()
    {
        pv = GetComponent<PhotonView>();

        characterToFollow = FindObjectOfType<Character>();
    }

    // Update is called once per frame
    void Update()
    {

        if (characterToFollow)
            transform.position = characterToFollow.transform.position + offSet;
    }
}
