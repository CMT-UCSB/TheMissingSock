using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; 

public class RegularBattle : MonoBehaviour
{
    public GameObject player;
    public GameObject enemy;

    public GameObject mainMenu;
    public GameObject attackMenu;
    public GameObject targetMenu;
    public GameObject itemMenu;

    public GameManager gm;

    PlayerBattle playerUnit;

    MoldController moldUnitOne;
    MoldController moldUnitTwo;
    MoldController moldUnitThree;
    MoldController moldUnitFour;

    public Transform playerPos;
    public Transform firstEnemyPos;
    public Transform secondEnemyPos;
    public Transform thirdEnemyPos;
    public Transform forthEnemyPos;

    private Button[] attackButtons;
    private Button[] targets;
    private Button[] itemButtons;
    private Button[] mainButtons;

    public TextMeshProUGUI dialogueText;

    private bool firstFight = true;
    private bool secondLevel = false;

    private int target;
    private int numEnemies;
    private int currEnemies;
    private int attackNum;

    private float experienceGained;

    private List<string> loot = new List<string>();
    private List<string> usedItems = new List<string>();

    public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }
    public BattleState state;

    public AudioSource battleAudio;

    public Image firstMoldHealth;
    public Image secondMoldHealth;
    public Image thirdMoldHealth;
    public Image forthMoldHealth;
    public Image playerHealth;

    public void Begin()
    {
        battleAudio.Play();

        firstMoldHealth.gameObject.SetActive(false);
        secondMoldHealth.gameObject.SetActive(false);
        thirdMoldHealth.gameObject.SetActive(false);
        forthMoldHealth.gameObject.SetActive(false);

        targets = targetMenu.GetComponentsInChildren<Button>();
        itemButtons = itemMenu.GetComponentsInChildren<Button>();
        mainButtons = mainMenu.GetComponentsInChildren<Button>();
        attackButtons = attackMenu.GetComponentsInChildren<Button>();

        state = BattleState.START;
        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle()
    { 
        GameObject playerGO = Instantiate(player, playerPos);
        playerUnit = playerGO.GetComponent<PlayerBattle>();

        firstEnemyPos.GetChild(0).GetComponent<TextMeshProUGUI>().text = "";
        secondEnemyPos.GetChild(0).GetComponent<TextMeshProUGUI>().text = "";
        thirdEnemyPos.GetChild(0).GetComponent<TextMeshProUGUI>().text = "";
        forthEnemyPos.GetChild(0).GetComponent<TextMeshProUGUI>().text = "";

        playerHealth.gameObject.SetActive(true);

        if (gm.GetLevel() == 1)
        {
            if (firstFight)
            {
                gm.SetPlayerHealth(10.0f);
                playerUnit.currHealth = 10.0f;

                GameObject enemy1 = Instantiate(enemy, firstEnemyPos);
                moldUnitOne = enemy1.GetComponent<MoldController>();
                moldUnitOne.SetLevel(1);
                firstEnemyPos.GetChild(0).GetComponent<TextMeshProUGUI>().text = "LVL 1";

                numEnemies = 1;

                firstFight = false;

                firstMoldHealth.gameObject.SetActive(true);
            }

            else
            {
                playerUnit.currHealth = gm.GetPlayerHealth();
                playerUnit.changeDamageAnimation();

                int moldSpawnChance = Random.Range(1, 100);

                if(gm.GetExperience() >= 3.0f)
                {
                    if(moldSpawnChance >= 75)
                    {
                        GameObject enemy1 = Instantiate(enemy, firstEnemyPos);
                        moldUnitOne = enemy1.GetComponent<MoldController>();
                        moldUnitOne.SetLevel(3);
                        firstEnemyPos.GetChild(0).GetComponent<TextMeshProUGUI>().text = "LVL 3";
                        numEnemies = 1;
                        firstMoldHealth.gameObject.SetActive(true);
                    }

                    else if(moldSpawnChance >= 50)
                    {
                        GameObject enemy1 = Instantiate(enemy, firstEnemyPos);
                        moldUnitOne = enemy1.GetComponent<MoldController>();
                        GameObject enemy2 = Instantiate(enemy, secondEnemyPos);
                        moldUnitTwo = enemy2.GetComponent<MoldController>();

                        moldUnitOne.SetLevel(3);
                        moldUnitTwo.SetLevel(3);
                        firstEnemyPos.GetChild(0).GetComponent<TextMeshProUGUI>().text = "LVL 3";
                        secondEnemyPos.GetChild(0).GetComponent<TextMeshProUGUI>().text = "LVL 3";
                        numEnemies = 2;

                        firstMoldHealth.gameObject.SetActive(true);
                        secondMoldHealth.gameObject.SetActive(true);
                    }

                    else if(moldSpawnChance >= 25)
                    {
                        GameObject enemy1 = Instantiate(enemy, firstEnemyPos);
                        moldUnitOne = enemy1.GetComponent<MoldController>();
                        GameObject enemy2 = Instantiate(enemy, secondEnemyPos);
                        moldUnitTwo = enemy2.GetComponent<MoldController>();
                        GameObject enemy3 = Instantiate(enemy, thirdEnemyPos);
                        moldUnitThree = enemy3.GetComponent<MoldController>();

                        moldUnitOne.SetLevel(3);
                        moldUnitTwo.SetLevel(3);
                        moldUnitThree.SetLevel(3);
                        firstEnemyPos.GetChild(0).GetComponent<TextMeshProUGUI>().text = "LVL 3";
                        secondEnemyPos.GetChild(0).GetComponent<TextMeshProUGUI>().text = "LVL 3";
                        thirdEnemyPos.GetChild(0).GetComponent<TextMeshProUGUI>().text = "LVL 3";
                        numEnemies = 3;

                        firstMoldHealth.gameObject.SetActive(true);
                        secondMoldHealth.gameObject.SetActive(true);
                        thirdMoldHealth.gameObject.SetActive(true);
                    }

                    else
                    {
                        GameObject enemy1 = Instantiate(enemy, firstEnemyPos);
                        moldUnitOne = enemy1.GetComponent<MoldController>();
                        GameObject enemy2 = Instantiate(enemy, secondEnemyPos);
                        moldUnitTwo = enemy2.GetComponent<MoldController>();
                        GameObject enemy3 = Instantiate(enemy, thirdEnemyPos);
                        moldUnitThree = enemy3.GetComponent<MoldController>();
                        GameObject enemy4 = Instantiate(enemy, forthEnemyPos);
                        moldUnitFour = enemy4.GetComponent<MoldController>();

                        moldUnitOne.SetLevel(3);
                        moldUnitTwo.SetLevel(3);
                        moldUnitThree.SetLevel(3);
                        moldUnitFour.SetLevel(3);
                        firstEnemyPos.GetChild(0).GetComponent<TextMeshProUGUI>().text = "LVL 3";
                        secondEnemyPos.GetChild(0).GetComponent<TextMeshProUGUI>().text = "LVL 3";
                        thirdEnemyPos.GetChild(0).GetComponent<TextMeshProUGUI>().text = "LVL 3";
                        forthEnemyPos.GetChild(0).GetComponent<TextMeshProUGUI>().text = "LVL 3";
                        numEnemies = 4;

                        firstMoldHealth.gameObject.SetActive(true);
                        secondMoldHealth.gameObject.SetActive(true);
                        thirdMoldHealth.gameObject.SetActive(true);
                        forthMoldHealth.gameObject.SetActive(true);
                    }
                }
                
                else if(gm.GetExperience() >= 2.0f)
                {
                    if (moldSpawnChance >= 67)
                    {
                        GameObject enemy1 = Instantiate(enemy, firstEnemyPos);
                        moldUnitOne = enemy1.GetComponent<MoldController>();
                        moldUnitOne.SetLevel(2);
                        firstEnemyPos.GetChild(0).GetComponent<TextMeshProUGUI>().text = "LVL 2";
                        numEnemies = 1;
                        firstMoldHealth.gameObject.SetActive(true);
                    }

                    else if (moldSpawnChance >= 37)
                    {
                        GameObject enemy1 = Instantiate(enemy, firstEnemyPos);
                        moldUnitOne = enemy1.GetComponent<MoldController>();
                        GameObject enemy2 = Instantiate(enemy, secondEnemyPos);
                        moldUnitTwo = enemy2.GetComponent<MoldController>();

                        moldUnitOne.SetLevel(2);
                        moldUnitTwo.SetLevel(2);
                        firstEnemyPos.GetChild(0).GetComponent<TextMeshProUGUI>().text = "LVL 2";
                        secondEnemyPos.GetChild(0).GetComponent<TextMeshProUGUI>().text = "LVL 2";
                        numEnemies = 2;

                        firstMoldHealth.gameObject.SetActive(true);
                        secondMoldHealth.gameObject.SetActive(true);
                    }

                    else if (moldSpawnChance >= 17)
                    {
                        GameObject enemy1 = Instantiate(enemy, firstEnemyPos);
                        moldUnitOne = enemy1.GetComponent<MoldController>();
                        GameObject enemy2 = Instantiate(enemy, secondEnemyPos);
                        moldUnitTwo = enemy2.GetComponent<MoldController>();
                        GameObject enemy3 = Instantiate(enemy, thirdEnemyPos);
                        moldUnitThree = enemy3.GetComponent<MoldController>();

                        moldUnitOne.SetLevel(2);
                        moldUnitTwo.SetLevel(2);
                        moldUnitThree.SetLevel(2);
                        firstEnemyPos.GetChild(0).GetComponent<TextMeshProUGUI>().text = "LVL 2";
                        secondEnemyPos.GetChild(0).GetComponent<TextMeshProUGUI>().text = "LVL 2";
                        thirdEnemyPos.GetChild(0).GetComponent<TextMeshProUGUI>().text = "LVL 2";
                        numEnemies = 3;

                        firstMoldHealth.gameObject.SetActive(true);
                        secondMoldHealth.gameObject.SetActive(true);
                        thirdMoldHealth.gameObject.SetActive(true);
                    }

                    else
                    {
                        GameObject enemy1 = Instantiate(enemy, firstEnemyPos);
                        moldUnitOne = enemy1.GetComponent<MoldController>();
                        GameObject enemy2 = Instantiate(enemy, secondEnemyPos);
                        moldUnitTwo = enemy2.GetComponent<MoldController>();
                        GameObject enemy3 = Instantiate(enemy, thirdEnemyPos);
                        moldUnitThree = enemy3.GetComponent<MoldController>();
                        GameObject enemy4 = Instantiate(enemy, forthEnemyPos);
                        moldUnitFour = enemy4.GetComponent<MoldController>();

                        moldUnitOne.SetLevel(2);
                        moldUnitTwo.SetLevel(2);
                        moldUnitThree.SetLevel(2);
                        moldUnitFour.SetLevel(2);
                        firstEnemyPos.GetChild(0).GetComponent<TextMeshProUGUI>().text = "LVL 2";
                        secondEnemyPos.GetChild(0).GetComponent<TextMeshProUGUI>().text = "LVL 2";
                        thirdEnemyPos.GetChild(0).GetComponent<TextMeshProUGUI>().text = "LVL 2";
                        forthEnemyPos.GetChild(0).GetComponent<TextMeshProUGUI>().text = "LVL 2";
                        numEnemies = 4;

                        firstMoldHealth.gameObject.SetActive(true);
                        secondMoldHealth.gameObject.SetActive(true);
                        thirdMoldHealth.gameObject.SetActive(true);
                        forthMoldHealth.gameObject.SetActive(true);
                    }
                }

                else if(gm.GetExperience() >= 1.0f)
                {
                    if (moldSpawnChance >= 50)
                    {
                        GameObject enemy1 = Instantiate(enemy, firstEnemyPos);
                        moldUnitOne = enemy1.GetComponent<MoldController>();
                        moldUnitOne.SetLevel(1);
                        firstEnemyPos.GetChild(0).GetComponent<TextMeshProUGUI>().text = "LVL 1";
                        numEnemies = 1;
                        firstMoldHealth.gameObject.SetActive(true);
                    }

                    else if (moldSpawnChance >= 25)
                    {
                        GameObject enemy1 = Instantiate(enemy, firstEnemyPos);
                        moldUnitOne = enemy1.GetComponent<MoldController>();
                        GameObject enemy2 = Instantiate(enemy, secondEnemyPos);
                        moldUnitTwo = enemy2.GetComponent<MoldController>();

                        moldUnitOne.SetLevel(1);
                        moldUnitTwo.SetLevel(1);
                        firstEnemyPos.GetChild(0).GetComponent<TextMeshProUGUI>().text = "LVL 1";
                        secondEnemyPos.GetChild(0).GetComponent<TextMeshProUGUI>().text = "LVL 1";
                        numEnemies = 2;

                        firstMoldHealth.gameObject.SetActive(true);
                        secondMoldHealth.gameObject.SetActive(true);
                    }

                    else if (moldSpawnChance >= 5)
                    {
                        GameObject enemy1 = Instantiate(enemy, firstEnemyPos);
                        moldUnitOne = enemy1.GetComponent<MoldController>();
                        GameObject enemy2 = Instantiate(enemy, secondEnemyPos);
                        moldUnitTwo = enemy2.GetComponent<MoldController>();
                        GameObject enemy3 = Instantiate(enemy, thirdEnemyPos);
                        moldUnitThree = enemy3.GetComponent<MoldController>();

                        moldUnitOne.SetLevel(1);
                        moldUnitTwo.SetLevel(1);
                        moldUnitThree.SetLevel(1);
                        firstEnemyPos.GetChild(0).GetComponent<TextMeshProUGUI>().text = "LVL 1";
                        secondEnemyPos.GetChild(0).GetComponent<TextMeshProUGUI>().text = "LVL 1";
                        thirdEnemyPos.GetChild(0).GetComponent<TextMeshProUGUI>().text = "LVL 1";
                        numEnemies = 3;

                        firstMoldHealth.gameObject.SetActive(true);
                        secondMoldHealth.gameObject.SetActive(true);
                        thirdMoldHealth.gameObject.SetActive(true);
                    }

                    else
                    {
                        GameObject enemy1 = Instantiate(enemy, firstEnemyPos);
                        moldUnitOne = enemy1.GetComponent<MoldController>();
                        GameObject enemy2 = Instantiate(enemy, secondEnemyPos);
                        moldUnitTwo = enemy2.GetComponent<MoldController>();
                        GameObject enemy3 = Instantiate(enemy, thirdEnemyPos);
                        moldUnitThree = enemy3.GetComponent<MoldController>();
                        GameObject enemy4 = Instantiate(enemy, forthEnemyPos);
                        moldUnitFour = enemy4.GetComponent<MoldController>();

                        moldUnitOne.SetLevel(1);
                        moldUnitTwo.SetLevel(1);
                        moldUnitThree.SetLevel(1);
                        moldUnitFour.SetLevel(1);
                        firstEnemyPos.GetChild(0).GetComponent<TextMeshProUGUI>().text = "LVL 1";
                        secondEnemyPos.GetChild(0).GetComponent<TextMeshProUGUI>().text = "LVL 1";
                        thirdEnemyPos.GetChild(0).GetComponent<TextMeshProUGUI>().text = "LVL 1";
                        forthEnemyPos.GetChild(0).GetComponent<TextMeshProUGUI>().text = "LVL 1";
                        numEnemies = 4;

                        firstMoldHealth.gameObject.SetActive(true);
                        secondMoldHealth.gameObject.SetActive(true);
                        thirdMoldHealth.gameObject.SetActive(true);
                        forthMoldHealth.gameObject.SetActive(true);
                    }
                }
            }

            currEnemies = numEnemies;
            StartCoroutine(PrintText("MOLD has sensed your presence..."));
            AdjustAttacks();
            AdjustTargets();
            PopulateItems();
        }

        for(int i = 0; i < 4; i++)
        {
            mainButtons[i].interactable = false;
        }

        yield return new WaitForSeconds(3.8f);

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    void AdjustTargets()
    {
        for (int i = 0; i < 4; i++)
        {
            targets[i].interactable = true;
        }

        for (int i = numEnemies; i < 4; i++)
        {
            targets[i].interactable = false;
        }
    }

    void AdjustAttacks()
    {
        if (gm.GetExperience() < 2.0f)
        {
            attackButtons[0].interactable = true;
            attackButtons[1].interactable = false;
            attackButtons[2].interactable = false;
            attackButtons[3].interactable = false;
        }

        else if(gm.GetExperience() < 3.0f)
        {
            attackButtons[0].interactable = true;
            attackButtons[1].interactable = true;
            attackButtons[2].interactable = false;
            attackButtons[3].interactable = false;
        }
    }

    void PopulateItems()
    {
        if(gm.GetInventory().Count == 0)
        {
            for(int i = 0; i < 5; i++)
            {
                itemButtons[i].interactable = false;
            }

            itemButtons[6].interactable = true;
        }

        else
        {
            for(int i = 0; i < gm.GetInventory().Count; i++)
            {
                Debug.Log(gm.GetInventory().Count);
                itemButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = gm.GetInventory()[i].ToUpper();
                itemButtons[i].interactable = true;
            }

            for(int i = gm.GetInventory().Count; i < itemButtons.Length; i++)
            {
                itemButtons[i].interactable = false;
            }

            itemButtons[6].interactable = true;
        }

    }

    void PlayerTurn()
    {
        StartCoroutine(PrintText("What will you do?"));

        Invoke("PlayerOptions", 2f);
    }

    void PlayerOptions()
    {
        for (int i = 0; i < 4; i++)
        {
            mainButtons[i].interactable = true;
        }
    }

    void EnemyTurn()
    {
        if(state != BattleState.ENEMYTURN)
        {
            return;
        }

        for (int i = 0; i < 4; i++)
        {
            mainButtons[i].interactable = false;
        }

        if (moldUnitOne.AttemptAttack(playerUnit.GetSpeed(), playerUnit.GetIntimidation(), playerUnit.GetDefense(false)))
        {
            StartCoroutine(PrintText("MOLD uses SPORES"));

            StartCoroutine(EnemyAttack(moldUnitOne.ReturnDamage()));
        }

        else
        {
            StartCoroutine(PrintText("MOLD misses attack"));

            if(numEnemies == 2)
            {
                StartCoroutine(EnemyTwoTurn()); 
            }

            else if(numEnemies == 3)
            {
                StartCoroutine(EnemyThreeTurn());
            }

            else if(numEnemies == 4)
            {
                StartCoroutine(EnemyFourTurn());
            }

            else
            {
                Invoke("EnemyActionDelay", 4f);
            }
        }
    }

    void EnemyActionDelay()
    {
        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    IEnumerator EnemyTwoTurn()
    {
        yield return new WaitForSeconds(2f);

        if (moldUnitTwo.AttemptAttack(playerUnit.GetSpeed(), playerUnit.GetIntimidation(), playerUnit.GetDefense(false)))
        {
            StartCoroutine(PrintText("MOLD 2 uses SPORES"));
            playerUnit.TakeDamage(moldUnitTwo.ReturnDamage());
        }

        else
        {
            StartCoroutine(PrintText("MOLD 2 misses its attack"));
        }

        yield return new WaitForSeconds(2f);

        if (playerUnit.currHealth <= 0.0f)
        {
            SceneManager.LoadScene("Game Over"); 
        }

        playerHealth.transform.localScale = new Vector3(-(System.Math.Max(playerUnit.currHealth / playerUnit.GetMaxHealth(), 0)), 1, 1);
        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    IEnumerator EnemyThreeTurn()
    {
        yield return new WaitForSeconds(2f);

        if (moldUnitThree.AttemptAttack(playerUnit.GetSpeed(), playerUnit.GetIntimidation(), playerUnit.GetDefense(false)))
        {
            StartCoroutine(PrintText("MOLD 3 uses SPORES"));
            playerUnit.TakeDamage(moldUnitThree.ReturnDamage());
        }

        else
        {
            StartCoroutine(PrintText("MOLD 3 misses its attack"));
        }

        if (playerUnit.currHealth <= 0.0f)
        {
            SceneManager.LoadScene("Game Over");
        }

        yield return new WaitForSeconds(2f);
        playerHealth.transform.localScale = new Vector3(-(System.Math.Max(playerUnit.currHealth / playerUnit.GetMaxHealth(), 0)), 1, 1);
        StartCoroutine(EnemyTwoTurn());
    }

    IEnumerator EnemyFourTurn()
    {
        yield return new WaitForSeconds(2f);

        if (moldUnitFour.AttemptAttack(playerUnit.GetSpeed(), playerUnit.GetIntimidation(), playerUnit.GetDefense(false)))
        {
            StartCoroutine(PrintText("MOLD 4 uses SPORES"));
            playerUnit.TakeDamage(moldUnitFour.ReturnDamage());
        }

        else
        {
            StartCoroutine(PrintText("MOLD 4 misses its attack"));
        }

        if (playerUnit.currHealth <= 0.0f)
        {
            SceneManager.LoadScene("Game Over");
        }

        yield return new WaitForSeconds(2f);
        playerHealth.transform.localScale = new Vector3(-(System.Math.Max(playerUnit.currHealth / playerUnit.GetMaxHealth(), 0)), 1, 1);
        StartCoroutine(EnemyThreeTurn());
    }

    IEnumerator EnemyAttack(float damage)
    {
        if(currEnemies == 1)
        {
            playerUnit.TakeDamage(damage);
            yield return new WaitForSeconds(2f);
            playerHealth.transform.localScale = new Vector3(-(System.Math.Max(playerUnit.currHealth / playerUnit.GetMaxHealth(), 0)), 1, 1);

            if (playerUnit.currHealth <= 0.0f)
            {
                SceneManager.LoadScene("Game Over");
            }

            state = BattleState.PLAYERTURN;
            PlayerTurn(); 
        }

        else if(currEnemies == 2)
        {
            playerUnit.TakeDamage(damage);
            //yield return new WaitForSeconds(2f);
            playerHealth.transform.localScale = new Vector3(-(System.Math.Max(playerUnit.currHealth / playerUnit.GetMaxHealth(), 0)), 1, 1);

            StartCoroutine(EnemyTwoTurn());
        }

        else if(currEnemies == 3)
        {
            playerUnit.TakeDamage(damage);
            //yield return new WaitForSeconds(2f);
            playerHealth.transform.localScale = new Vector3(-(System.Math.Max(playerUnit.currHealth / playerUnit.GetMaxHealth(), 0)), 1, 1);

            StartCoroutine(EnemyThreeTurn());
        }

        else if(currEnemies == 4)
        {
            playerUnit.TakeDamage(damage);
            //yield return new WaitForSeconds(2f);
            playerHealth.transform.localScale = new Vector3(-(System.Math.Max(playerUnit.currHealth / playerUnit.GetMaxHealth(), 0)), 1, 1);

            StartCoroutine(EnemyFourTurn());
        }
    }

    public void OnAttackButton()
    {
        //...
    }

    public void OnAttackMoveButton(int moveNum)
    {
        attackNum = moveNum;
    }

    public void OnTargetButton(int tarNum)
    {
        if(state != BattleState.PLAYERTURN)
        {
            return;
        }

        target = tarNum;

        StartCoroutine(PlayerAttack());
    }

    IEnumerator PlayerAttack()
    {
        for (int i = 0; i < 4; i++)
        {
            mainButtons[i].interactable = false;
        }

        if (target == 1)
        {
            if (playerUnit.AttemptAttack(attackNum, moldUnitOne.GetLevel()))
            {
                if (attackNum == 1)
                {
                    StartCoroutine(PrintText("You attacked Mold 1 \nIf he had a mom she would be \"so proud\""));
                    yield return new WaitForSeconds(6f);

                    moldUnitOne.TakeDamage(playerUnit.GetAttackDamage());
                    firstMoldHealth.transform.localScale = new Vector3(System.Math.Max(moldUnitOne.currHealth / moldUnitOne.GetMaxHealth(), 0), 1, 1);
                }

                else if(attackNum == 2)
                {
                    StartCoroutine(PrintText("You stare into the soul of Mold 1 \nHe flinches and hurts himself"));
                    yield return new WaitForSeconds(6f);

                    moldUnitOne.TakeDamage(moldUnitOne.ReturnDamage());
                    firstMoldHealth.transform.localScale = new Vector3(System.Math.Max(moldUnitOne.currHealth / moldUnitOne.GetMaxHealth(), 0), 1, 1);
                }

                if (moldUnitOne.currHealth <= 0.0f)
                {
                    StartCoroutine(PrintText("Oh, he died"));
                    yield return new WaitForSeconds(1.8f);
                    currEnemies--;
                    experienceGained += moldUnitOne.GetLevel() * 0.2f;
                    StartCoroutine(RemoveEnemy(moldUnitOne, 2f, 1));
                }
            }

            else
            {
                StartCoroutine(PrintText("You miss your attack"));
                yield return new WaitForSeconds(3f);
            }
        }

        else if(target == 2)
        {
            if (playerUnit.AttemptAttack(attackNum, moldUnitTwo.GetLevel()))
            {
                if (attackNum == 1)
                {
                    StartCoroutine(PrintText("You attacked Mold 2 \nHer self-esteem is irreparably damaged"));
                    yield return new WaitForSeconds(6f);

                    moldUnitTwo.TakeDamage(playerUnit.GetAttackDamage());
                    secondMoldHealth.transform.localScale = new Vector3(System.Math.Max(moldUnitTwo.currHealth / moldUnitTwo.GetMaxHealth(), 0), 1, 1);
                }

                else if(attackNum == 2)
                {
                    StartCoroutine(PrintText("You stare into the soul of Mold 2 \nShe is too afraid to cry outwardly"));
                    yield return new WaitForSeconds(6f);

                    moldUnitTwo.TakeDamage(moldUnitTwo.ReturnDamage());
                    secondMoldHealth.transform.localScale = new Vector3(System.Math.Max(moldUnitTwo.currHealth / moldUnitTwo.GetMaxHealth(), 0), 1, 1);
                }

                if (moldUnitTwo.currHealth <= 0.0f)
                {
                    StartCoroutine(PrintText("Oh, she died"));
                    yield return new WaitForSeconds(1.8f);
                    currEnemies--;
                    experienceGained += moldUnitTwo.GetLevel() * 0.2f;
                    StartCoroutine(RemoveEnemy(moldUnitTwo, 2f, 2));
                }
            }

            else
            {
                StartCoroutine(PrintText("You miss your attack"));
                yield return new WaitForSeconds(3f);
            }
        }

        else if(target == 3)
        {
            if (playerUnit.AttemptAttack(attackNum, moldUnitThree.GetLevel()))
            {
                if (attackNum == 1)
                {
                    StartCoroutine(PrintText("You attacked Mold 3 \nSplish splash your morality is trash"));
                    yield return new WaitForSeconds(6f);

                    moldUnitThree.TakeDamage(playerUnit.GetAttackDamage());
                    thirdMoldHealth.transform.localScale = new Vector3(System.Math.Max(moldUnitThree.currHealth / moldUnitThree.GetMaxHealth(), 0), 1, 1);
                }

                else if(attackNum == 2)
                {
                    StartCoroutine(PrintText("You stare into the soul of Mold 3 \nIt cannot bear the intensity"));
                    yield return new WaitForSeconds(6f);

                    moldUnitThree.TakeDamage(moldUnitThree.ReturnDamage());
                    thirdMoldHealth.transform.localScale = new Vector3(System.Math.Max(moldUnitThree.currHealth / moldUnitThree.GetMaxHealth(), 0), 1, 1);
                }

                if (moldUnitThree.currHealth <= 0.0f)
                {
                    StartCoroutine(PrintText("Oh, another dead"));
                    yield return new WaitForSeconds(2.5f);
                    currEnemies--;
                    experienceGained += moldUnitThree.GetLevel() * 0.2f;
                    StartCoroutine(RemoveEnemy(moldUnitThree, 2f, 3));
                }
            }

            else
            {
                StartCoroutine(PrintText("You miss your attack"));
                yield return new WaitForSeconds(3f);
            }
        }

        else if(target == 4)
        {
            if (playerUnit.AttemptAttack(attackNum, moldUnitFour.GetLevel()))
            {
                if (attackNum == 1)
                {
                    StartCoroutine(PrintText("You attacked Mold 4 \nHarsh \nHe didn't even want to be here"));
                    yield return new WaitForSeconds(6f);

                    moldUnitFour.TakeDamage(playerUnit.GetAttackDamage());
                    forthMoldHealth.transform.localScale = new Vector3(System.Math.Max(moldUnitFour.currHealth / moldUnitFour.GetMaxHealth(), 0), 1, 1);
                }

                else if(attackNum == 2)
                {
                    StartCoroutine(PrintText("You stare into the soul of Mold 4 \nWait, do they even have souls?"));
                    yield return new WaitForSeconds(6f);

                    moldUnitFour.TakeDamage(moldUnitFour.ReturnDamage());
                    forthMoldHealth.transform.localScale = new Vector3(System.Math.Max(moldUnitFour.currHealth / moldUnitFour.GetMaxHealth(), 0), 1, 1);
                }

                if (moldUnitFour.currHealth <= 0.0f)
                {
                    StartCoroutine(PrintText("Was he even alive?"));
                    yield return new WaitForSeconds(2.8f);
                    currEnemies--;
                    experienceGained += moldUnitFour.GetLevel() * 0.2f;
                    StartCoroutine(RemoveEnemy(moldUnitFour, 2f, 4));
                }
            }

            else
            {
                StartCoroutine(PrintText("You miss your attack"));
                yield return new WaitForSeconds(3f);
            }
        }

        if (currEnemies == 0)
        {
            string endBattle = "You recieved " + experienceGained * 10 + " XP";
            float delay = 3.5f;

            if (gm.GetLevel() == 1)
            {
                int chanceSoap = Random.Range(1, 100);

                if (chanceSoap >= 60)
                {
                    loot.Add("Soap");
                    endBattle += "\nMOLD dropped a bit of soap. Could be useful.";
                    delay += 4.5f;
                }
            }

            StartCoroutine(PrintText(endBattle));

            yield return new WaitForSeconds(delay);

            gm.AddLoot(loot);
            gm.SetExperiencePoints(experienceGained);
            experienceGained = 0;

            if (gm.GetExperience() >= 2.0f && !secondLevel)
            {
                StartCoroutine(PrintText("Congrats, you leveled up! Now you can simply stare at your enemies to harm them. Neat."));
                secondLevel = true;
                yield return new WaitForSeconds(10f);
            }

            state = BattleState.WON;
            FreeRoam();
        }

        else
        {
            yield return new WaitForSeconds(1f);

            state = BattleState.ENEMYTURN;
            EnemyTurn();
        }
    }

    IEnumerator RemoveEnemy(MoldController defeated, float wait, int target)
    {
        yield return new WaitForSeconds(wait);

        defeated.gameObject.SetActive(false);

        if(target == 1)
        {
            firstEnemyPos.GetChild(0).GetComponent<TextMeshProUGUI>().text = "";
            firstMoldHealth.transform.localScale = new Vector3(1, 1, 1);
            firstMoldHealth.gameObject.SetActive(false);
        }

        if (target == 2)
        {
            secondEnemyPos.GetChild(0).GetComponent<TextMeshProUGUI>().text = "";
            secondMoldHealth.transform.localScale = new Vector3(1, 1, 1);
            secondMoldHealth.gameObject.SetActive(false);
        }

        if (target == 3)
        {
            thirdEnemyPos.GetChild(0).GetComponent<TextMeshProUGUI>().text = "";
            thirdMoldHealth.transform.localScale = new Vector3(1, 1, 1);
            thirdMoldHealth.gameObject.SetActive(false);
        }

        if (target == 4)
        {
            forthEnemyPos.GetChild(0).GetComponent<TextMeshProUGUI>().text = "";
            forthMoldHealth.transform.localScale = new Vector3(1, 1, 1);
            forthMoldHealth.gameObject.SetActive(false);
        }

        if (currEnemies != 0)
        {
            targets[target - 1].interactable = false;
        }
    }

    public void OnItemButton(int item)
    {
        for (int i = 0; i < 4; i++)
        {
            mainButtons[i].interactable = false;
        }

        if (gm.GetInventory()[item - 1] == "Glue")
        {
            StartCoroutine(PrintText("You should hold on to this..."));
        }

        else if(gm.GetInventory()[item - 1] == "Soap")
        {

            StartCoroutine(PrintText("You used SOAP and regained 4 health"));
            usedItems.Add(gm.GetInventory()[item - 1]);
            itemButtons[item - 1].interactable = false;

            if(playerUnit.currHealth + 4.0f > playerUnit.GetMaxHealth())
            {
                playerUnit.currHealth = playerUnit.GetMaxHealth();
            }

            else
            {
                playerUnit.currHealth += 4.0f;
            }
        }

        playerUnit.changeDamageAnimation();
        Invoke("PlayerActionDelay", 5f);
    }

    void PlayerActionDelay()
    {
        state = BattleState.ENEMYTURN;
        EnemyTurn();
    }

    public void OnDefendButton()
    {
        for (int i = 0; i < 4; i++)
        {
            mainButtons[i].interactable = false;
        }

        StartCoroutine(PrintText("This strategy is too advanced to be effective against mold"));

        Invoke("PlayerActionDelay", 7.5f);
    }

    public void OnFleeButton()
    {
        for (int i = 0; i < 4; i++)
        {
            mainButtons[i].interactable = false;
        }

        int chanceFlee = Random.Range(1, 100);

        if(gm.GetLevel() == 1)
        {
            if(chanceFlee >= 80)
            {
                StartCoroutine(PrintText("Dread it. Run from it. Mold cannot be escaped."));
                Invoke("PlayerActionDelay", 6f);
            }

            else
            {
                StartCoroutine(DelayFlee());
            }
            
        }

        if(gm.GetLevel() == 2)
        {
            if (chanceFlee >= 60)
            {
                StartCoroutine(PrintText("Dread it. Run from it. Mold cannot be escaped."));
                Invoke("PlayerActionDelay", 6f);
            }

            else
            {
                StartCoroutine(DelayFlee());
            }
        }

        if(gm.GetLevel() == 3)
        {
            if (chanceFlee >= 40)
            {
                StartCoroutine(PrintText("Dread it. Run from it. Mold cannot be escaped."));
                Invoke("PlayerActionDelay", 6f);
            }

            else
            {
                StartCoroutine(DelayFlee());
            }
        }
    }

    IEnumerator DelayFlee()
    {
        StartCoroutine(PrintText("Coward"));
        yield return new WaitForSeconds(2f);
        FreeRoam();
    }

    void FreeRoam()
    {
        battleAudio.Pause(); 

        firstEnemyPos.GetChild(0).GetComponent<TextMeshProUGUI>().text = "";
        secondEnemyPos.GetChild(0).GetComponent<TextMeshProUGUI>().text = "";
        thirdEnemyPos.GetChild(0).GetComponent<TextMeshProUGUI>().text = "";
        forthEnemyPos.GetChild(0).GetComponent<TextMeshProUGUI>().text = "";

        firstMoldHealth.gameObject.SetActive(false);
        secondMoldHealth.gameObject.SetActive(false);
        thirdMoldHealth.gameObject.SetActive(false);
        forthMoldHealth.gameObject.SetActive(false);
        playerHealth.gameObject.SetActive(false);

        gm.SetPlayerHealth(playerUnit.currHealth);

        Destroy(playerUnit.gameObject);

        if (numEnemies == 1)
        {
            Destroy(moldUnitOne.gameObject);
        }

        else if (numEnemies == 2)
        {
            Destroy(moldUnitOne.gameObject);
            Destroy(moldUnitTwo.gameObject);
        }

        else if (numEnemies == 3)
        {
            Destroy(moldUnitOne.gameObject);
            Destroy(moldUnitTwo.gameObject);
            Destroy(moldUnitThree.gameObject);
        }

        else if (numEnemies == 4)
        {
            Destroy(moldUnitOne.gameObject);
            Destroy(moldUnitTwo.gameObject);
            Destroy(moldUnitThree.gameObject);
            Destroy(moldUnitFour.gameObject);
        }

        gm.EndRegularBattle();
    }

    IEnumerator PrintText(string t)
    {
        dialogueText.text = "";
        for(int i = 0; i < t.Length; i++)
        {
            dialogueText.text += t[i];
            yield return new WaitForSeconds(.08f);
        }
    }
}
