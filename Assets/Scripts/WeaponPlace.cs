using Photon.Pun;
using System.Linq;
using UnityEngine;

public class WeaponPlace : MonoBehaviourPun
{
    [SerializeField]
    Transform weaponPosition;

    [SerializeField]
    GameObject myCanvas;

    Weapon myWeapon;

    public bool hasWeapon;

    [SerializeField] GameObject instantiatedWeapon;

    PhotonView pv;
    GoldManager goldManager;
    float distance = 10;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
        goldManager = FindObjectOfType<GoldManager>();
        hasWeapon = false;
    }

    [PunRPC]
    private void OnMouseOver()
    {
        if (FindObjectOfType<InstantiatePlayer>().playerList.Count < 1)
            return;

        var anyPlayerClose = FindObjectOfType<InstantiatePlayer>().playerList.OrderBy(x => Vector3.Distance(x.transform.position, transform.position)).First();

        if (!anyPlayerClose)
            return;

        if (Vector3.Distance(anyPlayerClose.transform.position, transform.position) > distance)
            return;

        LookAtCamera.imInOptions = true;

        if (LookAtCamera.imInOptions)
            myCanvas.SetActive(true);

    }


    private void OnMouseExit()
    {
        Invoke("TurnOffCanvas", 4f);
    }

    public void TurnOffCanvas()
    {
        myCanvas.SetActive(false);
    }

    public void ChangeState()
    {
        hasWeapon = !hasWeapon;
    }

    [PunRPC]
    public void ChangeWeapon(Weapon weapon)
    {
        if (hasWeapon)
            return;

        hasWeapon = true;

        myWeapon = weapon;

        photonView.RPC("Change", RpcTarget.AllBuffered);

    }

    [PunRPC]
    public void Change()
    {
        hasWeapon = true;

        var newWeapon = PhotonNetwork.Instantiate(myWeapon.name, Vector3.zero, Quaternion.identity);


        instantiatedWeapon = newWeapon.gameObject;

        newWeapon.transform.position = weaponPosition.position;

    }
}
