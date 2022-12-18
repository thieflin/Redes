using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EnemySpawner : MonoBehaviourPun
{
    public Transform spawnTransform;

    public Queue<Enemy> enemiesToSpawn = new Queue<Enemy>();

    public List<Enemy> enemiesPrefab;

    [SerializeField]
    bool isSpawning;

    [SerializeField]
    float timeBetweenSpawns;

    float timer;

    int currentDifficulty;

    int enemiesDead;

    PhotonView pv;

    private void Awake()
    {
        currentDifficulty = 2;
        SetQueue(currentDifficulty);

        foreach (var item in enemiesToSpawn)
        {
            Debug.Log(item.name);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        pv = GetComponent<PhotonView>();

        //pv.RPC("SetQueue", RpcTarget.AllBuffered, 2);
    }

    //[PunRPC]
    void SetQueue(int difficulty)
    {
        enemiesToSpawn.Clear();

        for (int i = 0; i < difficulty; i++)
        {
            enemiesToSpawn.Enqueue(enemiesPrefab[Random.Range(0, 2)]);
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (IDManager.instance.canSpawn && WaitingPlayersManager.canStart)
        {
            if (isSpawning)
                WaveSpawner(currentDifficulty);


            //IDManager.instance.canSpawn = false;
            ////PhotonNetwork.Instantiate(pv.name, transform.position, Quaternion.identity);
        }
    }

    [PunRPC]
    public void SpawnEnemy(Enemy enemy)
    {
        isSpawning = false;
        PhotonNetwork.Instantiate(enemy.name, transform.position, Quaternion.identity);
        //IDManager.instance.canSpawn = false;
    }

    [PunRPC]
    void WaveSpawner(int difficulty, int count = 0)
    {
        timer += Time.deltaTime;

        if (timer >= timeBetweenSpawns)
        {
            timer = 0;

            if (enemiesToSpawn.Count > 0)
            {
                SpawnEnemy(enemiesToSpawn.Dequeue());
                count++;
            }

            if (count >= difficulty)
            {
                isSpawning = false;
                currentDifficulty++;
                SetQueue(currentDifficulty);
                return;
            }
        }

    }

    [PunRPC]
    IEnumerator WaveCoroutine(int difficulty)
    {
        for (int i = 0; i < difficulty; i++)
        {
            if (enemiesToSpawn.Count > 0)
                SpawnEnemy(enemiesToSpawn.Dequeue());

            yield return new WaitForSeconds(timeBetweenSpawns);
        }

        isSpawning = false;

        currentDifficulty++;

        SetQueue(currentDifficulty);

        yield return null;
    }
}
