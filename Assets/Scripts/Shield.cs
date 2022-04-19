using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public float _hp;

    public Character _owner;

    public Shield SetHp(int hp)
    {
        _hp = hp;
        return this;
    }
}
