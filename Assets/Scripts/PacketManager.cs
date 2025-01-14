using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Sprites;
using UnityEngine;

public class PacketManager : MonoBehaviour
{
    [SerializeField] float damageRatio;//ダメージ倍率
    [SerializeField] AttackObjectHandler attackObjectHandler;
    [SerializeField] PacketScript[] packetPrefab;

    public List<PacketScript> packets;

    [SerializeField] bool isRandom;
    [SerializeField] List<int> packetTypes;
    [SerializeField] bool isMarge;

    EnemyManager enemyManager;

    public bool GetIsMarge() { return isMarge; }


    // Start is called before the first frame update
    void Start()
    {
        enemyManager = FindAnyObjectByType<EnemyManager>();
        if (isRandom)
        {
            StartCoroutine(SpawnRandom());
        }
        else
        {
            StartCoroutine(Spawn());

        }

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
    //圧縮後nullになった箇所に圧縮オブジェクトを追加する
    //public void PacketInstantiate(int type, uint zipped, Vector3 pos)
    //{
    //    for (int i = 0; i < packets.Count; i++)
    //    {
    //        if (packets[i] == null)
    //        {

    //            packets.Add(Instantiate(packetPrefab[type], pos, Quaternion.identity, this.transform));
    //            packets[packets.Count - 1].SetZipped(zipped);
    //        }
    //    }
    //    //Instantiate(packetPrefab[index], this.transform);

    //    //attackObjectHandler.AddAttackObject(packets[packets.Count - 1]);


    //}
    private IEnumerator SpawnRandom()
    {
        for (int i = 0; i < 3; i++)
        {
            yield return new WaitForSecondsRealtime(0.5f);
            PacketSpawn(UnityEngine.Random.Range(0, packetPrefab.Length - 1));

        }

    }
    private IEnumerator Spawn()
    {
        for (int i = 0; i < packetTypes.Count; i++)
        {
            yield return new WaitForSecondsRealtime(0.5f);
            PacketSpawn(packetTypes[i]);

        }

    }
    private IEnumerator Marging()
    {
        while (isMarge)
        {
            yield return new WaitForSecondsRealtime(0.5f);

            isMarge = attackObjectHandler.ReturnAttackObject(attackObjectHandler.GetMargeIndex());
        }
    }

    void PacketAttack()
    {
        for (int i = 0; i < packets.Count; i++)
        {
            if (packets[i].GetGaugeRatio() >= 1.0f)
            {
                //長押しで攻撃する時は先にリストから消す
                int damage = (int)((packets[i].GetZipped() + 1) * (packets[i].GetZipped() + 1) * damageRatio);
                enemyManager.Attack(damage);

                if (damage < 20)
                {
                    enemyManager.Effect(packets[i].transform.position);
                }
                else
                {
                    enemyManager.EffectBig(packets[i].transform.position);

                }

                attackObjectHandler.DestroyIndex(i);
                Destroy(packets[i].gameObject);
                packets.RemoveAt(i);

                // StartCoroutine(Marge(i));
                isMarge = attackObjectHandler.ReturnAttackObject(i);
            }
        }
        StartCoroutine(Marging());
    }
    //private IEnumerator Marge(int index)
    //{
    //    while (attackObjectHandler.ReturnAttackObject(index)) { yield return new WaitForSecondsRealtime(0.5f); }
    //}

    //リストのリセット
    public void AllReset()
    {
        for (int i = 0; i < packets.Count; i++)
        {
            if (packets[i] != null) Destroy(packets[i].gameObject);

        }
        packets.Clear();
    }

    //マージ後に新しく生成する
    public void MargeSpown(int type, uint zipped, Vector3 pos)
    {
        packets.Add(Instantiate(packetPrefab[type], pos, Quaternion.identity, this.transform));
        packets[packets.Count - 1].SetZipped(zipped);
    }

    public void EnemyPacket()
    {
        PacketSpawn(UnityEngine.Random.Range(0, packetPrefab.Length - 1));
    }

    public int GetPacketNum()
    {
        return packets.Count;
    }
}
