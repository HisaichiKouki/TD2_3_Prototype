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
    bool isEnergy;

    [SerializeField] Animator animator;

    [SerializeField] GameObject hitPointGauge;

    bool touch;
 
    public bool GetIsHealing() { return isHealing; }
    public bool GetIsDead() { return isDead; }
    public bool GetIsEnergy() { return isEnergy; }
    public bool GetTouch() { return touch; }


    // Start is called before the first frame update
    void Start()
    {
        curHitPoint = maxHitPoint;
        curHealTime = 0;
        isHealing = false;
        isEnergy = true;
    }

    // Update is called once per frame
    void Update()
    {
        Healing();


        hitPointGauge.transform.localScale = new Vector3(Mathf.Clamp01(curHitPoint/ maxHitPoint),1,1);
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
            isHealing = false;
            isEnergy = true;

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
            isHealing = true;
            curHealCoolTime += Time.deltaTime;

            //âÒïúÇ®ÇÌÇË
            if (curHealCoolTime > healCoolTime)
            {
                curHealTime= healCoolTime;
                isHealing = false;
                isEnergy = true;

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "MousePointer")
        {
            touch = true;

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "MousePointer")
        {
            touch = false;
        }
    }

    
}
