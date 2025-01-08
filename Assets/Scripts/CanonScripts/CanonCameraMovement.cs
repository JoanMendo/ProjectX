using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonCameraMovement : MonoBehaviour
{
    public LineRenderer lineRenderer;  // El LineRenderer que dibujará la parábola
    public int numPoints = 100;       // Número de puntos que se calculan para la curva
    public float a = 1f;              // Parámetro de la parábola (ajusta la curvatura)
    public float b = 0f;              // Parámetro de la parábola
    public float c = 0f;              // Parámetro de la parábola
    public float xMin = -5f;          // Rango de valores para x (debe ser ajustable)
    public float xMax = 5f;           // Rango de valores para x
    public float moveSpeed = 2f;      // Velocidad de movimiento de la parábola
    public float scaleSpeed = 2f;     // Velocidad de escalado de la parábola

    public Material lineMaterial;     // El material URP para la línea

    private void Start()
    {
        // Inicializa el LineRenderer
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = numPoints;
        lineRenderer.startWidth = 0.1f;  // Ancho de la línea
        lineRenderer.endWidth = 0.1f;    // Ancho de la línea

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
        lineRenderer.startColor = Color.red;  // Color de la línea
        lineRenderer.endColor = Color.red;    // Color de la línea
    }

    private void Update()
    {
        // Calcular y dibujar los puntos de la parábola
        DrawParabola();

        // Mover la parábola (usando las teclas de flecha o WASD)
        if (Input.GetKey(KeyCode.W))
            transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
        if (Input.GetKey(KeyCode.S))
            transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);
        if (Input.GetKey(KeyCode.A))
            transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
        if (Input.GetKey(KeyCode.D))
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);

        // Escalar la parábola (con las teclas + y -)
        if (Input.GetKey(KeyCode.Equals))  // tecla '+'
            ScaleParabola(1);
        if (Input.GetKey(KeyCode.Minus))  // tecla '-'
            ScaleParabola(-1);
    }

    // Función para calcular y dibujar los puntos de la parábola
    private void DrawParabola()
    {
        float step = (xMax - xMin) / (numPoints - 1);  // Espaciado entre los puntos
        for (int i = 0; i < numPoints; i++)
        {
            // Calculamos el valor de x
            float x = xMin + i * step;

            // Calculamos el valor de y usando la ecuación de la parábola
            float y = a * Mathf.Pow(x, 2) + b * x + c;

            // Asignamos la posición al LineRenderer
            lineRenderer.SetPosition(i, new Vector3(x, y, 0f));  // Ajusta los valores de Z si es necesario
        }
    }

    // Función para escalar la parábola
    private void ScaleParabola(int direction)
    {
        // Cambiar la parábola multiplicando los coeficientes
        a += direction * scaleSpeed * Time.deltaTime;
        b += direction * scaleSpeed * Time.deltaTime;
        c += direction * scaleSpeed * Time.deltaTime;
    }
}
