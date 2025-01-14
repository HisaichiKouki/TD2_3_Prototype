using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] float totalPower;
    float curPower;
    [SerializeField] float diffPower;//’·‰Ÿ‚µ‚ÅŒ¸ŽZ
    [SerializeField] float addPower;//—£‚µ‚Ä‰ñ•œ
    [SerializeField] float normalPower;

    [SerializeField] GameObject gauge;

    [SerializeField]  float energyRatio;
    [SerializeField] GameObject[] playerTex;
    bool healing;
    // Start is called before the first frame update
    void Start()
    {
        curPower = totalPower;
        healing = false;
        playerTex[1].SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        energyRatio = curPower / totalPower;

        gauge.transform.localScale = new Vector3(1, Mathf.Clamp01(energyRatio), 1);

        PowerUse();

       
        Heal();
    }

    public float GetPower()
    {
        if (healing) { return 0; }
        return normalPower;
    }
    void Heal()
    {
        if (!healing) { return; }
        curPower += Time.deltaTime;

        if(curPower> totalPower)
        {
            healing = false;
            playerTex[0].SetActive(true);
            playerTex[1].SetActive(false);
        }
    }

    void PowerUse()
    {
        if (healing) { return; }
        if (Input.GetMouseButton(0))
        {
            curPower -= diffPower * Time.deltaTime;
        }
        else
        {
            curPower += addPower * Time.deltaTime;
        }
        curPower= Mathf.Clamp(curPower,-1,totalPower);
        if (curPower <= 0)
        {

            healing = true;
            playerTex[0].SetActive(false);
            playerTex[1].SetActive(true);
        }
    }
}
