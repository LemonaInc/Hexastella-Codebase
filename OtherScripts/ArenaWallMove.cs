using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ArenaWallMove : MonoBehaviour
{
    private float endPosition = 20f;
    private float duration = 3f;
    
    private Transform player;

    private void Start()
    {
        player = FindObjectOfType<PlayerController>().transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == player)
        {
            Tweener dropDown = transform.DOMoveY(endPosition, duration);
        }
    }
}
