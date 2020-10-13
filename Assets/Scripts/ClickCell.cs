using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickCell : MonoBehaviour
{
    public Transform chipObj;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        Instantiate(chipObj, transform.position, chipObj.rotation);
    }
}
