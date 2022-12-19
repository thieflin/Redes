using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;

public class MissilThrower : Weapon
{
    [SerializeField] Missil missilPrefab;
    [SerializeField] Transform lookAt;
    [SerializeField] float cd;

    int count;
    bool isChecking;

    [SerializeField]
    float checkTime;
    bool canShoot = true;
    bool isActive;

    PhotonView pv;

    EnemySpawner enemySpawner;

    private void Awake()
    {
        enemySpawner = FindObjectOfType<EnemySpawner>();
    }

    // Start is called before the first frame update
    void Start()
    {
        pv = GetComponent<PhotonView>();

        isChecking = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isChecking)
        {
            CheckForClosestEnemy();
            isChecking = false;
            Invoke("StartChecking", checkTime);
        }

        if (!pv.IsMine)
            return;

        if (isActive && canShoot)
        {
            pv.RPC("InstantiateBullet", RpcTarget.AllBuffered);
            canShoot = false;
            Invoke("ShootCD", cd);
        }


        if (targetEnemy != null)
            lookAt.transform.LookAt(targetEnemy.transform);


    }

    void ShootCD()
    {
        canShoot = true;
    }

    void StartChecking()
    {
        isChecking = true;
    }

    void CheckForClosestEnemy()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 15f);

        List<Enemy> enemies = new List<Enemy>();

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.GetComponent<Enemy>())
                enemies.Add(hitCollider.GetComponent<Enemy>());
        }

        foreach (var enemy in enemies)
        {
            var closestEnemy = enemies.OrderBy(x => Vector3.Distance(x.transform.position, transform.position)).First();

            if (!closestEnemy)
            {
                isActive = false;
                targetEnemy = null;
                return;
            }

            isActive = true;
            targetEnemy = closestEnemy;
            Debug.Log(targetEnemy);
        }
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.green;
    //    Gizmos.DrawSphere(transform.position, 15f);
    //}

    [PunRPC]
    void InstantiateBullet()
    {
        var newMissil = Instantiate(missilPrefab, fireTransforms[count].transform.position, transform.rotation);

        if (targetEnemy)
            newMissil.target = targetEnemy.transform;
  

        count++;
        if (count >= fireTransforms.Count())
            count = 0;
    }


}
