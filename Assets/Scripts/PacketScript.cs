using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class PacketScript : MonoBehaviour
{
    [SerializeField] uint type;
    [SerializeField] uint zipped;
    [SerializeField] float totalChargeTime;
    float curChargeTime;

    bool attack;
    bool touch;

    //触った時の色を変える用
    UnityEngine.Color initColor;
    //UnityEngine.Color curColor;
    SpriteRenderer spriteRenderer;

    public uint GetTypes() { return type; }
    public uint GetZipped() { return zipped; }
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer= gameObject.GetComponent<SpriteRenderer>();
        initColor = spriteRenderer.color;
        curChargeTime = totalChargeTime * (1 + zipped / 2);
    }

    // Update is called once per frame
    void Update()
    {
        Charge();
    }

    void Charge()
    {
        //if()ホールド中
        curChargeTime-=Time.deltaTime;
    }

   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "MousePointer")
        {
            touch = true;

            spriteRenderer.color = new UnityEngine.Color(initColor.r + 0.2f, initColor.r + 0.2f, initColor.b+ 0.2f);
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
