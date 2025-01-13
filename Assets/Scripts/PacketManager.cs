using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Sprites;
using UnityEngine;

public class PacketManager : MonoBehaviour
{
    [SerializeField]AttackObjectHandler attackObjectHandler;
    [SerializeField] PacketScript[] packetPrefab;

    [SerializeField]List<PacketScript> packets;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Spawn());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //パケット、攻撃を生成
    void PacketSpawn(int index)
    {
        packets.Add(packetPrefab[index]);
        attackObjectHandler.AddAttackObject(packets[packets.Count - 1]);
        Instantiate(packets[packets.Count - 1], this.transform);
    }
    private IEnumerator Spawn()
    {
        for (int i = 0; i < 3; i++) 
        {
            yield return new WaitForSecondsRealtime(0.5f);
            PacketSpawn(Random.Range(0, packetPrefab.Length-1));

        }
    }
}
