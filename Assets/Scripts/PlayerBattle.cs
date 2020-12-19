﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerBattle : MonoBehaviour
{
    public Canvas battleCanvas;

    public GameManager gm;

    public AudioSource sockAudio;
    public AudioClip hurt;
    public AudioClip attack;

    private Animator animator;

    private int speed;
    private int intimidation;
    private float attackDamage;
    private float defenseBuff;
    private float maxHealth;
    public float currHealth;

    private void Awake()
    {
        battleCanvas = GameObject.Find("BattleCanvas").GetComponent<Canvas>();
        gm = GameObject.Find("Game Manager").GetComponent<GameManager>();

        animator = GetComponent<Animator>();

        if(gm.GetExperience() < 2.0f)
        {
            speed = 4;
            intimidation = 2;
            attackDamage = 2.0f;
            defenseBuff = 80.0f;
            maxHealth = 10.0f;
        }

        else if(gm.GetExperience() < 3.0f)
        {
            speed = 4;
            intimidation = 3;
            attackDamage = 2.5f;
            defenseBuff = 80.0f;
            maxHealth = 15.0f;
        }

        else if(gm.GetExperience() < 4.0f)
        {
            speed = 4;
            intimidation = 4;
            attackDamage = 3.0f;
            defenseBuff = 85.0f;
            maxHealth = 20.0f;
        }

        //later

    }

    public void TakeDamage(float damage)
    {
        currHealth -= damage;
        sockAudio.clip = hurt;
        sockAudio.Play(); 
        changeDamageAnimation();
    }

    public void changeDamageAnimation()
    {
        if (currHealth <= 0.0f)
        {
            animator.SetInteger("damageLevel", 3);
        }

        else if ((currHealth / maxHealth) <= (1.0f / 3.0f))
        {
            animator.SetInteger("damageLevel", 2);
        }

        else if ((currHealth / maxHealth) <= (2.0f / 3.0f))
        {
            animator.SetInteger("damageLevel", 1);
        }

        else
        {
            animator.SetInteger("damageLevel", 0);
        }
    }

    public int GetSpeed()
    {
        return speed;
    }

    public int GetIntimidation()
    {
        return intimidation;
    }

    public float GetDefense(bool strongAgainst)
    {
        if(strongAgainst)
        {
            return defenseBuff;
        }

        return 0.0f;
    }

    public bool AttemptAttack(int attack, int level)
    {
        if(attack == 1)
        {
            int slapChance = Random.Range(1, 100);

            if(slapChance < 81)
            {
                animator.SetBool("isAttacking", true);
                StartCoroutine("DelaySlapAttack");
                Invoke("StopAttacking", 2);
                return true;
            }

            else
            {
                return false;
            }
        }

        //add animation
        else if(attack == 2)
        {
            int silenceChance = Random.Range(1, 100);

            if(level == 1 && silenceChance < 91)
            {
                return true;
            }

            else if(level == 2 && silenceChance < 81)
            {
                return true;
            }

            else if(level == 3 && silenceChance < 71)
            {
                return true;
            }

            else if(level == 4 && silenceChance < 61)
            {
                return true;
            }

            else
            {
                return false;
            }
        }

        return false;
    }

    IEnumerator DelaySlapAttack()
    {
        yield return new WaitForSeconds(0.75f);
        sockAudio.clip = attack;
        sockAudio.Play();
    }

    public void StopAttacking()
    {
        animator.SetBool("isAttacking", false); 
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public float GetAttackDamage()
    {
        return attackDamage;
    }

    public void ChangeAttackDamage(float damage)
    {
        attackDamage += damage;
    }
}
