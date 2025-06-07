using UnityEngine;

public class AOEEffect : MonoBehaviour
{
    public float duration = 0.5f;
    public float maxScale = 5f;
    public AnimationCurve scaleCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    public Color startColor = new Color(1f, 0f, 0f, 0.5f);
    public Color endColor = new Color(1f, 0f, 0f, 0f);
    
    private float timer = 0f;
    private SpriteRenderer spriteRenderer;
    
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        }
        
        // Set initial color
        spriteRenderer.color = startColor;
        
        // Auto destroy after duration
        Destroy(gameObject, duration);
    }
    
    void Update()
    {
        timer += Time.deltaTime;
        float progress = timer / duration;
        
        // Animate scale
        float scale = scaleCurve.Evaluate(progress) * maxScale;
        transform.localScale = Vector3.one * scale;
        
        // Animate color
        spriteRenderer.color = Color.Lerp(startColor, endColor, progress);
    }
} 