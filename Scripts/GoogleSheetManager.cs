using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using static System.Net.WebRequestMethods;

public class GoogleSheetManager : MonoBehaviour
{
    const string StringSheetURL = "Classified"; // StringTable
    const string GrowthStatus = "Classified"; // 펭귄 성장단계
    const string FeedingTable = "Classified"; // 먹이
    const string WarmingTable = "Classified"; // 보온 아이템

    const string Diet = "Classified"; // 식생활 개선
    const string Recycling = "Classified"; // 친환경 생활 실천
    const string Energy = "Classified"; // 에너지 효율 증가
    const string Renewable = "Classified"; // 재생 에너지 사용
    const string Transport = "Classified"; // 교통수단 개선
    const string Industry = "Classified"; // 산업분야 개선
    const string MarineProtection = "Classified"; // 해양 보호
    const string ForestAndSoil = "Classified"; // 숲,토양 관리

    const string TutorialQuest = "Classified";

    public bool recall = false; // 데이터베이스 재호출을 위한 변수  - 디버깅용 or 차후 쓸곳에 대비해서

    int nRows;

    int nTableCount = 0;

    public bool bGoodToGo = false;

    private void Update()
    {
        if (recall)
        {
            //StartCoroutine(Recall());
            StartCoroutine(CallRoutine(StringSheetURL));
            StartCoroutine(CallRoutine(GrowthStatus));
            StartCoroutine(CallRoutine(FeedingTable));
            StartCoroutine(CallRoutine(WarmingTable));

            StartCoroutine(CallRoutine(Diet));
            StartCoroutine(CallRoutine(Recycling));
            StartCoroutine(CallRoutine(Energy));
            StartCoroutine(CallRoutine(Renewable));
            StartCoroutine(CallRoutine(Transport));
            StartCoroutine(CallRoutine(Industry));
            StartCoroutine(CallRoutine(MarineProtection));
            StartCoroutine(CallRoutine(ForestAndSoil));

            StartCoroutine(CallRoutine(TutorialQuest));
            recall = false;
        }
    }

    IEnumerator CallRoutine(string __fileName__)
    {

        UnityWebRequest www = UnityWebRequest.Get(__fileName__);
        yield return www.SendWebRequest();

        nTableCount++;

        string data = www.downloadHandler.text;
        var a = data.Split('\t');
        CountRows(a);

        bGoodToGo = LoadingSceneController.sharedInstance.GetComponent<TSVLoader>().setData(data, nRows);

        if (bGoodToGo)
        {
            LoadingSceneController.sharedInstance.setGoodToGo(bGoodToGo);
            //GameManager.sharedInstance.getPenguriManager().DEBUG__Start();
        }
    }

    void CountRows(string[] data)
    {
        for(int i=0; i < data.Length; i++)
        {
            if(data[i].Contains("\r"))
            {
                nRows = i + 1;
                break;
            }
        }
    }

    public IEnumerator Recall()
    {
        UnityWebRequest www = UnityWebRequest.Get(StringSheetURL);
        yield return www.SendWebRequest();

        string data = www.downloadHandler.text;
        var a = data.Split('\t');
        CountRows(a);

        GameManager.sharedInstance.GetComponent<TSVLoader>().setData(data, nRows);


        UnityWebRequest GrowthStatusRequest = UnityWebRequest.Get(GrowthStatus);
        yield return GrowthStatusRequest.SendWebRequest();

        data = GrowthStatusRequest.downloadHandler.text;
        a = data.Split('\t');
        CountRows(a);

        GameManager.sharedInstance.GetComponent<TSVLoader>().setData(data, nRows);

    }
}
