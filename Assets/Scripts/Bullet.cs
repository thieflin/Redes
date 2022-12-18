using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    int _dmg; 
    [SerializeField]
    float _speed = 10;
    [SerializeField]
    Character _owner;

    private void Awake()
    {
        Invoke("Destroy", 3f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * _speed * Time.deltaTime;
    }


    void Destroy()
    {
        Destroy(gameObject);
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
