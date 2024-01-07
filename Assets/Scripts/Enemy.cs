using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int HP = 100;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damageAmount)
    {
        HP -= damageAmount;

        if(HP <= 0)
        {
            animator.SetTrigger("Die");
            Destroy(gameObject);
        }
        else{
            animator.SetTrigger("Damage");
        }
    }
}
