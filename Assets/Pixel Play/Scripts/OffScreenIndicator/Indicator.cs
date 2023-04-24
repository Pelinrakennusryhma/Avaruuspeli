using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Assign this script to the indicator prefabs.
/// </summary>
public class Indicator : MonoBehaviour
{
    [SerializeField] private IndicatorType indicatorType;
    private Image indicatorImage;
    private Sprite defaultSprite;
    [SerializeField] private TMP_Text distanceText;
    [SerializeField] private TMP_Text descriptionText;

    /// <summary>
    /// Gets if the game object is active in hierarchy.
    /// </summary>
    public bool Active
    {
        get
        {
            return transform.gameObject.activeInHierarchy;
        }
    }

    /// <summary>
    /// Gets the indicator type
    /// </summary>
    public IndicatorType Type
    {
        get
        {
            return indicatorType;
        }
    }

    public void SetSourceImage(Sprite sprite)
    {
        if(indicatorType == IndicatorType.BOX)
        {
            indicatorImage.sprite = sprite;
        }
    }

    public void SetDefaultImage()
    {
        if (indicatorType == IndicatorType.BOX)
        {
            indicatorImage.sprite = defaultSprite;
        }
    }

    public void SetDescription(string text)
    {
        descriptionText.text = text;       
    }

    public void SetScale(float scale)
    {
        indicatorImage.transform.localScale = Vector3.one * scale;
    }

    void Awake()
    {
        indicatorImage = transform.GetComponentInChildren<Image>();
        defaultSprite = indicatorImage.sprite;
        if(distanceText == null)
        {
            distanceText = transform.GetComponentInChildren<TMP_Text>();
        }    
    }

    /// <summary>
    /// Sets the image color for the indicator.
    /// </summary>
    /// <param name="color"></param>
    public void SetImageColor(Color color)
    {
        indicatorImage.color = color;
    }

    /// <summary>
    /// Sets the distance text for the indicator.
    /// </summary>
    /// <param name="value"></param>
    public void SetDistanceText(float value)
    {
        distanceText.text = value >= 0 ? Mathf.Floor(value) + " m" : "";
    }

    /// <summary>
    /// Sets the distance text rotation of the indicator.
    /// </summary>
    /// <param name="rotation"></param>
    public void SetTextRotation(Quaternion rotation)
    {
        distanceText.rectTransform.rotation = rotation;
        descriptionText.rectTransform.rotation = rotation;
    }

    /// <summary>
    /// Sets the indicator as active or inactive.
    /// </summary>
    /// <param name="value"></param>
    public void Activate(bool value)
    {
        transform.gameObject.SetActive(value);
    }
}

public enum IndicatorType
{
    BOX,
    ARROW
}
