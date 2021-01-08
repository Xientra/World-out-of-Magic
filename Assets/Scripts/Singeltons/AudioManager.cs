using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager singelton;

	private AudioSource audioSource;


	private void Awake()
	{
		singelton = this;
		audioSource = GetComponent<AudioSource>();
	}


	[Header("Button Sounds:")]

	[SerializeField]
	private AudioClip elementDiscoveredSound;

	[Space(5)]

	[SerializeField]
	private AudioUseType onClickSounds;
	[SerializeField]
	private AudioUseType onHoverSounds;

	public void PlayElementDiscoveredSound()
	{
		PlayAdditionalClip(elementDiscoveredSound, 1, 1);
	}

	public void PlayBtnClickSound()
	{
		PlayAudioUseType(onClickSounds);
	}

	public void PlayBtnHoverSound()
	{
		PlayAudioUseType(onHoverSounds);
	}

	private void PlayAudioUseType(AudioUseType aut)
	{
		audioSource.volume = aut.volume;
		audioSource.pitch = aut.pitch;

		int i = aut.lastIndexUsed;
		while (i == aut.lastIndexUsed)
			i = Random.Range(0, aut.audioClips.Length);

		audioSource.PlayOneShot(aut.audioClips[i]);
		aut.lastIndexUsed = i;
	}

	private void PlayAdditionalClip(AudioClip clip, float volume, float pitch)
	{
		AudioSource newAudioSource = gameObject.AddComponent<AudioSource>();

		newAudioSource.volume = volume;
		newAudioSource.pitch = pitch;

		newAudioSource.clip = clip;
		newAudioSource.Play();
		StartCoroutine(DestroyAudioSourceAfterCompleation(newAudioSource));
	}

	private IEnumerator DestroyAudioSourceAfterCompleation(AudioSource audioSource)
	{
		float time = audioSource.clip.length;
		yield return new WaitForSecondsRealtime(time + 1);
		Destroy(audioSource);
	}

	[System.Serializable]
	public class AudioUseType
	{
		[Range(0f, 1f)]
		public float volume = 1f;

		[Range(-3f, 3f)]
		public float pitch = 1f;

		public AudioClip[] audioClips;
		[HideInInspector]
		public int lastIndexUsed = -1;
	}
}
