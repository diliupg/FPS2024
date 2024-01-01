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
    }

    private void Update()
    {
        Weapon activeWeapon = WeaponManager.Instance.activeWeaponSlot.GetComponentInChildren<Weapon>();
        Weapon inactiveWeapon = GetUnactiveWeaponSlot().GetComponentInChildren<Weapon>();

        if(activeWeapon)
        {
            magazineAmmoUI.text = $"{activeWeapon.bulletsLeft / activeWeapon.bulletsPerBurst}";
            totalAmmoUI.text = $"{WeaponManager.Instance.CheckAmmoLeftFor(activeWeapon.thisWeaponModel)}";

            Weapon.WeaponModel model = activeWeapon.thisWeaponModel;
            ammoTypeUI.sprite = GetAmmoSprite(model);

            activeWeaponUI.sprite = GetWeaponSprite(model);

            if(inactiveWeapon)
            {
                unactiveWeaponUI.sprite = GetWeaponSprite(inactiveWeapon.thisWeaponModel);
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
            return Instantiate(Resources.Load<GameObject>("PistolIcon")).GetComponent<SpriteRenderer>().sprite;

            case Weapon.WeaponModel.Rifle:
            return Instantiate(Resources.Load<GameObject>("RifleIcon")).GetComponent<SpriteRenderer>().sprite;

            default:
            return null;
        }
    }

    private Sprite GetAmmoSprite(Weapon.WeaponModel model)
    {
        switch(model)
        {
            case Weapon.WeaponModel.Pistol:
            return Instantiate(Resources.Load<GameObject>("PistolAmmoIcon")).GetComponent<SpriteRenderer>().sprite;

            case Weapon.WeaponModel.Rifle:
            return Instantiate(Resources.Load<GameObject>("RifleAmmoIcon")).GetComponent<SpriteRenderer>().sprite;

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