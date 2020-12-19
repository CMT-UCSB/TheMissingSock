using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject playerRoam;
    public GameObject playerBattle;
    public GameObject hairTiesBattle;

    public static int level = 1;
    public static float XP = 1.0f;
    public static float healthPlayer;

    public Camera mc;
    public Camera bc;

    public Canvas battleCanvas;

    public GameObject RegularMenu;
    public GameObject BossMenu;

    public TextMeshProUGUI tip;

    public bool battling;

    private static List<string> inventory = new List<string>();

    public void Awake()
    {
        //FOR TESTING

        inventory.Add("Soap");
        inventory.Add("Soap");
        inventory.Add("Soap");

        //END

        battling = false;
    }

    public void StartRegularBattle()
    {
        battling = true;
        mc.gameObject.SetActive(false);
        bc.gameObject.SetActive(true);

        RegularMenu.gameObject.SetActive(true);
        BossMenu.gameObject.SetActive(false);

        tip.gameObject.SetActive(false);

        battleCanvas.GetComponent<RegularBattle>().Begin();
    }

    public void EndRegularBattle()
    {
        battling = false;
        mc.gameObject.SetActive(true);
        bc.gameObject.SetActive(false);

        RegularMenu.gameObject.SetActive(false);
        BossMenu.gameObject.SetActive(false);

        tip.gameObject.SetActive(true);

        playerRoam.GetComponent<PlayerController>().StartMusic();
    }

    public void StartFilterBattle()
    {
        battling = true;
        mc.gameObject.SetActive(false);
        bc.gameObject.SetActive(true);

        RegularMenu.gameObject.SetActive(false);
        BossMenu.gameObject.SetActive(true);

        tip.gameObject.SetActive(false);

        battleCanvas.GetComponent<FitlerBattle>().Begin(); 
    }

    public void EndFitlerBattle()
    {
        //to be continued scene
    }

    public int GetLevel()
    {
        return level;
    }

    public void SetExperiencePoints(float change)
    {
        XP += change; 
    }

    public float GetExperience()
    {
        return XP;
    }

    public List<string> GetInventory()
    {
        return inventory;
    }

    public void AddLoot(List<string> loot)
    {
        foreach(string item in loot)
        {
            inventory.Add(item); 
        }

        foreach(string item in inventory)
        {
            //Debug.Log(item);
        }
    }

    public void UseItem(string item)
    {
        inventory.Remove(item);
    }

    public void SetPlayerHealth(float health)
    {
        healthPlayer = health;
    }

    public float GetPlayerHealth()
    {
        return healthPlayer;
    }

    public float[] ConstructStats()
    {
        if (XP < 2.0f)
        {
            float[] stats = new float[5];

            if(healthPlayer <= 0.0f)
            {
                stats[0] = XP * 10.0f;
                stats[1] = 4.0f;
                stats[2] = 2.0f;
                stats[3] = 2.0f;
                stats[4] = 10.0f;
                return stats;
            }

            stats[0] = XP * 10.0f;
            stats[1] = 4.0f;
            stats[2] = 2.0f;
            stats[3] = 2.0f;
            stats[4] = healthPlayer;
            return stats;
        }

        else if (XP < 3.0f)
        {
            float[] stats = new float[5];
            stats[0] = XP * 10.0f;
            stats[1] = 4.0f;
            stats[2] = 2.5f;
            stats[3] = 3.0f;
            stats[4] = 15.0f;
            return stats;
        }

        else if (XP < 4.0f)
        {
            float[] stats = new float[5];
            stats[0] = XP * 10.0f;
            stats[1] = 4.0f;
            stats[2] = 3.0f;
            stats[3] = 4.0f;
            stats[4] = 20.0f;
            return stats;
        }

        else
        {
            float[] stats = new float[5];
            for(int i = 0; i < 5; i++)
            {
                stats[i] = 0.0f; 
            }
            return stats;
        }
    }
}
