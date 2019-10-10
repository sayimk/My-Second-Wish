using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_1_Indicator_Controller : MonoBehaviour {

    public GameObject CombatController;

	public void selectAsCurrentTurn() {
        CombatController.GetComponent<CombatController>().selectCurrentPlayersTurn(0);
        Debug.Log("Player turn");
    }
}
