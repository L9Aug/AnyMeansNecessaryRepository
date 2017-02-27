using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SkillTree : MonoBehaviour
{
    public static int skillPoints;

    public bool intialSkill;
    public GameObject [] previousSkill;
    public static Text SkillPointCounter;
    public SkillsController.Skills Skill;

    Button currentSkill;
    bool purchased;

	// Use this for initialization
	void Start () {
        currentSkill = this.gameObject.GetComponent<Button>();
        skillCheck();
        skillPoints = 999;
        SkillPointCounter = GameObject.Find("Number of Points").GetComponent<Text>();
        SkillPointCounter.text = skillPoints.ToString();
    }
	
	// Update is called once per frame
	void Update () {

        skillCheck();       
	}

    public void purchaseSkill()
    {
        if (skillPoints > 0 && purchased != true)
        {           
            purchased = true;
            currentSkill.image.color = Color.grey;
            skillPoints--;
            SkillPointCounter.text = skillPoints.ToString();
            SkillsController.SC.AddNewSkill(Skill);
        }
        
    }

    void skillCheck()
    {
            for (int i = 0; i < previousSkill.Length; i++)
            {
                if (intialSkill == false && previousSkill[i].GetComponent<SkillTree>().purchased == false)
                {
                    currentSkill.interactable = false;
                    //Debug.Log("Called");
                }
                if (previousSkill[i].GetComponent<SkillTree>().purchased == true)
                {
                    currentSkill.interactable = true;
                   // Debug.Log("Called.2");
                }
            }
        
    }

    public static void updateSkillPoints()
    {
        SkillPointCounter.text = skillPoints.ToString();
    }

}
