using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// josnファイル読み込み用のオブジェクト
[System.Serializable]
public class ScenarioContent
{
    public string Name;
    public string Text;
    public string Image;
}

[System.Serializable]
public class Place
{
    public string placeName;
    public string backgroundImage;
    public List<ScenarioContent> scenarioContents;
}

[System.Serializable]
public class SectionData
{
    public string sectionTitle;
    public List<Place> sectionContent;
}

public class NovelSystem : MonoBehaviour
{
    [Header("UI References")]
    public static SectionData sectionData;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI contentText;
    public Image backgroundImageDisplay;
    
    public static int placeIndex = 0;
    public static int mesageIndex = 0;

    void Start()
    {
        LoadJSON();
        DisplayCurrentSentence();
    }

    void LoadJSON()
    {
        placeIndex = 0;
        mesageIndex = 0;
        // Resources/scenario.json を読み込む
        TextAsset jsonFile = Resources.Load<TextAsset>("scenario");
        
        if (jsonFile == null)
        {
            Debug.LogError("Resourcesフォルダに 'scenario' (JSON) が見つかりません。");
            return;
        }

        // JSONをオブジェクトに変換
        sectionData = JsonUtility.FromJson<SectionData>(jsonFile.text);

        if (sectionData == null)
        {
            Debug.LogError("scenario.json'の読み込みに失敗しました");
            return;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                if (IsClickingSpecificObject("MenuBar") || IsClickingSpecificObject("SaveButtom")) 
                {
                    return; 
                }
            }
            AdvanceText();
        }
    }

    void AdvanceText()
    {
        if (sectionData == null || sectionData.sectionContent.Count == 0) return; // .Count : List型のプロパティ．sectionData.sectionContentの数を取ってくる

        bool isLastPlace = (placeIndex == sectionData.sectionContent.Count - 1);
        bool isLastEntry = (mesageIndex == sectionData.sectionContent[placeIndex].scenarioContents.Count - 1);

        if (isLastPlace && isLastEntry)
        {
            Debug.Log("最後のテキストに到達しているため、更新を停止します。");
            return; // これ以降の処理（インデックスの増加）を行わずに終了
        }

        mesageIndex++;

        // 現在の場所のセリフが終わったか確認
        if (mesageIndex >= sectionData.sectionContent[placeIndex].scenarioContents.Count)
        {
            mesageIndex = 0;
            placeIndex++; // 次の場所へ
        }

        // 全ての場所が終わったか確認
        if (placeIndex < sectionData.sectionContent.Count)
        {
            DisplayCurrentSentence();
        }
        else
        {
            Debug.Log("セクション終了");
            // ループさせる、またはタイトルへ戻る等の処理
        }
    }

    void DisplayCurrentSentence() 
    {
        if (sectionData == null) return;

        Place currentPlace = sectionData.sectionContent[placeIndex];
        ScenarioContent currentMesage = currentPlace.scenarioContents[mesageIndex];

        if (backgroundImageDisplay != null)
        {
            Sprite bg;

            if (string.IsNullOrEmpty(currentPlace.backgroundImage)) // ""を判定
            {
                bg = Resources.Load<Sprite>("bg_defolt");
                Debug.Log("背景指定がないためデフォルトをロードします");
            } else
            {
                bg = Resources.Load<Sprite>(currentPlace.backgroundImage);
            }
                
            if (bg != null) {
                backgroundImageDisplay.sprite = bg;
                Debug.Log("2");
            }
        }

        if (nameText != null) nameText.text = currentMesage.Name;
        if (contentText != null) contentText.text = currentMesage.Text;
    }

    bool IsClickingSpecificObject(string objectName)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (var result in results)
        {
            if (result.gameObject.name == objectName) return true;
        }
        return false;
    }
}

