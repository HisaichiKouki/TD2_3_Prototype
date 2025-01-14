using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    PacketManager packetManager;
    EnemyManager enemyManager;

    [SerializeField] float totalCooltime;
    [SerializeField] float curCoolTime;

    [SerializeField] int deadLine;
    [SerializeField] GameObject GameOverObj;
    [SerializeField] GameObject GameClearObj;


    float gameTime;

    bool isGameClear;
    bool isGameOver;

    float cooltime;
    bool startFunc;
    //float gametotalTime;
    // Start is called before the first frame update
    void Start()
    {
        packetManager = FindAnyObjectByType<PacketManager>();
        enemyManager = FindAnyObjectByType<EnemyManager>();
        gameTime = 0;

    }

    // Update is called once per frame
    void Update()
    {
        StartSpawn();
        gameTime += Time.deltaTime;

        if (enemyManager.GetGameClear())
        {
            if(!isGameClear)Instantiate(GameClearObj);
            isGameClear = true;

        }
        else if (packetManager.GetPacketNum() > deadLine)
        {
            if (!isGameOver) Instantiate(GameOverObj);

            isGameOver = true;
        }

        if (Input.GetKeyDown(KeyCode.R)) {

            SceneManager.LoadScene("GameScene");
        }

    }


    void StartSpawn()
    {
        cooltime += Time.deltaTime;
        if (cooltime > 6 && !startFunc)
        {
            startFunc = true;
            StartCoroutine(EnemyAttack());
        }

    }
    private IEnumerator EnemyAttack()
    {
        while (!isGameOver && !isGameClear)
        {
            curCoolTime = totalCooltime - enemyManager.GetIsEnergyEnemy();
            //curCoolTime = Mathf.Clamp(curCoolTime, 1.5f - (gameTime / 120), 6);
            curCoolTime = totalCooltime + enemyManager.GetIsEnergyEnemy();
            curCoolTime -= gameTime / 60;
            curCoolTime = Mathf.Clamp(curCoolTime, 0.4f, 6);
            packetManager.EnemyPacket();
            yield return new WaitForSeconds(curCoolTime);
        }
    }
}
