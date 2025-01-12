using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetTextScript: MonoBehaviour
{
    [SerializeField,Header("�����̑O�ɒǉ����錾�t")]private string frontString;
    [SerializeField,Header("�����̌�ɒǉ����錾�t")]private string backString;

    Text m_Text;
    // Start is called before the first frame update
    void Start()
    {
        m_Text = GetComponent<Text>();
    }

    public void SetText(int raito)
    {
        m_Text = GetComponent<Text>();
        //float raitoNum = raito * 100;
        m_Text.text= frontString+raito.ToString("f0")+ backString;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
