using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    private Queue<string> sentences = new Queue<string>();

    public TextMeshProUGUI dialogueText;

    public Button attack;
    public Button defend;
    public Button items;
    public Button flee;
    public Button cont;

    public Camera mc;
    public AudioSource player;
    public AudioClip sock;
    public AudioClip hairTies;

    public Animator htAni;

    public Image wash;

    private void Start()
    {
        //sentences = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        Debug.Log("Starting conversation with... " + dialogue.name);

        sentences.Clear();

        foreach(string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if(sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        if(sentences.Count == 24)
        {
            cont.interactable = false;
            StartCoroutine(PTSD());
        }

        if(sentences.Count == 10)
        {
            attack.interactable = true;
        }

        if(sentences.Count == 8)
        {
            defend.interactable = true;
        }

        if(sentences.Count == 7)
        {
            items.interactable = true;
        }

        if(sentences.Count == 6)
        {
            flee.interactable = true;
        }

        string s = sentences.Dequeue();
        dialogueText.text = s;
    }

    void EndDialogue()
    {
        SceneManager.LoadScene("Level 1"); 
    }

    IEnumerator PTSD()
    {
        player.Pause();
        player.clip = sock;

        yield return new WaitForSeconds(2f);
        player.Play();

        float sizeStart = 5f;
        float sizeCurr = 5f;
        float sizeEnd = 1.5f;

        float xStart = 0;
        float xCurr = 0;
        float xEnd = -2.5f;

        while(sizeCurr > sizeEnd && xCurr > xEnd)
        {
            sizeCurr -= .01f;
            xCurr -= .008f;
            mc.orthographicSize = sizeCurr;
            mc.transform.position = new Vector3(xCurr, mc.transform.position.y, mc.transform.position.z);

            if (-1.5f < xCurr && xCurr < -1.0f)
            {
                wash.gameObject.SetActive(true);
            }

            else
            {
                wash.gameObject.SetActive(false);
            }

            yield return new WaitForSeconds(.04f);
        }

        dialogueText.text = "...";
        mc.orthographicSize = sizeStart;
        mc.transform.position = new Vector3(xStart, mc.transform.position.y, mc.transform.position.z);
        htAni.SetBool("Idle", false);
        player.Pause();
        player.clip = hairTies;

        yield return new WaitForSeconds(8f);
        player.Play();
        htAni.SetBool("Idle", true);
        cont.interactable = true;
    }
}
