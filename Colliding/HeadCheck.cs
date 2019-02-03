using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadCheck : MonoBehaviour {

    private OnHit _onHit;
    public GameObject opponent;

    private void Awake()
    {
      _onHit = GetComponentInParent<OnHit>();
    }

    // add it to the player head
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "PlayerFeet")
        {
           _onHit.ExecuteDamage();
            other.gameObject.GetComponentInParent<OnHit>().KnockBack(20);
        }
    }
}
