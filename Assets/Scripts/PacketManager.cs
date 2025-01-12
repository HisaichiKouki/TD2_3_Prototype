using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacketManager : MonoBehaviour
{
    [SerializeField]AttackObjectHandler attackObjectHandler;
    [SerializeField] PacketScript testPrefab;

    [SerializeField]List<PacketScript> testScripts;
    // Start is called before the first frame update
    void Start()
    {
        attackObjectHandler.AddAttackObject(testPrefab.GetTypes(), testPrefab.GetZipped());
        Instantiate(testPrefab,this.transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
