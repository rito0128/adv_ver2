using UnityEngine;
using UnityEngine.SceneManagement;

public class ConfigBackButton : MonoBehaviour
{
    public static string lastSceneName;

    public void BackToLastScene()
    {
        Debug.Log("Backボタン");
        if (!string.IsNullOrEmpty(lastSceneName)) {
            SceneManager.LoadScene(lastSceneName);
            Debug.Log("ラストシーンへ");
        } else {
            SceneManager.LoadScene("00_Title");
            Debug.Log("タイトルへ");
        }
    }

    public void GoToConfig() 
    {
        lastSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("02_Config");
        Debug.Log("現在のシーンを記録");
    }

    public void GoToSave() 
    {
        Debug.Log("GoToSave()");
        lastSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("03_Save");
        Debug.Log("現在のシーンを記録");
    }
}