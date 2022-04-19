using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    float _dmg;
    float _speed = 10;
    [SerializeField]
    Character _owner;
    

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.right * _speed * Time.deltaTime;
    }


    public Bullet SetBullet(float dmg, Character owner/*, Color color*/)
    {
        _dmg = dmg;
        _owner = owner;
        //GetComponent<Renderer>().material.color = color;
        return this;
    }

    private void OnTriggerEnter(Collider other)
    {
        var ch = GetComponent<Character>();

        if (other.gameObject.GetComponent<Character>().photonView.IsMine) return;
        else
        {
            other.gameObject.GetComponent<Character>().TakeDmg(_dmg);
        }

            
            
        
    }
}
