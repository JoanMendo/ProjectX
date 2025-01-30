using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoExplosion : MonoBehaviour
{
    [Header("Ammo Explosion Variables")]
    public float maxmimumSpeed = 20f;
    public ParticleSystem explosionEffect;
    private Rigidbody Rigidbody;
    private float lastVelocity;
    void Start()
    {
        Rigidbody = GetComponent<Rigidbody>();
    }
    private void Update()
    {

    }

    private void FixedUpdate()
    {
        lastVelocity = Rigidbody.velocity.magnitude;
    }

    // Update is called once per frame
    public void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision");
        if (lastVelocity> maxmimumSpeed)
        {
            if (explosionEffect != null)
            {
                Instantiate(explosionEffect, transform.position, Quaternion.identity);
            }
            
            Destroy(gameObject);
            Debug.Log("Explosion");
            DeathManager.Instance.OnDeath();

        }
    }
}

