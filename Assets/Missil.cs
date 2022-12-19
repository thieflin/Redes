using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Missil : MonoBehaviourPun
{
    public Transform target;

    [SerializeField]
    ParticleSystem explosionPrefab;

    [SerializeField] float speed;

    Rigidbody rb;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
            Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        if (!target)
            return;

        var direction = (target.transform.position + new Vector3(0,2f,0) - transform.position).normalized;

        transform.forward = direction;

        rb.velocity = direction * speed * Time.fixedDeltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<Enemy>().TakeDamage(20);
            var explosion = Instantiate(explosionPrefab);
            explosion.transform.position = collision.transform.position + new Vector3(0,1.5f,0);
            Destroy(gameObject);
        }
    }

    //[PunRPC]
    //void InstantiateExplosion()
    //{

    //}
}
