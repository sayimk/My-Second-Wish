using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillsDataController : MonoBehaviour {

    // Use this for initialization
    List<SkillDataItem> allSkills = new List<SkillDataItem>();

	void Start () {

        //player skills
        createAndSaveNewSkill("Fatal Return", 3, "A 3 step sword attack", 1.3f, SkillDataItem.skillType_Attack);
        createAndSaveNewSkill("Battle-Healing Stage 1", 2, "Basic self healing Skill", 0.5f, SkillDataItem.skillType_Restoration, SkillDataItem.statEffected_HP);


        //enemy skills
        createAndSaveNewSkill("Heavy Attack", 3, "A basic heavy attack", 1.1f, SkillDataItem.skillType_Attack);
        createAndSaveNewSkill("Mana Charge", 2, "MP Restore Skill", 0.8f, SkillDataItem.skillType_Restoration, SkillDataItem.statEffected_MP);

        //test skill
        //createAndSaveNewSkill("Health Charge", 3, "-", 0.9f, SkillDataItem.skillType_Restoration, SkillDataItem.statEffected_HP);



    }

    public bool createAndSaveNewSkill(string skillName, int skillCooldownTime, string description, float multiplierPower, string skillType) {
        SkillDataItem newSkill = new SkillDataItem(skillName, skillCooldownTime, description, multiplierPower, skillType);
        allSkills.Add(newSkill);
        Debug.Log("Added " + newSkill.skillName);
        return true;
    }

    public bool createAndSaveNewSkill(string skillName, int skillCooldownTime, string description, float multiplierPower, string skillType, string statEffected) {
        SkillDataItem newSkill = new SkillDataItem(skillName, skillCooldownTime, description, multiplierPower, skillType, statEffected);
        allSkills.Add(newSkill);
        Debug.Log("Added " + newSkill.skillName);
        return true;
    }

    public bool doesSkillExist(string skillName) {

        for (int i = 0; i < allSkills.Count; i++) {
            if (allSkills[i].skillName.Equals(skillName)) {
                return true;
            }
        }

        return false;
    }

    public SkillDataItem getSkillData(string skillName) {

        for (int i = 0; i < allSkills.Count; i++) {
            if (allSkills[i].skillName.Equals(skillName)) {
                return allSkills[i];
            }
        }
        Debug.Log("No existing Skill - SkillDataController.getSkill");
        return null;
    }

    public List<SkillDataItem> getSkillData(List<string> allSkillString) {

        List<SkillDataItem> output = new List<SkillDataItem>();

        for (int i = 0; i < allSkillString.Count; i++) {
            if (doesSkillExist(allSkillString[i])) {
                output.Add(getSkillData(allSkillString[i]));
            }
        }

        return output;
    }

    public string getTargetType(string skillName) {
        return getSkillData(skillName).skillType;
    }
}
