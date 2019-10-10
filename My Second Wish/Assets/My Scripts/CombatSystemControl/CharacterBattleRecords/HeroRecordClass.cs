using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this class is used to represent heroes information
public class HeroRecordClass : RecordClass {

    public GameObject characterDataController;

    private string characterName;
    private int characterLevel;
    private int experience;
    private int levelThreshold;
    private int incrementAmount;

    private int strength;
    private int magic;
    private int speed;

    private int maxHP;
    private int currentHP;

    private int maxMP;
    private int currentMP;

    private List<string> skillsList = new List<string>();
    private List<string> magicList = new List<string>();

    //syntax for the locked skill list is <<name>,<unlockLvl>> 
    //need to decide where the locked skill data is stored
    private List<string> lockedSkillsList = new List<string>();
    private List<string> lockedMagicList = new List<string>();

    public HeroRecordClass(GameObject characterDataController) {
        characterName = "";
        characterLevel = 1;
        experience = 0;
        this.characterDataController = characterDataController;
    }

    public int getCharacterExperience() {
        return experience;
    }

    public void setInitialLevelThresholdAndIncrement(int initial, int incrementAmount) {
        levelThreshold = initial;
        this.incrementAmount = incrementAmount;
    }

    public int getCharacterLevel() {
        return characterLevel;
    }

    public string getCharacterName() {
        return characterName;
    }

    public int getcurrentHP() {
        return currentHP;
    }

    public int getcurrentMP() {
        return currentMP;
    }

    public int getMagicStat() {
        return magic;
    }

    public int getMaxHP() {
        return maxHP;
    }

    public int getMaxMP() {
        return maxMP;
    }

    public int getSpeedStat() {
        return speed;
    }

    public int getStrengthStat() {
        return strength;
    }

    public void setCharacterLevel(int totalLevel) {
        characterLevel = totalLevel;
    }

    public void setCharacterName(string name) {
        characterName = name;
    }

    public void setCharacterTotalExperience(int totalExperience) {
        experience = totalExperience;
    }

    public void setCurrentHP(int current) {
        currentHP = current;
    }

    public void setCurrentMP(int current) {
        currentMP = current;
    }

    public void setMagicStat(int magic) {
        this.magic = magic;
    }

    public void setMaxHP(int max) {
        maxHP = max;
    }

    public void setMaxMP(int max) {
        maxMP = max;
    }

    public void setSpeedStat(int speed) {
        this.speed = speed;
    }

    public void setStrengthStat(int strength) {
        this.strength = strength;
    }

    public bool addSkill(string skillName) {

       // if (characterDataController.GetComponent<SkillsDataController>().doesSkillExist(skillName) == false)
        //    return false;

        skillsList.Add(skillName);
        return true;
    } 

    public bool lockSkill(string skillName, int unlockLevel) {

        for (int i = 0; i < skillsList.Count; i++) {
            if (skillsList[i].Equals(skillName)) {
                lockedSkillsList.Add((skillsList[i] + "," + unlockLevel));
                return true;
            }
        }
        return false;
    }

    public bool addMagic(string magicName) {

        magicList.Add(magicName);
        return true;
    }

    public bool lockMagic(string magicName, int unlockLevel) {

        for (int i = 0; i < magicList.Count; i++) {
            if (magicList[i].Equals(magicName)) {
                lockedMagicList.Add((magicList[i] + "," + unlockLevel));
                return true;
            }
        }
        return false;
    }

    public void checkForNewUnlockedSkills() {

        List<string> unlocked = new List<string>();
        for (int i = 0; i < lockedSkillsList.Count; i++) {

            string[] temp = lockedSkillsList[i].Split(',');

            if (Convert.ToInt32(temp[1])<=characterLevel) {
                unlocked.Add(lockedSkillsList[i]);
            }
        }

        for (int i = 0; i < unlocked.Count; i++) {
            lockedSkillsList.Remove(unlocked[i]);

            string[] toAdd = unlocked[i].Split(',');
            skillsList.Add(toAdd[0]);
            Debug.Log("Unlocked " + toAdd[0]);
        }
    }

    public void checkForNewUnlockedMagic() {

        List<string> unlocked = new List<string>();
        for (int i = 0; i < lockedMagicList.Count; i++) {

            string[] temp = lockedMagicList[i].Split(',');

            if (Convert.ToInt32(temp[1]) <= characterLevel) {
                unlocked.Add(lockedMagicList[i]);
            }
        }

        for (int i = 0; i < unlocked.Count; i++) {
            lockedMagicList.Remove(unlocked[i]);

            string[] toAdd = unlocked[i].Split(',');
            magicList.Add(toAdd[0]);
            Debug.Log("Unlocked "+toAdd[0]);
        }
    }

    public List<string> getAllAvailableSkills() {
        return skillsList;
    }

    public List<string> getAllAvailableMagic() {
        return magicList;
    }

    public void addExperience(int exp) {
        experience = experience+exp;

        //check if new level attained and unlock skills accordingly
    }


}
