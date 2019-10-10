using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Battle_UI_Controller : MonoBehaviour {
    public GameObject Gauge;
    public GameObject EnemySelector;
    public GameObject player1TurnIcon;
    public GameObject combatController;
    public GameObject characterDataController;
    //public GameObject player2TurnIcon;
    //public GameObject player3TurnIcon;
    //public GameObject player4TurnIcon;
    public GameObject enemy1TurnIcon;
    //public GameObject enemy2TurnIcon;
    //public GameObject enemy3TurnIcon;
    //public GameObject enemy4TurnIcon;

    public const string Enemy_Target_For_Selector = "enemy";
    public const string Ally_Target_For_Selector = "ally";

    int subMenuDisplayStartInt = 0;
    List<string> currentListToDisplay;
    bool subMenuSet = false;

    string attackType;
    string selectedTargetType;


    public void hidePlayerUI() {
        gameObject.GetComponent<Animator>().SetTrigger("EndTurn");
        gameObject.GetComponent<Animator>().ResetTrigger("displayBattleOptions");
        gameObject.GetComponent<Animator>().ResetTrigger("DisplayFrom1");
        gameObject.GetComponent<Animator>().ResetTrigger("DisplayFrom2");
        gameObject.GetComponent<Animator>().ResetTrigger("DisplayFrom3");
        gameObject.GetComponent<Animator>().ResetTrigger("DisplayFrom4");
        gameObject.GetComponent<Animator>().ResetTrigger("HideEnemySelector");
        gameObject.GetComponent<Animator>().ResetTrigger("ShowEnemySelector");
        gameObject.GetComponent<Animator>().ResetTrigger("Hide");




    }

    public void showPlayerUI() {
        gameObject.GetComponent<Animator>().SetTrigger("displayBattleOptions");
    }

    //event handler for the enemy selector
    public void selectEnemy(int enemyInt) {

        //defaulting selection window to false interactivity

        for (int i = 0; i < 4; i++) {
            EnemySelector.transform.GetChild(i).gameObject.GetComponent<Button>().interactable = false;
        }

        hideEnemySelector();
        //sending information to combat controller
        combatController.GetComponent<CombatController>().ExecutePlayerMove(enemyInt, selectedTargetType);
    }

    public void attackOptionEvent() {
        gameObject.GetComponent<Animator>().SetTrigger("DisplayFrom1");
        setupTargetSelector(Enemy_Target_For_Selector);
        combatController.GetComponent<CombatController>().selectAttackType(CombatController.Attack_CombatController_Keyword);
    }

    public void skillOptionEvent() {

        gameObject.GetComponent<Animator>().SetTrigger("DisplayFrom2");

        resetSubOptions();
        attackType = CombatController.Skill_CombatController_Keyword;
        setSubOptions(combatController.GetComponent<CombatController>().getCurrentPlayersSkills());

    }

    public void magicOptionEvent() {

        gameObject.GetComponent<Animator>().SetTrigger("DisplayFrom3");

        attackType = CombatController.Magic_CombatController_Keyword;
        setSubOptions(combatController.GetComponent<CombatController>().getCurrentPlayersMagic());

    }

    public void itemOptionEvent() {

        gameObject.GetComponent<Animator>().SetTrigger("DisplayFrom4");

    }

    public void setupTargetSelector(string targetType_For_Selector) {

        List<string> toDisplay;

        Debug.Log("displaying selector for " + targetType_For_Selector);

        switch (targetType_For_Selector) {

            case Enemy_Target_For_Selector: toDisplay = combatController.GetComponent<CombatController>().getAllEnemyNamesInOrder();
                break;

            case Ally_Target_For_Selector: toDisplay = combatController.GetComponent<CombatController>().getAllHeroNamesInOrder();
                break;

            default: toDisplay = new List<string>();
                break;
        }


        int remainingSpace = 4;
        if (toDisplay.Count<4) {
            remainingSpace = 4 - toDisplay.Count;

            Debug.Log("slot remaining space = " + remainingSpace);
            for (int j = 0; j < remainingSpace; j++) {
                toDisplay.Add("");
            }
        }

        for (int i = 0; i < toDisplay.Count; i++) {
            EnemySelector.transform.GetChild(i).GetChild(0).gameObject.GetComponent<Text>().text = toDisplay[i];

            if (EnemySelector.transform.GetChild(i).GetChild(0).gameObject.GetComponent<Text>().text!="") {
                EnemySelector.transform.GetChild(i).gameObject.GetComponent<Button>().interactable = true;
            } else {
                EnemySelector.transform.GetChild(i).gameObject.GetComponent<Button>().interactable = false;

            }
        }


        selectedTargetType = targetType_For_Selector;
        showEnemySelector();

    }

    public void hideEnemySelector() {
        gameObject.GetComponent<Animator>().SetTrigger("HideEnemySelector");
    }

    public void showEnemySelector() {
        gameObject.GetComponent<Animator>().SetTrigger("ShowEnemySelector");

    }

    public void hideSubMenuOptions() {
        gameObject.GetComponent<Animator>().SetTrigger("Hide");
    }

    public void updateHealthAndMPGauge(GameObject characterBattleInstance) {

        float currentHealth = characterBattleInstance.GetComponent<CharacterInstanceDataContainer>().getDataRecord().getcurrentHP();
        float maxHealth = characterBattleInstance.GetComponent<CharacterInstanceDataContainer>().getDataRecord().getMaxHP();
        float currentMP = characterBattleInstance.GetComponent<CharacterInstanceDataContainer>().getDataRecord().getcurrentMP();
        float maxMP = characterBattleInstance.GetComponent<CharacterInstanceDataContainer>().getDataRecord().getMaxMP();

        Gauge.transform.Find("CurrentHP").gameObject.GetComponent<Text>().text = currentHealth.ToString();
        Gauge.transform.Find("CurrentMP").gameObject.GetComponent<Text>().text = currentMP.ToString();

        float HPPercentage = currentHealth / maxHealth;
        float MPPercentage = currentMP / maxMP;
        Gauge.transform.Find("HPBar").gameObject.GetComponent<Image>().fillAmount = HPPercentage;
        Gauge.transform.Find("MPBar").gameObject.GetComponent<Image>().fillAmount = MPPercentage;

    }

    //sub event handlers
    public void selectSubOption(int option) {

        string selected = gameObject.transform.Find("Sub_Option_"+option).Find("Text").gameObject.GetComponent<Text>().text;
        combatController.GetComponent<CombatController>().selectAttackType(attackType, selected);

        switch (combatController.GetComponent<CombatController>().getTargetType(attackType,selected)) {

            case MagicDataItem.magicType_Attack: setupTargetSelector(Enemy_Target_For_Selector);
                break;

            case SkillDataItem.skillType_Attack: setupTargetSelector(Enemy_Target_For_Selector);
                break;

            case MagicDataItem.magicType_AttackSupport: setupTargetSelector(Enemy_Target_For_Selector);
                break;

            case SkillDataItem.skillType_AttackSupport: setupTargetSelector(Enemy_Target_For_Selector);
                break;

            case MagicDataItem.magicType_Restoration: setupTargetSelector(Ally_Target_For_Selector);
                break;

            case SkillDataItem.skillType_Restoration: setupTargetSelector(Ally_Target_For_Selector);
                break;

            case MagicDataItem.magicType_Support: setupTargetSelector(Ally_Target_For_Selector);
                break;

            case SkillDataItem.skillType_Support: setupTargetSelector(Ally_Target_For_Selector);
                break;

            default: setupTargetSelector(Enemy_Target_For_Selector);
                break;
        }

    }

    public void pauseAllTurnIndicators() {
        player1TurnIcon.GetComponent<Animator>().enabled = false;
        //player2TurnIcon.GetComponent<Animator>().enabled = false;
        //player3TurnIcon.GetComponent<Animator>().enabled = false;
        //player4TurnIcon.GetComponent<Animator>().enabled = false;
        enemy1TurnIcon.GetComponent<Animator>().enabled = false;
        //enemy2TurnIcon.GetComponent<Animator>().enabled = false;
        //enemy3TurnIcon.GetComponent<Animator>().enabled = false;
        //enemy4TurnIcon.GetComponent<Animator>().enabled = false;
    }

    public void endAllTurnIndicators() {
        player1TurnIcon.GetComponent<Animator>().SetTrigger("stop");
        player1TurnIcon.GetComponent<Animator>().enabled = false;
        //player2TurnIcon.GetComponent<Animator>().enabled = false;
        //player3TurnIcon.GetComponent<Animator>().enabled = false;
        //player4TurnIcon.GetComponent<Animator>().enabled = false;
        enemy1TurnIcon.GetComponent<Animator>().SetTrigger("stop");
        enemy1TurnIcon.GetComponent<Animator>().enabled = false;
        //enemy2TurnIcon.GetComponent<Animator>().enabled = false;
        //enemy3TurnIcon.GetComponent<Animator>().enabled = false;
        //enemy4TurnIcon.GetComponent<Animator>().enabled = false;
    }

    public void resumeAllTurnIndicators() {

        player1TurnIcon.GetComponent<Animator>().enabled = true;
        player1TurnIcon.GetComponent<Animator>().SetTrigger("resume");

        //player2TurnIcon.GetComponent<Animator>().enabled = true;
        //player3TurnIcon.GetComponent<Animator>().enabled = true;
        //player4TurnIcon.GetComponent<Animator>().enabled = true;
        enemy1TurnIcon.GetComponent<Animator>().SetTrigger("resume");
        enemy1TurnIcon.GetComponent<Animator>().enabled = true;
        //enemy2TurnIcon.GetComponent<Animator>().enabled = true;
        //enemy3TurnIcon.GetComponent<Animator>().enabled = true;
        //enemy4TurnIcon.GetComponent<Animator>().enabled = true;
    }

    public void enableSpecificPlayerTurnIndicator(int oneToFourPlayerNo) {

        switch (oneToFourPlayerNo) {

            case 1: {
                    player1TurnIcon.GetComponent<Animator>().SetTrigger("reset");
                    player1TurnIcon.GetComponent<Animator>().enabled = false;
                    player1TurnIcon.GetComponent<Image>().enabled = true;
                }
                break;

            default:
                break;
        }

    }

    public void enableSpecificEnemyTurnIndicator(int oneToFourEnemyNo) {

        switch (oneToFourEnemyNo) {
            default:
                break;
        }

    }

    public void disableAllTurnOrders() {

       // player1TurnIcon.GetComponent<Animator>().SetTrigger("reset");
       // player1TurnIcon.GetComponent<Animator>().enabled = false;
        //player1TurnIcon.GetComponent<Image>().enabled = false;

        /* player2TurnIcon.GetComponent<Animator>().SetTrigger("reset");
         player2TurnIcon.GetComponent<Animator>().enabled = false;
         player2TurnIcon.GetComponent<Image>().enabled = false;*/

        /* player3TurnIcon.GetComponent<Animator>().SetTrigger("reset");
         player3TurnIcon.GetComponent<Animator>().enabled = false;
         player3TurnIcon.GetComponent<Image>().enabled = false;*/

        /* player4TurnIcon.GetComponent<Animator>().SetTrigger("reset");
           player4TurnIcon.GetComponent<Animator>().enabled = false;
           player4TurnIcon.GetComponent<Image>().enabled = false;*/

        //   enemy1TurnIcon.GetComponent<Animator>().SetTrigger("reset");
         //  enemy1TurnIcon.GetComponent<Animator>().enabled = false;
         //  enemy1TurnIcon.GetComponent<Image>().enabled = false;

        /* enemy2TurnIcon.GetComponent<Animator>().SetTrigger("reset");
           enemy2TurnIcon.GetComponent<Animator>().enabled = false;
           enemy2TurnIcon.GetComponent<Image>().enabled = false;*/

        /* enemy3TurnIcon.GetComponent<Animator>().SetTrigger("reset");
           enemy3TurnIcon.GetComponent<Animator>().enabled = false;
           enemy3TurnIcon.GetComponent<Image>().enabled = false;*/

        /* enemy4TurnIcon.GetComponent<Animator>().SetTrigger("reset");
           enemy4TurnIcon.GetComponent<Animator>().enabled = false;
           enemy4TurnIcon.GetComponent<Image>().enabled = false;*/
    }

    public void setSubOptions(List<string> listToDisplay) {

        int remainingSpace = 4;

        if (!subMenuSet) {
            currentListToDisplay = listToDisplay;
            subMenuSet = true;
        }
        if (listToDisplay.Count<4) {
            remainingSpace = 4 - listToDisplay.Count;

            for (int j = 0; j < remainingSpace; j++) {
                listToDisplay.Add("----");
            }
        }

        for (int i = 1; i < 5; i++) {
            string prefix = "Sub_Option_"+i;

            if (combatController.GetComponent<CombatController>().getCurrentHeroObject().GetComponent<CharacterInstanceDataContainer>().checkIfSkillInCooldown(listToDisplay[(i - 1)]) !=0) {
                gameObject.transform.Find(prefix).Find("Text").gameObject.GetComponent<Text>().text = listToDisplay[(i - 1)]+" ( "+ combatController.GetComponent<CombatController>().getCurrentHeroObject().GetComponent<CharacterInstanceDataContainer>().checkIfSkillInCooldown(listToDisplay[(i - 1)])+")";
                gameObject.transform.Find(prefix).gameObject.GetComponent<Button>().interactable = false;
            } else {
                gameObject.transform.Find(prefix).Find("Text").gameObject.GetComponent<Text>().text = listToDisplay[(i - 1)];
                gameObject.transform.Find(prefix).gameObject.GetComponent<Button>().interactable = true;

            }


        }

    }

    public void resetSubOptions() {
        subMenuSet = false;
        attackType = "";
        selectedTargetType = "";
    }

    public void incrementSubMenu() {

        if (subMenuDisplayStartInt + 4 < currentListToDisplay.Count) {
            subMenuDisplayStartInt = subMenuDisplayStartInt + 1;
            Debug.Log("displaying new options");
            displayOptions(currentListToDisplay, subMenuDisplayStartInt, subMenuDisplayStartInt + 4);
        }
    }

    public void decrementSubMenu() {
        if ((subMenuDisplayStartInt-1)>=0) {
            subMenuDisplayStartInt = subMenuDisplayStartInt - 1;
            displayOptions(currentListToDisplay, subMenuDisplayStartInt, subMenuDisplayStartInt + 4);
        }
    }

    public void displayOptions(List<string> options, int startInclusive, int end) {

        Debug.Log("Start =" + startInclusive + " end= " + end + "Option size = " + options.Count);

        List<string> tempList = new List<string>();

       // if (endInclusive > options.Count)
         //    endInclusive = options.Count - 1;

        for (int i = startInclusive; i < end; i++) {
            tempList.Add(options[i]);
            Debug.Log(options[i]);
        }

        setSubOptions(tempList);

    }


}
