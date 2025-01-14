using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{

    [SerializeField] EnemyScript[] enemys;

    int target;
    bool[] isDead=new bool[3];
    [SerializeField] TargetScript targetObj;
    // Start is called before the first frame update
    void Start()
    {
        target = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Target();
        if (Input.GetMouseButtonDown(0)) 
        {
            IsTarget();
        }
    }

    //‹N‚«‚Ä‚é“G‚ª‚Ç‚Ì‚­‚ç‚¢‚¢‚é‚©”»’è
    public int GetIsEnergyEnemy()
    {
        int count = 0;
        for (int i = 0; i < enemys.Length; i++)
        {
            if (enemys[i].GetIsHealing())
            {
                count+=1;
            }else if (enemys[i].GetIsEnergy())
            {
                count += 3;
            }
        }
        return count;
    }

    public void Target()
    {

        if (!isDead[0]&& isDead[1] && isDead[2]) { targetObj.gameObject.SetActive(false); }

        if (target == 0)
        {
            if (enemys[target].GetIsDead())
            {
                target=1;
                isDead[0] = true;
                targetObj.SetTargetPos(enemys[target].transform.position);
            }
        }
        if (target == 1)
        {
            if (enemys[target].GetIsDead())
            {
                target = 2;
                isDead[1] = true;
                targetObj.SetTargetPos(enemys[target].transform.position);
            }
        }
        if (target == 2)
        {
            if (enemys[target].GetIsDead())
            {
                target = 0;
                isDead[2] = true;
                targetObj.SetTargetPos(enemys[target].transform.position);
            }
        }
    }

    void IsTarget()
    {
        for (int i = 0; i < enemys.Length; i++)
        {
            if (enemys[i].GetTouch())
            {
                target = i;
                targetObj.SetTargetPos(enemys[i].transform.position);
                break;
            }
        }
    }
}
