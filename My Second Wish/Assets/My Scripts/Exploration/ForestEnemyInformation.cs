using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestEnemyInformation : MonoBehaviour, EnemyBattleInstanceInformation {
    public GameObject thisObjectMiniMapIcon;

    public string enemy_1_CDC_Name;
    public int enemy_1_Level;

    public string enemy_2_CDC_Name;
    public int enemy_2_Level;

    public string enemy_3_CDC_Name;
    public int enemy_3_Level;

    public string enemy_4_CDC_Name;
    public int enemy_4_Level;

    public bool Akira_Available_In_Battle;
    public bool Tama_Available_In_Battle;
    public bool Nero_Avaiable_In_Battle;
    public bool Elliot_Available_In_Battle;

    //this will return which heroes are available in this instance
    public List<bool> getAkira_Tama_Nero_Elliot_Availablity() {
        List<bool> temp = new List<bool>();

        temp.Add(Akira_Available_In_Battle);
        temp.Add(Tama_Available_In_Battle);
        temp.Add(Nero_Avaiable_In_Battle);
        temp.Add(Elliot_Available_In_Battle);

        return temp;
    }

    //this will return all instance enemy Character Data Controller const names
    public List<string> getEnemyCDCNames() {
        List<string> temp = new List<string>();

        if(enemy_1_CDC_Name!="")
            temp.Add(enemy_1_CDC_Name);

        if (enemy_2_CDC_Name != "")
            temp.Add(enemy_2_CDC_Name);

        if (enemy_3_CDC_Name != "")
            temp.Add(enemy_3_CDC_Name);

        if (enemy_4_CDC_Name != "")
            temp.Add(enemy_4_CDC_Name);

        return temp;
    }

    // this will return enemy levels
    public List<int> getEnemyLevels() {
        List<int> tempL = new List<int>();

        if (enemy_1_CDC_Name != "")
            tempL.Add(enemy_1_Level);

        if (enemy_2_CDC_Name != "")
            tempL.Add(enemy_2_Level);

        if (enemy_3_CDC_Name != "")
            tempL.Add(enemy_3_Level);

        if (enemy_4_CDC_Name != "")
            tempL.Add(enemy_4_Level);

        return tempL;
    }

    public GameObject getMapIcon() {
        return thisObjectMiniMapIcon;
    }

    //this will return the area param const for the CharacterPositionManager
    public string getRegion() {
        return CharacterPositionManager.Forest_fieldType;
    }
}
