using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonCameraMovement : MonoBehaviour
{
    public LineRenderer lineRenderer;  // El LineRenderer que dibujar� la par�bola
    public int numPoints = 100;       // N�mero de puntos que se calculan para la curva
    public float a = 1f;              // Par�metro de la par�bola (ajusta la curvatura)
    public float b = 0f;              // Par�metro de la par�bola
    public float c = 0f;              // Par�metro de la par�bola
    public float xMin = -5f;          // Rango de valores para x (debe ser ajustable)
    public float xMax = 5f;           // Rango de valores para x
    public float moveSpeed = 2f;      // Velocidad de movimiento de la par�bola
    public float scaleSpeed = 2f;     // Velocidad de escalado de la par�bola

    public Material lineMaterial;     // El material URP para la l�nea

    private void Start()
    {
        // Inicializa el LineRenderer
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = numPoints;
        lineRenderer.startWidth = 0.1f;  // Ancho de la l�nea
        lineRenderer.endWidth = 0.1f;    // Ancho de la l�nea

        // Asigna el material URP al LineRenderer
        if (lineMaterial != null)
        {
            lineRenderer.material = lineMaterial;
        }
        else
        {
            // Si no se ha asignado el material, usa un material URP predeterminado
            lineRenderer.material = new Material(Shader.Find("Universal Render Pipeline/Unlit"));
        }

        // Ajuste de color
        lineRenderer.startColor = Color.red;  // Color de la l�nea
        lineRenderer.endColor = Color.red;    // Color de la l�nea
    }

    private void Update()
    {
        // Calcular y dibujar los puntos de la par�bola
        DrawParabola();

        // Mover la par�bola (usando las teclas de flecha o WASD)
        if (Input.GetKey(KeyCode.W))
            transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
        if (Input.GetKey(KeyCode.S))
            transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);
        if (Input.GetKey(KeyCode.A))
            transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
        if (Input.GetKey(KeyCode.D))
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);

        // Escalar la par�bola (con las teclas + y -)
        if (Input.GetKey(KeyCode.Equals))  // tecla '+'
            ScaleParabola(1);
        if (Input.GetKey(KeyCode.Minus))  // tecla '-'
            ScaleParabola(-1);
    }

    // Funci�n para calcular y dibujar los puntos de la par�bola
    private void DrawParabola()
    {
        float step = (xMax - xMin) / (numPoints - 1);  // Espaciado entre los puntos
        for (int i = 0; i < numPoints; i++)
        {
            // Calculamos el valor de x
            float x = xMin + i * step;

            // Calculamos el valor de y usando la ecuaci�n de la par�bola
            float y = a * Mathf.Pow(x, 2) + b * x + c;

            // Asignamos la posici�n al LineRenderer
            lineRenderer.SetPosition(i, new Vector3(x, y, 0f));  // Ajusta los valores de Z si es necesario
        }
    }

    // Funci�n para escalar la par�bola
    private void ScaleParabola(int direction)
    {
        // Cambiar la par�bola multiplicando los coeficientes
        a += direction * scaleSpeed * Time.deltaTime;
        b += direction * scaleSpeed * Time.deltaTime;
        c += direction * scaleSpeed * Time.deltaTime;
    }
}
