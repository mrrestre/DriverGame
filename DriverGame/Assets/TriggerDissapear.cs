using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDissapear : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Person_Dissapears")
        {
            other.gameObject.SetActive(false);
            Destroy(other.gameObject);
        }
    }
}
