using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyBox : MonoBehaviour
{
    public Material skyBoxMaterial;
    public Color skyBoxColor;
    public bool isThereFog;
    public float fogDensity;

    //Reference
    public GameObject directionalLight;
    public float newDirectionalIntesity = 1f;

    public void SetSkyBoxSettings()
    {
        RenderSettings.skybox = skyBoxMaterial;
        RenderSettings.ambientSkyColor = skyBoxColor;
        RenderSettings.fog = isThereFog;
        
        if(isThereFog)
        {
            RenderSettings.fogDensity = fogDensity;
        }

        directionalLight.GetComponent<Light>().intensity = newDirectionalIntesity;
    }
}
