using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this class will control all the different magic skills;
public class MagicDataController : MonoBehaviour {

    private List<MagicDataItem> allMagic = new List<MagicDataItem>();


	// Use this for initialization
	void Start () {
        //player magic
        createAndSaveNewMagic("Lightning Spark", 25, "A basic Electric based magic attack", "electricity", 1.5f, MagicDataItem.magicType_Attack);

        //enemy magic
        createAndSaveNewMagic("Electric Shock", 20, "A weak Electric magical attack", "electricity", 1.2f, MagicDataItem.magicType_Attack);
        createAndSaveNewMagic("High-Charged Bolt", 40, "Much more powerful version of Electric Shock", "electricity", 1.5f, MagicDataItem.magicType_Attack);
        createAndSaveNewMagic("Fast Heal", 35, "-", "-", 1.0f,MagicDataItem.magicType_Restoration,MagicDataItem.statEffected_HP);


    }

    public bool createAndSaveNewMagic(string magicName, int MPCost, string description, string elementAffinity, float multiplier, string magicTypeConst) {

        if (doesMagicExist(magicName)) {
            return false;
        }

        MagicDataItem newMagic = new MagicDataItem(magicName, MPCost, description, elementAffinity, multiplier, magicTypeConst);
        allMagic.Add(newMagic);

        return true;
    }

    public bool createAndSaveNewMagic(string magicName, int MPCost, string description, string elementAffinity, float multiplier, string magicTypeConst, string statEffectedConst) {
        if (doesMagicExist(magicName)) {
            return false;
        }

        MagicDataItem newMagic = new MagicDataItem(magicName, MPCost, description, elementAffinity, multiplier, magicTypeConst, statEffectedConst);
        allMagic.Add(newMagic);

        return true;
    }

    public bool doesMagicExist(string magicName) {

        for (int i = 0; i < allMagic.Count; i++) {
            if (allMagic[i].magicName.Equals(magicName))
                return true;
        }
        return false;
    }

    public MagicDataItem getMagicData(string magicName) {

        for (int i = 0; i < allMagic.Count; i++) {
            if (allMagic[i].magicName.Equals(magicName))
                return allMagic[i];
        }
        Debug.Log("No existing Magic - MagicDataController.getMagic");

        return null;
    }

    public bool addMagicAsAdditionalEffect(string baseMagic, string AdditionalMagicEffect) {

        if (!doesMagicExist(baseMagic)) {
            return false;
        }

        if (!doesMagicExist(AdditionalMagicEffect)) {
            return false;
        }

        for (int i = 0; i < allMagic.Count; i++) {
            if (allMagic[i].magicName.Equals(baseMagic)) {
                allMagic[i].addAdditionalEffect(AdditionalMagicEffect);
                return true;
            }
        }

        return false;
    }

    public List<MagicDataItem> getMagicData(List<string> magicNames) {

        List<MagicDataItem> output = new List<MagicDataItem>();

        for (int i = 0; i < magicNames.Count; i++) {
            if (doesMagicExist(magicNames[i])) {
                output.Add(getMagicData(magicNames[i]));
            }
        }

        return output;
    }

    public string getTargetType(string magic) {

        return getMagicData(magic).magicType;
    }

}
