using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OptionsMenu : MonoBehaviour {

    public Slider VolumeSlider;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        AudioListener.volume = VolumeSlider.value;
        
	}

    public void FastestQuality()
    {
        QualitySettings.SetQualityLevel(0);
    }
    public void FastQuality()
    {
        QualitySettings.SetQualityLevel(1);
    }
    public void SimpleQuality()
    {
        QualitySettings.SetQualityLevel(2);
    }
    public void GoodQuality()
    {
        QualitySettings.SetQualityLevel(3);
    }
    public void BeautifulQuality()
    {
        QualitySettings.SetQualityLevel(4);
    }
    public void FantasticQuality()
    {
        QualitySettings.SetQualityLevel(5);
    }

}
