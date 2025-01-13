using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouceObj : MonoBehaviour
{
    [SerializeField] float offset;
    Vector3 mousePos;

 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = Input.mousePosition;
        
        mousePos.z = offset;
        this.transform.position = mousePos;
    }
}
