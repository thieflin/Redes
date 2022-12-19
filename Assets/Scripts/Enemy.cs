using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Enemy : MonoBehaviourPun
{
    public Baron target;

    [SerializeField]
    private float _hp;
    [SerializeField]
    private float _maxHp;
    public Slider slider;

    [SerializeField] float range;

    public float moveSpeed;
    Vector3 direction;


    Rigidbody rb;

    Animator anim;

    [SerializeField]
    List<SkinnedMeshRenderer> mySkins;

    //[SerializeField]
    //private int _dmgBullet;

    [SerializeField]
    float smooth;

    //[SerializeField]
    //private GameObject _ebSpawner;

    //[SerializeField]
    //private float _shootTime;
    //private bool _canShoot;

    [SerializeField]
    bool isAvoiding;
    public PhotonView pv;
    bool dead;

    [SerializeField]
    GameObject coinPrefab;

    //public GameObject _ebulletPref;

    private void Start()
    {
        _hp = _maxHp;
        anim = GetComponent<Animator>();
        //_canShoot = true;
        pv = GetComponent<PhotonView>();
        target = FindObjectOfType<Baron>();
        anim.Play("Walking");

        rb = GetComponent<Rigidbody>();

        foreach (Transform child in transform)
        {
            if (child.GetComponent<SkinnedMeshRenderer>())
                mySkins.Add(child.GetComponent<SkinnedMeshRenderer>());
        }

        //foreach (var skin in mySkins)
        //{
        //    skin.material.color = Color.red;
        //}

    }

    private void Update()
    {
        if (!isAvoiding)
            transform.LookAt(target.transform);

        if (Input.GetKeyDown(KeyCode.K))
            TakeDamage(50);


    }

    private void FixedUpdate()
    {
        if (dead)
        {
            rb.isKinematic = true;
            return;
        }

        if (!isAvoiding)
            direction = (target.transform.position - transform.position).normalized;

        rb.velocity = direction * moveSpeed * Time.fixedDeltaTime;

        if (Vector3.Distance(transform.position, target.transform.position) < range)
            Attack();

        Ray ray = new Ray(transform.position + transform.forward + new Vector3(0f, 0.5f, 0f), transform.TransformDirection(transform.forward * -1));

        //Si tengo un enemigo cerca, freno
        if (Physics.Raycast(ray, out RaycastHit hit, 1) && !isAvoiding)
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                isAvoiding = true;
                StartCoroutine(DoAvoiding(hit));
            }
            else
                return;
        }
    }

    IEnumerator DoAvoiding(RaycastHit hit)
    {
        Avoid(hit);

        yield return new WaitForSeconds(0.5f);

        FocusTarget();
    }

    [PunRPC]
    public void Attack()
    {
        rb.velocity = Vector3.zero;
        anim.Play("Attack");
    }

    public void Avoid(RaycastHit hit)
    {
        var left = Random.Range(0, 2) == 1;

        if (left)
        {
            rb.MovePosition(transform.position + new Vector3(-1.5f, 0, 0));

        }
        else
        {
            rb.MovePosition(transform.position + new Vector3(1.5f, 0, 0));

        }

    }

    public void FocusTarget()
    {
        direction = (target.transform.position - transform.position).normalized;
        transform.LookAt(target.transform);
        isAvoiding = false;
    }

    [PunRPC]
    public void Die()
    {
        dead = true;
        anim.Play("Dying");

        var newCoin = PhotonNetwork.Instantiate(coinPrefab.name, transform.position + new Vector3(0f, 2f, 0f), Quaternion.identity);

        Destroy(gameObject, 2.5f);
    }
    //private void Update()
    //{
    //    if (!pv.IsMine && !PhotonNetwork.IsMasterClient) return;
    //    if (_canShoot)
    //        StartCoroutine(shootingTime());

    //    if (!WaitingPlayersManager.canStart)
    //        Destroy(gameObject);

    //    if (WinLossCondition.gameIsOver)
    //        pv.RPC("DestroyEnemy", RpcTarget.All);

    //}

    ////eSTA FUNCION ES LA QUE UPDATE EL SLIDER EN BASE A LA VIDA CON UN RPC PARA NOTIFICARLE A LOS OTROS LA VIDA QUE PIERDO
    ///
    //[PunRPC]
    //public void UpdateHp()
    //{
    //    _hp = _hp;
    //}

    //[PunRPC]
    //public void DestroyEnemy()
    //{
    //    Destroy(gameObject);
    //}

    ////Este take dmg lo que hace es updatear mi estado actual en MI pantalla
    ////Si no soy yo, lo que hace es decirle al otro, bueno maestro perdiste vida, y acordate de que
    ////tenes que cambiar el slider
    ///
    public void TakeDamage(int dmg)
    {
        if (dead)
            return;

        if (!pv.IsMine)
        {
            _hp -= dmg;

            //photonView.RPC("UpdateHp", RpcTarget.All);

            foreach (var skin in mySkins)
            {
                skin.material.color = Color.red;
            }

            Invoke("DamageFeedBack", 0.1f);

            //Bueno aca consulta si muere o no para hacer el pedidito de bicho
            if (_hp <= 0)
            {
                photonView.RPC("Die", RpcTarget.MasterClient);
                //Die();
                StartCoroutine(WaitTillSpawnCoroutine());
            }
        }

        else
        {
            //Si soy yo, entonces updateame a mi sin problemas
            _hp -= dmg;
            //slider.value = _hp;

            foreach (var skin in mySkins)
            {
                skin.material.color = Color.red;
            }

            Invoke("DamageFeedBack", 0.1f);

            if (_hp <= 0)
            {
                //Die();
                photonView.RPC("Die", RpcTarget.MasterClient);
                StartCoroutine(WaitTillSpawnCoroutine());
            }
        }

    }

    void DamageFeedBack()
    {
        foreach (var skin in mySkins)
        {
            skin.material.color = Color.white;
        }
    }

    ////Bichito spawner hehe
    IEnumerator WaitTillSpawnCoroutine()
    {
        //GetComponent<Renderer>().enabled = false;
        GetComponent<BoxCollider>().isTrigger = true;
        yield return new WaitForSeconds(1);
        //Sumo 1 a la veces que mataron al boss y lo aplico en la interfaz
        BossCounter.timeDefeatBoss++;
        IDManager.instance.canSpawn = true;
        Destroy(gameObject);
    }

    //[PunRPC]
    //public void EnemyShooting(float dirX)
    //{
    //    GameObject b = Instantiate(_ebulletPref, _ebSpawner.transform.position, Quaternion.identity);
    //    b.GetComponent<EnemyBullet>().SetBullet(_dmgBullet, dirX);
    //}

    ////Para ubicarlo mas facil la duda esta aca:
    //public IEnumerator shootingTime()
    //{
    //    _canShoot = false;
    //    yield return new WaitForSeconds(_shootTime);
    //    //Aca le seteo el DirX para que sea la misma en todas pero por algun motivo me lo sobreescribe
    //    float dirxx = Random.Range(-2f, 2f);
    //    pv.RPC("EnemyShooting", RpcTarget.All, dirxx);
    //    _canShoot = true;
    //}


}
