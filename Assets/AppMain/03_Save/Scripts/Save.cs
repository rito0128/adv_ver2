using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;

public class SaveLoad : MonoBehaviour
{
    public const int MaxSaveCount = 9; // 保存できる最大件数（バー2〜10）

    private string saveFilePath;

    // Start内でパスを設定
    void Awake()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, "savedata.json");
    }

    public bool SaveGame()
    {
        SaveData newSaveData = new SaveData();
        SaveDataList saveDataList = LoadSaveDataList();
        DateTime now = DateTime.Now; // 現在時刻を取得

        if (saveDataList.saveDatas.Count >= MaxSaveCount)
        {
            Debug.LogWarning("セーブデータが満杯です");
            return false;
        }

        // シナリオファイルを読み込む（未読み込みのときのみ）
        if (NovelSystem.sectionData == null)
        {
            TextAsset jsonFile = Resources.Load<TextAsset>("scenario");
            NovelSystem.sectionData = JsonUtility.FromJson<SectionData>(jsonFile.text);
        }

        Debug.Log("セーブを始めます");

        // 現在の進行状況を記録
        newSaveData.saveDate = now.ToString("yyyy/MM/dd HH:mm:ss");
        newSaveData.placeIndex = NovelSystem.placeIndex;
        newSaveData.mesageIndex = NovelSystem.mesageIndex;
        string text = NovelSystem.sectionData.sectionContent[newSaveData.placeIndex].scenarioContents[newSaveData.mesageIndex].Text;
        newSaveData.currentTextSnippet = text.Substring(0, Mathf.Min(5, text.Length));

        // 最新のデータを先頭に追加
        saveDataList.saveDatas.Insert(0, newSaveData);

        // jsonファイルにして保存
        string updatedJson = JsonUtility.ToJson(saveDataList, true);
        Debug.Log(updatedJson);
        File.WriteAllText(saveFilePath, updatedJson);

        Debug.Log("セーブが完了しました");
        Debug.Log(saveFilePath);
        return true;
    }

    public SaveDataList LoadSaveDataList()
    {
        SaveDataList saveDataList = new SaveDataList();

        if (File.Exists(saveFilePath))
        {
            string jsonText = File.ReadAllText(saveFilePath);
            if (!string.IsNullOrWhiteSpace(jsonText))
            {
                try
                {
                    // jsonファイルを読み込む
                    saveDataList = JsonUtility.FromJson<SaveDataList>(jsonText) ?? new SaveDataList();
                    Debug.Log("セーブファイルを開きました");
                }
                catch (System.ArgumentException e)
                {
                    Debug.LogWarning("既存のJSONが破損しているため、新規作成します: " + e.Message);
                }
            }
        }
        return saveDataList;
    }

    public void DeleteSaveData(int index)
    {
        SaveDataList saveDataList = LoadSaveDataList();
        if (index < 0 || index >= saveDataList.saveDatas.Count) return;

        // 削除すると後ろのデータが前に詰まる
        saveDataList.saveDatas.RemoveAt(index);
        File.WriteAllText(saveFilePath, JsonUtility.ToJson(saveDataList, true));
        Debug.Log("セーブデータを削除しました: " + index);
    }

    public void DeleteAllSaveData()
    {
        if (File.Exists(saveFilePath))
        {
            File.Delete(saveFilePath);
            Debug.Log("セーブデータをすべて削除しました");
        }
    }

    // public void LoadGame()
    // {
    //     if (!File.Exists(saveFilePath))
    //     {
    //         Debug.LogWarning("セーブデータがありません。");
    //         return;
    //     }

    //     string json = File.ReadAllText(saveFilePath);
    //     SaveDateList SaveDateList = JsonUtility.FromJson<SaveDateList>(json);

    //     if (SaveDateList.saveDatas.Count > 0)
    //     {
    //         // リストの最後（最新）のデータを取得
    //         SaveData latest = SaveDateList.saveDatas[SaveDateList.saveDatas.Count - 1];

    //         NovelSystem.placeIndex = latest.placeIndex;
    //         NovelSystem.mesageIndex = latest.mesageIndex;

    //         NovelSystem.DisplayCurrentSentence();
    //         Debug.Log("ロード完了: " + latest.saveDate);
    //     }
    // }
}

[System.Serializable]
public class SaveData
{
    public string saveDate; // 保存日時
    public int placeIndex; // 現在の場所番号
    public int mesageIndex; // 現在のメッセージ番号
    public string currentTextSnippet; // データの識別用にテキストの一部を保存
}

[System.Serializable]
public class SaveDataList
{
    public List<SaveData> saveDatas = new List<SaveData>();
}
