using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangerInTitle : MonoBehaviour
{
    public void GoToHome()
    {
        NovelSystem.ResetProgress();
        SceneManager.LoadScene("01_Home");
    }
}
