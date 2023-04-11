using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PointOfInterest : MonoBehaviour
{
    [Tooltip("Should the PoI be destroyed after visit?")]
    public bool oneTimeVisit = false;
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

    private void Awake()
    {
        targetScript = GetComponent<Target>();
        targetScript.descriptionText = Icon;

        enterButton.onClick.AddListener(OnEnterClicked);
    }

    void OnEnterClicked()
    {
        GameManager.Instance.EnterPOI(this);
    }

    void Start()
    {
        // placeholder mechanism for planets
        if(Data != null)
        {
            Init(Data);
        }

        ApplyIcon();
    }

    private void OnEnable()
    {
        DisableInfoPanel();
    }

    void Update()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        screenPos += UIItemOffset;
        uiComponents.position = screenPos;

        //CheckIfMothershipInVicinity();
    }

    public void Init(POISceneData data)
    {
        Data = data;
        description.text = data.GetDescription();
        title.text = data.Title;
    }

    void CheckIfMothershipInVicinity()
    {
        float distance = (MotherShipOnWorldMapController.Instance.transform.position - transform.position).sqrMagnitude;
        if(distance < 0.005)
        {
            EnableInfoPanel();
            GameManager.Instance.currentPOI = this;
        } else
        {
            DisableInfoPanel();
        }
    }

    private void OnTriggerEnter(Collider other)
    {      
        if (other.gameObject.CompareTag("PlayerShip"))
        {
            EnableInfoPanel();
            GameManager.Instance.currentPOI = this;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerShip"))
        {
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
}
