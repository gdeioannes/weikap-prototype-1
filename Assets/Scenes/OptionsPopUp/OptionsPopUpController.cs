using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsPopUpController : MonoBehaviour 
{
	[SerializeField] [SceneListAttribute] string levelSelectSceneName;

	[SerializeField] Toggle vfxToggle;
	[SerializeField] Toggle musicToggle;

	[SerializeField] GameObject creditsButtonContainer;
	[SerializeField] GameObject backToMenuButtonContainer;

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
		creditsButtonContainer.SetActive(openedFromLevelSelect);
		backToMenuButtonContainer.SetActive(!openedFromLevelSelect);
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

	void OnDestroy()
	{
		Time.timeScale = 1;
	}
}
