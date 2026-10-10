using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

// セーブ画面のバー一覧を管理する
public class SaveListView : MonoBehaviour
{
    public SaveLoad saveLoad;
    public ScrollRect scrollRect;
    public SaveSlotView slotPrefab;
    public int slotCount = 10;   // バーの本数
    public int visibleCount = 4; // 一度に表示する本数

    private List<SaveSlotView> slots = new List<SaveSlotView>();

    void Start()
    {
        CreateSlots();
        FitViewportHeight();
        Refresh();
    }

    // プレハブからバーを生成してContentに並べる
    void CreateSlots()
    {
        for (int i = 0; i < slotCount; i++)
        {
            SaveSlotView slot = Instantiate(slotPrefab, scrollRect.content);
            slot.name = "SaveBar" + (i + 1);
            if (i > 0) slot.Setup(i - 1, OnClickDeleteSlot); // バー2がsaveDatas[0]
            slots.Add(slot);
        }

        // 一番上のバーはセーブデータ作成ボタン
        slots[0].button.onClick.AddListener(OnClickCreate);
    }

    // Viewportの高さをバーvisibleCount本分に合わせる
    void FitViewportHeight()
    {
        float barHeight = ((RectTransform)slotPrefab.transform).rect.height;
        float spacing = 0;
        VerticalLayoutGroup layout = scrollRect.content.GetComponent<VerticalLayoutGroup>();
        if (layout != null) spacing = layout.spacing;

        float height = barHeight * visibleCount + spacing * (visibleCount - 1);
        if (layout != null) height += layout.padding.top + layout.padding.bottom;

        // ScrollRect本体の高さを変えるとViewport（ストレッチ設定）も追従する
        RectTransform scrollRectTransform = (RectTransform)scrollRect.transform;
        scrollRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
    }

    // セーブデータを読み込んで全バーの表示を更新する
    public void Refresh()
    {
        SaveDataList saveDataList = saveLoad.LoadSaveDataList();

        slots[0].ShowCreateLabel();
        for (int i = 1; i < slots.Count; i++)
        {
            int dataIndex = i - 1; // バー2がsaveDatas[0]（最新）
            if (dataIndex < saveDataList.saveDatas.Count)
            {
                slots[i].ShowData(i + 1, saveDataList.saveDatas[dataIndex]);
            }
            else
            {
                slots[i].ShowEmpty(i + 1);
            }
        }
    }

    void OnClickCreate()
    {
        if (saveLoad.SaveGame())
        {
            Refresh();
        }
    }

    // 各バーの削除ボタンから呼ばれる
    void OnClickDeleteSlot(int dataIndex)
    {
        saveLoad.DeleteSaveData(dataIndex);
        Refresh();
    }

    // 全件削除ボタンのOnClickに登録する
    public void OnClickDeleteAll()
    {
        saveLoad.DeleteAllSaveData();
        Refresh();
    }
}
