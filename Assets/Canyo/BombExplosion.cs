using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombExplosion : MonoBehaviour
{

    [SerializeField] GameObject sparks;
    Animator animator;
    void Start()
    {
        sparks.SetActive(false);
        animator = GetComponent<Animator>();
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FireStick"))
        {
            Debug.Log("Colision");
            if (HandleFire.instance.isFireActive)
            {
                sparks.SetActive(true);
                animator.SetBool("IsFiring", true);

            }          
        }
    }

}
