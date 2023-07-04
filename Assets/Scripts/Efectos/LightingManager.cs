using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class LightingManager : MonoBehaviour
{
    //Scene References
    [SerializeField] private Light DirectionalLight;
    [SerializeField] private LightingPreset Preset;
    //Variables
    [SerializeField, Range(0, 24)] public float TimeOfDay;
    [SerializeField, Range(-10, 10)] private float speedMultiplier;  // used to adjust the cycle time. Note that values < 0 will reverse it!
    [SerializeField, Range(1, 10)] private float nightSpeed; // how much to speed up late-night hours
    [SerializeField] private float maxIntensity = 1.5f;
    private float baseIntensity = 0f;
    [SerializeField] private float maxShadowStrength = 1f;
    [SerializeField] private float minShadowStrength = 0.2f;

    [Header("Skybox")]
    [SerializeField]  private float skyboxBaseIntensity = 0.3f;
    [SerializeField] private float skyboxMaxIntensity = 0.6f;
    [SerializeField] private Material skyboxMaterial;
    [SerializeField] private GameObject sky;
    [SerializeField] private GameObject stars;
    [SerializeField] private GameObject sun;
    [SerializeField] private GameObject moon;

    [Header("Farolas")]
    [SerializeField] private List<Light> farolas;
    [SerializeField] private float maxIntensityFarola;

    private float nightSpeedUpStart = 20f;
    private float nightSpeedUpEnd = 4f;
    private float dawn = 6f;
    private float dusk = 18f;
    private float noon = 12f;


    private FadeMusic fadeMusic;
    private GameManager gameManager;
    public bool daySongPlaying, nightSongPlaying, duskSongPlaying;

    private void Start()
    {
        // default values
        baseIntensity = maxIntensity / 2f;
        skyboxMaterial.mainTextureOffset = new Vector2((TimeOfDay / 24f) + 0.5f, 0);
        daySongPlaying = true;
        nightSongPlaying = false;
        duskSongPlaying = false;


        fadeMusic = GetComponent<FadeMusic>();
        gameManager = GetComponent<GameManager>();

        //De 0 a 0.4 atardecer
        //De 0.5 a 0.6 noche
        //De 0.7 a 1 amanece

    }

    private void Update()
    {
        if (Preset == null)
            return;

        if (Application.isPlaying)
        {

                TimeOfDay += Time.deltaTime * speedMultiplier;
                

                // adjust light intensity and shadow softness for time of day
                if (TimeOfDay >= dawn && TimeOfDay <= noon)
                {
                    //Amaneciendo
                    if (!daySongPlaying)
                    {
                        fadeMusic.CambiarCancion(gameManager.canciones[1]);
                        daySongPlaying = true;
                        duskSongPlaying = false;
                        nightSongPlaying = false;
                    }
                    
                    DirectionalLight.intensity = baseIntensity + (baseIntensity / (noon - dawn)) * (TimeOfDay - dawn);
                    DirectionalLight.shadowStrength = minShadowStrength + ((maxShadowStrength - minShadowStrength) / (noon - dawn)) * (TimeOfDay - dawn);


                    RenderSettings.ambientIntensity = skyboxBaseIntensity + ((skyboxMaxIntensity - skyboxBaseIntensity) / (noon - dawn)) * (TimeOfDay - dawn);


                    //Apagar farolas
                    foreach (Light farola in farolas)
                    {
                        if (farola.intensity >= 0) farola.intensity -= 1f * Time.deltaTime;
                        else farola.enabled = false;
                    }

                    //Quitar estrellas y luna y poner sol
                    stars.SetActive(false);
                    moon.SetActive(false);
                    sun.SetActive(true);
                }
                else if (TimeOfDay > noon && TimeOfDay <= dusk)
                {
                    //Atardeciendo
                    if (!duskSongPlaying)
                    {
                        fadeMusic.CambiarCancion(gameManager.canciones[2]);
                        duskSongPlaying = true;
                        daySongPlaying = false;
                        nightSongPlaying = false;
                    }
                    
                    DirectionalLight.intensity = baseIntensity + (baseIntensity / (dusk - noon)) * (dusk - TimeOfDay);
                    DirectionalLight.shadowStrength = minShadowStrength + ((maxShadowStrength - minShadowStrength) / (dusk - noon)) * (dusk - TimeOfDay);

                    RenderSettings.ambientIntensity = skyboxBaseIntensity + ((skyboxMaxIntensity - skyboxBaseIntensity) / (dusk - noon)) * (dusk - TimeOfDay);

                    //Quitar sol y poner luna y estrellas
                    stars.SetActive(true);
                    moon.SetActive(true);
                    sun.SetActive(false);
                }
                else
                {
                    //noche
                    if (!nightSongPlaying)
                    {
                        fadeMusic.CambiarCancion(gameManager.canciones[3]);
                        duskSongPlaying = false;
                        daySongPlaying = false;
                        nightSongPlaying = true;
                    }
                    DirectionalLight.intensity = baseIntensity;
                    DirectionalLight.shadowStrength = minShadowStrength;
                    RenderSettings.ambientIntensity = skyboxBaseIntensity;


                    //Encender farolas
                    foreach (Light farola in farolas)
                    {
                        if(farola.enabled == false) farola.enabled = true;
                        else if(farola.intensity <= maxIntensityFarola) farola.intensity += 1f * Time.deltaTime;
                    }

                }
            
            TimeOfDay %= 24; //Modulus to ensure always between 0-24
            UpdateLighting(TimeOfDay / 24f);
        }
        else
        {
            UpdateLighting(TimeOfDay / 24f);
        }
    }

    private void UpdateLighting(float timePercent)
    {
        //Set ambient and fog
        RenderSettings.ambientLight = Preset.AmbientColor.Evaluate(timePercent);
        RenderSettings.fogColor = Preset.FogColor.Evaluate(timePercent);

        //If the directional light is set then rotate and set it's color, I actually rarely use the rotation because it casts tall shadows unless you clamp the value
        if (DirectionalLight != null)
        {
            DirectionalLight.color = Preset.DirectionalColor.Evaluate(timePercent);

            DirectionalLight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360f) - 90f, 170f, 0));
            sky.transform.localRotation = Quaternion.Euler(new Vector3(0, (timePercent * 360f) - 90f, 0));

            skyboxMaterial.mainTextureOffset = new Vector2((TimeOfDay / 24f) + 0.5f, 0);
        }

    }

    //Try to find a directional light to use if we haven't set one
    private void OnValidate()
    {
        if (DirectionalLight != null)
            return;

        //Search for lighting tab sun
        if (RenderSettings.sun != null)
        {
            DirectionalLight = RenderSettings.sun;
        }
        //Search scene for light that fits criteria (directional)
        else
        {
            Light[] lights = GameObject.FindObjectsOfType<Light>();
            foreach (Light light in lights)
            {
                if (light.type == LightType.Directional)
                {
                    DirectionalLight = light;
                    return;
                }
            }
        }
    }
}