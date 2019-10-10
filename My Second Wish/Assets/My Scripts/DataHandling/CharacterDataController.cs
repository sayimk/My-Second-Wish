using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDataController : MonoBehaviour {

    private List<List<string>> combatLogs = new List<List<string>>();

    //constant Hero References
    public const string Hero_Akira = "Akira";
    public const string Hero_Tama = "Tama";
    public const string Hero_Nero = "Nero";
    public const string Hero_Elliot = "Elliot";

    //player character data
    private HeroRecordClass playerAkiraRecord;
    private HeroRecordClass playerTamaRecord;
    private HeroRecordClass playerNeroRecord;
    private HeroRecordClass playerElliotRecord;

    //useful const prefixes
    public const string battle_Prefab_Prefit = "Battle_";

    //constant Enemy References
    public const string Enemy_Sphere = "Sphere";

    //enemy character data
    private EnemyRecordClass enemySphere;

    public void Start() {

        //initialisation of Akira
        playerAkiraRecord = new HeroRecordClass(gameObject);
        playerAkiraRecord.setCharacterLevel(10);
        playerAkiraRecord.setCharacterName("Akira");
        playerAkiraRecord.setMaxHP(600);
        playerAkiraRecord.setCurrentHP(600);
        playerAkiraRecord.setCurrentMP(250);
        playerAkiraRecord.setMaxMP(250);
        playerAkiraRecord.setStrengthStat(100);
        playerAkiraRecord.setMagicStat(90);
        playerAkiraRecord.setSpeedStat(110);
        playerAkiraRecord.addSkill("Fatal Return");
        playerAkiraRecord.addSkill("Battle-Healing Stage 1");
        playerAkiraRecord.addMagic("Lightning Spark");

        Debug.Log("CDC = " + playerAkiraRecord.getAllAvailableMagic().Count);

        //initialisation of Sphere
        enemySphere = new EnemyRecordClass();
        enemySphere.characterDataController = gameObject;
        enemySphere.setCharacterLevel(8);
        enemySphere.setCharacterName("Evil Sphere");
        enemySphere.setCurrentHP(450);
        enemySphere.setMaxHP(450);
        enemySphere.setCurrentMP(200);
        enemySphere.setMaxMP(200);
        enemySphere.setStrengthStat(20);
        enemySphere.setMagicStat(15);
        enemySphere.setSpeedStat(50);
        enemySphere.addSkill("Heavy Attack");
        enemySphere.addSkill("Mana Charge");
        enemySphere.addMagic("Electric Shock");
        enemySphere.addMagic("High-Charged Bolt");
        //enemySphere.addMagic("Fast Heal");
        //enemySphere.addSkill("Health Charge");


    }


    //this method will fetch the enemy record class based on a passed in string
    public RecordClass fetchRecordClassViaString(string CDC_ConstName) {

        switch (CDC_ConstName) {

            //hero Records
            case Hero_Akira: {
                    return playerAkiraRecord;
                }

            case Hero_Tama: return playerTamaRecord;

            case Hero_Nero: return playerNeroRecord;

            case Hero_Elliot: return playerElliotRecord;
            
            //enemy records
            case Enemy_Sphere: {

                    enemySphere.setCurrentHP(enemySphere.getMaxHP());
                    enemySphere.setCurrentMP(enemySphere.getMaxMP());
                    Debug.Log("Loading enemy, current hp = " + enemySphere.getcurrentHP());
                    return enemySphere;
                }

            //add additional enemy cases when new enemy
            //case -name- : return -enemy record-
            default: return null;
         
        }
    }

    public void addCombatLog(List<string> combatLog) {
        combatLogs.Add(combatLog);
    }

    public void addExperiencePoints(string CDCName, int exp) {

        switch (CDCName) {

            //hero Records
            case Hero_Akira: {
                    playerAkiraRecord.addExperience(exp);
                }
                break;

            case Hero_Tama: playerTamaRecord.addExperience(exp);
                break;

            case Hero_Nero: playerNeroRecord.addExperience(exp);
                break;

            case Hero_Elliot: playerElliotRecord.addExperience(exp);
                break;

            default:
                break;

        }
    }
}
