using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetTextScript: MonoBehaviour
{
    [SerializeField,Header("数字の前に追加する言葉")]private string frontString;
    [SerializeField,Header("数字の後に追加する言葉")]private string backString;

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
