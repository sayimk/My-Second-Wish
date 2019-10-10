using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngagementControl : MonoBehaviour {

    public GameObject PlayerViewCamera;
    public GameObject UIObject;
    public GameObject CombatController;

    public void OnTriggerEnter(Collider other) {
        if (other.tag.Equals("Enemy")) {

            //disable player movement
            gameObject.GetComponent<MovementControl>().disableMovement();
            Debug.Log("disabled movement");

            //fetch all available characters from a db object, get the enemy instances and data from enemy
            List<bool> ATNE_Availablility = other.gameObject.GetComponent<EnemyBattleInstanceInformation>().getAkira_Tama_Nero_Elliot_Availablity();
            List<string> enemyNames = other.gameObject.GetComponent<EnemyBattleInstanceInformation>().getEnemyCDCNames();
            List<int> enemylvl = other.gameObject.GetComponent<EnemyBattleInstanceInformation>().getEnemyLevels();

            //load enemies into battle instance
            for (int i = 0; ((i < enemyNames.Count)&&(i<enemylvl.Count)); i++) {
                CombatController.GetComponent<CharacterPositionManager>().loadCharacterIntoBattlePosition(other.gameObject.GetComponent<EnemyBattleInstanceInformation>().getRegion(), CharacterPositionManager.Enemy_setCharacterType, enemyNames[i], (CharacterDataController.battle_Prefab_Prefit+enemyNames[i]), (i+1));
            }

            //load heroes into battle instance
            if (ATNE_Availablility[0]) {
                CombatController.GetComponent<CharacterPositionManager>().loadCharacterIntoBattlePosition(other.gameObject.GetComponent<EnemyBattleInstanceInformation>().getRegion(), CharacterPositionManager.Hero_setCharacterType, CharacterDataController.Hero_Akira, (CharacterDataController.battle_Prefab_Prefit+CharacterDataController.Hero_Akira), 1);
            }

            if (ATNE_Availablility[1]) {
                CombatController.GetComponent<CharacterPositionManager>().loadCharacterIntoBattlePosition(other.gameObject.GetComponent<EnemyBattleInstanceInformation>().getRegion(), CharacterPositionManager.Hero_setCharacterType, CharacterDataController.Hero_Tama, (CharacterDataController.battle_Prefab_Prefit+CharacterDataController.Hero_Tama), 2);
            }

            if (ATNE_Availablility[2]) {
                CombatController.GetComponent<CharacterPositionManager>().loadCharacterIntoBattlePosition(other.gameObject.GetComponent<EnemyBattleInstanceInformation>().getRegion(), CharacterPositionManager.Hero_setCharacterType, CharacterDataController.Hero_Nero, (CharacterDataController.battle_Prefab_Prefit+CharacterDataController.Hero_Nero), 3);
            }

            if (ATNE_Availablility[3]) {
                CombatController.GetComponent<CharacterPositionManager>().loadCharacterIntoBattlePosition(other.gameObject.GetComponent<EnemyBattleInstanceInformation>().getRegion(), CharacterPositionManager.Hero_setCharacterType, CharacterDataController.Hero_Elliot, (CharacterDataController.battle_Prefab_Prefit+CharacterDataController.Hero_Elliot), 4);
            }

            //add a "Encounter animation trigger here"

            //switching the UI to combat mode
            UIObject.GetComponent<UIController>().switchToBattleUI(CombatController.GetComponent<CharacterPositionManager>().getBattleCamera(other.gameObject.GetComponent<EnemyBattleInstanceInformation>().getRegion()));


            //destroy object and map icon
            Destroy(other.gameObject.GetComponent<EnemyBattleInstanceInformation>().getMapIcon());
            Destroy(other.gameObject);
        }
    }
}
