using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviourPun
{
    public Transform spawnTransform;

    //public Queue<Enemy> enemiesToSpawn = new Queue<Enemy>();

    public bool won;
    public GameObject winScreen;

    public List<Enemy> enemiesPrefab;

    public List<Enemy> allEnemiesCreated = new List<Enemy>();

    public List<int> levelDiffuculty;

    [SerializeField]
    public bool isSpawning;

    [SerializeField]
    int enemyCount = 0;

    [SerializeField]
    float timeBetweenSpawns;

    [SerializeField]
    float timer;

    int currentDifficulty;

    [SerializeField]
    bool finishWave;

    [SerializeField]
    int waveCounter = 0;

    [SerializeField]
    int wavesToWin;

    [SerializeField] TMPro.TextMeshProUGUI waveText;

    [SerializeField] Animator animSpawner;

    PhotonView pv;

    private void Awake()
    {
        won = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        pv = GetComponent<PhotonView>();
        pv.RPC("UpdateWaveText", RpcTarget.All);
        //pv.RPC("SetQueue", RpcTarget.AllBuffered, 2);
    }

    //[PunRPC]
    void SetQueue(int difficulty)
    {
        //enemiesToSpawn.Clear();

        //for (int i = 0; i < difficulty; i++)
        //{
        //    enemiesToSpawn.Enqueue(enemiesPrefab[Random.Range(0, 2)]);
        //}

    }

    [PunRPC]
    void UpdateWaveText()
    {
        waveText.text = "Wave " + (waveCounter + 1);
        //animSpawner.Play("StartSpawnAnim");
    }


    // Update is called once per frame
    void Update()
    {
        if (waveCounter == wavesToWin)
        {
            if (FindObjectsOfType<Enemy>().Length < 1 && !FindObjectOfType<Baron>().isDead)
            {
                won = true;
                winScreen.SetActive(true);
            }
        }


        if (IDManager.instance.canSpawn && WaitingPlayersManager.canStart)
        {
            //pv.RPC("UpdateWaveText", RpcTarget.All);
            if (waveCounter == wavesToWin)
                return;
            animSpawner.Play("StartSpawnAnim");
        }


        //START THE SPAWNER
        if (isSpawning)
        {
            if (waveCounter == wavesToWin)
            {
                return;
            }


            animSpawner.Play("StartSpawnAnim");
            WaveSpawner(levelDiffuculty[waveCounter]);
            timer += Time.deltaTime;
        }

        if (finishWave)
        {
            finishWave = false;

            pv.RPC("UpdateWaveText", RpcTarget.All);

            //if (waveCounter == wavesToWin)
            //    animSpawner.Play("StartSpawnAnim");
        }

    }

    public void SpawnEnemy(Enemy enemy)
    {
        if (!pv.IsMine)
            return;

        var newEnemy = PhotonNetwork.Instantiate(enemy.name, transform.position, Quaternion.identity);

        //IDManager.instance.canSpawn = false;
    }

    [PunRPC]
    void WaveSpawner(int difficulty)
    {

        if (timer >= timeBetweenSpawns)
        {
            timer = 0;

            if (enemyCount < difficulty)
            {
                var enemy = enemiesPrefab[Random.Range(0, 2)];
                SpawnEnemy(enemy);
                enemyCount++;
            }
            else if (enemyCount >= difficulty)
            {
                finishWave = true;
                waveCounter++;
                enemyCount = 0;
                animSpawner.Play("Idle");
                isSpawning = false;
            }

        }

    }

}
