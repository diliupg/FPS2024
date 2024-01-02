using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance {get; set;}

    public Sprite emptySlot;

    [Header("Ammo")]
    public TextMeshProUGUI magazineAmmoUI;
    public TextMeshProUGUI totalAmmoUI;
    public Image ammoTypeUI;

    [Header("Weapon")]
    public Image activeWeaponUI;
    public Image unactiveWeaponUI;

    [Header("Throwables")]
    public Image lethalUI;
    public TextMeshProUGUI lethalAmountUI;

    public Image tacticalUI;
    public TextMeshProUGUI tacticalAmountUI;

    [Header("Weapon Sprites")]
    public GameObject pistolWeapon;
    public GameObject rifleWeapon;
    public GameObject pistolAmmo;
    public GameObject rifleAmmo;
    private Sprite pistolSprite;
    private Sprite rifleSprite;
    private Sprite pistolAmmoSprite;
    private Sprite rifleSAmmoSprite;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        pistolSprite = pistolWeapon.GetComponent<SpriteRenderer>().sprite;
        rifleSprite = rifleWeapon.GetComponent<SpriteRenderer>().sprite;
        pistolAmmoSprite= pistolAmmo.GetComponent<SpriteRenderer>().sprite;
        rifleSAmmoSprite = rifleAmmo.GetComponent<SpriteRenderer>().sprite;
    }

    private void Update()
    {
        Weapon activeWeapon = WeaponManager.Instance.activeWeaponSlot.GetComponentInChildren<Weapon>();
        Weapon unactiveWeapon = GetUnactiveWeaponSlot().GetComponentInChildren<Weapon>();

        if(activeWeapon)
        {
            magazineAmmoUI.text = $"{activeWeapon.bulletsLeft / activeWeapon.bulletsPerBurst}";
            totalAmmoUI.text = $"{WeaponManager.Instance.CheckAmmoLeftFor(activeWeapon.thisWeaponModel)}";

            Weapon.WeaponModel model = activeWeapon.thisWeaponModel;
            ammoTypeUI.sprite = GetAmmoSprite(model);

            activeWeaponUI.sprite = GetWeaponSprite(model);

            if(unactiveWeapon)
            {
                unactiveWeaponUI.sprite = GetWeaponSprite(unactiveWeapon.thisWeaponModel);
            }
        }
        else
        {
            magazineAmmoUI.text = "";
            totalAmmoUI.text = "";

            ammoTypeUI.sprite = emptySlot;

            activeWeaponUI.sprite = emptySlot;
            unactiveWeaponUI.sprite = emptySlot;
        }
    }

    private Sprite GetWeaponSprite(Weapon.WeaponModel model)
    {
        switch(model)
        {
            case Weapon.WeaponModel.Pistol:
            //return (Resources.Load<GameObject>("PistolIcon")).GetComponent<SpriteRenderer>().sprite;
            return pistolSprite;

            case Weapon.WeaponModel.Rifle:
            //return (Resources.Load<GameObject>("RifleIcon")).GetComponent<SpriteRenderer>().sprite;
            return rifleSprite;


            default:
            return null;
        }
    }

    private Sprite GetAmmoSprite(Weapon.WeaponModel model)
    {
        switch(model)
        {
            
            case Weapon.WeaponModel.Pistol:
            //return (Resources.Load<GameObject>("PistolAmmoIcon")).GetComponent<SpriteRenderer>().sprite;
            return pistolAmmoSprite;


            case Weapon.WeaponModel.Rifle:
            //return (Resources.Load<GameObject>("RifleAmmoIcon")).GetComponent<SpriteRenderer>().sprite;
            return rifleSAmmoSprite;

            default:
            return null;
        }
    }

    private GameObject GetUnactiveWeaponSlot()
    {
        foreach(GameObject weaponSlot in WeaponManager.Instance.weaponSlots)
        {
            if(weaponSlot != WeaponManager.Instance.activeWeaponSlot)
            {
                return weaponSlot;
            }
        }
        //this will never be reached but we need to put this cause we need to return something from the main function
        return null;
    }
}
