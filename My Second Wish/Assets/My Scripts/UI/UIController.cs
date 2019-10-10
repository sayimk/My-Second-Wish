using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class UIController : MonoBehaviour {

    public GameObject controlUI;
    public GameObject battleUI;
    public GameObject turnOrderIndicator;
    public GameObject mainCamera;
    public GameObject currentBattleCamera;
    public GameObject explorationModePlayerObject;
    public GameObject explorationAudio;
    public GameObject combatAudio;


    public void switchToBattleUI(GameObject battleCamera) {
        explorationAudio.GetComponent<AudioSource>().enabled = false;
        combatAudio.GetComponent<AudioSource>().enabled = true;
        currentBattleCamera = battleCamera;
        mainCamera.GetComponent<MainCameraDeactivate>().setBattleCamera(currentBattleCamera);
        mainCamera.GetComponent<Animator>().SetTrigger("EnterCombat");

        controlUI.GetComponent<Animator>().SetTrigger("hide");
        turnOrderIndicator.GetComponent<Canvas>().enabled = true;
        battleUI.GetComponent<Battle_UI_Controller>().resumeAllTurnIndicators();
    }

    public void switchToControlUI() {
        combatAudio.GetComponent<AudioSource>().enabled = false;
        explorationAudio.GetComponent<AudioSource>().enabled = true;
        turnOrderIndicator.GetComponent<Canvas>().enabled = false;
        //battleUI.GetComponent<Battle_UI_Controller>().disableAllTurnOrders();
        battleUI.GetComponent<Battle_UI_Controller>().endAllTurnIndicators();
        currentBattleCamera.GetComponent<Animator>().SetTrigger("exit");
        mainCamera.GetComponent<Animator>().SetTrigger("ExitCombat");
        controlUI.GetComponent<Animator>().SetTrigger("display");
        explorationModePlayerObject.GetComponent<MovementControl>().enableMovement();

    }
}
