using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {

	internal Rigidbody rb;
	internal Animator anim; 

	protected virtual void Start()
    {
		rb = GetComponent<Rigidbody>();
		anim = GetComponentInChildren<Animator>(); 
	}
}