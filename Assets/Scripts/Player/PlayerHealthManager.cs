﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class PlayerHealthManager : MonoBehaviour,IDamageable
{
	
	Health playerHealth;

	[SerializeField] private Volume globalVolume;

	private Vignette profileVignette;
	private ChromaticAberration profileChromaticAberration;
	
	
	[SerializeField] private List<GameObject> bloodstains;


	private int lowHPThreshold = 50;
	public delegate void PlayerHealthState();

	public event PlayerHealthState lowHpEvent, highHPEvent;
	
	// Start is called before the first frame update
	void  Awake()
	{
		playerHealth = new Health(100);
		globalVolume.profile.TryGet(out profileVignette);
		globalVolume.profile.TryGet(out profileChromaticAberration);
		
	}


	public void HealHealth(int healAmount)
	{

		playerHealth.currenthealth =
			Mathf.Clamp(playerHealth.currenthealth + healAmount, 0, playerHealth.maxHealth);
		
		if(playerHealth.currenthealth >= lowHPThreshold) HighHPEffects();
		
		
		
	}

	public float GetPlayerHP => playerHealth.currenthealth;
	
	
	public void TakeDamage(float damage)
	{
		Debug.Log(damage);
		playerHealth.currenthealth -= damage;
		
		if(playerHealth.currenthealth <= lowHPThreshold) LowHPEffects();
	}

	private void LowHPEffects()
	{
		lowHpEvent?.Invoke();
		foreach (GameObject stain in bloodstains)
		{
			stain.SetActive(true);
		}
		
		profileVignette.intensity.value = 0.5f;
		profileChromaticAberration.active = true;
	}

	private void HighHPEffects()
	{
		highHPEvent?.Invoke();
		foreach (GameObject stain in bloodstains)
		{
			stain.SetActive(false);
		}
		
		profileVignette.intensity.value = 0.25f;

		profileChromaticAberration.active = false;
	}
}
