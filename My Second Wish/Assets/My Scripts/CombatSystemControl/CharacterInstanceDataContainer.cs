using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInstanceDataContainer : MonoBehaviour {

    public string CDCName { get; set; }

    public GameObject combatController;
    public GameObject characterDataController;

    //this contains all of the stats
    private RecordClass dataRecord;
    private bool isDead = false;

    private List<string> magicList = new List<string>();
    private List<string> skillList = new List<string>();

    private int turnCount = 0;
    private List<string> skillCooldownList = new List<string>(); 

    //methods used to load and save statistics between this instance class and the permenant records in exploration mode
    public void loadCharacterRecordIntoInstance(RecordClass recordClass, string CDCName) {

        dataRecord = recordClass;
        this.CDCName = CDCName;
        magicList = recordClass.getAllAvailableMagic();
        skillList = recordClass.getAllAvailableSkills();
        Debug.Log("Total magic amount = "+recordClass.getAllAvailableMagic().Count);
        isDead = false;
        turnCount = 0;
        skillCooldownList.Clear();

    }

    public RecordClass updateCharacterRecord(RecordClass recordToBeUpdated) {

        return null;
    }

    public RecordClass getDataRecord() {

        RecordClass temp = dataRecord;
        return temp;
    }

    public void addCombatControllerAndCharacterDatabaseObject(GameObject combatController, GameObject characterDatabaseController) {
        this.combatController = combatController;
        this.characterDataController = characterDatabaseController;
    }

    //returns the sequence by calling add damage from combat controller
    public void animationEndEvent() {
        combatController.GetComponent<CombatController>().addEffectToCharacter();
    }

    public void startAttackAnimation() {
        gameObject.transform.GetChild(0).gameObject.GetComponent<Animator>().SetTrigger("Attack");
    }

    public void addInflictedDamage(int damageRecieved) {
        dataRecord.setCurrentHP((dataRecord.getcurrentHP() - damageRecieved));

        if (dataRecord.getcurrentHP()<=0) {
            isDead = true;
        }

        //play a damage recieved animation
    }

    public bool checkForSufficientMP(int MPCost) {

        if (dataRecord.getcurrentMP()>=MPCost) {
            return true;
        }else 
            return false;
    }

    public void reduceMP(int reductionAmount) {
        dataRecord.setCurrentMP(dataRecord.getcurrentMP() - reductionAmount);
    }

    public void restoreMP(int restorationAmount) {
        int totalToRestore = dataRecord.getcurrentMP() + restorationAmount;

        if (totalToRestore>dataRecord.getMaxMP()) {
            totalToRestore = dataRecord.getMaxMP();
        }

        dataRecord.setCurrentMP(totalToRestore);
    }

    public void restoreHP(int restorationAmount) {
        int totalToRestore = dataRecord.getcurrentHP() + restorationAmount;

        if (totalToRestore > dataRecord.getMaxHP()) {
            totalToRestore = dataRecord.getMaxHP();
        }

        dataRecord.setCurrentHP(totalToRestore);
    }

    public void startTurnInitialChecks() {

        turnCount=turnCount+1;
        refreshSkillCooldowns();
    }

    public int getTurnCounter() {
        return turnCount;
    }

    public void refreshSkillCooldowns() {

        List<string> tempL = new List<string>();

        for (int i = 0; i < skillCooldownList.Count; i++) {
            string[] tempSplit = skillCooldownList[i].Split(',');

            if (0<Convert.ToInt32(tempSplit[1]) - 1) {
                int newCooldown = Convert.ToInt32(tempSplit[1]) - 1;
                tempL.Add(tempSplit[0] + "," + newCooldown);
            }

        }

        skillCooldownList = tempL;
    }

    public void addSkillToCooldownList(string skillName) {
        Debug.Log("Added " + skillName + " to cooldown list");

        if (characterDataController.GetComponent<SkillsDataController>().doesSkillExist(skillName)) {
            string temp = skillName + "," + characterDataController.GetComponent<SkillsDataController>().getSkillData(skillName).skillCooldownTime;
            skillCooldownList.Add(temp);
        }

    }

    public int checkIfSkillInCooldown(string skillName) {

        for (int i = 0; i < skillCooldownList.Count; i++) {
            string[] temp = skillCooldownList[i].Split(',');
            if (temp[0].Equals(skillName))
                return Convert.ToInt32(temp[1]);
        }
        return 0;
    }

    public List<string> getSkills() {
        return skillList;
    }

    public List<string> getMagic() {
        return magicList;
    }

    public void useRestoreAbility(string statEffected, int amount) {

        Debug.Log("Stat Effected = " + statEffected + " Amount = " + amount);

        if ((statEffected.Equals(SkillDataItem.statEffected_HP))||(statEffected.Equals(MagicDataItem.statEffected_HP))) {
            restoreHP(amount);
            Debug.Log("HP = " + dataRecord.getcurrentHP());

        }else if ((statEffected.Equals(SkillDataItem.statEffected_MP)) || (statEffected.Equals(MagicDataItem.statEffected_MP))) {
            restoreMP(amount);
            Debug.Log("MP = " + dataRecord.getcurrentMP());
        }
    }

    public void addStatBoost(string name, string statEffected, int boostAmount) {

    }

    public bool CheckIfCharacterIsDead() {
        return isDead;
    }

    public void killCharacter() {
        Destroy(gameObject.transform.GetChild(0).gameObject);
    }
}
