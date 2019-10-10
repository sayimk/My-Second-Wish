using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//this class is used to control the 'flow' of the battle, it will handle changing the UI like changing HP bar to current turn, and turn order
public class CombatController : MonoBehaviour {

    public GameObject Battle_UI;
    public GameObject characterDatabaseController;
    public GameObject Overall_UI;
    public GameObject EnemyLog;

    private List<GameObject> heroes = new List<GameObject>();
    private List<GameObject> enemies = new List<GameObject>();
    private List<int> deadEnemies = new List<int>();
    private int totalInitialEnemies = 0;

    //constants for keywords
    public const string Attack_CombatController_Keyword = "attack";
    public const string Skill_CombatController_Keyword = "skill";
    public const string Magic_CombatController_Keyword = "magic";
    public const string Item_CombatController_Keyword = "item";

    //a turn queue system, incase multiple turns trigger
    private List<string> turnQueue = new List<string>();
    private bool turnInUse = false;
    //used for logging gamestate

    //current turn character
    private int currentPlayerInt = -1;

    private int currentEnemyInt = -1;

    //this in conjunction with the keyword constants can hold the current state of the combat motion
    private string playerCurrentlySelectedActionKeyword;

    //used to hold magic or skill name ids while target enemy is being selected
    private string actionSpecificReference;

    //current target
    private int currentSelectedTarget;
    private string targetType = "";

    //calculated damage/heal amount
    private int calculatedValue = 0;

    //selected skills or magic
    private SkillDataItem selectedSkill;

    private MagicDataItem selectedMagic;

    //-1 = none, 0 = attack, 1 = restore, 2 = buffs
    private int skillOrMagicType = -1;

    //CombatLog
    List<string> combatLog = new List<string>();

    //end of battle experience gain
    int experience = 10;

    //TEMPORARY QUEST LOGGING
    public GameObject menuUI;

    //used to set the current player turn
    public void selectCurrentPlayersTurn(int playerNo) {

        Debug.Log("Recieved Turn player");
        Debug.Log("is turn in use = " + turnInUse + " , queue length = " + turnQueue.Count);

        if (!turnInUse) {
            turnInUse = true;
            heroes[playerNo].GetComponent<CharacterInstanceDataContainer>().startTurnInitialChecks();
            Battle_UI.GetComponent<Battle_UI_Controller>().pauseAllTurnIndicators();
            Battle_UI.GetComponent<Battle_UI_Controller>().showPlayerUI();
            Battle_UI.GetComponent<Battle_UI_Controller>().updateHealthAndMPGauge(heroes[playerNo]);
            currentPlayerInt = playerNo;
        } else {
            turnQueue.Add("h," + playerNo);
        }


    }

    //same as player, but used for the enemy
    public void selectCurrentEnemyTurn(int enemyNo) {

        Debug.Log("Enemy Current HP = " + enemies[enemyNo].GetComponent<CharacterInstanceDataContainer>().getDataRecord().getcurrentHP());
        if (!turnInUse) {
              turnInUse = true;
              Battle_UI.GetComponent<Battle_UI_Controller>().pauseAllTurnIndicators();
              Battle_UI.GetComponent<Battle_UI_Controller>().updateHealthAndMPGauge(enemies[enemyNo]);
              Battle_UI.GetComponent<Animator>().SetTrigger("showEnemy");
              currentEnemyInt = enemyNo;
              Debug.Log("Enemy Turn");

              //start turn
              enemies[enemyNo].GetComponent<CharacterInstanceDataContainer>().startTurnInitialChecks();

              //Call AI Calculation method
              enemies[enemyNo].transform.GetComponent<CombatAI>().calculateTurnDecision();

              //call AI decision fetch Methods
              string[] splitTypeAndInt = enemies[enemyNo].transform.GetComponent<CombatAI>().getTarget().Split(',');
              string[] splitCombatDecision = enemies[enemyNo].transform.GetComponent<CombatAI>().getCombatDecision().Split(',');
              selectAttackType(splitCombatDecision[0], splitCombatDecision[1]);
              EnemyLog.GetComponent<Text>().text = enemies[currentEnemyInt].GetComponent<CharacterInstanceDataContainer>().getDataRecord().getCharacterName() + " used " + splitCombatDecision[1];
            Debug.Log("target = " + splitTypeAndInt[0]);
              ExecutePlayerMove(Convert.ToInt32(splitTypeAndInt[0]), splitTypeAndInt[1]);

          } else {
              turnQueue.Add("e," + enemyNo);
          }
    }

    public void selectAttackType(string CombatController_Keyword, string skillOrMagicName) {
        playerCurrentlySelectedActionKeyword = CombatController_Keyword;
        actionSpecificReference = skillOrMagicName;
    }

    public void selectAttackType(string CombatController_Keyword) {
        playerCurrentlySelectedActionKeyword = CombatController_Keyword;
        actionSpecificReference = "";
    }

    public void ExecutePlayerMove(int enemyNo, string selectedTargetType) {

        //execute associated move on target
        switch (playerCurrentlySelectedActionKeyword) {

            case "attack": attackCharacterMove();
                break;

            case "skill": useSkillOnCharacterMove();
                break;

            case "magic": useMagicOnCharacterMove();
                break;

            case "item": useItemOnCharacterMove();
                break;

            default: endTurn();
                break;
        }

        currentSelectedTarget = enemyNo;
        targetType = selectedTargetType;
    }

    public void attackCharacterMove() {
        heroes[currentPlayerInt].GetComponent<CharacterInstanceDataContainer>().startAttackAnimation();
        //call the animation, and set parameters for damage to be inflicted on characterInstanceDataContainer after animationEvent is called on addDamageToCharacter()
        calculatedValue = heroes[currentPlayerInt].GetComponent<CharacterInstanceDataContainer>().getDataRecord().getStrengthStat();
        skillOrMagicType = 0;
    }

    public void useSkillOnCharacterMove() {

        //use case on skill type for different skill effect
        selectedSkill = characterDatabaseController.GetComponent<SkillsDataController>().getSkillData(actionSpecificReference);

        switch (selectedSkill.skillType) {

            case SkillDataItem.skillType_Attack: {
                    float temp;

                    //this needs to be changed to use the proper way of calculating power
                    if (currentPlayerInt!=-1) {
                        temp = 120f * selectedSkill.multiplierPower;
                    } else {
                        temp = 50f * selectedSkill.multiplierPower;
                    }
                    //

                    calculatedValue = (int)Math.Round(temp);
                    skillOrMagicType = 0;
            }
                break;

            case SkillDataItem.skillType_AttackSupport: {
                    skillOrMagicType = 2;
            }
                break;

            case SkillDataItem.skillType_Restoration: {
                    skillOrMagicType = 1;
                    float temp = 150f * selectedSkill.multiplierPower;
                    calculatedValue = (int)Math.Round(temp);

                }
                break;
            case SkillDataItem.skillType_Support:  {
                    skillOrMagicType = 2; 

                    //support type skills not implemented yet
            }
             break;

            default:
                break;
        }

        if (currentEnemyInt > -1) {
            enemies[currentEnemyInt].GetComponent<CharacterInstanceDataContainer>().startAttackAnimation();
            enemies[currentEnemyInt].GetComponent<CharacterInstanceDataContainer>().addSkillToCooldownList(selectedSkill.skillName);
        } else if (currentPlayerInt > -1) {
            heroes[currentPlayerInt].GetComponent<CharacterInstanceDataContainer>().startAttackAnimation();
            heroes[currentPlayerInt].GetComponent<CharacterInstanceDataContainer>().addSkillToCooldownList(selectedSkill.skillName);

        }
    }

    public void useMagicOnCharacterMove() {

        selectedMagic = characterDatabaseController.GetComponent<MagicDataController>().getMagicData(actionSpecificReference);

        switch (selectedMagic.magicType) {

            case MagicDataItem.magicType_Attack: {

                    if (currentPlayerInt!=-1) {
                        skillOrMagicType = 0;
                        float temp = 120f * selectedMagic.multiplierPower;
                        heroes[currentPlayerInt].GetComponent<CharacterInstanceDataContainer>().reduceMP(selectedMagic.MPCost);
                        calculatedValue = (int)Math.Round(temp);
                        heroes[currentPlayerInt].GetComponent<CharacterInstanceDataContainer>().startAttackAnimation();
                    } else if (currentEnemyInt != -1) {

                        skillOrMagicType = 0;
                        float temp = 50f * selectedMagic.multiplierPower;
                        enemies[currentEnemyInt].GetComponent<CharacterInstanceDataContainer>().reduceMP(selectedMagic.MPCost);
                        calculatedValue = (int)Math.Round(temp);
                        enemies[currentEnemyInt].GetComponent<CharacterInstanceDataContainer>().startAttackAnimation();

                    }

                }
                break;

            case MagicDataItem.magicType_AttackSupport: {

            }
                break;

            case MagicDataItem.magicType_Restoration: {

                    if (currentPlayerInt!=-1) {

                        skillOrMagicType = 1;
                        float temp = 100f * selectedMagic.multiplierPower;
                        calculatedValue = (int)Math.Round(temp);
                        heroes[currentPlayerInt].GetComponent<CharacterInstanceDataContainer>().reduceMP(selectedMagic.MPCost);
                        heroes[currentPlayerInt].GetComponent<CharacterInstanceDataContainer>().startAttackAnimation();


                    } else if (currentEnemyInt != -1) {


                        skillOrMagicType = 1;
                        float temp = 100f * selectedMagic.multiplierPower;
                        calculatedValue = (int)Math.Round(temp);
                        enemies[currentEnemyInt].GetComponent<CharacterInstanceDataContainer>().reduceMP(selectedMagic.MPCost);
                        enemies[currentEnemyInt].GetComponent<CharacterInstanceDataContainer>().startAttackAnimation();
                    }

                }
                break;

            case MagicDataItem.magicType_Support: {

                    //support type magic not implemented yet
            }

                break;

            default:
                break;
        }





    }

    public void useItemOnCharacterMove() {

    }

    public void addEffectToCharacter() {

        //calculate damage and add to calculatedDamage then call
        if (skillOrMagicType ==0) {

            if (currentPlayerInt!=-1) {
                enemies[currentSelectedTarget].GetComponent<CharacterInstanceDataContainer>().addInflictedDamage(calculatedValue);
                Debug.Log("Damage = " + calculatedValue);
                Debug.Log(enemies[currentSelectedTarget].GetComponent<CharacterInstanceDataContainer>().getDataRecord().getcurrentHP() + "/" + enemies[currentSelectedTarget].GetComponent<CharacterInstanceDataContainer>().getDataRecord().getMaxHP());
            } else if (currentEnemyInt !=-1) {
                heroes[currentSelectedTarget].GetComponent<CharacterInstanceDataContainer>().addInflictedDamage(calculatedValue);
                Debug.Log("Damage = " + calculatedValue);
                Debug.Log(heroes[currentSelectedTarget].GetComponent<CharacterInstanceDataContainer>().getDataRecord().getcurrentHP() + "/" + enemies[currentSelectedTarget].GetComponent<CharacterInstanceDataContainer>().getDataRecord().getMaxHP());
            }


            if ((enemies[currentSelectedTarget].GetComponent<CharacterInstanceDataContainer>().CheckIfCharacterIsDead())&& currentPlayerInt!=-1) {
                deadEnemies.Add(currentSelectedTarget);
                enemies[currentSelectedTarget].GetComponent<CharacterInstanceDataContainer>().killCharacter();
            }


        } else if (skillOrMagicType == 1) {
            string temp;

            if (selectedSkill!=null) {
                temp = selectedSkill.statEffected;
            }else {
                temp = selectedMagic.statEffected;
            }

            if (currentEnemyInt >-1) {
                enemies[currentSelectedTarget].GetComponent<CharacterInstanceDataContainer>().useRestoreAbility(temp, calculatedValue);
            } else if (currentPlayerInt >-1) {
                heroes[currentSelectedTarget].GetComponent<CharacterInstanceDataContainer>().useRestoreAbility(temp, calculatedValue);
            }

        } else if(skillOrMagicType == 2) {

        }
  

        endTurn();
    }

    public void endTurn() {

        //add turn information to combatLog

        string turnData = "";

        //storing current character information
        if (currentPlayerInt != -1) {
            turnData = turnData + "p," + currentPlayerInt+",";
        } else if (currentEnemyInt != -1) {
            turnData = turnData + "e," + currentEnemyInt + ",";
        } else {
            turnData = turnData + "n,-1,";
        }

        //storing attackType;
        turnData = turnData + playerCurrentlySelectedActionKeyword+",";
        
        //storing actionSpecific
        if (!actionSpecificReference.Equals("")) {
            turnData = turnData + actionSpecificReference+",";
        } else {
            turnData = turnData + "-,";
        }

        //storing target int
        if (targetType.Equals(Battle_UI_Controller.Enemy_Target_For_Selector)) {
            turnData = turnData + "e," + currentSelectedTarget+",";
        }else if (targetType.Equals(Battle_UI_Controller.Ally_Target_For_Selector)) {
            turnData = turnData + "p," + currentSelectedTarget+",";
        } else {
            turnData = turnData + "-," + -1+",";
        }

        //storing damage/heal value
        turnData = turnData + calculatedValue;

        combatLog.Add(turnData);

        //reset all parameters
        playerCurrentlySelectedActionKeyword = "";
        actionSpecificReference = "";
        calculatedValue = 0;
        currentSelectedTarget = 0;
        selectedMagic = null;
        selectedSkill = null;
        currentEnemyInt = -1;
        currentPlayerInt = -1;
        skillOrMagicType = -1;
        Battle_UI.GetComponent<Battle_UI_Controller>().hidePlayerUI();
        Debug.Log("----END OF TURN----");

        if (totalInitialEnemies==deadEnemies.Count) {
            endCombat();
        } else {
            moveOnToNextTurn();
        }
    }

    public void moveOnToNextTurn() {

        if (turnQueue.Count==0) {
            turnInUse = false;
            Battle_UI.GetComponent<Battle_UI_Controller>().resumeAllTurnIndicators();
        } else {

            string[] newTurn = turnQueue[0].Split(',');
            turnQueue.RemoveAt(0);
            turnInUse = false;

            if (newTurn[0].Equals("h")) {
                selectCurrentPlayersTurn(Convert.ToInt32(newTurn[1]));
            } else if (newTurn[0].Equals("e")) {
                selectCurrentEnemyTurn(Convert.ToInt32(newTurn[1]));
            } else {
                turnInUse = false;
                Battle_UI.GetComponent<Battle_UI_Controller>().resumeAllTurnIndicators();
            }
        }
    }

    public void addInstanceHeroesDataForUI(GameObject hero) {

        heroes.Add(hero);
        Debug.Log("Added hero");
    }

    public void addInstanceEnemyDataForUI(GameObject enemy) {

        enemies.Add(enemy);
        totalInitialEnemies = totalInitialEnemies + 1;
        Debug.Log("Added enemy");

    }

    public List<string> getAllHeroNamesInOrder() {

        List<string> temp = new List<string>();

        for (int i = 0; i < heroes.Count; i++) {
            temp.Add(heroes[i].GetComponent<CharacterInstanceDataContainer>().getDataRecord().getCharacterName());
        }

    return temp;
    }

    public List<string> getAllEnemyNamesInOrder() {

        List<string> temp = new List<string>();

        for (int i = 0; i < heroes.Count; i++) {
            temp.Add(enemies[i].GetComponent<CharacterInstanceDataContainer>().getDataRecord().getCharacterName());
        }

        return temp;
    }

    public List<string> getCurrentPlayersSkills() {
        return heroes[currentPlayerInt].GetComponent<CharacterInstanceDataContainer>().getSkills();
    }

    public List<string> getCurrentPlayersMagic() {
        return heroes[currentPlayerInt].GetComponent<CharacterInstanceDataContainer>().getMagic();

    }

    public GameObject getCurrentHeroObject() {
        return heroes[currentPlayerInt];
    }

    public string getTargetType(string attackType, string skillOrMagic) {

        if (attackType.Equals(Skill_CombatController_Keyword)) {

            return characterDatabaseController.GetComponent<SkillsDataController>().getTargetType(skillOrMagic);

        } else if (attackType.Equals(Magic_CombatController_Keyword)) {

            return characterDatabaseController.GetComponent<MagicDataController>().getTargetType(skillOrMagic);

        } else {
            return "";
        }
    }

    public List<GameObject> getPlayerOpponentRecords() {
        return heroes;
    }

    public List<GameObject> getEnemyAlliesRecords() {
        return enemies;
    }

    public List<string> getCombatLog() {
        return combatLog;
    }

    public int getCurrentPlayerEnemyIndex() {
        return currentEnemyInt;
    }

    public void setExperiencePointGain(int expGain) {

    }

    public void endCombat() {
        EnemyLog.GetComponent<Text>().text = "";

        for (int i = 0; i < heroes.Count; i++) {
            characterDatabaseController.GetComponent<CharacterDataController>().addExperiencePoints(heroes[i].GetComponent<CharacterInstanceDataContainer>().CDCName, experience);
        }

        characterDatabaseController.GetComponent<CharacterDataController>().addCombatLog(combatLog);
        combatLog.Clear();
        //reset Turn
        turnQueue.Clear();
        turnInUse = false;

        //reset lists
        heroes.Clear();
        enemies.Clear();
        deadEnemies.Clear();
        totalInitialEnemies = 0;

        //call end instance animation
        Overall_UI.GetComponent<UIController>().switchToControlUI();
        gameObject.GetComponent<CharacterPositionManager>().unloadCharactersFromField();

        //temporary
        menuUI.GetComponent<MainMenuController>().incrementKillCount();
    }

}
