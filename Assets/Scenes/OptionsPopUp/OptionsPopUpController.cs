using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsPopUpController : MonoBehaviour 
{
	[SerializeField] [SceneListAttribute] string levelSelectSceneName;

	[SerializeField] Toggle vfxToggle;
	[SerializeField] Toggle musicToggle;
    [SerializeField] SamplesPopUpManager samplesPopUpManager;

    [SerializeField] GameObject[] objectsToShowInGameMode;
    [SerializeField] GameObject[] objectsToShowInMenuMode;    

	bool _vfxEnabled;
	bool _musicEnabled;

	public bool VfxEnabled 
	{ 
		get { return _vfxEnabled; }
		set 
		{ 
			if(_vfxEnabled != value)
			{
				_vfxEnabled = value; 
				PlayerPrefs.SetString("VfxEnabled", _vfxEnabled.ToString()); 
			}
		}
	}

	public bool MusicEnabled 
	{
		get { return _musicEnabled; }
		set 
		{ 
			if(_musicEnabled != value)
			{
				_musicEnabled = value;
				PlayerPrefs.SetString("MusicEnabled", _musicEnabled.ToString());
			} 
		}
	}

	void Awake()
	{
		_vfxEnabled = bool.Parse(PlayerPrefs.GetString("VfxEnabled", true.ToString()));
		_musicEnabled = bool.Parse(PlayerPrefs.GetString("MusicEnabled", true.ToString()));

		vfxToggle.isOn = _vfxEnabled;
		musicToggle.isOn = _musicEnabled;

		vfxToggle.onValueChanged.AddListener(UpdateToggle);
		musicToggle.onValueChanged.AddListener(UpdateToggle);

		bool openedFromLevelSelect = OpenedFromLevelSelect();

        foreach (var item in objectsToShowInGameMode)
        {
            item.SetActive(!openedFromLevelSelect);
        }

        foreach (var item in objectsToShowInMenuMode)
        {
            item.SetActive(openedFromLevelSelect);
        }
	}

	void Start()
	{
		Time.timeScale = 0;
	}

	bool OpenedFromLevelSelect()
	{
		for(int i = 0, max = UnityEngine.SceneManagement.SceneManager.sceneCount; i < max; ++i)
		{
			if(UnityEngine.SceneManagement.SceneManager.GetSceneAt(i).name.Equals(levelSelectSceneName))
			{
				return true;
			}
		}
		return false;
	}

	public void UpdateToggle(bool newValue)
	{
		VfxEnabled = vfxToggle.isOn;
		MusicEnabled = musicToggle.isOn;
	}    

    public void ClearPlayerData()
    {
        PlayerData.RemoveValuesFromPlayerPrefs();
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetSceneAt(0).buildIndex);
    }

	void OnDestroy()
	{
		Time.timeScale = 1;
	}
}
