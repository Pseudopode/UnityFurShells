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
    //public bool export = false;

    public Button buttonFur;

    public GameObject infoText;

   
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void createFur()
    {
        //if (duplicate)
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

                //localY = shellRoot.transform.position.y;
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
                //Material mat = new Material(Shader.Find("Standard");
                m.SetAsFade();

                Color color = m.color;
                //Color color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                color.r = 1.0f / shellNumber * i + colorStartingValue;
                color.g = 1.0f / shellNumber * i + colorStartingValue;
                color.b = 1.0f / shellNumber * i + colorStartingValue;
                m.color = color;

                m.name = "Shell_" + i.ToString();
                m.SetTexture("_MainTex", shellTexture);
                //m.SetAlpha(1.0f - (1.0f / i));
                m.SetAlpha((1.0f / (i / 2)));

                if (i == 0)
                {
                    Debug.Log("i: " + i + "Alpha value: " + color.a);
                    m.SetAlpha(0.9f);
                }
                if (i == 1)
                {
                    Debug.Log("i: " + i + "Alpha value: " + color.a);
                    m.SetAlpha(0.75f);
                }

                shell.GetComponent<Renderer>().sharedMaterial = m;
                //shell.GetComponent<Renderer>().material.SetAsFade();
                //shell.GetComponent<Renderer>().material.SetAlpha(1.0f - (1.0f / i));
                //shell.GetComponent<Renderer>().material.name = "Shell_" + i.ToString();
                //shell.GetComponent<Renderer>().material.SetTexture("_MainTex", shellTexture);
                //shell.GetComponent<Renderer>().material.SetColor("_Color", color);
                // shell.GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
            }

            shellRoot.transform.parent = objToDuplicate.transform;
            //objToDuplicate.transform.position = new Vector3(0.0f, 0.0f, 0.0f);
            //shellRoot.transform.position = new Vector3(0.0f, 0.0f, 0.0f);

            Material mRoot = Instantiate(baseMaterial);
            shellRoot.GetComponent<Renderer>().material = mRoot;

            duplicate = false;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
           // Debug.Log("Mouse is down");

            RaycastHit hitInfo = new RaycastHit();
            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
            if (hit)
            {
                objToDuplicate = hitInfo.transform.gameObject;

                buttonFur.interactable = true;
                infoText.SetActive(false);
               /* Debug.Log("Hit " + hitInfo.transform.gameObject.name);
                if (hitInfo.transform.gameObject.tag == "Construction")
                {
                    Debug.Log("It's working!");
                }
                else
                {
                    Debug.Log("nopz");
                }*/
            }
            /*else
            {
                Debug.Log("No hit");
            }
            Debug.Log("Mouse is down");*/
        }



        


    }
}
