using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SortFunctionScript: MonoBehaviour
{
    // 圧縮対象を仮想的に表示するオブジェクト（仮の演出用）
    public GameObject numberPrefab; // 数字を表示するプレハブ
    public Transform container; // 数字を配置する親オブジェクト

    private List<GameObject> numberObjects = new List<GameObject>(); // 表示中の数字オブジェクトリスト

    // メイン処理
    public IEnumerator CompressListWithEffect(List<int> numbers, int removeIndex)
    {// インデックス範囲チェック
        if (removeIndex < 0 || removeIndex >= numbers.Count)
        {
            Debug.LogError("Invalid removeIndex: Out of range.");
            yield break;
        }
        // 指定されたインデックスを削除
        numbers.RemoveAt(removeIndex);

        // 圧縮処理を繰り返す
        bool didCompress;
        do
        {
            didCompress = false;

            for (int i = 0; i < numbers.Count - 2; i++)
            {
                if (numbers[i] == numbers[i + 2])
                {
                    // 挟まれた部分の数字を取得
                    var middle = numbers.GetRange(i + 1, 1);

                    // 挟まれた数字の種類が1種類のみなら圧縮
                    if (middle.Distinct().Count() == 1)
                    {
                        // 仮の演出を実行
                        yield return StartCoroutine(PlayCompressionEffect(i, i + 2));

                        // 圧縮処理
                        int count = 1; // 中央の数字のカウント
                        numbers[i] = numbers[i]; // 同じ値で上書き（圧縮後の値を反映）
                        numbers.RemoveAt(i + 2); // 右端を削除
                        numbers.RemoveAt(i + 1); // 中央を削除
                        didCompress = true;
                        break;
                    }
                }
            }
        } while (didCompress);

        // 数字を再描画
        DrawNumbers(numbers);
    }

    // 圧縮演出（仮のエフェクト）
    private IEnumerator PlayCompressionEffect(int startIndex, int endIndex)
    {// 範囲チェック
        if (startIndex < 0 || endIndex >= numberObjects.Count || startIndex > endIndex)
        {
            Debug.LogWarning($"PlayCompressionEffect: Invalid range (startIndex: {startIndex}, endIndex: {endIndex}, count: {numberObjects.Count}).");
            yield break;
        }
        // 対象範囲を強調表示（点滅する仮演出）
        for (int i = 0; i < 3; i++)
        {
            HighlightNumbers(startIndex, endIndex, true); // 強調ON
            yield return new WaitForSeconds(0.2f);
            HighlightNumbers(startIndex, endIndex, false); // 強調OFF
            yield return new WaitForSeconds(0.2f);
        }
    }

    // 強調表示用
    private void HighlightNumbers(int startIndex, int endIndex, bool highlight)
    {// 範囲チェックを追加
        if (startIndex < 0 || endIndex >= numberObjects.Count)
        {
            Debug.LogWarning($"HighlightNumbers: Invalid range (startIndex: {startIndex}, endIndex: {endIndex}, count: {numberObjects.Count}).");
            return;
        }
        for (int i = startIndex; i <= endIndex; i++)
        {
            var renderer = numberObjects[i].GetComponent<SpriteRenderer>();
            renderer.color = highlight ? Color.red : Color.white; // 強調表示
        }
    }

    // 数字を描画
    private void DrawNumbers(List<int> numbers)
    {
        // 既存の数字を削除
        foreach (var obj in numberObjects)
        {
            Destroy(obj);
        }
        numberObjects.Clear();

        // 新しい数字を配置
        for (int i = 0; i < numbers.Count; i++)
        {
            var obj = Instantiate(numberPrefab, container);
            obj.transform.localPosition = new Vector3(i * 1.5f, 0, 0); // 横一列に配置
            obj.GetComponentInChildren<TextMesh>().text = numbers[i].ToString(); // 数字を表示
            numberObjects.Add(obj);
        }
    }

    // テスト用ボタン
    [ContextMenu("Run Test")]
    public void RunTest()
    {
        // テストケース
        List<int> testCase = new List<int> { 1, 2, 3, 1 };
        StartCoroutine(CompressListWithEffect(testCase, 1));
    }
    private void Start()
    {
        // テストケース
        List<int> testCase = new List<int> { 1, 2, 3, 1 };
        StartCoroutine(CompressListWithEffect(testCase, 2));
    }
}
