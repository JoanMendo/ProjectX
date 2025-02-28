
using Ditzelgames;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class WaterFloat : MonoBehaviour
{
    //public properties
    public float AirDrag = 1;
    public float WaterDrag = 10;
    public bool AffectDirection = true;
    public bool AttachToSurface = false;
    public Transform[] FloatPoints;

    // Altura inicial que queremos mantener
    private float initialHeight;
    // Factor para controlar la fuerza de estabilización
    public float stabilizationForce = 2.0f;

    //used components
    protected Rigidbody Rigidbody;
    protected Waves Waves;

    //water line
    protected float WaterLine;
    protected Vector3[] WaterLinePoints;

    //help Vectors
    protected Vector3 smoothVectorRotation;
    protected Vector3 TargetUp;
    protected Vector3 centerOffset;

    public Vector3 Center { get { return transform.position + centerOffset; } }

    void Awake()
    {
        //get components
        Waves = FindObjectOfType<Waves>();
        Rigidbody = GetComponent<Rigidbody>();
        Rigidbody.useGravity = false;

        //compute center
        WaterLinePoints = new Vector3[FloatPoints.Length];
        for (int i = 0; i < FloatPoints.Length; i++)
            WaterLinePoints[i] = FloatPoints[i].position;
        centerOffset = PhysicsHelper.GetCenter(WaterLinePoints) - transform.position;

        // Guardar la altura inicial
        initialHeight = transform.position.y;
    }

    void FixedUpdate()
    {
        //default water surface
        var newWaterLine = 0f;
        var pointUnderWater = false;
        var pointsUnderWaterCount = 0;

        //set WaterLinePoints and WaterLine
        for (int i = 0; i < FloatPoints.Length; i++)
        {
            //height
            WaterLinePoints[i] = FloatPoints[i].position;
            WaterLinePoints[i].y = Waves.GetHeight(FloatPoints[i].position);
            newWaterLine += WaterLinePoints[i].y / FloatPoints.Length;

            if (WaterLinePoints[i].y > FloatPoints[i].position.y)
            {
                pointUnderWater = true;
                pointsUnderWaterCount++;
            }
        }

        var waterLineDelta = newWaterLine - WaterLine;
        WaterLine = newWaterLine;

        //compute up vector
        TargetUp = PhysicsHelper.GetNormal(WaterLinePoints);

        // Calcular la diferencia entre la altura actual y la inicial
        float heightDifference = transform.position.y - initialHeight;

        // Aplicar resistencia al aire o agua
        Rigidbody.drag = pointUnderWater ? WaterDrag : AirDrag;

        // Calcular la fuerza de estabilización para mantener la altura inicial
        Vector3 stabilizingForce = Vector3.zero;

        if (pointUnderWater)
        {
            // Si está bajo el agua, aplicar fuerza hacia arriba proporcional a la profundidad
            float buoyancyFactor = (float)pointsUnderWaterCount / FloatPoints.Length;
            Vector3 upForce = (AffectDirection ? TargetUp : Vector3.up) * buoyancyFactor * stabilizationForce;

            // Si está por debajo de la altura inicial, aplicar más fuerza hacia arriba
            if (heightDifference < 0)
            {
                stabilizingForce = upForce * Mathf.Abs(heightDifference) * 2.0f;
            }
            // Si está por encima de la altura inicial, reducir la fuerza hacia arriba
            else
            {
                stabilizingForce = -Vector3.up * heightDifference * stabilizationForce;
            }
        }
        else
        {
            // Si está completamente fuera del agua, aplicar gravedad normal
            stabilizingForce = Physics.gravity;

            // Si está por debajo de la altura inicial y cerca del agua, aplicar una pequeña fuerza hacia arriba
            if (heightDifference < 0 && Mathf.Abs(WaterLine - Center.y) < 1.0f)
            {
                stabilizingForce += Vector3.up * Mathf.Abs(heightDifference) * stabilizationForce * 0.5f;
            }
        }

        // Aplicar la fuerza estabilizadora
        Rigidbody.AddForce(stabilizingForce, ForceMode.Acceleration);

        // Aplicar amortiguación vertical para evitar rebotes
        Rigidbody.velocity = new Vector3(
            Rigidbody.velocity.x,
            Rigidbody.velocity.y * 0.95f, // Reducir velocidad vertical gradualmente
            Rigidbody.velocity.z
        );

        //rotation - mantener orientación con la superficie del agua
        if (pointUnderWater)
        {
            TargetUp = Vector3.SmoothDamp(transform.up, TargetUp, ref smoothVectorRotation, 0.2f);
            Rigidbody.rotation = Quaternion.FromToRotation(transform.up, TargetUp) * Rigidbody.rotation;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        if (FloatPoints == null)
            return;

        for (int i = 0; i < FloatPoints.Length; i++)
        {
            if (FloatPoints[i] == null)
                continue;

            if (Waves != null && Application.isPlaying)
            {
                //draw cube
                Gizmos.color = Color.red;
                Gizmos.DrawCube(WaterLinePoints[i], Vector3.one * 0.3f);
            }

            //draw sphere
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(FloatPoints[i].position, 0.1f);
        }

        //draw center
        if (Application.isPlaying)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawCube(new Vector3(Center.x, WaterLine, Center.z), Vector3.one * 1f);
            Gizmos.DrawRay(new Vector3(Center.x, WaterLine, Center.z), TargetUp * 1f);

            // Visualizar la altura inicial
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(
                new Vector3(Center.x - 1, initialHeight, Center.z),
                new Vector3(Center.x + 1, initialHeight, Center.z)
            );
        }
    }
}