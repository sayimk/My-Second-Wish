using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour {

    public GameObject player;
    public GameObject openMenuButton;
    public GameObject questTempText;

    //temp quest counter

    int enemyBeat = 0;

	public void displayMenu() {
        player.GetComponent<MovementControl>().disableMovement();
        gameObject.GetComponent<Animator>().SetTrigger("displayMenu");

    }

    public void closeMenu() {
        player.GetComponent<MovementControl>().enableMovement();
        gameObject.GetComponent<Animator>().SetTrigger("exitMenu");
    }

    public void showQuests() {
        gameObject.GetComponent<Animator>().SetTrigger("showQuests");
        questTempText.GetComponent<Text>().text = enemyBeat + "/3";
    }

    //temp method for demo purposes
    public void incrementKillCount() {
        enemyBeat = enemyBeat + 1;
    }



}
