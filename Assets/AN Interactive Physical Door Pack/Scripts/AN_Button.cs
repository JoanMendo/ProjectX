using UnityEngine;

public class AN_Button : MonoBehaviour
{
    public GameObject canones;

    [Tooltip("Si está bloqueado, no se puede usar")]
    public bool Locked = false;

    [Tooltip("Indica si la palanca está activada")]
    public bool isLeverUp = false;

    Animator anim;
    Animator canon;

    void Start()
    {
        anim = GetComponent<Animator>();
        canon= canones.GetComponent<Animator>();
    }

    void Update()
    {
        if (!Locked && Input.GetKeyDown(KeyCode.E) && NearView())
        {
            // Cambia el estado de la palanca
            isLeverUp = !isLeverUp;

            // Activa o desactiva la animación según el estado
            anim.SetBool("LeverUp", isLeverUp);
            canon.SetBool("localizacion",isLeverUp);
        }
    }

    bool NearView() // Verifica si el jugador está cerca del objeto interactivo
    {
        float distance = Vector3.Distance(transform.position, Camera.main.transform.position);
        Vector3 direction = transform.position - Camera.main.transform.position;
        float angleView = Vector3.Angle(Camera.main.transform.forward, direction);
        return angleView < 45f && distance < 2f;
    }
}