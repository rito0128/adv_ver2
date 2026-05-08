using UnityEngine;

public class AspectKeeper : MonoBehaviour
{
    [SerializeField] private Vector2 targetAspect = new Vector2(16, 9);

    void Awake()
    {
        Camera cam = GetComponent<Camera>();
        float targetRatio = targetAspect.x / targetAspect.y;
        float currentRatio = (float)Screen.width / Screen.height;
        float scale = currentRatio / targetRatio;

        if (scale < 1.0f)
        {
            Rect rect = cam.rect;
            rect.width = 1.0f;
            rect.height = scale;
            rect.x = 0;
            rect.y = (1.0f - scale) / 2.0f;
            cam.rect = rect;
        }
        else
        {
            float scaleWidth = 1.0f / scale;
            Rect rect = cam.rect;
            rect.width = scaleWidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scaleWidth) / 2.0f;
            rect.y = 0;
            cam.rect = rect;
        }
    }
}