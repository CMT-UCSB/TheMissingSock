using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    //public static int level = 1;
    //private static float XP = 1.0f;

    public bool inBattle;
    private bool isMoving;

    public float moveSpeed;

    public LayerMask EnemyTile;
    public LayerMask BossTile;

    private Vector2 input;

    private Animator animator;

    public GameManager gm;

    public TextMeshProUGUI dialogueText;
    public Image enemyEncounter;
    public Canvas escape;
    public Canvas display;
    public Canvas statsMenu;
    public TextMeshProUGUI statsText;

    private bool printing;
    private bool paused;

    public AudioSource sockAudio;
    public AudioSource backgroundAudio;

    public AudioClip level1;
    public AudioClip PTSD;
    public AudioClip alert;
    public AudioClip walking;

    private void Awake()
    {
        paused = false;
        inBattle = false;
        isMoving = false;
        printing = false;
        animator = GetComponent<Animator>();
        dialogueText.text = "";

        backgroundAudio.clip = level1;
        backgroundAudio.Play();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (paused)
            {
                gm.battling = false;
                escape.gameObject.SetActive(false);
                display.gameObject.SetActive(true);
                paused = false;
            }

            else
            {
                gm.battling = true;
                escape.gameObject.SetActive(true);
                display.gameObject.SetActive(false);
                paused = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            float[] stats = new float[5];
            stats = gm.ConstructStats();
            statsText.text = "XP: " + stats[0].ToString() + "\nSpeed: " + stats[1].ToString() + "\nAttack: " + stats[2].ToString() + "\nIntim: " + stats[3].ToString() + "\nHealth: " + stats[4].ToString(); 
            statsMenu.gameObject.SetActive(true);
        }

        if(Input.GetKeyUp(KeyCode.Tab))
        {
            statsMenu.gameObject.SetActive(false);
        }

        inBattle = gm.battling;

        if (!inBattle)
        {
            if (!isMoving)
            {
                if(sockAudio.isPlaying)
                {
                    sockAudio.Pause();
                }
                
                input.x = Input.GetAxisRaw("Horizontal");
                input.y = Input.GetAxisRaw("Vertical");

                if (input.x != 0)
                {
                    input.y = 0;
                }

                if (input != Vector2.zero)
                {
                    animator.SetFloat("moveX", input.x);
                    animator.SetFloat("moveY", input.y);

                    var targetPos = transform.position;
                    targetPos.x += input.x;
                    targetPos.y += input.y;

                    if(targetPos.x < 33 && targetPos.x > 7 && targetPos.y <= 13 && targetPos.y >= -12)
                    {
                        StartCoroutine(Move(targetPos));
                    }

                }
            }

            if (!sockAudio.isPlaying && isMoving)
            {
                sockAudio.clip = walking; 
                sockAudio.Play();
            }

            animator.SetBool("isMoving", isMoving);
        }
    }

    IEnumerator Move(Vector3 targetPos)
    {
        isMoving = true;

        while((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null; 
        }
        transform.position = targetPos;

        isMoving = false;

        CheckForEncounter();
    }

    void CheckForEncounter()
    {
        if(Physics2D.OverlapBox(new Vector2(transform.position.x, transform.position.y - 0.375f), new Vector2(0.3f, 0.2f), 0.0f, EnemyTile) != null)
        {
            int chanceBattle = Random.Range(1, 101);

            if(chanceBattle <= 10)
            {
                gm.battling = true;

                backgroundAudio.Pause();

                sockAudio.Pause();
                sockAudio.clip = alert;
                sockAudio.Play();

                enemyEncounter.gameObject.SetActive(true);

                animator.SetBool("isMoving", isMoving);

                StartCoroutine("BattleTime"); 
            }
        }

        else if (Physics2D.OverlapBox(new Vector2(transform.position.x, transform.position.y - 0.375f), new Vector2(0.3f, 0.2f), 0.0f, BossTile) != null)
        {
            if(gm.GetExperience() >= 3.0f)
            {
                gm.battling = true;

                backgroundAudio.Pause();

                sockAudio.Pause();
                sockAudio.clip = alert;
                sockAudio.Play();

                enemyEncounter.gameObject.SetActive(true);

                animator.SetBool("isMoving", isMoving);

                StartCoroutine("FitlerTime");
            }
            
            else if(!printing)
            {
                StartCoroutine(PrintText("SOMETHING EVIL LURKS HERE... I SHOULD AVOID IT FOR NOW"));
                printing = true;
            }
        }
    }

    IEnumerator PrintText(string t)
    {
        dialogueText.text = "";
        float wait = 0.0f;
        for (int i = 0; i < t.Length; i++)
        {
            dialogueText.text += t[i];
            yield return new WaitForSeconds(.1f);
            wait += 0.1f;
        }

        yield return new WaitForSeconds(wait);

        dialogueText.text = "";
        printing = false;
    }

    IEnumerator BattleTime()
    {
        yield return new WaitForSeconds(1.5f);
        enemyEncounter.gameObject.SetActive(false);
        gm.StartRegularBattle();
    }

    IEnumerator FitlerTime()
    {
        yield return new WaitForSeconds(1.5f);
        enemyEncounter.gameObject.SetActive(false);
        gm.StartFilterBattle();
    }

    public void StartMusic()
    {
        backgroundAudio.clip = level1;
        backgroundAudio.Play();
    }
}
