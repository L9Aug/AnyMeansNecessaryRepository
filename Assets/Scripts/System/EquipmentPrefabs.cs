using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Equipment { SilencedPistol, AssaultRifle, SniperRifle }

public class EquipmentPrefabs : MonoBehaviour {

    public static EquipmentPrefabs EQ;

    public List<GameObject> EquipmentPrefabsList = new List<GameObject>();

	// Use this for initialization
	void Start () {
        EQ = this;
	}
}
