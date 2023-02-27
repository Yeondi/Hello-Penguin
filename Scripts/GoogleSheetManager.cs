using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using static System.Net.WebRequestMethods;

public class GoogleSheetManager : MonoBehaviour
{
    const string StringSheetURL = "Classified"; // StringTable
    const string GrowthStatus = "Classified"; // ��� ����ܰ�
    const string FeedingTable = "Classified"; // ����
    const string WarmingTable = "Classified"; // ���� ������

    const string Diet = "Classified"; // �Ļ�Ȱ ����
    const string Recycling = "Classified"; // ģȯ�� ��Ȱ ��õ
    const string Energy = "Classified"; // ������ ȿ�� ����
    const string Renewable = "Classified"; // ��� ������ ���
    const string Transport = "Classified"; // ������� ����
    const string Industry = "Classified"; // ����о� ����
    const string MarineProtection = "Classified"; // �ؾ� ��ȣ
    const string ForestAndSoil = "Classified"; // ��,��� ����

    const string TutorialQuest = "Classified";

    public bool recall = false; // �����ͺ��̽� ��ȣ���� ���� ����  - ������ or ���� ������ ����ؼ�

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
