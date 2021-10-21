using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiscMenuManager : MonoBehaviour
{
	public void Btn_SwitchObjectActive(GameObject go)
	{
		go.SetActive(!go.activeSelf);
	}

	public void Btn_Exit()
	{
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
	}

	public void Btn_ShowHint()
	{
		Debug.Log("Tell Game Data to show a hint now...");
	}

	public void Btn_SetSoundeffects(bool value)
	{
		AudioManager.singelton.muted = !value;
	}
}
