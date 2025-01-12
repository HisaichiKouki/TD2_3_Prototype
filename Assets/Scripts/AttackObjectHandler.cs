using System;
using System.Collections.Generic;
using UnityEngine;

public class AttackObjectHandler : MonoBehaviour
{
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
        left.ZippedSum -= left.ElementZipped[^1];
        left.ElementZipped.RemoveAt(left.ElementZipped.Count - 1);

        var target = compressed[index];
        left.ZippedSum += target.ZippedSum + target.NumObject;
        left.ElementZipped.Add(target.ZippedSum + target.NumObject);

        var right = compressed[index + 1];
        right.NumObject--;
        right.ZippedSum -= right.ElementZipped[0];
        right.ElementZipped.RemoveAt(0);

        left.NumObject += right.NumObject;
        left.ZippedSum += right.ZippedSum;
        left.ElementZipped.AddRange(right.ElementZipped);

        compressed.RemoveAt(index);
    }

    public List<AttackObject> Uncompress(List<CompressObject> compressed)
    {
        var result = new List<AttackObject>();
        foreach (var compObj in compressed)
        {
            foreach (var elem in compObj.ElementZipped)
            {
                result.Add(new AttackObject { Type = compObj.Type, Zipped = elem });
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
        };
    }
    [SerializeField] int testIndex;
    [ContextMenu("Test Compression")]
    public void TestCompression()
    {
        int index = testIndex; // サンプル
        

        Debug.Log("Before Compression:");
        foreach (var obj in attackObjects)
        {
            Debug.Log($"Type: {obj.Type}, Zipped: {obj.Zipped}");
        }

        attackObjects.RemoveAt(index);
        while (ZipAttackObject(ref attackObjects, index)) { }

        Debug.Log("After Compression:");
        foreach (var obj in attackObjects)
        {
            Debug.Log($"Type: {obj.Type}, Zipped: {obj.Zipped}");
        }
    }
}
