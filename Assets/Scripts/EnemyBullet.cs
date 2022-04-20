using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField]
    private int _dmg;
    private float _randomDir;
    private Vector3 dir;
    [SerializeField]
    private int speed;
    public void Update()
    {
        transform.position += dir*speed*Time.deltaTime;
        Debug.Log(dir);
    }



    public EnemyBullet SetBullet(int dmg, float randomDir)
    {
        _dmg = dmg;
        _randomDir = randomDir;
        dir = new Vector3(randomDir, -1, 0);
        Debug.Log(dir);
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
