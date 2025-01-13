using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePosition : MonoBehaviour
{
    private GameObject cursorObj;
    [SerializeField] float offset;
    Vector3 screenMousePos;
    Vector3 worldMousePos;

    public Vector3 GetWorldMousePos() { return worldMousePos; }
    // Start is called before the first frame update
    void Start()
    {
        cursorObj = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        screenMousePos = Input.mousePosition;
        worldMousePos = Camera.main.ScreenToWorldPoint(screenMousePos);
        //if (Input.GetMouseButtonDown(0))
        //{

        //    Debug.Log("SmousePos" + screenMousePos);
        //    Debug.Log("WmousePos" + worldMousePos);
        //}
        worldMousePos.z = offset;
        cursorObj.transform.position = worldMousePos;

        
    }

    
}
