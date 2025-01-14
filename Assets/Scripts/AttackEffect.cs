using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEffect : MonoBehaviour
{
    [SerializeField] float easeTime;
    float curEaseT;

    Vector2 initPos;
    Vector2 targetPos;

    public void SetPosition (Vector2 init,Vector2 target)
    {
        initPos = init;
        targetPos = target;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 newPos = Vector3.zero;
        newPos.x = Easing.InQuart(curEaseT, easeTime, initPos.x, targetPos.x);
        newPos.y = Easing.InQuart(curEaseT, easeTime, initPos.y, targetPos.y);
        newPos.z = -10;
        transform.position = newPos;
        curEaseT += Time.deltaTime;

    }
}
