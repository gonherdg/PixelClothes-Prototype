using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text dialogueText;

    private Queue<string> sentences;

    [SerializeField] private Animator animator;

    private void Start()
    {
        sentences = new Queue<string>();

        if (animator == null)
        {
            animator = GameObject.Find("Dialogue Box").GetComponent<Animator>();
        }
    }

    public void StartDialogue(Dialogue dialogue)
    {

        animator.SetBool("open", true);

        nameText.text = dialogue.name;

        sentences.Clear();

        foreach(string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        Debug.Log(sentence);
        dialogueText.text = sentence;
    }

    private void EndDialogue()
    {
        Debug.Log("End of conversation");
        animator.SetBool("open", false);
    }
}
