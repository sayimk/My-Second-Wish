using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicDataItem{

    //magic const types
    public const string magicType_Attack = "attackTypeMagic";
    public const string magicType_Restoration = "restorationTypeMagic";
    public const string magicType_Support = "supportTypeMagic";
    public const string magicType_AttackSupport = "attackSupportTypeMagic";

    public const string statEffected_Strength = "str";
    public const string statEffected_Speed = "spd";
    public const string statEffected_Magic = "mag";
    public const string statEffected_HP = "hp";
    public const string statEffected_MP = "mp";

    public string magicName { get; set; }
    public int MPCost { get; set; }
    public string description { get; set; }
    public string elementAffinity { get; set; }
    public float multiplierPower { get; set; }
    public string additionalEffect { get; set; }
    public string magicType { get; set; }
    public string statEffected { get; set; }

    public bool hasSecondaryEffect { get; set; }
    public MagicDataItem() {

    }

    public MagicDataItem(string magicName, int MPCost, string description, string elementAffinity, float multiplier, string magicType) {
        this.magicName = magicName;
        this.MPCost = MPCost;
        this.description = description;
        this.elementAffinity = elementAffinity;
        multiplierPower = multiplier;
        this.magicType = magicType;
        statEffected = "";
        hasSecondaryEffect = false;

    }

    public MagicDataItem(string magicName, int MPCost, string description, string elementAffinity, float multiplier, string magicType, string statEffected) {

        this.magicName = magicName;
        this.MPCost = MPCost;
        this.description = description;
        this.elementAffinity = elementAffinity;
        multiplierPower = multiplier;
        this.magicType = magicType;
        this.statEffected = statEffected;
        hasSecondaryEffect = false;

    }

    public void addAdditionalEffect(string additonalMagicName) {
        additionalEffect = additonalMagicName;
        hasSecondaryEffect = true;
    }


}
