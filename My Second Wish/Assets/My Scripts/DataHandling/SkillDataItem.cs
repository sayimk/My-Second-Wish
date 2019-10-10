using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillDataItem {

    public const string skillType_Attack = "attackTypeSkill";
    public const string skillType_Restoration = "restorationTypeSkill";
    public const string skillType_Support = "supportTypeSkill";
    public const string skillType_AttackSupport = "attackSupportTypeSkill";

    public const string statEffected_Strength = "str";
    public const string statEffected_Speed = "spd";
    public const string statEffected_Magic = "mag";
    public const string statEffected_HP = "hp";
    public const string statEffected_MP = "mp";


    public string skillName { get; set; }
    public int skillCooldownTime { get; set; }
    public string description { get; set; }
    public float multiplierPower { get; set; }
    public string skillType { get; set; }
    public string statEffected { get; set; }

    public SkillDataItem() {

    }

    public SkillDataItem(string skillName, int skillCooldownTime, string description, float multiplierPower, string skillType) {
        this.skillName = skillName;
        this.skillCooldownTime = skillCooldownTime;
        this.description = description;
        this.multiplierPower = multiplierPower;
        this.skillType = skillType;
        statEffected = "";
    }

    public SkillDataItem(string skillName, int skillCooldownTime, string description, float multiplierPower, string skillType, string statEffected) {
        this.skillName = skillName;
        this.skillCooldownTime = skillCooldownTime;
        this.description = description;
        this.multiplierPower = multiplierPower;
        this.statEffected = statEffected;
        this.skillType = skillType;

    }
}
