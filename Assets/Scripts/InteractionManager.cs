using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public static InteractionManager Instance {get; set;}

    public Weapon selectedWeapon = null;
    public AmmoBox selectedAmmoBox;

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
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit))
        {
            GameObject objectHitByRaycast = hit.transform.gameObject;

            //Weapon
            if(objectHitByRaycast.GetComponent<Weapon>() &&
                objectHitByRaycast.GetComponent<Weapon>().isActiveWeapon == false)
            {
                selectedWeapon = objectHitByRaycast.gameObject.GetComponent<Weapon>();
                selectedWeapon.GetComponent<Outline>().enabled = true;

                if(Input.GetKeyDown(KeyCode.E))
                {
                    WeaponManager.Instance.PickupWeapon(objectHitByRaycast.gameObject);
                }
            }
            else
            {
                if(selectedWeapon)
                {
                    selectedWeapon.GetComponent<Outline>().enabled = false;
                }
            }

            // Ammo box
            if(objectHitByRaycast.GetComponent<AmmoBox>())
            {
                selectedAmmoBox = objectHitByRaycast.gameObject.GetComponent<AmmoBox>();
                selectedAmmoBox.GetComponent<Outline>().enabled = true;

                if(Input.GetKeyDown(KeyCode.E))
                {
                    WeaponManager.Instance.PickupAmmo(selectedAmmoBox);
                    Destroy(objectHitByRaycast.gameObject);
                }
            }
            else
            {
                if(selectedAmmoBox)
                {
                    selectedAmmoBox.GetComponent<Outline>().enabled = false;
                }
            }
            
        }
    }
}
