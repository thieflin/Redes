using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    [SerializeField]
    GameObject myCanvas;
    [SerializeField]
    Weapon weaponToBuy;
    [SerializeField]
    int cost;
    GoldManager goldManager;

    public static bool imInOptions;

    // Start is called before the first frame update
    void Start()
    {
        goldManager = FindObjectOfType<GoldManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //transform.LookAt(new Vector3(Camera.main.transform.position.x, transform.position.y, Camera.main.transform.position.z));
        transform.eulerAngles = new Vector3(Camera.main.transform.position.x, 0f, 0f);
    }

    private void OnMouseOver()
    {
        //if (Vector3.Distance(FindObjectOfType<Character>().transform.position, transform.position) > 10)
        //    return;


        imInOptions = true;
    }

    private void OnMouseExit()
    {
        imInOptions = false;
    }

    private void OnMouseDown()
    {
        if (FindObjectOfType<GoldManager>().totalGold >= cost && !transform.parent.parent.GetComponent<WeaponPlace>().hasWeapon)
        {
            FindObjectOfType<GoldManager>().RemoveGold(cost);
            transform.parent.parent.GetComponent<WeaponPlace>().ChangeWeapon(weaponToBuy);
        }

    }
}
