using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Base_Mission : Mission_Giver
{

    ///general quest information
    public static int questCompletedAmount;
    public int xpReward;
    public int questNumber;
    protected bool rewardGiven;

    private GameObject canvas;
    private GameObject _togglePrefab;
    public GameObject _TooglePrefab
    {
        get { return _togglePrefab; }
        set { _togglePrefab = value; }
    }

    public void MissonDetails(string text, int mission)
    {
        _TooglePrefab = (GameObject)Resources.Load("Toggle");

        _TooglePrefab = (GameObject)Instantiate(_TooglePrefab, Vector3.zero, Quaternion.identity);

        _TooglePrefab.transform.parent = GameObject.Find("PlayTimeHud").transform;

        _TooglePrefab.GetComponent<RectTransform>().transform.localPosition = new Vector3(459, -mission * 25+25, 0);
        _TooglePrefab.GetComponent<RectTransform>().transform.localScale = new Vector3(1, 1, 1);
        _TooglePrefab.GetComponent<RectTransform>().transform.localRotation = Quaternion.Euler(0, 0, 0);
        _TooglePrefab.GetComponentInChildren<Text>().text = text;
    }


    public void UpdateMissionText(string text)
    {
        _TooglePrefab.GetComponentInChildren<Text>().text = text;
    }

    public void MissionComplete()
    {
        _TooglePrefab.GetComponent<Toggle>().isOn = true;
        missionCompleted = true;
    }
    public void giveXP(int amount)
    {
        canvas = GameObject.Find("mainCanvas");
        canvas.GetComponent<UIElements>().xpGain(amount);
    }
}
