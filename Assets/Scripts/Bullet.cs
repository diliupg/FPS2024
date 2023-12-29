 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnCollisionEnter(Collision ObjectWeHit)
    {
        if(ObjectWeHit.gameObject.CompareTag("Target"))
        {
            createBulletImpactEffect(ObjectWeHit);

            Destroy(gameObject);
        }

        if(ObjectWeHit.gameObject.CompareTag("Wall"))
        {
            createBulletImpactEffect(ObjectWeHit);

            Destroy(gameObject);
        }

        if(ObjectWeHit.gameObject.CompareTag("Ground"))
        {
            createBulletImpactEffect(ObjectWeHit);

            Destroy(gameObject);
        }

        if(ObjectWeHit.gameObject.CompareTag("Shatter"))
        {
            ObjectWeHit.gameObject.GetComponent<Bottle>().Shatter();

            // we will not destroy the bullet on impact, as it will destroy on life time end
        }            
    }

    void createBulletImpactEffect(Collision ObjectWeHit)
    {
        ContactPoint contact = ObjectWeHit.contacts[0];

        GameObject hole = Instantiate(
            GlobalReferences.Instance.bulletImpactEffectPrefab,
            contact.point,
            Quaternion.LookRotation(contact.normal)
        );

        hole.transform.SetParent(ObjectWeHit.gameObject.transform);
    }
}
