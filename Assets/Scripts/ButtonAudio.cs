using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Button))]
public class ButtonAudio : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler
{
	public  void OnPointerClick(PointerEventData eventData)
	{
		AudioManager.singelton.PlayBtnClickSound();
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		AudioManager.singelton.PlayBtnHoverSound();
	}
}
