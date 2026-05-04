using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangerInTitle : MonoBehaviour
{
    public void GoToHome()
    {
        SceneManager.LoadScene("01_Home");
    }
}
