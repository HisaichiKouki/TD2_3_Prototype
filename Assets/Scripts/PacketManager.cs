using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Sprites;
using UnityEngine;

public class PacketManager : MonoBehaviour
{
    [SerializeField] AttackObjectHandler attackObjectHandler;
    [SerializeField] PacketScript[] packetPrefab;

    [SerializeField] List<PacketScript> packets;
    // Start is called before the first frame update
    void Start()
    {
        
        StartCoroutine(Spawn());
    }

    // Update is called once per frame
    void Update()
    {
        PacketAttack();
    }

    //パケット、攻撃を生成
    void PacketSpawn(int index)
    {
        //Instantiate(packetPrefab[index], this.transform);
        packets.Add(Instantiate(packetPrefab[index], this.transform));
        attackObjectHandler.AddAttackObject(packets[packets.Count - 1]);


    }
    private IEnumerator Spawn()
    {
        for (int i = 0; i < 3; i++)
        {
            yield return new WaitForSecondsRealtime(0.5f);
            PacketSpawn(Random.Range(0, packetPrefab.Length - 1));

        }
        
    }

    void PacketAttack()
    {
        for (int i = 0; i < packets.Count; i++)
        {
            if (packets[i].GetGaugeRatio() >= 1.0f)
            {
                //長押しで攻撃する時は先にリストから消す
                attackObjectHandler.DestroyIndex(i);
                attackObjectHandler.ReturnAttackObject(i);

               
                Destroy(packets[i].gameObject);
                packets.RemoveAt(i);
            }

        }
    }
}
