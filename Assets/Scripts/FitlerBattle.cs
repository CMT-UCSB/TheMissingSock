using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FitlerBattle : MonoBehaviour
{
    public GameObject player;
    public GameObject npc;
    public GameObject enemy;
    public GameObject boss;

    public GameObject attackMenu;
    public GameObject targetMenu;
    public GameObject itemMenu;
    public GameObject mainMenu;

    public GameManager gm;

    PlayerBattle playerUnit;
    HairTiesController npcUnit;

    MoldController moldUnitOne;
    MoldController moldUnitTwo;

    FilterController bossUnit;

    public Transform playerPos;
    public Transform npcPos;
    public Transform firstEnemyPos;
    public Transform secondEnemyPos;
    public Transform bossPos; 

    private Button[] targets;
    private Button[] itemButtons;
    private Button[] mainButtons;
    private Button[] attackButtons;

    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI fText;

    private int target;
    private int numEnemies;
    private int currEnemies;
    private int attackNum;

    private float experienceGained;

    private bool sockCanAttack;
    private bool hairTiesCanAttack;
    private bool firstMinionAlive;
    private bool secondMinionAlive;

    private List<string> loot = new List<string>();
    private List<string> usedItems = new List<string>();

    public enum BattleState { START, PLAYERTURN, NPCTURN, ENEMYTURN, BOSSTURN, WON, LOST }
    public BattleState state;

    public AudioSource battleAudio;
    public AudioClip bossMusic;

    public Image firstMoldHealth;
    public Image secondMoldHealth;
    public Image fitlerHealth;
    public Image playerHealth;
    public Image npcHealth;

    public void Begin()
    {
        battleAudio.clip = bossMusic;
        battleAudio.Play();

        firstMoldHealth.gameObject.SetActive(true);
        secondMoldHealth.gameObject.SetActive(true);
        fitlerHealth.gameObject.SetActive(true);
        playerHealth.gameObject.SetActive(true);
        npcHealth.gameObject.SetActive(true);

        targets = targetMenu.GetComponentsInChildren<Button>();
        itemButtons = itemMenu.GetComponentsInChildren<Button>();
        mainButtons = mainMenu.GetComponentsInChildren<Button>();
        attackButtons = attackMenu.GetComponentsInChildren<Button>();

        sockCanAttack = true;
        hairTiesCanAttack = true;
        firstMinionAlive = true;
        secondMinionAlive = true;

        state = BattleState.START;
        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle()
    {
        GameObject playerGO = Instantiate(player, playerPos);
        playerUnit = playerGO.GetComponent<PlayerBattle>();

        GameObject npcGO = Instantiate(npc, npcPos);
        npcUnit = npcGO.GetComponent<HairTiesController>();

        GameObject bossGO = Instantiate(boss, bossPos);
        bossUnit = bossGO.GetComponent<FilterController>();

        GameObject enemy1 = Instantiate(enemy, firstEnemyPos);
        moldUnitOne = enemy1.GetComponent<MoldController>();
        moldUnitOne.SetLevel(1);

        GameObject enemy2 = Instantiate(enemy, secondEnemyPos);
        moldUnitTwo = enemy2.GetComponent<MoldController>();
        moldUnitTwo.SetLevel(1);

        firstEnemyPos.GetChild(0).GetComponent<TextMeshProUGUI>().text = "LVL 1";
        secondEnemyPos.GetChild(0).GetComponent<TextMeshProUGUI>().text = "LVL 1";
        bossPos.GetChild(0).GetComponent<TextMeshProUGUI>().text = "You made a mistake crossing me...";

        numEnemies = 2;
        currEnemies = 2;

        AdjustAttacks();
        AdjustTargets();
        PopulateItems();

        playerUnit.currHealth = gm.GetPlayerHealth();

        StartCoroutine(PrintText("If you lose here, you will be lost for good..."));

        for(int i = 0; i < 4; i++)
        {
            mainButtons[i].interactable = false;
        }

        yield return new WaitForSeconds(6f);

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

        targets[2].interactable = true;
        targets[2].GetComponentInChildren<TextMeshProUGUI>().text = "FITLER";
        targets[3].GetComponentInChildren<TextMeshProUGUI>().text = "-";
    }

    void AdjustAttacks()
    {
        attackButtons[0].interactable = true;
        attackButtons[1].interactable = true;
        attackButtons[2].interactable = false;
        attackButtons[3].interactable = false;
    }

    void PopulateItems()
    {
        if (gm.GetInventory().Count == 0)
        {
            for (int i = 0; i < 5; i++)
            {
                itemButtons[i].interactable = false;
            }

            itemButtons[6].interactable = true;
        }

        else
        {
            for (int i = 0; i < gm.GetInventory().Count; i++)
            {
                itemButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = gm.GetInventory()[i].ToUpper();
            }

            for (int i = gm.GetInventory().Count; i < itemButtons.Length; i++)
            {
                itemButtons[i].interactable = false;
            }

            itemButtons[6].interactable = true;
        }

    }

    void PlayerTurn()
    {
        if (!sockCanAttack)
        {
            StartCoroutine(PrintText("You cannot do anything. Unsurprising..."));
            Invoke("PlayerActionDelay", 5f);
            return;
        }

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

    public void OnAttackButton()
    {
        //nothing
    }

    public void OnAttackMoveButton(int moveNum)
    {
        attackNum = moveNum;
    }

    public void OnTargetButton(int tarNum)
    {
        if (state != BattleState.PLAYERTURN)
        {
            return;
        }

        target = tarNum;

        StartCoroutine(PlayerAttack());
    }

    IEnumerator PlayerAttack()
    {
        if (target == 1)
        {
            if (playerUnit.AttemptAttack(attackNum, moldUnitOne.GetLevel()))
            {
                if (attackNum == 1)
                {
                    StartCoroutine(PrintText("You attacked Minion 1 \nHe has no free will and cannot respond"));
                    yield return new WaitForSeconds(6f);

                    moldUnitOne.TakeDamage(playerUnit.GetAttackDamage());
                    firstMoldHealth.transform.localScale = new Vector3(System.Math.Max(moldUnitOne.currHealth / moldUnitOne.GetMaxHealth(), 0), 1, 1);
                }

                else if(attackNum == 2)
                {
                    StartCoroutine(PrintText("You stare into the soul of Minion 1 \nHe has no soul. Nothing happens."));
                    yield return new WaitForSeconds(2f);
                }

                if (moldUnitOne.currHealth <= 0.0f)
                {
                    StartCoroutine(PrintText("Freedom at last"));
                    yield return new WaitForSeconds(2f);
                    currEnemies--;
                    experienceGained += moldUnitOne.GetLevel() * 0.2f;
                    StartCoroutine(RemoveEnemy(moldUnitOne, 2f, 1));
                }
            }

            else
            {
                StartCoroutine(PrintText("You miss your attack"));
            }
        }

        else if (target == 2)
        {
            if (playerUnit.AttemptAttack(attackNum, moldUnitOne.GetLevel()))
            {
                if (attackNum == 1)
                {
                    StartCoroutine(PrintText("You attacked Minion 2 \nShe stares blankly into your soul"));
                    yield return new WaitForSeconds(6f);

                    moldUnitTwo.TakeDamage(gm.playerBattle.GetComponent<PlayerBattle>().GetAttackDamage());
                    secondMoldHealth.transform.localScale = new Vector3(System.Math.Max(moldUnitTwo.currHealth / moldUnitTwo.GetMaxHealth(), 0), 1, 1);
                }

                else if (attackNum == 2)
                {
                    StartCoroutine(PrintText("You stare into the soul of Minion 2 \nShe doesn't even blink."));
                    yield return new WaitForSeconds(2f);
                }

                if (moldUnitTwo.currHealth <= 0.0f)
                {
                    StartCoroutine(PrintText("Rest in pieces"));
                    yield return new WaitForSeconds(2f);
                    currEnemies--;
                    experienceGained += moldUnitTwo.GetLevel() * 0.2f;
                    StartCoroutine(RemoveEnemy(moldUnitTwo, 2f, 2));
                }
            }

            else
            {
                StartCoroutine(PrintText("You miss your attack"));
            }
        }

        else if (target == 3)
        {
            if (playerUnit.AttemptAttack(attackNum, 4))
            {
                if (attackNum == 1)
                {
                    StartCoroutine(PrintText("You attacked Fitler \nUnfortunately for you, her ego is unaffected"));
                    yield return new WaitForSeconds(7.5f);

                    bossUnit.TakeDamage(true, playerUnit.GetAttackDamage());
                    fitlerHealth.transform.localScale = new Vector3(System.Math.Max((bossUnit.GetHealth() / bossUnit.GetMaxHealth()) / 2f, 0), 0.5f, 1);
                }

                else if (attackNum == 2)
                {
                    StartCoroutine(PrintText("You stare into Fitler's soul \nNo one has attempted this. She is terrified."));
                    yield return new WaitForSeconds(7.5f);

                    bossUnit.fitlerCanAttack = false;
                    bossUnit.TakeDamage(true, playerUnit.GetAttackDamage() * 1.25f);
                    fitlerHealth.transform.localScale = new Vector3(System.Math.Max((bossUnit.GetHealth() / bossUnit.GetMaxHealth()) / 2f, 0), 0.5f, 1);
                }

                if (bossUnit.GetHealth() <= 0.0f)
                {
                    experienceGained += 10 * 0.2f;
                    SceneManager.LoadScene("To Be Continued"); 
                }
            }

            else
            {
                StartCoroutine(PrintText("You miss your attack"));
            }
        }

        if(currEnemies == 0)
        {
            Destroy(moldUnitOne.gameObject);
            Destroy(moldUnitTwo.gameObject);
        }

        yield return new WaitForSeconds(4f);

        /*
        state = BattleState.ENEMYTURN;
        EnemyTurn();
        */

        state = BattleState.NPCTURN;
        NPCTurn();
    }

    IEnumerator RemoveEnemy(MoldController defeated, float wait, int target)
    {
        yield return new WaitForSeconds(wait);

        defeated.gameObject.SetActive(false);

        if (target == 1)
        {
            firstEnemyPos.GetChild(0).GetComponent<TextMeshProUGUI>().text = "";
            firstMoldHealth.transform.localScale = new Vector3(1, 1, 1);
            firstMoldHealth.gameObject.SetActive(false);
            Destroy(moldUnitOne.gameObject);
            firstMinionAlive = false;
        }

        if (target == 2)
        {
            secondEnemyPos.GetChild(0).GetComponent<TextMeshProUGUI>().text = "";
            secondMoldHealth.transform.localScale = new Vector3(1, 1, 1);
            secondMoldHealth.gameObject.SetActive(false);
            Destroy(moldUnitTwo.gameObject);
            secondMinionAlive = false; 
        }

        if (currEnemies != 0)
        {
            targets[target - 1].interactable = false;
        }
    }

    void NPCTurn()
    {
        if(state != BattleState.NPCTURN)
        {
            return;
        }

        StartCoroutine(NPCAction());
    }

    IEnumerator NPCAction()
    {
        int chanceAction = Random.Range(1, 100);

        if(chanceAction <= 5)
        {
            if(currEnemies > 1)
            {
                if(targets[0].IsInteractable())
                {
                    StartCoroutine(PrintText("Hair Ties snaps and instantly kills Minion 1"));
                    moldUnitOne.TakeDamage(moldUnitOne.currHealth + 1f);
                    yield return new WaitForSeconds(5f);
                }

                else if(targets[1].IsInteractable())
                {
                    StartCoroutine(PrintText("Hair Ties snaps and instantly kills Minion 2"));
                    moldUnitTwo.TakeDamage(moldUnitTwo.currHealth + 1f);
                    yield return new WaitForSeconds(5f);
                }
            }

            else
            {
                StartCoroutine(PrintText("Hair Ties watches fearfully."));
                yield return new WaitForSeconds(3.3f);
            }
        }

        else if(chanceAction <= 40)
        {
            StartCoroutine(PrintText("Hair Ties cheers you on! Your attack damage increases."));
            playerUnit.ChangeAttackDamage(.25f);
            yield return new WaitForSeconds(6f);
        }

        else if(chanceAction <= 60)
        {
            StartCoroutine(PrintText("Hair Ties finds a bit of soap. Your health increases slightly"));
            playerUnit.currHealth += 1.0f;
            yield return new WaitForSeconds(6.5f);
        }

        else
        {
            StartCoroutine(PrintText("Hair Ties watches fearfully."));
            yield return new WaitForSeconds(3f);
        }

        state = BattleState.ENEMYTURN;
        EnemyTurn();
    }

    public void OnItemButton(int item)
    {
        /*if (gm.GetInventory()[item - 1] == "Glue")
        {
            StartCoroutine(PrintText("You should hold on to this..."));
        }*/

        if (gm.GetInventory()[item - 1] == "Soap")
        {
            StartCoroutine(PrintText("You used SOAP and regained 3 health"));
            usedItems.Add(gm.GetInventory()[item - 1]);
            itemButtons[item - 1].interactable = false;

            if (playerUnit.currHealth + 4.0f > playerUnit.GetMaxHealth())
            {
                playerUnit.currHealth = playerUnit.GetMaxHealth();
            }

            else
            {
                playerUnit.currHealth += 4.0f;
            }
        }

        playerUnit.changeDamageAnimation();
        Invoke("PlayerActionDelay", 4.5f);
    }

    void PlayerActionDelay()
    {
        sockCanAttack = true;
        fText.text = "";
        state = BattleState.ENEMYTURN;
        EnemyTurn();
    }

    public void OnDefendButton()
    {
        for (int i = 0; i < 4; i++)
        {
            mainButtons[i].interactable = false;
        }

        StartCoroutine(PrintText("Fitler admires your courage and backs off a bit."));
        bossUnit.fitlerCanAttack = false;

        Invoke("PlayerActionDelay", 4f);
    }

    public void OnFleeButton()
    {
        for (int i = 0; i < 4; i++)
        {
            mainButtons[i].interactable = false;
        }

        StartCoroutine(PrintText("The ride never ends"));
        Invoke("PlayerActionDelay", 4f);
    }

    void EnemyTurn()
    {
        if(state != BattleState.ENEMYTURN)
        {
            return;
        }

        if (firstMinionAlive)
        {
            if (moldUnitOne.AttemptAttack(playerUnit.GetSpeed(), playerUnit.GetIntimidation(), playerUnit.GetDefense(false)))
            {
                StartCoroutine(PrintText("MOLD uses SPORES"));

                StartCoroutine(EnemyAttack(moldUnitOne.ReturnDamage()));
            }

            else
            {
                StartCoroutine(PrintText("MOLD misses attack"));

                if (numEnemies > 1)
                {
                    StartCoroutine(EnemyTwoTurn());
                }

                else
                {
                    DelayBoss(3f);
                }
            }
        }

        else
        {
            StartCoroutine(EnemyTwoTurn());
        }
    }

    IEnumerator EnemyTwoTurn()
    {
        yield return new WaitForSeconds(3f);

        if (moldUnitTwo.AttemptAttack(playerUnit.GetSpeed(), playerUnit.GetIntimidation(), playerUnit.GetDefense(false)))
        {
            StartCoroutine(PrintText("MOLD 2 uses SPORES"));
            playerUnit.TakeDamage(moldUnitTwo.ReturnDamage());
        }

        else
        {
            StartCoroutine(PrintText("MOLD 2 misses its attack"));
        }

        yield return new WaitForSeconds(3f);

        if (playerUnit.currHealth <= 0.0f || npcUnit.currHealth <= 0.0f)
        {
            SceneManager.LoadScene("Game Over");
        }

        playerHealth.transform.localScale = new Vector3(-(System.Math.Max(playerUnit.currHealth / playerUnit.GetMaxHealth(), 0)), 1, 1);
        state = BattleState.BOSSTURN;
        BossTurn();
    }

    IEnumerator EnemyAttack(float damage)
    {
        if (currEnemies == 1)
        {
            playerUnit.TakeDamage(damage);
            yield return new WaitForSeconds(3f);
            playerHealth.transform.localScale = new Vector3(-(System.Math.Max(playerUnit.currHealth / playerUnit.GetMaxHealth(), 0)), 1, 1);

            if (playerUnit.currHealth <= 0.0f || npcUnit.currHealth <= 0.0f)
            {
                SceneManager.LoadScene("Game Over");
            }

            state = BattleState.BOSSTURN;
            BossTurn();
        }

        else if (currEnemies == 2)
        {
            playerUnit.TakeDamage(damage);
            //yield return new WaitForSeconds(3f);

            StartCoroutine(EnemyTwoTurn());
        }
    }

    void BossTurn()
    {
        if(state != BattleState.BOSSTURN)
        {
            return;
        }

        float[] attackStats = bossUnit.Attack(currEnemies);

        //no attack
        if(attackStats[0] == 0.0f)
        {
            StartCoroutine(FitlerSpeaks("Well m-maybe I just don't want to attack..."));
            StartCoroutine(PrintText("Fitler \"decides\" not to attack"));
            StartCoroutine(DelayBoss(6f));
        }

        //tear
        else if(attackStats[0] == 1.0f)
        {
            playerUnit.TakeDamage(attackStats[1]);
            npcUnit.TakeDamage(attackStats[1]); 
            StartCoroutine(FitlerSpeaks("Who's holy now?"));
            StartCoroutine(PrintText("Fitler uses TEAR"));
            StartCoroutine(DelayBoss(4f));
        }

        //tangle
        else if (attackStats[0] == 2.0f)
        {
            if(attackStats[2] == 0.0f)
            {
                StartCoroutine(FitlerSpeaks("Well m-maybe I just don't want to attack..."));
                StartCoroutine(PrintText("Fitler's attack TANGLE misses"));
                StartCoroutine(DelayBoss(6f));
            }

            else if(attackStats[2] == 1.0f)
            {
                sockCanAttack = false;
                StartCoroutine(FitlerSpeaks("Tripping over yourself again?"));
                StartCoroutine(PrintText("Fitler uses TANGLE on SOCK"));
                StartCoroutine(DelayBoss(6f));
            }

            //TODO
            else if(attackStats[2] == 2.0f)
            {
                hairTiesCanAttack = false;
                StartCoroutine(FitlerSpeaks("As you were - as I prefer"));
                StartCoroutine(PrintText("Fitler uses TANGLE on HAIR TIES"));
                StartCoroutine(DelayBoss(6f));
            }
        }

        //no minions
        else if (attackStats[0] == 3.0f)
        {
            StartCoroutine(FitlerSpeaks("Well m-maybe I just don't want to attack..."));
            StartCoroutine(PrintText("Fitler cannot summon more MOLD"));
            StartCoroutine(DelayBoss(6f));
        }

        //one minion
        else if (attackStats[0] == 4.0f)
        {
            if(firstMinionAlive)
            {
                GameObject enemy2 = Instantiate(enemy, secondEnemyPos);
                moldUnitTwo = enemy2.GetComponent<MoldController>();
                int level = (int)attackStats[1];
                moldUnitTwo.SetLevel((int)attackStats[1]);
                secondEnemyPos.GetChild(0).GetComponent<TextMeshProUGUI>().text = "LVL " + level.ToString();
                secondMoldHealth.gameObject.SetActive(true);
            }
            
            else if(secondMinionAlive)
            {
                GameObject enemy1 = Instantiate(enemy, firstEnemyPos);
                moldUnitOne = enemy1.GetComponent<MoldController>();
                int level = (int)attackStats[1]; 
                moldUnitOne.SetLevel((int)attackStats[1]);
                firstEnemyPos.GetChild(0).GetComponent<TextMeshProUGUI>().text = "LVL " + level.ToString();
                firstMoldHealth.gameObject.SetActive(true);
            }

            StartCoroutine(FitlerSpeaks("FIGHT FOR ME!"));
            StartCoroutine(PrintText("Fitler summons a minion. He's kinda cute."));
            StartCoroutine(DelayBoss(6f));
        }

        //two minion
        else if (attackStats[0] == 5.0f)
        {
            GameObject enemy1 = Instantiate(enemy, firstEnemyPos);
            moldUnitOne = enemy1.GetComponent<MoldController>();
            int level1 = (int)attackStats[1];
            moldUnitOne.SetLevel((int)attackStats[1]);
            firstEnemyPos.GetChild(0).GetComponent<TextMeshProUGUI>().text = "LVL " + level1.ToString();

            GameObject enemy2 = Instantiate(enemy, secondEnemyPos);
            moldUnitTwo = enemy2.GetComponent<MoldController>();
            int level2 = (int)attackStats[1];
            moldUnitTwo.SetLevel((int)attackStats[1]);
            secondEnemyPos.GetChild(0).GetComponent<TextMeshProUGUI>().text = "LVL " + level2.ToString();

            firstMoldHealth.gameObject.SetActive(true);
            secondMoldHealth.gameObject.SetActive(true);

            StartCoroutine(FitlerSpeaks("FIGHT FOR ME!"));
            StartCoroutine(PrintText("Fitler summons two minions. You are \"definitely\" impressed"));
            StartCoroutine(DelayBoss(6f));
        }

        if(playerUnit.currHealth <= 0.0f || npcUnit.currHealth <= 0.0f)
        {
            SceneManager.LoadScene("Game Over");
        }
    }

    IEnumerator DelayBoss(float delay)
    {
        yield return new WaitForSeconds(delay);
        playerHealth.transform.localScale = new Vector3(-(System.Math.Max(playerUnit.currHealth / playerUnit.GetMaxHealth(), 0)), 1, 1);
        npcHealth.transform.localScale = new Vector3(System.Math.Max(npcUnit.currHealth / npcUnit.GetMaxHealth(), 0), 1, 1);
        bossUnit.fitlerCanAttack = true;
        fText.text = "";
        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    IEnumerator PrintText(string t)
    {
        dialogueText.text = "";
        for (int i = 0; i < t.Length; i++)
        {
            dialogueText.text += t[i];
            yield return new WaitForSeconds(.08f);
        }
    }

    IEnumerator FitlerSpeaks(string t)
    {
        fText.text = "";
        for (int i = 0; i < t.Length; i++)
        {
            fText.text += t[i];
            yield return new WaitForSeconds(.08f);
        }
    }
}
