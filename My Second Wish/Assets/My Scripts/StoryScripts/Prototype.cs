using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prototype : MonoBehaviour {

    public GameObject Player;
    public GameObject IntroTextPanel;
    public GameObject button;
	// Use this for initialization
	void Start () {
        Player.transform.Find("Main Camera").gameObject.GetComponent<Animator>().SetTrigger("PanToText");
	}
	

    public void returnToGame() {
        Player.transform.Find("Main Camera").gameObject.GetComponent<Animator>().SetTrigger("ReturnFromPanText");
        IntroTextPanel.GetComponent<Animator>().SetTrigger("PanOut");
    }
}
