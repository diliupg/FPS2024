using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public static InteractionManager Instance {get; set;}

    public Weapon selectedWeapon = null;

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

            if(objectHitByRaycast.GetComponent<Weapon>())
            {
                selectedWeapon = objectHitByRaycast.gameObject.GetComponent<Weapon>();
                selectedWeapon.GetComponent<Outline>().enabled = true;
            }
            else
            {
                if(selectedWeapon)
                {
                    selectedWeapon.GetComponent<Outline>().enabled = false;
                }
            }
            
        }
    }
}
