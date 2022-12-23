using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EzrealBullet : MonoBehaviour
{
    public Transform target;

    Vector3 direction;

    public float speed;

    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (target != null)
        {
            direction = (target.transform.position + new Vector3(0,3f,0) - transform.position).normalized;

            transform.eulerAngles = new Vector3(-90, 0, 0);

            rb.velocity = direction * speed * Time.fixedDeltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Baron"))
        {
            other.GetComponent<Baron>().TakeDamage(75);
            Destroy(gameObject);
        }
    }
}
