using UnityEngine;

public class DetectorDeProyectil : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Projectil"))
        {
            Debug.Log("Objetivo alcanzado");
        }
    }
}