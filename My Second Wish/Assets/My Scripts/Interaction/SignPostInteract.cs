using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignPostInteract : MonoBehaviour, InteractionInterface {
    public GameObject DialogueDisplayPrefab;

    public void interact() {
        DialogueSystem.instance.displayDialogueWindow(DialogueDisplayPrefab, "test");
    }
}
