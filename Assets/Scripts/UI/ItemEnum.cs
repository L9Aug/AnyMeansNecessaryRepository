using UnityEngine;
using System.Collections;

public class ItemEnum : MonoBehaviour {

    

    public Item thisItem;

    [System.Serializable]
    public enum Item
    {
        Pistol = 0,
        SilencedPistol = 1,
        SniperRifle,
        AssualtRifle,
        Explosive,
        TranqPistol,
        BodyArmour,
        MedKit,
        Bottle,
        Rock,
        DistractionBox,
        Key,
        KeyCard,
        Intel,
        TranqAmmo,
        PistolAmmo,
        SniperAmmo,
        AssualtRifleAmmo,
        QuestItem

    }


}
