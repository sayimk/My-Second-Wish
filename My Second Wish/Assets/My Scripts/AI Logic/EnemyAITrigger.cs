using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAITrigger : MonoBehaviour {


    public void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag.Equals("Player")) {
            gameObject.transform.parent.gameObject.GetComponent<ExplorationAI>().engagePlayer(other.gameObject);
        }
    }

    public void OnTriggerExit(Collider other) {

        if (other.gameObject.tag.Equals("Player")) {
            gameObject.transform.parent.gameObject.GetComponent<ExplorationAI>().searchForPlayer();
        }
    }
}
