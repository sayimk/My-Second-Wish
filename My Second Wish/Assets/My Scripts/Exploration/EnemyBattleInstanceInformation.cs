using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this class is put on enemy objects in exploration mode and engagement control will use this data to fetch enemies
public interface EnemyBattleInstanceInformation {

    List<string> getEnemyCDCNames();
    List<int> getEnemyLevels();
    string getRegion();
    List<bool> getAkira_Tama_Nero_Elliot_Availablity();

    GameObject getMapIcon();
}
