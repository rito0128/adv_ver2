using UnityEngine;
using UnityEngine.UI;
using TMPro;

// セーブバー1本分の表示
public class SaveSlotView : MonoBehaviour
{
    public TextMeshProUGUI titleText;  // 上段：No.と日時、または「セーブデータを作成」
    public TextMeshProUGUI detailText; // 下段：本文の冒頭
    public Button button;

    public void ShowCreateLabel()
    {
        titleText.text = "セーブデータを作成";
        detailText.text = "";
    }

    public void ShowData(int slotNo, SaveData data)
    {
        titleText.text = "No." + slotNo + "　" + data.saveDate;
        detailText.text = data.currentTextSnippet + "…";
    }

    public void ShowEmpty(int slotNo)
    {
        titleText.text = "No." + slotNo + "　NO DATA";
        detailText.text = "";
    }
}
