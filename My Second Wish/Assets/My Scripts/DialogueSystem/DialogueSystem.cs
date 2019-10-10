using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour {

    //creates a universal instance of the DialogueSystem that is accessible to every class
    public static DialogueSystem instance { get; set; }

    //points to the current Dialogue system output UI, these UIs are world space 3d floating windows that are actively switched based on the npc, this helps with managing the positioning
    public GameObject CurrentDialogueUI;
    public GameObject PlayerReference;
    public GameObject ProceedWithDialogueButton;
    private List<TextLine> dialogueQueue;



    void Awake() {
        if ((instance != this) && (instance != null))
            Destroy(gameObject);
        else
            instance = this;
    }

    // Use this for initialization
    void Start () {
        //refer to previous code for how to set up instance
	}
	
    public DialogueSystem(){
        dialogueQueue = new List<TextLine>();
    }

    //open dialogue window and disable movement
    public void displayDialogueWindow(GameObject currentDialogueUI, string ScriptedDialogueFileNameNoExtension) {
        CurrentDialogueUI = currentDialogueUI;
        disableMovementDuringDialogue();
        panCameraToText();
        loadDialogueScript(ScriptedDialogueFileNameNoExtension);
        setDialogueWindowText(dialogueQueue[0].getSpeaker(), dialogueQueue[0].getLine());
        CurrentDialogueUI.GetComponent<Canvas>().enabled = true;

        ProceedWithDialogueButton.GetComponent<Image>().enabled = true;
        ProceedWithDialogueButton.GetComponent<Button>().enabled = true;


    }

    //disable window and reenable movement
    public void closeDialogueWindow() {
        CurrentDialogueUI.transform.Find("Text").gameObject.GetComponent<Text>().text = "";
        reenableMovementDuringDialogue();
        returnCameraToDefaultPos();
        dialogueQueue.Clear();
        CurrentDialogueUI.GetComponent<Canvas>().enabled = false;

        ProceedWithDialogueButton.GetComponent<Image>().enabled = false;
        ProceedWithDialogueButton.GetComponent<Button>().enabled = false;
    }

    //diable movement, hide movement stick
    public void disableMovementDuringDialogue() {
        PlayerReference.GetComponent<MovementControl>().disableMovement();
    }

    //reenable movement and show movement stick
    public void reenableMovementDuringDialogue() {
        PlayerReference.GetComponent<MovementControl>().enableMovement();

    }

    public void panCameraToText() {
        PlayerReference.transform.Find("Main Camera").gameObject.GetComponent<Animator>().SetTrigger("PanToText");
    }

    public void returnCameraToDefaultPos() {
        PlayerReference.transform.Find("Main Camera").gameObject.GetComponent<Animator>().SetTrigger("ReturnFromPanText");
    }

    //set the dialogue window speaker and line
    public void setDialogueWindowText(string speaker, string line) {
        String temp = speaker + ": " + line;
        CurrentDialogueUI.transform.Find("Text").gameObject.GetComponent<Text>().text = temp;
    }

    //moves dialogue along and deletes previous line
    public void nextTextLine() {

        if (dialogueQueue.Count > 1) {
            //if another line is present then fetch new line from queue head
            dialogueQueue.RemoveAt(0);
            setDialogueWindowText(dialogueQueue[0].getSpeaker(), dialogueQueue[0].getLine());
            Debug.Log("Queue remaining size = "+dialogueQueue.Count);
        } else {
            //if no new textlines, clean and close the dialogue window
            Debug.Log("Closing dialogue window");
            closeDialogueWindow();
        }
    }

    //add a custom line to the dialogue system
    public void addDialogueToQueue(string speaker, string line) {
        dialogueQueue.Add(new TextLine(speaker, line));
    }

    //load a dialogue script file
    public void loadDialogueScript(string scriptFilename) {
        dialogueQueue.AddRange(parseScriptFile(scriptFilename));

        Debug.Log(dialogueQueue.Count);
    }

    //parse an external dialogue files
    public List<TextLine> parseScriptFile(string scriptFilename) {

        List<TextLine> temp = new List<TextLine>();

        //check for txt file with the same file name, then parse line by line
        TextAsset dialogueFile = Resources.Load("DialogueLines/ScriptedDialogue/" + scriptFilename) as TextAsset;
        string dialogue = dialogueFile.text;

        string[] lines = dialogue.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

        for (int i = 0; i < lines.Length; i++) {
            int colonPos = -1;
            int count = 0;
            

            while ((colonPos==-1)&&(count<lines[i].Length)) {
                if (lines[i][count] == ':') {
                    colonPos = count;
                }

                count = count + 1;
            }

            if (colonPos > -1) {
                TextLine tempText = new TextLine(lines[i].Substring(0, colonPos), lines[i].Substring((colonPos + 1), (lines[i].Length - (colonPos+1))));
                temp.Add(tempText);
            } else {
                //throw an exception
            }
        }

        return temp;
    }

}
