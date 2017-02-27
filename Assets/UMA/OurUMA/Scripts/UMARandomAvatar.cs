/// Adapted from UMA Crowd for use in AnyMeansNessicary by Tristan Bampton

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UMA;

public class UMARandomAvatar : MonoBehaviour {

    public UMARandomAvatarController RandomController;
    public UMARecipeBase[] additionalRecipes;


    // Use this for initialization
    void Start()
    {
        if (RandomController == null) RandomController = FindObjectOfType<UMARandomAvatarController>();
        if (RandomController != null)
        {
            RandomController.GenerateOneUMA(additionalRecipes, gameObject);
        }
        else
        {
            Debug.LogError("No UMARandomAvatarController in scene.");
        }
    }
	
}
