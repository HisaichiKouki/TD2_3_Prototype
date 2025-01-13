using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AttackObjectHandler : MonoBehaviour
{
    PacketManager packetManager;
    Vector3 destroyPos;//消した箇所の座標を保持して圧縮後に使う
    [Serializable]
    public struct AttackObject
    {
        public uint Type;
        public uint Zipped;
    }

    [Serializable]
    public class CompressObject
    {
        public uint Type;
        public uint ZippedSum;
        public uint NumObject;
        public List<uint> ElementZipped;

        public CompressObject(uint type)
        {
            Type = type;
            ZippedSum = 0;
            NumObject = 0;
            ElementZipped = new List<uint>();
        }
    }

    public List<AttackObject> attackObjects;

    private void Start()
    {
        packetManager = GetComponent<PacketManager>();
    }
    public void CompressAttackObject(List<AttackObject> vec, out List<CompressObject> compressed, out List<int> reference)
    {
        compressed = new List<CompressObject>();
        reference = new List<int>();

        if (vec.Count == 0) return;

        CompressObject current = new CompressObject(vec[0].Type);
        compressed.Add(current);

        foreach (var elem in vec)
        {
            if (elem.Type == current.Type)
            {
                current.NumObject++;
                current.ZippedSum += elem.Zipped;
                current.ElementZipped.Add(elem.Zipped);
            }
            else
            {
                current = new CompressObject(elem.Type);
                current.NumObject = 1;
                current.ZippedSum = elem.Zipped;
                current.ElementZipped.Add(elem.Zipped);
                compressed.Add(current);
               
            }
            reference.Add(compressed.Count - 1);
        }
    }

    public uint? ZippedSize(List<CompressObject> compressed, int index)
    {
        if (compressed.Count == 0 || index <= 0 || index + 1 >= compressed.Count)
        {
            return null;
        }

        if (compressed[index - 1].Type == compressed[index + 1].Type)
        {
            return compressed[index].ZippedSum + compressed[index].NumObject;
        }

        return null;
    }

    public void Zipping(List<CompressObject> compressed, int index)
    {
        var left = compressed[index - 1];
        //left.ZippedSum -= left.ElementZipped[^1];
        //left.ElementZipped.RemoveAt(left.ElementZipped.Count - 1);


        var target = compressed[index];
        left.ZippedSum += target.ZippedSum + target.NumObject;
        //left.ElementZipped.Add(target.ZippedSum + target.NumObject);
        left.ElementZipped[left.ElementZipped.Count-1] += target.ZippedSum + target.NumObject;

        var right = compressed[index + 1];
        //right.NumObject--;
        right.ZippedSum -= right.ElementZipped[0];
        
        left.ElementZipped[left.ElementZipped.Count - 1] += right.ElementZipped[0];
        right.ElementZipped.RemoveAt(0);

        left.NumObject += right.NumObject;
        left.ZippedSum += right.ZippedSum;
        left.ElementZipped.AddRange(right.ElementZipped);

        compressed.RemoveRange(index,2);

        //ゲームオブジェクトをDestroy
        for (int i = 0; i < index + 2; i++)
        {
            destroyPos= packetManager.packets[i].gameObject.transform.position;
            Destroy(packetManager.packets[i].gameObject);
        }
        //リストから削除
        packetManager.packets.RemoveRange(index,2);

        
    }

    public List<AttackObject> Uncompress(List<CompressObject> compressed)
    {
        var result = new List<AttackObject>();
        packetManager.AllReset();
        int count=0;

        
        foreach (var compObj in compressed)
        {
            foreach (var elem in compObj.ElementZipped)
            {
                //ここでリストを更新してるっぽい
                result.Add(new AttackObject { Type = compObj.Type, Zipped = elem });
                count++;
                //高さをゴリ押しで調整
                Vector3 newPos = new Vector3(-1.47935629f, -4.91727686f+(count*0.8f), -0.02f);
                packetManager.MargeSpown((int)compObj.Type, elem, newPos);

            }
        }
        return result;
    }

    public bool ZipAttackObject(ref List<AttackObject> vec, int index)
    {
        CompressAttackObject(vec, out var compressed, out var reference);

        var zippedSizeL = index > 0 ? ZippedSize(compressed, index - 1) : null;
        var zippedSizeR = ZippedSize(compressed, index);

        bool result = false;
        if (zippedSizeL.HasValue && zippedSizeR.HasValue)
        {
            if (zippedSizeL.Value < zippedSizeR.Value)
                Zipping(compressed, index);
            else
                Zipping(compressed, index - 1);

            result = true;
        }
        else if (zippedSizeL.HasValue)
        {
            Zipping(compressed, index - 1);
            result = true;
        }
        else if (zippedSizeR.HasValue)
        {
            Zipping(compressed, index);
            result = true;
        }

        vec = Uncompress(compressed);
        return result;
    }

    // Unity Editorでのデバッグ実行用
    // Unity Editorでのデバッグ実行用
    [ContextMenu("SetType")]
    public void TestSetType()
    {
        attackObjects = new List<AttackObject>
        {
            new AttackObject { Type = 1, Zipped = 0 },
            new AttackObject { Type = 2, Zipped = 0 },
            new AttackObject { Type = 3, Zipped = 0 },
            new AttackObject { Type = 2, Zipped = 0 },
            new AttackObject { Type = 1, Zipped = 0 },
            new AttackObject { Type = 2, Zipped = 0 },
            new AttackObject { Type = 1, Zipped = 0 },
        };
    }
    [ContextMenu("SetType2")]
    public void TestSetType2()
    {
        attackObjects = new List<AttackObject>
        {
            new AttackObject { Type = 1, Zipped = 0 },
            new AttackObject { Type = 2, Zipped = 0 },
            new AttackObject { Type = 3, Zipped = 2 },
            new AttackObject { Type = 1, Zipped = 0 },
            new AttackObject { Type = 2, Zipped = 0 },
        };
    }
    [ContextMenu("SetType3")]
    public void TestSetType3()
    {
        attackObjects = new List<AttackObject>
        {
            new AttackObject { Type = 1, Zipped = 0 },
            new AttackObject { Type = 2, Zipped = 0 },
            new AttackObject { Type = 3, Zipped = 2 },
            new AttackObject { Type = 1, Zipped = 0 },
            new AttackObject { Type = 0, Zipped = 0 },
            new AttackObject { Type = 2, Zipped = 0 },
            new AttackObject { Type = 0, Zipped = 0 },
        };
    }
    [SerializeField] int testIndex;
    [ContextMenu("Test Compression while")]
    public void TestCompression()
    {
        int index = testIndex; // サンプル
        

        Debug.Log("Before Compression:");
        foreach (var obj in attackObjects)
        {
            Debug.Log($"Type: {obj.Type}, Zipped: {obj.Zipped}");
        }

        attackObjects.RemoveAt(index);
        while (ZipAttackObject(ref attackObjects, index)) { };//継続するかしないかのflagが帰ってる

        Debug.Log("After Compression:");
        foreach (var obj in attackObjects)
        {
            Debug.Log($"Type: {obj.Type}, Zipped: {obj.Zipped}");
        }
    }
    [ContextMenu("Test Compression2")]
    public void TestCompression2()
    {
        int index = testIndex; // サンプル


        Debug.Log("Before Compression:");
        foreach (var obj in attackObjects)
        {
            Debug.Log($"Type: {obj.Type}, Zipped: {obj.Zipped}");
        }

        //attackObjects.RemoveAt(index);
        ZipAttackObject(ref attackObjects, index);//継続するかしないかのflagが帰ってる

        Debug.Log("After Compression:");
        foreach (var obj in attackObjects)
        {
            Debug.Log($"Type: {obj.Type}, Zipped: {obj.Zipped}");
        }
    }

    //リストにパケットを追加する
    public void AddAttackObject(uint type,uint zipped)
    {
        attackObjects.Add(new AttackObject {Type = type, Zipped = zipped});
    }
    public void AddAttackObject(PacketScript packet)
    {
        attackObjects.Add(new AttackObject { Type = packet.GetTypes(), Zipped = packet.GetZipped() });
    }
    //チャージが溜まったら攻撃開始
    public bool ReturnAttackObject(int index)
    {
        return ZipAttackObject(ref attackObjects, index);
        
    }

    public void DestroyIndex(int index)
    {
        attackObjects.RemoveAt(index);
    }
}
