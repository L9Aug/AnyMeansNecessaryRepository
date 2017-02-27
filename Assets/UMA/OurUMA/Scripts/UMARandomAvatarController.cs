/// Adapted from UMA Crowd for use in AnyMeansNessicary by Tristan Bampton

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UMA;

public class UMARandomAvatarController : MonoBehaviour {

    [HideInInspector]
    [System.Obsolete("Crowd slotLibrary is obsolete, please use the Crowd umaContext", false)]
    public SlotLibrary slotLibrary;
    [HideInInspector]
    [System.Obsolete("Crowd overlayLibrary is obsolete, please use the Crowd umaContext", false)]
    public OverlayLibrary overlayLibrary;
    [HideInInspector]
    [System.Obsolete("Crowd raceLibrary is obsolete, please use the Crowd umaContext", false)]
    public RaceLibrary raceLibrary;

    public UMACrowdRandomSet[] randomPool;
    public UMAGeneratorBase generator;
    public UMAData umaData;
    public UMAContext umaContext;
    public RuntimeAnimatorController animationController;
    public float atlasResolutionScale = 1;
    public bool randomDna;
    public string[] keywords;

    public UMADataEvent CharacterCreated;
    public UMADataEvent CharacterDestroyed;
    public UMADataEvent CharacterUpdated;

    [HideInInspector]
    public UMARecipeBase[] additionalRecipes;

    public GameObject GenerateOneUMA(UMARecipeBase[] AdditionalRecipes, GameObject RandObj)
    {
        additionalRecipes = AdditionalRecipes;

        UMADynamicAvatar umaDynamicAvatar = RandObj.AddComponent<UMADynamicAvatar>();
        umaDynamicAvatar.Initialize();
        umaData = umaDynamicAvatar.umaData;
        umaData.CharacterCreated = new UMADataEvent(CharacterCreated);
        umaData.CharacterDestroyed = new UMADataEvent(CharacterDestroyed);
        umaData.CharacterUpdated = new UMADataEvent(CharacterUpdated);
        umaDynamicAvatar.umaGenerator = generator;
        umaData.umaGenerator = generator;
        var umaRecipe = umaDynamicAvatar.umaData.umaRecipe;
        UMACrowdRandomSet.CrowdRaceData race = null;

        if (randomPool != null && randomPool.Length > 0)
        {
            int randomResult = Random.Range(0, randomPool.Length);
            race = randomPool[randomResult].data;
            umaRecipe.SetRace(GetRaceLibrary().GetRace(race.raceID));
        }
        else
        {
            Debug.LogError("Error, No random pool. " + RandObj);
            return null;
        }

        SetUMAData();

        if (race != null && race.slotElements.Length > 0)
        {
            DefineSlots(race);
        }
        else
        {
            Debug.LogError("Error, No race data. " + RandObj);
            return null;
        }

        AddAdditionalSlots();

        GenerateUMAShapes();

        if (animationController != null)
        {
            umaDynamicAvatar.animationController = animationController;
        }
        umaDynamicAvatar.Show();

        return RandObj;

    }

    protected virtual void GenerateUMAShapes()
    {
        UMADnaHumanoid umaDna = umaData.umaRecipe.GetDna<UMADnaHumanoid>();
        if (umaDna == null)
        {
            umaDna = new UMADnaHumanoid();
            umaData.umaRecipe.AddDna(umaDna);
        }

        if (randomDna)
        {
            umaDna.height = Random.Range(0.3f, 0.5f);
            umaDna.headSize = Random.Range(0.485f, 0.515f);
            umaDna.headWidth = Random.Range(0.4f, 0.6f);

            umaDna.neckThickness = Random.Range(0.495f, 0.51f);

            if (umaData.umaRecipe.raceData.raceName == "HumanMale")
            {
                umaDna.handsSize = Random.Range(0.485f, 0.515f);
                umaDna.feetSize = Random.Range(0.485f, 0.515f);
                umaDna.legSeparation = Random.Range(0.4f, 0.6f);
                umaDna.waist = 0.5f;
            }
            else
            {
                umaDna.handsSize = Random.Range(0.485f, 0.515f);
                umaDna.feetSize = Random.Range(0.485f, 0.515f);
                umaDna.legSeparation = Random.Range(0.485f, 0.515f);
                umaDna.waist = Random.Range(0.3f, 0.8f);
            }

            umaDna.armLength = Random.Range(0.485f, 0.515f);
            umaDna.forearmLength = Random.Range(0.485f, 0.515f);
            umaDna.armWidth = Random.Range(0.3f, 0.8f);
            umaDna.forearmWidth = Random.Range(0.3f, 0.8f);

            umaDna.upperMuscle = Random.Range(0.0f, 1.0f);
            umaDna.upperWeight = Random.Range(-0.2f, 0.2f) + umaDna.upperMuscle;
            if (umaDna.upperWeight > 1.0) { umaDna.upperWeight = 1.0f; }
            if (umaDna.upperWeight < 0.0) { umaDna.upperWeight = 0.0f; }

            umaDna.lowerMuscle = Random.Range(-0.2f, 0.2f) + umaDna.upperMuscle;
            if (umaDna.lowerMuscle > 1.0) { umaDna.lowerMuscle = 1.0f; }
            if (umaDna.lowerMuscle < 0.0) { umaDna.lowerMuscle = 0.0f; }

            umaDna.lowerWeight = Random.Range(-0.1f, 0.1f) + umaDna.upperWeight;
            if (umaDna.lowerWeight > 1.0) { umaDna.lowerWeight = 1.0f; }
            if (umaDna.lowerWeight < 0.0) { umaDna.lowerWeight = 0.0f; }

            umaDna.belly = umaDna.upperWeight;
            umaDna.legsSize = Random.Range(0.4f, 0.6f);
            umaDna.gluteusSize = Random.Range(0.4f, 0.6f);

            umaDna.earsSize = Random.Range(0.3f, 0.8f);
            umaDna.earsPosition = Random.Range(0.3f, 0.8f);
            umaDna.earsRotation = Random.Range(0.3f, 0.8f);

            umaDna.noseSize = Random.Range(0.3f, 0.8f);

            umaDna.noseCurve = Random.Range(0.3f, 0.8f);
            umaDna.noseWidth = Random.Range(0.3f, 0.8f);
            umaDna.noseInclination = Random.Range(0.3f, 0.8f);
            umaDna.nosePosition = Random.Range(0.3f, 0.8f);
            umaDna.nosePronounced = Random.Range(0.3f, 0.8f);
            umaDna.noseFlatten = Random.Range(0.3f, 0.8f);

            umaDna.chinSize = Random.Range(0.3f, 0.8f);
            umaDna.chinPronounced = Random.Range(0.3f, 0.8f);
            umaDna.chinPosition = Random.Range(0.3f, 0.8f);

            umaDna.mandibleSize = Random.Range(0.45f, 0.52f);
            umaDna.jawsSize = Random.Range(0.3f, 0.8f);
            umaDna.jawsPosition = Random.Range(0.3f, 0.8f);

            umaDna.cheekSize = Random.Range(0.3f, 0.8f);
            umaDna.cheekPosition = Random.Range(0.3f, 0.8f);
            umaDna.lowCheekPronounced = Random.Range(0.3f, 0.8f);
            umaDna.lowCheekPosition = Random.Range(0.3f, 0.8f);

            umaDna.foreheadSize = Random.Range(0.3f, 0.8f);
            umaDna.foreheadPosition = Random.Range(0.15f, 0.65f);

            umaDna.lipsSize = Random.Range(0.3f, 0.8f);
            umaDna.mouthSize = Random.Range(0.3f, 0.8f);
            umaDna.eyeRotation = Random.Range(0.3f, 0.8f);
            umaDna.eyeSize = Random.Range(0.3f, 0.8f);
            umaDna.breastSize = Random.Range(0.3f, 0.8f);
        }
    }

    private void AddAdditionalSlots()
    {
        umaData.AddAdditionalRecipes(additionalRecipes, UMAContext.FindInstance());
    }

    protected virtual void SetUMAData()
    {
        umaData.atlasResolutionScale = atlasResolutionScale;
    }

    private void DefineSlots(UMACrowdRandomSet.CrowdRaceData race)
    {
        float skinTone = Random.Range(0.1f, 0.6f);
        Color skinColor = new Color(skinTone + Random.Range(0.35f, 0.4f), skinTone + Random.Range(0.25f, 0.4f), skinTone + Random.Range(0.35f, 0.4f), 1);
        Color HairColor = new Color(Random.Range(0.1f, 0.9f), Random.Range(0.1f, 0.9f), Random.Range(0.1f, 0.9f), Random.Range(0.1f, 0.5f));
        var keywordsLookup = new HashSet<string>(keywords);
        UMACrowdRandomSet.Apply(umaData, race, skinColor, HairColor, keywordsLookup, GetSlotLibrary(), GetOverlayLibrary());
    }

    private RaceLibraryBase GetRaceLibrary()
    {
        if (umaContext != null) return umaContext.raceLibrary;
#pragma warning disable 618
        return raceLibrary;
#pragma warning restore 618
    }

    private SlotLibraryBase GetSlotLibrary()
    {
        if (umaContext != null) return umaContext.slotLibrary;
#pragma warning disable 618
        return slotLibrary;
#pragma warning restore 618
    }

    private OverlayLibraryBase GetOverlayLibrary()
    {
        if (umaContext != null) return umaContext.overlayLibrary;
#pragma warning disable 618
        return overlayLibrary;
#pragma warning restore 618
    }

}
