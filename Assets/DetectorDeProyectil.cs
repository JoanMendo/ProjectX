using UnityEngine;

public class DetectorDeProyectil : MonoBehaviour
{
    private bool isHit = false;
    private ShipMovement shipMovement;

    private void Start()
    {
        // Buscar el componente ShipMovement en el mismo gameObject
        shipMovement = GetComponent<ShipMovement>();
    }

    private void Update()
    {
        if (isHit)
        {
            // Mover el objeto hacia abajo en Y
            transform.position = new Vector3(
                transform.position.x,
                transform.position.y - 0.01f,
                transform.position.z
            );

            // Destruir el objeto cuando llega a Y = -50
            if (transform.position.y <= -50f)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Projectil"))
        {
            Debug.Log("Objetivo alcanzado");

            // Verificar si se encontró el componente ShipMovement
            if (shipMovement != null)
            {
                // Establecer moveSpeed a 0
                shipMovement.MoveSpeed = 0f;
            }

            // Activar el descenso
            isHit = true;
        }
    }
}