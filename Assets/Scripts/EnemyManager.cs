using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{

    [SerializeField] EnemyScript[] enemys;

    int target;
    bool[] isDead = new bool[3];
    [SerializeField] TargetScript targetObj;
    [SerializeField] AttackEffect attackEffectPrefab;
    [SerializeField] AttackEffect attackEffectPrefabBig;

    bool gameClear;
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

    //起きてる敵がどのくらいいるか判定
    public int GetIsEnergyEnemy()
    {
        int count = 0;
        for (int i = 0; i < enemys.Length; i++)
        {
            if (enemys[i].GetIsHealing())
            {
                count += 1;
            }
            //else if (enemys[i].GetIsEnergy())
            //{
            //    count += 3;
            //}
        }
        return count;
    }

    public void Target()
    {

        if (isDead[0] && isDead[1] && isDead[2])
        {
            targetObj.gameObject.SetActive(false);
            gameClear = true;
            return;
        }

        if (target == 0)
        {
            if (enemys[target].GetIsDead())
            {
                target = 1;
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

    public void Attack(int damage)
    {
        enemys[target].Damage(damage);
    }

    public void Effect(Vector2 initPos)
    {
        AttackEffect effect = Instantiate(attackEffectPrefab, initPos, Quaternion.identity);
        effect.SetPosition(initPos, enemys[target].transform.position);
    }
    public void EffectBig(Vector2 initPos)
    {
        AttackEffect effect = Instantiate(attackEffectPrefabBig, initPos, Quaternion.identity);
        effect.SetPosition(initPos, enemys[target].transform.position);
    }

    public bool GetGameClear()
    {
        return gameClear;
    }
}
