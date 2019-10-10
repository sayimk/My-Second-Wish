using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteractionController : MonoBehaviour {

    private GameObject CollidedObject;
    public GameObject interactUIButton;
    //add code to face the object interacting with

    public void InteractWithObject() {
        if (CollidedObject!= null) {
            CollidedObject.GetComponent<InteractionInterface>().interact();
            gameObject.transform.LookAt(new Vector3(CollidedObject.transform.position.x, gameObject.transform.position.y, CollidedObject.transform.position.z));

        } else {
            interactUIButton.GetComponent<Image>().enabled = false;
            interactUIButton.GetComponent<Button>().enabled = false;
        }
    }

    public void Start() {
        interactUIButton.GetComponent<Image>().enabled = false;
        interactUIButton.GetComponent<Button>().enabled = false;
    }

    public void OnTriggerEnter(Collider collider) {
        if (collider.tag.Equals("Interactable")) {
            interactUIButton.GetComponent<Image>().enabled = true;
            interactUIButton.GetComponent<Button>().enabled = true;
            CollidedObject = collider.gameObject;
        }

    }

    public void OnTriggerExit(Collider collider) {

        if (collider.tag.Equals("Interactable")) {
            interactUIButton.GetComponent<Image>().enabled = false;
            interactUIButton.GetComponent<Button>().enabled = false;
            CollidedObject = null;
        }

    }
}
