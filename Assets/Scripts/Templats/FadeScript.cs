using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeScript : MonoBehaviour
{
    //SpriteRenderer spriteRenderer;

    [SerializeField, Header("�J�ڃV�[����")] string sceneName;
    Image renderer;
    [SerializeField, Header("����")] float totalTime;
    [SerializeField, Header("�t�F�[�h�A�E�g(�V�[����؂�ւ���)")] bool fadeOut;
    public void SetSceneName(string name)
    {
        sceneName = name;
    }
    public void SetTotalTime(float value)
    {
        totalTime = value;
    }
    public void SetFadeOut(bool value)
    {
        fadeOut = value;
    }
    float currentTime;
    Color32 initColor;
    Color32 currentColor;
    // Start is called before the first frame update
    void Start()
    {
        renderer = gameObject.transform.GetChild(0).GetComponent<Image>();

        initColor = renderer.color;
        currentColor = initColor;
        if (fadeOut)
        {
            currentColor.a = 0;
        }
        else
        {
            currentColor.a = 255;

        }
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        if (fadeOut)
        {
            currentColor.a = (byte)Easing.InSine(currentTime, totalTime, 0, 255);
        }
        else
        {
            currentColor.a = (byte)Easing.InSine(currentTime, totalTime, 255, 0);

        }
        renderer.color = currentColor;
        if (currentTime > totalTime)
        {
            if (sceneName != "" && fadeOut)
            {
                SceneManager.LoadScene(sceneName);
            }
            if (currentTime > totalTime + 0.5f) Destroy(this.gameObject);
        }
    }
}