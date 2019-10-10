using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface RecordClass{

    //returns character name, it can also be used as a reference to the character prefab when set to the same string
    string getCharacterName();

    //sets character name
    void setCharacterName(string name);

    //returns character total level
    int getCharacterLevel();

    //sets the total of the characters level
    void setCharacterLevel(int totalLevel);

    int getStrengthStat();

    void setStrengthStat(int strength);

    int getMagicStat();

    void setMagicStat(int magic);

    int getSpeedStat();

    void setSpeedStat(int speed);

    int getMaxHP();

    void setMaxHP(int max);

    int getcurrentHP();

    void setCurrentHP(int current);

    int getMaxMP();

    int getcurrentMP();

    void setMaxMP(int max);

    void setCurrentMP(int current);

    List<string> getAllAvailableMagic();

    List<string> getAllAvailableSkills();

}
