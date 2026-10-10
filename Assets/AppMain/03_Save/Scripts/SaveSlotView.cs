using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

// セーブバー1本分の表示
public class SaveSlotView : MonoBehaviour
{
    public TextMeshProUGUI titleText;  // 上段：No.と日時、または「セーブデータを作成」
    public TextMeshProUGUI detailText; // 下段：本文の冒頭
    public Button button;
    public Button deleteButton;        // 個別削除ボタン

    public Action<int> onDelete; // 削除ボタンが押されたときに呼ぶ処理
    private int dataIndex;       // このバーが表示しているデータの番号（バー2が0）

    public void Setup(int dataIndex, Action<int> onDelete)
    {
        this.dataIndex = dataIndex;
        this.onDelete = onDelete;
    }

    // 削除ボタンのOnClickに登録する
    public void OnClickDelete()
    {
        onDelete?.Invoke(dataIndex);
    }

    public void ShowCreateLabel()
    {
        titleText.text = "セーブデータを作成";
        detailText.text = "";
        SetDeleteButtonActive(false);
    }

    public void ShowData(int slotNo, SaveData data)
    {
        titleText.text = "No." + slotNo + "　" + data.saveDate;
        detailText.text = data.currentTextSnippet + "…";
        SetDeleteButtonActive(true);
    }

    public void ShowEmpty(int slotNo)
    {
        titleText.text = "No." + slotNo + "　NO DATA";
        detailText.text = "";
        SetDeleteButtonActive(false);
    }

    void SetDeleteButtonActive(bool active)
    {
        if (deleteButton != null) deleteButton.gameObject.SetActive(active);
    }
}
