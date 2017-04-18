using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UpdateSkillNumber : MonoBehaviour
{
    private void Update()
    {
        GetComponent<Text>().text = SkillTree.skillPoints.ToString();
    }
}
