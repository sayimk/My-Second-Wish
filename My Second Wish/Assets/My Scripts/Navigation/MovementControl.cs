using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class MovementControl : MonoBehaviour {

    public bool movementEnabled = true;
    public GameObject analogStick;
    // Update is called once per frame
    void Update() {

        //used to control the movement blend tree using 2 parameters floats obtained from the cross platform input manager
        if (movementEnabled) {
            gameObject.GetComponent<Animator>().SetFloat("VerticalAxis", CrossPlatformInputManager.GetAxis("Vertical"));
            gameObject.GetComponent<Animator>().SetFloat("HorizontalAxis", CrossPlatformInputManager.GetAxis("Horizontal"));
        }
    }

    public void disableMovement() {
        movementEnabled = false;
        analogStick.GetComponent<Canvas>().enabled = false;
        gameObject.GetComponent<Animator>().SetTrigger("stop");
    }

    public void enableMovement() {
        movementEnabled = true;
        analogStick.GetComponent<Canvas>().enabled = true;
        gameObject.GetComponent<Animator>().SetTrigger("resume");

    }
}
