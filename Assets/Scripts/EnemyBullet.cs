using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField]
    private int _dmg;
    private float _randomDir;
    [SerializeField]
    private int speed;
    private void Start()
    {
        _randomDir = Random.Range(-2f, 2f);
    }
    public void Update()
    {
        transform.position += new Vector3(_randomDir, -1, 0)*speed*Time.deltaTime;
    }



    public EnemyBullet SetBullet(int dmg)
    {
        _dmg = dmg;

        return this;
    }

    private void OnTriggerEnter(Collider other)
    {
        var character = other.GetComponent<Character>();

        if (character)
        {
            Debug.Log("toy pegandooo");
            character.TakeDmg(_dmg);
            Destroy(gameObject);
        }


        if (other.gameObject.tag == "wall")
        {
            Destroy(gameObject);
        }
    }

}
