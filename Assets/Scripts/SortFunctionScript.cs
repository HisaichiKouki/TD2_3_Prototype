using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SortFunctionScript: MonoBehaviour
{
    // ���k�Ώۂ����z�I�ɕ\������I�u�W�F�N�g�i���̉��o�p�j
    public GameObject numberPrefab; // ������\������v���n�u
    public Transform container; // ������z�u����e�I�u�W�F�N�g

    private List<GameObject> numberObjects = new List<GameObject>(); // �\�����̐����I�u�W�F�N�g���X�g

    // ���C������
    public IEnumerator CompressListWithEffect(List<int> numbers, int removeIndex)
    {// �C���f�b�N�X�͈̓`�F�b�N
        if (removeIndex < 0 || removeIndex >= numbers.Count)
        {
            Debug.LogError("Invalid removeIndex: Out of range.");
            yield break;
        }
        // �w�肳�ꂽ�C���f�b�N�X���폜
        numbers.RemoveAt(removeIndex);

        // ���k�������J��Ԃ�
        bool didCompress;
        do
        {
            didCompress = false;

            for (int i = 0; i < numbers.Count - 2; i++)
            {
                if (numbers[i] == numbers[i + 2])
                {
                    // ���܂ꂽ�����̐������擾
                    var middle = numbers.GetRange(i + 1, 1);

                    // ���܂ꂽ�����̎�ނ�1��ނ݂̂Ȃ爳�k
                    if (middle.Distinct().Count() == 1)
                    {
                        // ���̉��o�����s
                        yield return StartCoroutine(PlayCompressionEffect(i, i + 2));

                        // ���k����
                        int count = 1; // �����̐����̃J�E���g
                        numbers[i] = numbers[i]; // �����l�ŏ㏑���i���k��̒l�𔽉f�j
                        numbers.RemoveAt(i + 2); // �E�[���폜
                        numbers.RemoveAt(i + 1); // �������폜
                        didCompress = true;
                        break;
                    }
                }
            }
        } while (didCompress);

        // �������ĕ`��
        DrawNumbers(numbers);
    }

    // ���k���o�i���̃G�t�F�N�g�j
    private IEnumerator PlayCompressionEffect(int startIndex, int endIndex)
    {// �͈̓`�F�b�N
        if (startIndex < 0 || endIndex >= numberObjects.Count || startIndex > endIndex)
        {
            Debug.LogWarning($"PlayCompressionEffect: Invalid range (startIndex: {startIndex}, endIndex: {endIndex}, count: {numberObjects.Count}).");
            yield break;
        }
        // �Ώ۔͈͂������\���i�_�ł��鉼���o�j
        for (int i = 0; i < 3; i++)
        {
            HighlightNumbers(startIndex, endIndex, true); // ����ON
            yield return new WaitForSeconds(0.2f);
            HighlightNumbers(startIndex, endIndex, false); // ����OFF
            yield return new WaitForSeconds(0.2f);
        }
    }

    // �����\���p
    private void HighlightNumbers(int startIndex, int endIndex, bool highlight)
    {// �͈̓`�F�b�N��ǉ�
        if (startIndex < 0 || endIndex >= numberObjects.Count)
        {
            Debug.LogWarning($"HighlightNumbers: Invalid range (startIndex: {startIndex}, endIndex: {endIndex}, count: {numberObjects.Count}).");
            return;
        }
        for (int i = startIndex; i <= endIndex; i++)
        {
            var renderer = numberObjects[i].GetComponent<SpriteRenderer>();
            renderer.color = highlight ? Color.red : Color.white; // �����\��
        }
    }

    // ������`��
    private void DrawNumbers(List<int> numbers)
    {
        // �����̐������폜
        foreach (var obj in numberObjects)
        {
            Destroy(obj);
        }
        numberObjects.Clear();

        // �V����������z�u
        for (int i = 0; i < numbers.Count; i++)
        {
            var obj = Instantiate(numberPrefab, container);
            obj.transform.localPosition = new Vector3(i * 1.5f, 0, 0); // �����ɔz�u
            obj.GetComponentInChildren<TextMesh>().text = numbers[i].ToString(); // ������\��
            numberObjects.Add(obj);
        }
    }

    // �e�X�g�p�{�^��
    [ContextMenu("Run Test")]
    public void RunTest()
    {
        // �e�X�g�P�[�X
        List<int> testCase = new List<int> { 1, 2, 3, 1 };
        StartCoroutine(CompressListWithEffect(testCase, 1));
    }
    private void Start()
    {
        // �e�X�g�P�[�X
        List<int> testCase = new List<int> { 1, 2, 3, 1 };
        StartCoroutine(CompressListWithEffect(testCase, 2));
    }
}
