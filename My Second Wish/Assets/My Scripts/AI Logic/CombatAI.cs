using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CombatAI : MonoBehaviour {

    public GameObject combatController;
    public GameObject characterDataController;

    //combat AI Mental States
    //aggressive = use most powerful skills and magic
    private const string CombatAI_Aggressive = "aggressive";

    //balanced = use mixture of efficient magic and skills for magic try power*100 -mp cost
    //maintain debuffs on player
    private const string CombatAI_Balanced = "balanced";

    //focus on most efficient magic and skills while focusing on buff and hp regen and mp regen
    private const string CombatAI_Defensive = "defensive";

    //holds information about the current characters, update these everytime the 3 main methods are called
    private List<GameObject> otherAllies = new List<GameObject>();
    private List<GameObject> EnemyPlayers = new List<GameObject>();

    //hold information about decisions
    private string combatDecisionType = "";
    private string target = "";
    private string specificAction = "";

    //logging current turn available options
    //restore magic/skills
    List<string> allHPRestoreMagic;
    List<string> allHPRestoreSkills;
    List<string> allMPRestoreSkills;

    //offensive skills/magic
    List<string> sortedMagicList;
    List<string> sortedSkillList;

    //current AI_State
    private string currentAIState = "";

    //current stats
    float MPPercentage;
    float healthPercentage;
    int characterIndex;

    //current combat log
    private List<string> currentCombatLog;

    //-------- 5 main methods--------------------------
    //this method is used to select one of the targets, e,1 = player character 1
    public void calculateTurnDecision() {

        //check this characters health point status
        float currentHP = gameObject.GetComponent<CharacterInstanceDataContainer>().getDataRecord().getcurrentHP();
        float maxHP = gameObject.GetComponent<CharacterInstanceDataContainer>().getDataRecord().getMaxHP();
        healthPercentage = ((currentHP / maxHP) * 100);
        Debug.Log("AI: HealthPercentage = " + healthPercentage+"%");

        //check this characters magic point status
        float currentMP = gameObject.GetComponent<CharacterInstanceDataContainer>().getDataRecord().getcurrentMP();
        float maxMP = gameObject.GetComponent<CharacterInstanceDataContainer>().getDataRecord().getMaxMP();
        MPPercentage = ((currentMP / maxMP) * 100);
        Debug.Log("AI: Magic Percentage = " + MPPercentage + "%");

        //fetching data from Combat Controller
        //combatLog
        currentCombatLog = combatController.GetComponent<CombatController>().getCombatLog();


        for (int i = 0; i < currentCombatLog.Count; i++) {
            Debug.Log("AI: Combat Log #" + i + " = " + currentCombatLog[i]);
        }

        //target Records
        EnemyPlayers = combatController.GetComponent<CombatController>().getPlayerOpponentRecords();
        Debug.Log("AI: Number of Player Enemies = " + EnemyPlayers.Count);

        //any Ally targets
        otherAllies = combatController.GetComponent<CombatController>().getEnemyAlliesRecords();
        
        Debug.Log("AI: Number of Allies = " + otherAllies.Count);

        //note restore HP magic
        allHPRestoreMagic = getAllHPRestoreMagic();
        //Debug.Log("AI: Number of HP Restore Magic Available = " + allHPRestoreMagic.Count);
        //note restore HP skills
        allHPRestoreSkills = getAllRestoreSkillsViaStatEffectedType(SkillDataItem.statEffected_HP);
        Debug.Log("AI: Number of HP Restore Skills Available ="+allHPRestoreSkills.Count);

        //note restore MP Skills
        allMPRestoreSkills = getAllRestoreSkillsViaStatEffectedType(SkillDataItem.statEffected_MP);
        //Debug.Log("AI: Number of MP Restore Skills Available =" + allHPRestoreSkills.Count);

        characterIndex = combatController.GetComponent<CombatController>().getCurrentPlayerEnemyIndex();


        //decision logic
        //if no current AI State randomly choose one
        if (currentAIState=="") {
            int choice = 0;//Random.Range(0, 3);

            switch (choice) {

                case 0: currentAIState = CombatAI_Aggressive;
                    break;
                case 1: currentAIState = CombatAI_Balanced;
                    break;
                case 2: currentAIState = CombatAI_Defensive;
                    break;
            }
        }

        Debug.Log("AI Current State " + currentAIState);

        //different cases to switch the current ai state based on pieces of information

        //check if in a defensive state and if hp is greater than to, then switch to a balanced or aggressive state
        if (healthPercentage>50 && currentAIState.Equals(CombatAI_Defensive)) {
            int randomDecision = Random.Range(0, 2);

            switch (randomDecision) {

                case 0:
                    currentAIState = CombatAI_Balanced;
                    break;

                case 1:
                    currentAIState = CombatAI_Aggressive;
                    break;

            }

            Debug.Log("AI: HP Greater than 50, switching to " + currentAIState);
        }

        //switch to defensive when between 50 and 25 to try and restore hp or mp
        if ((healthPercentage<=50) && (healthPercentage>25)) {

            currentAIState = CombatAI_Defensive;

            Debug.Log("AI: HP Between 50 and 25, switching to " + currentAIState);
        } else if ((healthPercentage <= 25) && (healthPercentage > 0)) {

            //either stay in defensive or fight back aggressively like a cornered animal

            int randomDecision = Random.Range(0, 2);

            switch (randomDecision) {

                case 0: currentAIState = CombatAI_Aggressive;
                    break;

                case 1: currentAIState = CombatAI_Defensive;
                    break;

            }

            Debug.Log("AI: HP between 25 and 1, switching to " + currentAIState);
        }

        //to try and restore mp
        if (MPPercentage<25) {
            Debug.Log("Low MP, trying to restore");
            currentAIState = CombatAI_Defensive;
        }
        

        switch (currentAIState) {

            case CombatAI_Aggressive: {
                generateDecision_Aggressive();
            }
                break;

            case CombatAI_Balanced: {
                generateDecision_Balanced();
            }
                break;

            case CombatAI_Defensive: {
                    generateDecision_Defensive();
            }
                break;

            default:
                break;
        }

        Debug.Log("AI: State "+currentAIState);
        Debug.Log("AI: Combat Type decision = " + combatDecisionType);
        Debug.Log("AI: Specific Action = " + specificAction);


    }
    public string getTarget() {
        //output in format targetInt,targetType using BattleUI const
        //Battle_UI_Controller.Ally_Target_For_Selector = player
        //Battle_UI_Controller.Enemy_Target_For_Selector = enemies
        string output=target.Trim()+","+ Battle_UI_Controller.Ally_Target_For_Selector;

       


        return output;
    }

    //this is used to select the AttackType used, Attack, Skill or Magic
    public string getCombatDecision() {
        //in format "attackType,Skill/MagicName"
        string output = combatDecisionType+","+specificAction;

        return output;
    }

    //end of 5 main methods-------------------------

    //use most powerful skills/magic
    public void generateDecision_Aggressive() {

        Debug.Log("Ranking magic according to power");
        sortedMagicList = rankMagicAvailable(CombatAI_Aggressive);
        sortedSkillList = sortSkillsViaPower();

        if ((sortedMagicList.Count>0)&&(sortedSkillList.Count>0)) {
            if ((characterDataController.GetComponent<MagicDataController>().getMagicData(sortedMagicList[0]).multiplierPower)>(characterDataController.GetComponent<SkillsDataController>().getSkillData(sortedSkillList[0]).multiplierPower)) {

                combatDecisionType = CombatController.Magic_CombatController_Keyword;
                specificAction = sortedMagicList[0];
                generateTargetDecision(CombatAI_Aggressive);

            } else {

                combatDecisionType = CombatController.Skill_CombatController_Keyword;
                specificAction = sortedSkillList[0];
                generateTargetDecision(CombatAI_Aggressive);

            }
        } else if (sortedMagicList.Count > 0) {

            combatDecisionType = CombatController.Magic_CombatController_Keyword;
            specificAction = sortedMagicList[0];
            generateTargetDecision(CombatAI_Aggressive);

        }else if (sortedSkillList.Count > 0) {

            combatDecisionType = CombatController.Skill_CombatController_Keyword;
            specificAction = sortedSkillList[0];
            generateTargetDecision(CombatAI_Aggressive);
        } else {

            //if in case there are NO OFFENSIVE SKILLS OR MAGIC, switch to either stall or do a normal attack

            int choice = UnityEngine.Random.Range(0, 2);

            switch (choice) {
                case 0: {

                        combatDecisionType = CombatController.Attack_CombatController_Keyword;
                        specificAction = "";
                        generateTargetDecision(CombatAI_Aggressive);

                }
                    break;

                case 1: {

                        generateDecision_Defensive();
                }
                    break;
            }
        }
    }

    //efficiently use magic and skills
    public void generateDecision_Balanced() {
        //look through and rank magic skills available based on mental state, most efficient or most powerful
        Debug.Log("Ranking magic according to efficiency");
        sortedMagicList = rankMagicAvailable(CombatAI_Balanced);
        sortedSkillList = sortSkillsViaPower();


        //check for weaknesses then use them
        //not added so, just choose more efficient skills

        if (healthPercentage < 15) {
            generateDecision_Defensive();

        } else if ((MPPercentage < 25) && (allMPRestoreSkills.Count > 0)) {

            combatDecisionType = CombatController.Skill_CombatController_Keyword;
            specificAction = allMPRestoreSkills[0];
            generateDecision_Balanced();

        } else {

            if ((sortedSkillList.Count > 0) && (sortedMagicList.Count > 0)) {

                int choice = UnityEngine.Random.Range(0, 2);
                Debug.Log("Both skill and magic available = "+ choice);

                switch (choice) {
                    case 0: {

                            combatDecisionType = CombatController.Skill_CombatController_Keyword;
                            specificAction = sortedSkillList[0];
                            generateTargetDecision(CombatAI_Balanced);

                        }
                        break;

                    case 1: {

                            combatDecisionType = CombatController.Magic_CombatController_Keyword;
                            specificAction = sortedMagicList[0];
                            generateTargetDecision(CombatAI_Balanced);

                        }
                        break;

                }
            } else if (sortedSkillList.Count > 0) {
                //if there is an available skill then use it
                Debug.Log("Only skill available");

                combatDecisionType = CombatController.Skill_CombatController_Keyword;
                specificAction = sortedSkillList[0];
                generateTargetDecision(CombatAI_Balanced);

            } else if (sortedMagicList.Count > 0) {
                Debug.Log("only magic available");

                //if there is an available magic then use it
                combatDecisionType = CombatController.Magic_CombatController_Keyword;
                specificAction = sortedMagicList[0];
                generateTargetDecision(CombatAI_Balanced);

            } else {
                generateDecision_Defensive();
            }
        }
    }

    //focus on hp or mp regeneration
    public void generateDecision_Defensive() {
        bool restorationActionSet = false;

        //look through and rank magic skills available based on mental state, most efficient or most powerful
        Debug.Log("Ranking magic according to MP Cost");
        sortedMagicList = rankMagicAvailable(CombatAI_Defensive);
        sortedSkillList = sortSkillsViaPower();

        //check if any skill based regen options are available
        int mostPowerfulHPRegenSkill = -1;
        if (allHPRestoreSkills.Count>0) {
            for (int i = 0; i < allHPRestoreSkills.Count; i++) {

                if (i==0) {
                    mostPowerfulHPRegenSkill = 0;
                }

                if(characterDataController.GetComponent<SkillsDataController>().getSkillData(allHPRestoreSkills[i]).multiplierPower> characterDataController.GetComponent<SkillsDataController>().getSkillData(allHPRestoreSkills[mostPowerfulHPRegenSkill]).multiplierPower) {
                    mostPowerfulHPRegenSkill = i;
                }
            }
        }

        Debug.Log("Searched for most powerful skill HP Regen, found at" + mostPowerfulHPRegenSkill);

        //check for any magic based restore skills
        int mostPowerfulHPRegenMagic = -1;
        if (allHPRestoreMagic.Count > 0) {
            for (int i = 0; i < allHPRestoreMagic.Count; i++) {

                if (i == 0) {
                    mostPowerfulHPRegenMagic = 0;
                }

                if (characterDataController.GetComponent<MagicDataController>().getMagicData(allHPRestoreMagic[i]).multiplierPower > characterDataController.GetComponent<MagicDataController>().getMagicData(allHPRestoreMagic[mostPowerfulHPRegenMagic]).multiplierPower) {
                    mostPowerfulHPRegenMagic = i;
                }
            }
        }

        //put in a hp and mp threshold to restore when under -----------------------------------------------------------------
        if ((mostPowerfulHPRegenSkill != -1) && healthPercentage < 75) {

            //if skill is available then use it first as it is only a cooldown and not an mp reduction
            combatDecisionType = CombatController.Skill_CombatController_Keyword;
            specificAction =  allHPRestoreSkills[mostPowerfulHPRegenSkill];
            restorationActionSet = true;
            Debug.Log("AI: Defensive state - HP restoration skill " + allHPRestoreSkills[mostPowerfulHPRegenSkill] + "selected");

        } else if ((mostPowerfulHPRegenMagic != -1) && healthPercentage < 75) {

            //if no skill available then use a magic 
            combatDecisionType = CombatController.Magic_CombatController_Keyword;
            specificAction = allHPRestoreMagic[mostPowerfulHPRegenMagic];
            restorationActionSet = true;
            Debug.Log("AI: Defensive state - HP restoration magic " + allHPRestoreMagic[mostPowerfulHPRegenMagic] + "selected");

        } 

        //this case if there are no HP Restoration skills available
        if ((!restorationActionSet) && true) {
            //try to regenerate MP if possible to allow for hp magic to be available or to use the turn for a defensive move to stall till skill becomes available, check mp threshold, then decide

            if ((allMPRestoreSkills.Count>0) && MPPercentage<75) {
                combatDecisionType = CombatController.Skill_CombatController_Keyword;
                specificAction = allMPRestoreSkills[0];
                restorationActionSet = true;
                Debug.Log("AI: Defensive state - MP restoration magic " + allMPRestoreSkills[0] + "selected");
            } else {

                //if none of this work then stall
                //if both skill and magic exist then choose at random
                if ((sortedSkillList.Count > 0) && (sortedMagicList.Count > 0)) {

                    int choice = UnityEngine.Random.Range(0, 2);

                    switch (choice) {
                        case 0: {

                                combatDecisionType = CombatController.Skill_CombatController_Keyword;
                                specificAction = sortedSkillList[0];
                                restorationActionSet = false;
                                generateTargetDecision(CombatAI_Defensive);

                            }
                            break;

                        case 1: {

                                combatDecisionType = CombatController.Magic_CombatController_Keyword;
                                specificAction = sortedMagicList[0];
                                restorationActionSet = false;
                                generateTargetDecision(CombatAI_Defensive);

                            }
                            break;

                    }
                } else if (sortedSkillList.Count > 0) {
                    //if there is an available skill then use it
                    combatDecisionType = CombatController.Skill_CombatController_Keyword;
                    specificAction = sortedSkillList[0];
                    restorationActionSet = false;
                    generateTargetDecision(CombatAI_Defensive);

                } else if (sortedMagicList.Count > 0) {
                    //if there is an available magic then use it
                    combatDecisionType = CombatController.Magic_CombatController_Keyword;
                    specificAction = sortedMagicList[0];
                    restorationActionSet = false;
                    generateTargetDecision(CombatAI_Defensive);

                } else {
                    //if none of the others exist, then just use a basic attack and hope for the best
                    combatDecisionType = CombatController.Attack_CombatController_Keyword;
                    specificAction = "";
                    restorationActionSet = false;
                    generateTargetDecision(CombatAI_Defensive);

                }
            }
        }
    }

    public void generateTargetDecision(string AIState) {

        Debug.Log("Current Target strategy = " + AIState);

        switch (AIState) {

            case CombatAI_Aggressive: {

                    Debug.Log("AI: Fetching target, " + AIState);

                    //get agro
                    List<string> playerInt = new List<string>();
                    List<int> occur = new List<int>();


                    //aggressive check who has been attacking this target the most
                    //for (int i = 0; i < currentCombatLog.Count; i++) {
                    for (int i = (currentCombatLog.Count-1); (i>-1)&&(i > (currentCombatLog.Count-11)); i--) {

                            //format example = p,0,skill,fatal return,e,0,130
                            string[] logSplit = currentCombatLog[i].Split(',');

                        if ((logSplit[0].Equals("p"))&&(logSplit[4].Equals("e")) && (Convert.ToInt32(logSplit[5])==characterIndex)) {

                            if (playerInt.Contains(logSplit[1])) {

                                int temp = playerInt.IndexOf(logSplit[1]);
                                occur[temp] = occur[temp] + 1;


                            } else {
                                playerInt.Add(logSplit[1]);
                                occur.Add(1);
                            }
                        }
                    }

                    int highestOccurs = 0;
                    for (int i = 0; i < occur.Count; i++) {
                        if (occur[highestOccurs]<occur[i]) {
                            highestOccurs = i;
                        }
                    }

                    //if there are opponents then target the opponent with the highest high occurrences
                    if (playerInt.Count>0) {
                        target = playerInt[highestOccurs];

                    } else {

                        //else if there are no attacks in the combat log then randomly select a target
                        int random_Target = UnityEngine.Random.Range(0, EnemyPlayers.Count);
                        target = random_Target.ToString();

                    }

                    Debug.Log("Target selected = " + target);
            }
                break;

            case CombatAI_Balanced: {

                    Debug.Log("AI: Fetching target, " + AIState);

                    //target opponents with the highest damage
                    List<string> playerInt = new List<string>();
                    List<int> highestDamage = new List<int>();

                    //aggressive check who has been attacking this target the most
                    //for (int i = 0; i < currentCombatLog.Count; i++) {
                    for (int i = (currentCombatLog.Count - 1); (i > -1) && (i > (currentCombatLog.Count - 11)); i--) {

                        //format example = p,0,skill,fatal return,e,0,130
                        string[] logSplit = currentCombatLog[i].Split(',');

                        if ((logSplit[0].Equals("p")) && (logSplit[4].Equals("e")) && (Convert.ToInt32(logSplit[5]) == characterIndex)) {

                            if (playerInt.Contains(logSplit[1])) {

                                if (highestDamage[playerInt.IndexOf(logSplit[1])] < Convert.ToInt32(logSplit[6])) {
                                    highestDamage[playerInt.IndexOf(logSplit[1])] = Convert.ToInt32(logSplit[6]);
                                }

                            } else {
                                playerInt.Add(logSplit[1]);
                                highestDamage.Add(Convert.ToInt32(logSplit[6]));
                            }
                           
                        }
                    }


                    int mostDamageOpponent = 0;
                    for (int i = 0; i < highestDamage.Count; i++) {
                        if (highestDamage[mostDamageOpponent] < highestDamage[i]) {
                            mostDamageOpponent = i;
                        }
                    }

                    //if there are opponents then target the opponent with the highest high occurrences
                    if (playerInt.Count > 0) {
                        target = playerInt[mostDamageOpponent];

                    } else {

                        //else if there are no attacks in the combat log then randomly select a target
                        int random_Target = UnityEngine.Random.Range(0, EnemyPlayers.Count);
                        target = random_Target.ToString();

                    }

                    Debug.Log("Target selected = " + target);


                }
                break;

            case CombatAI_Defensive: {
                    //target units with the lowest HP

                    Debug.Log("AI: Fetching target, " + AIState);

                    int lowestHP = -1;

                    for (int i = 0; i < EnemyPlayers.Count; i++) {

                        if (lowestHP ==-1) {
                            lowestHP = 0;
                        }

                        if (EnemyPlayers[i].GetComponent<CharacterInstanceDataContainer>().getDataRecord().getcurrentHP() < EnemyPlayers[lowestHP].GetComponent<CharacterInstanceDataContainer>().getDataRecord().getcurrentHP())
                            lowestHP = i;
                    }

                    target = lowestHP.ToString();
            }
                break;
        }

    }
    public List<string> rankMagicAvailable(string AIState) {

        switch (AIState) {

            case CombatAI_Aggressive:
                return sortMagicViaPower();

            case CombatAI_Balanced:
                return sortMagicViaMPEfficiency(); 

            case CombatAI_Defensive:
                return sortMagicViaMPCost();

            default:
            break;
        }

        return null;
    }

    //sort available magic via power
    public List<string> sortMagicViaPower() {

        List<string> tempList = new List<string>();
        List<string> output = new List<string>();

        for (int i = 0; i < gameObject.GetComponent<CharacterInstanceDataContainer>().getMagic().Count; i++) {
            if (characterDataController.GetComponent<MagicDataController>().getMagicData(gameObject.GetComponent<CharacterInstanceDataContainer>().getMagic()[i]).magicType.Equals(MagicDataItem.magicType_Attack)) {
                tempList.Add(gameObject.GetComponent<CharacterInstanceDataContainer>().getMagic()[i]);
            }
        }


        int count = 0;
        int highestPower = -1;
        while (tempList.Count>0) {

            Debug.Log("AI: SortViaPower list size remaining = " + tempList.Count);
            if (count>=tempList.Count) {
                output.Add(tempList[highestPower]);
                tempList.RemoveAt(highestPower);
                highestPower = -1;
                count = 0;
            }

            if (highestPower ==-1) {
                highestPower = 0;
            } else {
                if (characterDataController.GetComponent<MagicDataController>().getMagicData(tempList[highestPower]).multiplierPower< characterDataController.GetComponent<MagicDataController>().getMagicData(tempList[count]).multiplierPower) {
                    highestPower = count;
                }
            }

            count = count + 1;
        }

        return output;
    }

    //sorting magic based on mp consumption, to maximise mp usage
    public List<string> sortMagicViaMPCost() {

        //same as the above, but sorting based on  mp data
        List<string> tempList = new List<string>();
        List<string> output = new List<string>();

        for (int i = 0; i < gameObject.GetComponent<CharacterInstanceDataContainer>().getMagic().Count; i++) {
            if (characterDataController.GetComponent<MagicDataController>().getMagicData(gameObject.GetComponent<CharacterInstanceDataContainer>().getMagic()[i]).magicType.Equals(MagicDataItem.magicType_Attack)) {
                tempList.Add(gameObject.GetComponent<CharacterInstanceDataContainer>().getMagic()[i]);
            }
        }

        Debug.Log(gameObject.GetComponent<CharacterInstanceDataContainer>().getMagic().Count);

        Debug.Log("Size of list to sort" + " = " + tempList.Count);
        int count = 0;
        int lowestMP = -1;
        while (tempList.Count > 0) {

            Debug.Log("AI: SortViaMPCost list size remaining = " + tempList.Count);
            if (count >= tempList.Count) {
                output.Add(tempList[lowestMP]);
                tempList.RemoveAt(lowestMP);
                lowestMP = -1;
                count = 0;
            }

            if (lowestMP == -1) {
                lowestMP = 0;
            } else {
                if (characterDataController.GetComponent<MagicDataController>().getMagicData(tempList[lowestMP]).MPCost > characterDataController.GetComponent<MagicDataController>().getMagicData(tempList[count]).MPCost) {
                    lowestMP = count;
                }
            }

            count = count + 1;
        }

        return output;
    }

    //calculating efficiency of each magic, then sorting based on that
    public List<string> sortMagicViaMPEfficiency() {

        List<string> magicName = new List<string>();
        List<float> magicEfficiency = new List<float>();
        List<string> output = new List<string>();

        for (int i = 0; i < gameObject.GetComponent<CharacterInstanceDataContainer>().getMagic().Count; i++) {
            if (characterDataController.GetComponent<MagicDataController>().getMagicData(gameObject.GetComponent<CharacterInstanceDataContainer>().getMagic()[i]).magicType.Equals(MagicDataItem.magicType_Attack)) {
                float power = characterDataController.GetComponent<MagicDataController>().getMagicData(gameObject.GetComponent<CharacterInstanceDataContainer>().getMagic()[i]).multiplierPower;
                float mpCost = characterDataController.GetComponent<MagicDataController>().getMagicData(gameObject.GetComponent<CharacterInstanceDataContainer>().getMagic()[i]).MPCost;

                //calculating the efficiency of power per MP point
                magicName.Add(gameObject.GetComponent<CharacterInstanceDataContainer>().getMagic()[i]);
                magicEfficiency.Add((power * 100) / mpCost);

                float tm = ((power * 100) / mpCost);
                Debug.Log(tm +" = "+ gameObject.GetComponent<CharacterInstanceDataContainer>().getMagic()[i]);
            }
        }

        int count = 0;
        int highestEfficency = -1;
        while (magicName.Count > 0) {

            Debug.Log("AI: SortViaEffiency list size remaining = " + magicName.Count);
            if (count >= magicName.Count) {
                output.Add(magicName[highestEfficency]);
                magicName.RemoveAt(highestEfficency);
                magicEfficiency.RemoveAt(highestEfficency);

                highestEfficency = -1;
                count = 0;
            }

            if (highestEfficency == -1) {
                highestEfficency = 0;
            } else {
                if ((magicEfficiency[highestEfficency]) < (magicEfficiency[count])) {
                    highestEfficency = count;
                }
            }

            count = count + 1;
        }

        return output;
    }

    //power sorting on the available skills
    public List<string> sortSkillsViaPower() {

        List<string> tempList = new List<string>();
        List<string> output = new List<string>();

        for (int i = 0; i < gameObject.GetComponent<CharacterInstanceDataContainer>().getSkills().Count; i++) {
            if (characterDataController.GetComponent<SkillsDataController>().getSkillData(gameObject.GetComponent<CharacterInstanceDataContainer>().getSkills()[i]).skillType.Equals(SkillDataItem.skillType_Attack)) {
                tempList.Add(gameObject.GetComponent<CharacterInstanceDataContainer>().getSkills()[i]);
            }
        }


        int count = 0;
        int highestPower = -1;
        while (tempList.Count > 0) {

            Debug.Log("AI: SortViaPower list size remaining = " + tempList.Count);
            if (count >= tempList.Count) {
                output.Add(tempList[highestPower]);
                tempList.RemoveAt(highestPower);
                highestPower = -1;
                count = 0;
            }

            if (highestPower == -1) {
                highestPower = 0;
            } else {
                if (characterDataController.GetComponent<SkillsDataController>().getSkillData(tempList[highestPower]).multiplierPower < characterDataController.GetComponent<SkillsDataController>().getSkillData(tempList[count]).multiplierPower) {
                    highestPower = count;
                }
            }

            count = count + 1;
        }

        return output;
    }

    public List<string> getAllHPRestoreMagic() {

        List<string> output = new List<string>();

        for (int i = 0; i < gameObject.GetComponent<CharacterInstanceDataContainer>().getMagic().Count; i++) {
            string tm = gameObject.GetComponent<CharacterInstanceDataContainer>().getMagic()[i];

            if ((characterDataController.GetComponent<MagicDataController>().getMagicData(tm).magicType.Equals(MagicDataItem.magicType_Restoration))&&(characterDataController.GetComponent<MagicDataController>().getMagicData(tm).statEffected.Equals(MagicDataItem.statEffected_HP))) {

                if (gameObject.GetComponent<CharacterInstanceDataContainer>().checkForSufficientMP(characterDataController.GetComponent<MagicDataController>().getMagicData(tm).MPCost)) {
                    output.Add(tm);
                }
            }
        }
        return output;
    }
    
    //this will check for all AVAILABLE MP Restore skills that are not in cooldown
    public List<string> getAllRestoreSkillsViaStatEffectedType(string skillDataItem_StatEffected_Type) {

        List<string> output = new List<string>();

        for (int i = 0; i < gameObject.GetComponent<CharacterInstanceDataContainer>().getSkills().Count; i++) {

            //checking if the skill is a restoration type that restores MP/HP
            SkillDataItem temSkill = characterDataController.GetComponent<SkillsDataController>().getSkillData(gameObject.GetComponent<CharacterInstanceDataContainer>().getSkills()[i]);

            if ((temSkill.skillType.Equals(SkillDataItem.skillType_Restoration)) && temSkill.statEffected.Equals(skillDataItem_StatEffected_Type)) {

                //checking if the skill is in a cooldown period
                if (gameObject.GetComponent<CharacterInstanceDataContainer>().checkIfSkillInCooldown(temSkill.skillName)==0) {
                output.Add(temSkill.skillName);

                }
            }
        }

        return output;

    }

}
