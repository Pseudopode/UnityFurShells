using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetStandardMaterial : MonoBehaviour
{
    public GameObject objectToSet;
    public Material mat;
    // Start is called before the first frame update
    void Start()
    {
        objectToSet.GetComponent<Renderer>().material = mat;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
