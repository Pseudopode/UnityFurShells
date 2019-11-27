using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;


public static class Extensions
{
    public static void SetAsFade(this Material material)
    {
        material.SetFloat("_Mode", 2);
        material.SetOverrideTag("RenderType", "Fade");
        material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        material.SetInt("_ZWrite", 0);
        material.DisableKeyword("_ALPHATEST_ON");
        material.EnableKeyword("_ALPHABLEND_ON");
        material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        material.renderQueue = 3000;
    }

    public static void SetAlpha(this Material material, float value)
    {
        Color color = material.color;
        color.a = value;
        material.color = color;
    }

    public static void SetColorFloat(this Material material, float value)
    {
        Color color = material.color;
        color.r = value;
        color.g = value;
        color.b = value;
        material.color = color;
    }
}



public class PickObject : MonoBehaviour
{
    public GameObject objToDuplicate;

    public bool duplicate = false;

    public int shellNumber = 5;
    public GameObject shellRoot;
    public float shellScaleValue = 0.1f;

    public float colorStartingValue = 0.75f;

    public Material baseMaterial;
    public Material shellMaterial;
    public Texture2D shellTexture;

    public Button buttonFur;

    public GameObject infoText;

    

    public void createFur()
    {

        {
            float scaleX = 1, scaleY = 1, scaleZ = 1;
            float localY = 0.0f;
            if (objToDuplicate != null)
            {
                shellRoot = Instantiate(objToDuplicate);
                shellRoot.name = "ShellRoot";
                scaleX = shellRoot.transform.localScale.x;
                scaleY = shellRoot.transform.localScale.y;
                scaleZ = shellRoot.transform.localScale.z;

                //we just need to set the parent GameObject to have the same material (only useful for this demo)
                Material m = Instantiate(baseMaterial);
                objToDuplicate.GetComponent<Renderer>().sharedMaterial = m;
            }

            for (int i = 0; i < shellNumber; i++)
            {
                GameObject shell = Instantiate(objToDuplicate);
                shell.transform.parent = shellRoot.transform;
                scaleX = scaleX + shellScaleValue;
                scaleY = scaleY + shellScaleValue;
                scaleZ = scaleZ + shellScaleValue;
                shell.transform.localScale = new Vector3(scaleX, scaleY, scaleZ);

                Material m = Instantiate(shellMaterial);
                m.SetAsFade();

                Color color = m.color;
                color.r = Mathf.Lerp(colorStartingValue, 1.0f, shellNumber);
                color.g = Mathf.Lerp(colorStartingValue, 1.0f, shellNumber);
                color.b = Mathf.Lerp(colorStartingValue, 1.0f, shellNumber);
                m.color = color;

                m.name = "Shell_" + i.ToString();
                m.SetTexture("_MainTex", shellTexture);

                float layerCoeff = ((float)i) / ((float)shellNumber);
                Debug.Log("layerCoeff: " + layerCoeff);
                Debug.Log("Value: " + Mathf.Lerp(colorStartingValue, 1.0f, layerCoeff));

                m.SetAlpha(Mathf.Lerp(colorStartingValue, 1.0f, layerCoeff));

                shell.GetComponent<Renderer>().sharedMaterial = m;
                shell.GetComponent<Renderer>().material.SetFloat("_Glossiness", Mathf.Lerp(colorStartingValue, 1.0f, layerCoeff) - 0.5f);
            }

            shellRoot.transform.parent = objToDuplicate.transform;

            Material mRoot = Instantiate(baseMaterial);
            shellRoot.GetComponent<Renderer>().material = mRoot;

            duplicate = false;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hitInfo = new RaycastHit();
            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
            if (hit)
            {
                objToDuplicate = hitInfo.transform.gameObject;

                buttonFur.interactable = true;
                infoText.SetActive(false);
            }
        }
    }
}
