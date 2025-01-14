using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{

    [SerializeField] float maxHitPoint;
    [SerializeField] float curHitPoint;

    [SerializeField] float healTime;
    float curHealTime;
    [SerializeField] float healCoolTime;//àÍâÒÇÃÉqÅ[ÉãÇÃéûä‘
    float curHealCoolTime;

    [SerializeField, Tooltip("ÇPïbÇ†ÇΩÇËÇÃâÒïúó ")] float healPower;

    bool isHealing;
    bool isDead;

    [SerializeField] Animator animator;

    [SerializeField] GameObject hitPointGauge;

    // Start is called before the first frame update
    void Start()
    {
        curHitPoint = maxHitPoint;
        curHealTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Healing();


        hitPointGauge.transform.localScale = new Vector3(curHitPoint/ maxHitPoint,1,1);
    }

    public void Damage(float damage)
    {
        curHitPoint -= damage;
        curHealTime = healTime;
        if (curHitPoint <= 0) 
        {
            isDead = true;
            animator.SetBool("IsBreak", false);
            animator.SetBool("IsEnergy", false);
            animator.SetBool("IsDead", true);
        }
    }

    void Healing()
    {
        if (isDead)
        {
            
            return;
        }
        if (curHitPoint >= maxHitPoint) 
        {
            animator.SetBool("IsBreak", false);
            animator.SetBool("IsEnergy", true);
            return;
        }
        if (curHealTime > 0)
        {
            curHealTime -= Time.deltaTime;
            curHealCoolTime = 0;
        }
        else
        {
            animator.SetBool("IsBreak", true);
            animator.SetBool("IsEnergy", false);
            curHitPoint += healPower * Time.deltaTime;
            curHitPoint = Mathf.Clamp(curHitPoint, 0, maxHitPoint);

            curHealCoolTime += Time.deltaTime;
            if (curHealCoolTime > healCoolTime)
            {
                curHealTime= healCoolTime;
                animator.SetBool("IsBreak", false);
                animator.SetBool("IsEnergy", true);
            }
        }

    }

    [ContextMenu("Attack")]
    public void Attack2Damage()
    {
        Damage(10);
    }
}
