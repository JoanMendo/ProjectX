using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightObjects : MonoBehaviour
{
    public Camera playerCamera;
    public LayerMask layers;
    public Material highlightMaterial;

    private GameObject currentObject = null;
    // Update is called once per frame
    void Update()
    {
        HighlightObjectsWithMaterial();
    }
    void HighlightObjectsWithMaterial()
    {

        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 2.5f, layers))
        {
            
                if (currentObject == null)
                {

                    Material[] material = hit.collider.GetComponent<MeshRenderer>().materials;
                    Material[] newMaterials = new Material[material.Length + 1];
                    for (int i = 0; i < material.Length; i++)
                    {
                        newMaterials[i] = material[i]; // Copia los materiales antiguos
                    }
                    newMaterials[newMaterials.Length - 1] = highlightMaterial; // Añade el nuevo material

                    hit.collider.GetComponent<MeshRenderer>().materials = newMaterials;
                    currentObject = hit.collider.gameObject;
                }  
                else if (currentObject != hit.collider.gameObject)
                {
                    Material[] material = currentObject.GetComponent<MeshRenderer>().materials;
                    Material[] newMaterials = new Material[material.Length - 1];
                    for (int i = 0; i < newMaterials.Length; i++)
                    {
                        newMaterials[i] = material[i]; // Copia los materiales antiguos
                    }
                    currentObject.GetComponent<MeshRenderer>().materials = newMaterials; // Elimina el material de resaltado
                    currentObject = hit.collider.gameObject;

                    Material[] material2 = hit.collider.GetComponent<MeshRenderer>().materials;
                    Material[] newMaterials2 = new Material[material2.Length + 1];
                    for (int i = 0; i < material2.Length; i++)
                    {
                        newMaterials2[i] = material2[i]; // Copia los materiales antiguos
                    }
                    newMaterials2[newMaterials2.Length - 1] = highlightMaterial; // Añade el nuevo material

                    hit.collider.GetComponent<MeshRenderer>().materials = newMaterials2;
                }
            
            
           
        }
        else if (currentObject != null)
        {
            Material[] material = currentObject.GetComponent<MeshRenderer>().materials;
            Material[] newMaterials = new Material[material.Length - 1];
            for (int i = 0; i < newMaterials.Length; i++)
            {
                newMaterials[i] = material[i]; // Copia los materiales antiguos
            }
            currentObject.GetComponent<MeshRenderer>().materials = newMaterials; // Elimina el material de resaltado
            currentObject = null;
        }

    }
}
