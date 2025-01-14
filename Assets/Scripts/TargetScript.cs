using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetScript : MonoBehaviour
{
    [SerializeField] float offset;
    [SerializeField] float easeT;
    [SerializeField] Vector2 initTargetPos;
    Vector3 targetPos;
    public void SetTargetPos(Vector3 newPos) {  targetPos = newPos; }
    // Start is called before the first frame update
    void Start()
    {
        targetPos=initTargetPos;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 newPos=transform.position;

        newPos=Vector2.Lerp(newPos, targetPos, easeT);
        newPos.z = offset;
        transform.position = newPos;
    }
}
