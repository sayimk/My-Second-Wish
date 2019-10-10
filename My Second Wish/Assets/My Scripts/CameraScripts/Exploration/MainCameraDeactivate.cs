using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraDeactivate : MonoBehaviour {

    public GameObject battleCamera;

    public void deactiveCamera() {

        //edit in future to fetch a variable which will define which battlefield to use
        battleCamera.GetComponent<Animator>().SetTrigger("SingleCombat");
    }

    public void setBattleCamera(GameObject cameraObject){
        battleCamera = cameraObject;
    }
}
