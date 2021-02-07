using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinController : MonoBehaviour
{
    private float rotateZ = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(transform.position.x, 0.0f + 2, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, rotateZ, Space.Self);
    }

}
