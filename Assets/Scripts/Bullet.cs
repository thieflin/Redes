using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    int _dmg;
    float _speed = 10;
    [SerializeField]
    Character _owner;
    

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.right * _speed * Time.deltaTime;
    }


    public Bullet SetBullet(int dmg)
    {
        _dmg = dmg;
        return this;
    }

    private void OnTriggerEnter(Collider other)
    {
        var enemy = other.GetComponent<Enemy>();

        if (enemy)
        {
            enemy.TakeDamage(_dmg);
            Destroy(gameObject);
        }


        if(other.gameObject.tag == "wall")
        {
            Destroy(gameObject);
        }
    }


}
