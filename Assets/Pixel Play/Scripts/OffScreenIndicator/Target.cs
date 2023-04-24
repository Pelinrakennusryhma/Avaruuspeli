using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Attach this script to all the target game objects in the scene.
/// </summary>
[DefaultExecutionOrder(0)]
public class Target : MonoBehaviour
{
    [Tooltip("Change this color to change the indicators color for this target")]
    [SerializeField] private Color targetColor = Color.red;

    [Tooltip("Select if box indicator is required for this target")]
    [SerializeField] private bool needBoxIndicator = true;

    [Tooltip("Select if arrow indicator is required for this target")]
    [SerializeField] private bool needArrowIndicator = true;

    [Tooltip("Select if distance text is required for this target")]
    [SerializeField] private bool needDistanceText = true;

    [Tooltip("Optional description text to show above the indicator")]
    public string descriptionText;

    [Tooltip("Select if description text should be shown with arrow indicator")]
    [SerializeField] private bool needDescriptionOnArrow = false;

    [Tooltip("Optional sprite to use instead of the default circle")]
    public Sprite customSprite;

    [Tooltip("Scale of the icon")]
    public float targetScale = 1f;
    /// <summary>
    /// Please do not assign its value yourself without understanding its use.
    /// A reference to the target's indicator, 
    /// its value is assigned at runtime by the offscreen indicator script.
    /// </summary>
    [HideInInspector] public Indicator indicator;

    /// <summary>
    /// Gets the color for the target indicator.
    /// </summary>
    public Color TargetColor
    {
        get
        {
            return targetColor;
        }
    }

    public void SetColor(Color color)
    {
        targetColor = color;
    }

    public void SetScale(float scale)
    {
        targetScale = scale;
    }

    /// <summary>
    /// Gets if box indicator is required for the target.
    /// </summary>
    public bool NeedBoxIndicator
    {
        get
        {
            return needBoxIndicator;
        }
    }

    /// <summary>
    /// Gets if arrow indicator is required for the target.
    /// </summary>
    public bool NeedArrowIndicator
    {
        get
        {
            return needArrowIndicator;
        }
    }

    /// <summary>
    /// Gets if the distance text is required for the target.
    /// </summary>
    public bool NeedDistanceText
    {
        get
        {
            return needDistanceText;
        }
    }

    public bool NeedDescriptionOnArrow
    {
        get
        {
            return needDescriptionOnArrow;
        }
    }

    /// <summary>
    /// On enable add this target object to the targets list.
    /// </summary>
    private void OnEnable()
    {
        if(OffScreenIndicator.TargetStateChanged != null)
        {
            OffScreenIndicator.TargetStateChanged.Invoke(this, true);
        }
    }

    /// <summary>
    /// On disable remove this target object from the targets list.
    /// </summary>
    private void OnDisable()
    {
        if(OffScreenIndicator.TargetStateChanged != null)
        {
            OffScreenIndicator.TargetStateChanged.Invoke(this, false);
        }
    }

    /// <summary>
    /// Gets the distance between the camera and the target.
    /// </summary>
    /// <param name="cameraPosition">Camera position</param>
    /// <returns></returns>
    public float GetDistanceFromCamera(Vector3 cameraPosition)
    {
        float distanceFromCamera = Vector3.Distance(cameraPosition, transform.position);
        return distanceFromCamera;
    }
}
