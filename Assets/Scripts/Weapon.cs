using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Weapon : MonoBehaviourPun
{
    protected Enemy targetEnemy;

    [SerializeField] protected List<Transform> fireTransforms;

}
