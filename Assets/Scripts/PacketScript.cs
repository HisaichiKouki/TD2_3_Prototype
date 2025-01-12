using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacketScript : MonoBehaviour
{
    [SerializeField] uint type;
    [SerializeField] uint zipped;

    public uint GetTypes() { return type; }
    public uint GetZipped() { return zipped; }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
