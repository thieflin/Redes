using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class Character : MonoBehaviourPun
{
    Renderer r;
    public PhotonView pv;
    [SerializeField] Bullet _bulletPref;
    [SerializeField] GameObject _bSpawner;
    [SerializeField] Animator animator;
    [SerializeField] GameObject playerSign;
    [SerializeField] Renderer rendererSkin;


    public static bool isAttacking;
    public Shield shield;


    [Header("Physics")]
    public float speed;

    public int bulletDmg;
    [SerializeField]
    float jumpForce;
    Rigidbody _rb;

    public float currentHp, maxHp;
    public bool isDead;
    public float percent;
    public Slider hpSlider;


    void Start()
    {
        currentHp = maxHp;
        r = GetComponent<Renderer>();
        pv = GetComponent<PhotonView>();
        animator = GetComponentInChildren<Animator>();
        hpSlider.maxValue = maxHp;
        hpSlider.value = maxHp;

        //Los posiciono a uno en cada lado
        if (PhotonNetwork.PlayerList.Length < 2)
            transform.position = new Vector3(-7f, 1f, -10f);
        else
        {
            transform.Rotate(0, -180, 0);
            transform.position = new Vector3(7f, 1f, -10f);
        }


        if (pv.IsMine)
        {
            isAttacking = false;
            r.material.color = Color.blue;
            _rb = GetComponent<Rigidbody>();
            //Esto es para que la vida sea del color que le corresponde
            hpSlider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = Color.blue;
            playerSign.SetActive(true);
        }
        else
        {
            isAttacking = true;
            r.material.color = Color.red;
            hpSlider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = Color.red;
            playerSign.SetActive(false);
        }

    }

    private void Update()
    {
        if (!photonView.IsMine)
            return;

        if (WaitingPlayersManager.canStart)
        {
            if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && !isDead)
            {
                Jump();
            }

            if (Input.GetKeyDown(KeyCode.Space) && !isDead)
            {
                photonView.RPC("Shoot", RpcTarget.All);
            }
        }
    }

    private void FixedUpdate()
    {
        if (!pv.IsMine)
            return;

        if (WaitingPlayersManager.canStart && !isDead)
        {
            if (Input.GetAxis("Horizontal") > 0)
            {
                _rb.MovePosition(_rb.position + transform.right * speed * Input.GetAxis("Horizontal") * Time.fixedDeltaTime);
                transform.right = Vector3.right;
                animator.SetFloat("Speed", 1);
            }
            else if (Input.GetAxis("Horizontal") < 0)
            {
                _rb.MovePosition(_rb.position - transform.right * speed * Input.GetAxis("Horizontal") * Time.fixedDeltaTime);
                transform.right = Vector3.left;
                animator.SetFloat("Speed", 1);
            }
            else
            {
                animator.SetFloat("Speed", 0);
            }
        }
    }

    void Jump()
    {
        _rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
    }

    [PunRPC]
    void Shoot()
    {
        Instantiate(_bulletPref, _bSpawner.transform.position, transform.rotation);
        //Creo la bala con las caracteristicas que quiera
        _bulletPref.SetBullet(bulletDmg);

    }

    public void TakeDmg(int dmg)
    {
        if (!pv.IsMine) return;

        currentHp -= dmg;
        photonView.RPC("UpdateHpChar", RpcTarget.All, currentHp);

        if (currentHp <= 0)
        {
            //Die();
            photonView.RPC("Die", RpcTarget.All);
        }

    }
    [PunRPC]
    public void UpdateHpChar(float hp)
    {
        currentHp = hp;
        hpSlider.value = hp;
    }

    [PunRPC]
    public void Die()
    {
        //if (!pv.IsMine)
        //    return;
        animator.SetFloat("Speed", 0);
        rendererSkin.material.color = Color.red;
        isDead = true;
        Debug.Log("a casitaaaaaaaaaaaaa");
    }
}

