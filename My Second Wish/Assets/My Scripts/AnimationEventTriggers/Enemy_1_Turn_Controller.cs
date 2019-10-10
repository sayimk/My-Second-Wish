using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_1_Turn_Controller : MonoBehaviour {

    public GameObject CombatController;

    public void selectAsCurrentTurn() {
        CombatController.GetComponent<CombatController>().selectCurrentEnemyTurn(0);
        Debug.Log("Enemy Turn");
    }
}
