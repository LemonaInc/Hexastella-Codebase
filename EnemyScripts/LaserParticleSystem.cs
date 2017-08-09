using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserParticleSystem : MonoBehaviour {

	ParticleSystem[] pSystems;

	// Use this for initialization
	void Start () {
		pSystems = GetComponentsInChildren<ParticleSystem>(true);
	}
	
	public void Play()
	{
		foreach (ParticleSystem ps in pSystems)
		{
			ps.Stop();
			ps.Play();
		}
	}
}
