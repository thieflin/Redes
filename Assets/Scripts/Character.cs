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

    [Header("Physics")]
    public float speed;

    public int bulletDmg;
    [SerializeField]
    float jumpForce;
    Rigidbody _rb;

    //public float currentHp, maxHp;
    public bool isDead;
    public float percent;
    //public Slider hpSlider;
    public bool canShoot;
    public float shootTimer;
    Vector3 direction;
    public float verticalMovement;
    public float horizontalMovement;
    public ParticleSystem shootParticle;

    [Header("Rotation values")]
    [SerializeField] private float _smoothRotation = 0.1f;
    float turnSmoothVelocity;
    [SerializeField] private float rotateOnMove;

    [SerializeField] Transform mySign;

    //public int characterID = 0;

    private void Awake()
    {

    }


    void Start()
    {


        canShoot = true;
        //currentHp = maxHp;
        r = GetComponent<Renderer>();
        pv = GetComponent<PhotonView>();
        animator = GetComponentInChildren<Animator>();
        //hpSlider.maxValue = maxHp;
        //hpSlider.value = maxHp;

        FindObjectOfType<InstantiatePlayer>().playerList.Add(this.gameObject);

        //Los posiciono a uno en cada lado
        //if ()
        //{
        if (SetID.instance.characterID == 1)
        {
            //transform.position = new Vector3(-7f, 1f, -10f);
            transform.position -= new Vector3(-1f, 0f, 0f);
            //SetID.instance.characterID = 1;
            WinLossCondition.player1 = this;
        }
        else if (SetID.instance.characterID == 2)
        {
            //SetID.instance.characterID = 2;
            transform.position -= new Vector3(5f, 0f, 0f);
            WinLossCondition.player2 = this;
        }
        //}

        if (pv.IsMine)
        {
            isAttacking = false;
            r.material.color = Color.blue;
            _rb = GetComponent<Rigidbody>();
            //Esto es para que la vida sea del color que le corresponde
            //hpSlider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = Color.blue;
            playerSign.SetActive(true);
        }
        else
        {
            isAttacking = true;
            r.material.color = Color.red;
            //hpSlider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = Color.red;
            playerSign.SetActive(false);
        }

    }

    private void Update()
    {
        if (!photonView.IsMine)
            return;

        verticalMovement = Input.GetAxis("Vertical");
        horizontalMovement = Input.GetAxis("Horizontal");


        if (WaitingPlayersManager.canStart)
        {
            //if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow)) && !isDead)
            //{
            //    Jump();
            //}

            if (Input.GetMouseButton(0) && !isDead && canShoot)
            {
                photonView.RPC("Shoot", RpcTarget.All);
                StartCoroutine(WaitForShoot());
            }
        }
    }

    private void FixedUpdate()
    {
        if (!pv.IsMine)
            return;

        //direction = new Vector3(Input.GetAxis("Horizontal"), -9.81f, Input.GetAxis("Vertical"));


        if (WaitingPlayersManager.canStart && !isDead)
        {
            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            {
                //Move(new Vector3(horizontalMovement, -9.81f, verticalMovement));
                _rb.MovePosition(_rb.position + ((Vector3.right * Input.GetAxis("Horizontal")) + (Vector3.forward * Input.GetAxis("Vertical"))) * speed * Time.fixedDeltaTime);

                direction = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

                var matrix = Matrix4x4.Rotate(Quaternion.Euler(0, -rotateOnMove, 0));

                var newInput = matrix.MultiplyPoint3x4(direction);

                var newInputRotation = matrix.MultiplyPoint3x4(direction);

                if (newInputRotation.z >= 0.1f || newInputRotation.x >= 0.1f || newInputRotation.x <= -0.1 || newInputRotation.z <= -0.1)
                {
                    float targetAngle = Mathf.Atan2(newInputRotation.x, newInputRotation.z) * Mathf.Rad2Deg;

                    float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, _smoothRotation);

                    transform.rotation = Quaternion.Euler(0f, angle, 0f);
                }

                //transform.right = Vector3.right;
                animator.SetFloat("Speed", 1);
            }
            else
            {
                _rb.MovePosition(_rb.position);
                animator.SetFloat("Speed", 0);
            }
        }
    }

    public void Move(Vector3 direction)
    {
        _rb.velocity = direction * speed * Time.fixedDeltaTime;

    }

    void Jump()
    {
        _rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
    }

    [PunRPC]
    void Shoot()
    {
        Instantiate(_bulletPref, _bSpawner.transform.position, transform.rotation);
        var particles = Instantiate(shootParticle, _bSpawner.transform.position, transform.rotation);
        particles.transform.parent = _bSpawner.gameObject.transform;
        //Creo la bala con las caracteristicas que quiera
        _bulletPref.SetBullet(bulletDmg);

    }

    //public void TakeDmg(int dmg)
    //{
    //    if (!pv.IsMine) return;

    //    currentHp -= dmg;
    //    photonView.RPC("UpdateHpChar", RpcTarget.All, currentHp);

    //    if (currentHp <= 0)
    //    {
    //        photonView.RPC("Die", RpcTarget.All);
    //    }

    //}
    //[PunRPC]
    //public void UpdateHpChar(float hp)
    //{
    //    currentHp = hp;
    //    hpSlider.value = hp;
    //}

    //[PunRPC]
    //public void Die()
    //{
    //    if (!isDead)
    //    {
    //        animator.SetFloat("Speed", 0);
    //        this.GetComponent<BoxCollider>().isTrigger = true;
    //        rendererSkin.material.color = Color.red;
    //        WinLossCondition.playersDead++;
    //    }

    //    isDead = true;
    //}

    IEnumerator WaitForShoot()
    {
        canShoot = false;
        yield return new WaitForSeconds(shootTimer);
        canShoot = true;
    }

}

