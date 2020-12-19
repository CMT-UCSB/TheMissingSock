using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HairTiesController : MonoBehaviour
{
    public float currHealth = 10.0f;
    private float maxHealth = 10.0f;

    public Animator animator;

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currHealth -= damage;

        if (currHealth <= 0.0f)
        {
            animator.SetInteger("Injured", 3);
        }

        else if ((currHealth / maxHealth) <= (1.0f / 3.0f))
        {
            animator.SetInteger("Injured", 2);
        }

        else if ((currHealth / maxHealth) <= (2.0f / 3.0f))
        {
            animator.SetInteger("Injured", 1);
        }

        else
        {
            animator.SetInteger("Injured", 0);
        }
    }
}
