using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    public Image barImage;
    private float currentHealth;


    // Start is called before the first frame update
    void Start()
    {
        currentHealth = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void findImageBar()
    {
        barImage = transform.Find("Bar").GetComponent<Image>();
    }

    public void setHealthBarFull()
    {
        barImage.fillAmount = 1f;
    }

    public void setHealthBarDamage(float damageSize)
    {
        currentHealth -= damageSize;
        barImage.fillAmount = currentHealth;
    }

    public void setHealthBarValue(float healthValue)
    {
        barImage.fillAmount = healthValue;
    }



}
