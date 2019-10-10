using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPositionManager : MonoBehaviour {

    //Character Database Controller
    public GameObject CharacterDataController;

    //used as a selectable parameter for load character into position
    public const string Enemy_setCharacterType = "Enemy";
    public const string Hero_setCharacterType = "Hero";

    //used to select the field in where to load the character
    public const string Forest_fieldType = "FOREST";
    public const string Lake_fieldType = "LAKE";
    //add other fieldtypes as other battle instances are created

    //Instance Battle Fields
	public GameObject forestBattleCharactersParentObject;
    public GameObject lakeBattleCharactersParentObject;
    //add additional parent object for other battlefields

    public GameObject forestBattleCamera;
    public GameObject lakeBattleCamera;

    GameObject currentFieldSelection = null;

    //this method will load a requested character instance from resources and into a specific battlefield, role and positon of the role
    public void loadCharacterIntoBattlePosition(string fieldType, string setCharacterType, string CDCNameId, string prefabIdentifierName, int position) {

        //selecting the correct battlefield
        switch (fieldType) {

            case "FOREST":
                currentFieldSelection = forestBattleCharactersParentObject;
                break;

            case "LAKE":
                currentFieldSelection = lakeBattleCharactersParentObject;
                break;

            //add other cases for other battle instances

            default:
                break;
        }

        //selecting the correct field position
        string tempPos = setCharacterType + position;
        Instantiate(Resources.Load(("CharacterPrefabs/" + prefabIdentifierName)), currentFieldSelection.transform.Find(tempPos));
        Debug.Log("Loaded " + prefabIdentifierName);
        Debug.Log(setCharacterType);
        //adding to combatController

        if (setCharacterType.Equals(Hero_setCharacterType)) {
            
            currentFieldSelection.transform.Find(tempPos).gameObject.GetComponent<CharacterInstanceDataContainer>().loadCharacterRecordIntoInstance(CharacterDataController.GetComponent<CharacterDataController>().fetchRecordClassViaString(CDCNameId), CDCNameId);
            currentFieldSelection.transform.Find(tempPos).gameObject.GetComponent<CharacterInstanceDataContainer>().addCombatControllerAndCharacterDatabaseObject(gameObject, CharacterDataController);
            gameObject.GetComponent<CombatController>().addInstanceHeroesDataForUI(currentFieldSelection.transform.Find(tempPos).gameObject);
            Debug.Log("loaded hero Data intoInstance, CPM");

        } else {
            currentFieldSelection.transform.Find(tempPos).gameObject.GetComponent<CombatAI>().combatController = gameObject.GetComponent<CombatController>().gameObject;
            currentFieldSelection.transform.Find(tempPos).gameObject.GetComponent<CombatAI>().characterDataController = CharacterDataController;
            currentFieldSelection.transform.Find(tempPos).gameObject.GetComponent<CharacterInstanceDataContainer>().loadCharacterRecordIntoInstance(CharacterDataController.GetComponent<CharacterDataController>().fetchRecordClassViaString(CDCNameId), CDCNameId);
            currentFieldSelection.transform.Find(tempPos).gameObject.GetComponent<CharacterInstanceDataContainer>().addCombatControllerAndCharacterDatabaseObject(gameObject, CharacterDataController);
            gameObject.GetComponent<CombatController>().addInstanceEnemyDataForUI(currentFieldSelection.transform.Find(tempPos).gameObject);

            Debug.Log("loaded enemy Data intoInstance, CPM");

        }



    }

    public void unloadCharactersFromField() {

        for (int i = 1; i < 5; i++) {
            if (currentFieldSelection.transform.Find("Hero" + i).childCount>0) {
                Destroy(currentFieldSelection.transform.Find("Hero" + i).GetChild(0).gameObject);
            }
        }

    }

    public GameObject getBattleCamera(string region) {

        switch (region) {

            case "FOREST":
                return forestBattleCamera;

            case "LAKE":
                return lakeBattleCamera;
            //add other cases for other battle instances


        }

        return null;
    }





}
