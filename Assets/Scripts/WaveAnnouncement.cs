using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WaveAnnouncement : MonoBehaviour
{
    public float fadeInDuration = 0.5f;
    public float displayDuration = 2f;
    public float fadeOutDuration = 0.5f;
    public AnimationCurve scaleCurve = AnimationCurve.EaseInOut(0, 0.5f, 1, 1);
    
    private Text textComponent;
    private CanvasGroup canvasGroup;
    
    void Awake()
    {
        textComponent = GetComponent<Text>();
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }
    
    public void ShowAnnouncement(string message, int fontSize = 50)
    {
        if (textComponent != null)
        {
            textComponent.text = message;
            textComponent.fontSize = fontSize;
        }
        
        StartCoroutine(AnimateAnnouncement());
    }
    
    IEnumerator AnimateAnnouncement()
    {
        // Fade in
        float timer = 0;
        while (timer < fadeInDuration)
        {
            timer += Time.deltaTime;
            float progress = timer / fadeInDuration;
            canvasGroup.alpha = progress;
            
            float scale = scaleCurve.Evaluate(progress);
            transform.localScale = Vector3.one * scale;
            
            yield return null;
        }
        
        canvasGroup.alpha = 1;
        transform.localScale = Vector3.one;
        
        // Display
        yield return new WaitForSeconds(displayDuration);
        
        // Fade out
        timer = 0;
        while (timer < fadeOutDuration)
        {
            timer += Time.deltaTime;
            float progress = 1 - (timer / fadeOutDuration);
            canvasGroup.alpha = progress;
            
            yield return null;
        }
        
        canvasGroup.alpha = 0;
    }
} 