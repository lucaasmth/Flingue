using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifetime = 2f;

    private GameObject _owner;
    
    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    public void SetOwner(GameObject owner)
    {
        _owner = owner;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.Equals(_owner)) return;
        
        Health health = other.gameObject.GetComponent<Health>();
        if (health != null)
        {
            health.TakeDamage(50);
            Destroy(gameObject);
            return;
        }

        if (other.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log(other.gameObject);
            Destroy(gameObject, 0.05f);
            GetComponent<Renderer>().enabled = false;
        } 
    }
}
