using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this class is used to represent enemy information
public class EnemyRecordClass :RecordClass {

    public GameObject characterDataController;

    private int enemyLevel;
    private string enemyName;

    private int enemyStrength;
    private int enemyMagic;
    private int enemySpeed;

    private int maxHP;
    private int currentHP;

    private int maxMP;
    private int currentMP;

    private List<string> skillsList = new List<string>();
    private List<string> magicList = new List<string>();

    public int getCharacterLevel() {
        return enemyLevel;
    }

    public string getCharacterName() {
        return enemyName;
    }

    public int getcurrentHP() {
        return currentHP;
    }

    public int getcurrentMP() {
        return currentMP;
    }

    public int getMagicStat() {
        return enemyMagic;
    }

    public int getMaxHP() {
        return maxHP;
    }

    public int getMaxMP() {
        return maxMP;
    }

    public int getSpeedStat() {
        return enemySpeed;
    }

    public int getStrengthStat() {
        return enemyStrength;
    }

    public void setCharacterLevel(int totalLevel) {
        enemyLevel = totalLevel;
    }

    public void setCharacterName(string name) {
        enemyName = name;
    }

    public void setCurrentHP(int current) {
        currentHP = current;
    }

    public void setCurrentMP(int current) {
        currentMP = current;
    }

    public void setMagicStat(int magic) {
        enemyMagic = magic;
    }

    public void setMaxHP(int max) {
        maxHP = max;
    }

    public void setMaxMP(int max) {
        maxMP = max;
    }

    public void setSpeedStat(int speed) {
        enemySpeed = speed;
    }

    public void setStrengthStat(int strength) {
        enemyStrength = strength;
    }

    public bool addSkill(string skillName) {

        skillsList.Add(skillName);
        return true;
    }


    public bool addMagic(string magicName) {

        magicList.Add(magicName);
        return true;
    }

    public List<string> getAllAvailableMagic() {
        return magicList;
    }

    public List<string> getAllAvailableSkills() {
        return skillsList;
    }
}
