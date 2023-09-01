using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum POIType
{
    Asteroid,
    Planet
}

public class PointOfInterest : MonoBehaviour
{
    [SerializeField]
    POIType poiType;
    public bool OneTimeVisit { get; private set; }
    [SerializeField]
    Transform uiComponents;
    [SerializeField]
    Vector3 UIItemOffset = new Vector3(0f, 40f, 0f);
    [field: SerializeField]
    public string Icon { get; private set; }
    [SerializeField]
    TMP_Text[] iconTexts;
    [SerializeField]
    TMP_Text title;
    [SerializeField]
    TMP_Text description;
    [SerializeField]
    GameObject iconBig;
    [SerializeField]
    GameObject infoPanel;
    [SerializeField]
    Button enterButton;
    [SerializeField]
    Button maximizeButton;
    [SerializeField]
    WorldMapClickDetector worldMapClickDetector;
    [SerializeField]
    Canvas canvas;
    [field: SerializeField]
    public POISceneData Data { get; private set; }

    private Target targetScript;
    bool mothershipInTrigger = false;

    private void Awake()
    {
        targetScript = GetComponent<Target>();
        targetScript.descriptionText = Icon;

        if(poiType == POIType.Asteroid)
        {
            OneTimeVisit = true;
        } else
        {
            OneTimeVisit = false;
        }

        enterButton.onClick.AddListener(OnEnterClicked);
    }

    void OnEnterClicked()
    {
        if(poiType == POIType.Asteroid)
        {
            if (GameManager.Instance.ShipLifeSupportSystem.CheckIfWeCanEnterShip()) 
            {
                GameManager.Instance.EnterPOI(this);
            }

            else
            {
                GameManager.Instance.ShipLifeSupportSystem.DisplayPromptAboutNotBeingAbleToEnterShip();
            }

        } else
        {
            GameManager.Instance.EnterPlanet();
        }

    }

    void Start()
    {
        ApplyIcon();
        UpdatePosition();
        DisableInfoPanel();
    }

    void Update()
    {
        UpdatePosition();
    }

    void UpdatePosition()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        screenPos += UIItemOffset;
        uiComponents.position = screenPos;
    }

    public void Init(PlanetOnWorldMap planet)
    {
        // Just some random text since nobody seems to care enough
        description.text = "A planet with minerals and a shop.";
        title.text = "Habitable planet";

        enterButton.onClick.AddListener(() =>
        {
            GameManager.Instance.CurrentPlanet = planet;
            GameManager.Instance.CurrentPlanetData = planet.PlanetData;
        });
    }

    public void Init(POISceneData data, POISpawner.OnPOIEnteredDelegate callback=null)
    {
        Data = data;
        description.text = data.GetDescription();
        title.text = data.Title;

        if(callback != null)
        {
            enterButton.onClick.AddListener(() => callback(this));
        }
    }

    private void OnTriggerEnter(Collider other)
    {      
        if (other.gameObject.CompareTag("PlayerShip"))
        {
            mothershipInTrigger = true;
            StartCoroutine(EnableInfoPanelWithDelay());
        }
    }

    IEnumerator EnableInfoPanelWithDelay(float delay = 0.25f)
    {
        yield return new WaitForSeconds(delay);
        if (mothershipInTrigger)
        {
            EnableInfoPanel();
            GameManager.Instance.currentPOI = this;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerShip"))
        {
            mothershipInTrigger = false;
            DisableInfoPanel();
        }
    }

    void ApplyIcon()
    {
        foreach (TMP_Text iconText in iconTexts)
        {
            if(Icon != "")
            {
                iconText.text = Icon;
            } else
            {
                Destroy(iconText.gameObject);
            }

        }
    }

    void EnableInfoPanel()
    {
        if(iconBig != null)
        {
            iconBig.SetActive(false);
        }

        infoPanel.SetActive(true);
        maximizeButton.gameObject.SetActive(false);
    }

    void DisableInfoPanel()
    {
        if (iconBig != null)
        {
            iconBig.SetActive(true);
        }
        infoPanel.SetActive(false);
        maximizeButton.gameObject.SetActive(false);
    }

    public void OnMinimize()
    {
        if (iconBig != null)
        {
            iconBig.SetActive(true);
        }

        infoPanel.SetActive(false);
        maximizeButton.gameObject.SetActive(true);
    }

    public void OnMaximize()
    {
        if (iconBig != null)
        {
            iconBig.SetActive(false);
        }

        infoPanel.SetActive(true);
        maximizeButton.gameObject.SetActive(false);
    }

    public void Destroy()
    {
        Destroy(transform.parent.gameObject);
    }

    private void OnDisable()
    {
        DisableInfoPanel();
    }
}
