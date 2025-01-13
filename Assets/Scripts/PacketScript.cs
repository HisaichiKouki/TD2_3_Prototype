using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class PacketScript : MonoBehaviour
{
    [SerializeField] uint type;
    [SerializeField] uint zipped;
    [SerializeField] float totalChargeTime=0.1f;
    float curChargeTime;

    bool attack;
    bool touch;

    //触った時の色を変える用
    UnityEngine.Color initColor;
    //UnityEngine.Color curColor;
    SpriteRenderer spriteRenderer;

    GameObject gaugeObj;
    float gaugeRatio;

    public uint GetTypes() { return type; }
    public uint GetZipped() { return zipped; }
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        initColor = spriteRenderer.color;
        curChargeTime = totalChargeTime * (1 + zipped / 2);

        gaugeObj = transform.GetChild(0).gameObject;
        gaugeObj.transform.localScale = new Vector3(0, 1, 1);
    }

    // Update is called once per frame
    void Update()
    {
        Charge();
        gaugeRatio = (totalChargeTime - curChargeTime) / totalChargeTime;
        gaugeObj.transform.localScale = new Vector3(Mathf.Clamp01(gaugeRatio), 1, 1);

    }

    void Charge()
    {
        if (!touch) { return; } //ホールド中
        if (Input.GetMouseButton(0))
        {
            curChargeTime -= Time.deltaTime;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "MousePointer")
        {
            touch = true;

            spriteRenderer.color = new UnityEngine.Color(initColor.r + 0.2f, initColor.r + 0.2f, initColor.b + 0.2f);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "MousePointer")
        {
            spriteRenderer.color = initColor;
            touch = false;
        }
    }
}
