using MiddleAges.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisions : MonoBehaviour
{
    private GameEvents events;


    private void Start()
    {
        events = GetComponent<GameEvents>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Wall")
        {
            events.FireWallHit(this, 0);
        }
    }
}
