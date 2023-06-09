using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private float typePerSeconds = 0.015f;
    private TextMeshProUGUI dialogueText;
    private GameObject nextButton;
    private GameObject doneButton;
    private Queue<string> lines;

    // DialogueManager handles the dialogue by displaying
    // Inspired by: Brackeys' "How to make a Dialogue System in Unity"
    public static DialogueManager Instance
    {
        get;
        private set;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            lines = new Queue<string>();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Set up text box, dialogue, and button
    public void StartDialogue(TextMeshProUGUI textBox, Dialogue dialogue, GameObject button, GameObject otherButton)
    {
        Debug.Log("Connected to Dialogue Manager");
        dialogueText = textBox;
        nextButton = button;
        doneButton = otherButton;
        lines.Clear();
        foreach (string line in dialogue.sentences)
        {
            lines.Enqueue(line);
        }
        // After accessing all sentences from dialogue object, display them:
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        string displaySentence = lines.Dequeue();
        if (dialogueText != null)
        {
            StopAllCoroutines();
            StartCoroutine(TypeOutSentence(displaySentence));
        }
    }

    private IEnumerator TypeOutSentence(string displaySentence)
    {
        dialogueText.text = "";
        foreach (char character in displaySentence.ToCharArray())
        {
            dialogueText.text += character;
            if (dialogueText.text == displaySentence)
            {
                if (lines.Count == 0)
                {
                    doneButton.SetActive(true);
                }
                else
                {
                    nextButton.SetActive(true);
                }
            }
            yield return new WaitForSecondsRealtime(typePerSeconds);
        }
    }
}
