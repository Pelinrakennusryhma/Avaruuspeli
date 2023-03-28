using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PointOfInterest : MonoBehaviour
{
    [SerializeField]
    GameObject[] models;
    [SerializeField]
    float modelSpawnArea = 0.1f;
    [SerializeField]
    float modelSize = 0.5f;
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
    WorldMapClickDetector worldMapClickDetector;
    int modelAmount = 3;

    private void Awake()
    {
        worldMapClickDetector.OnObjectClicked -= OnAsteroidFieldClicked;
        worldMapClickDetector.OnObjectClicked += OnAsteroidFieldClicked;
    }

    void Start()
    {
        CreateGraphics();
        ApplyIcon();
    }
    void Update()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        screenPos.x -= Screen.width / 2;
        screenPos.y -= Screen.height / 2;
        screenPos += UIItemOffset;

        uiComponents.localPosition = screenPos;

        CheckIfMothershipInVicinity();
    }

    public void Init(POISceneData data)
    {
        description.text = data.GetDescription();
        title.text = data.SceneName;
    }

    void CheckIfMothershipInVicinity()
    {
        float distance = (MotherShipOnWorldMapController.Instance.transform.position - transform.position).sqrMagnitude;
        if(distance < 0.003)
        {
            EnableInfoPanel();
        } else
        {
            DisableInfoPanel();
        }
    }

    public void OnAsteroidFieldClicked(WorldMapClickDetector.ClickableObjectType objectType)
    {
        GameManager.Instance.currentPOI = this;
        //GameManager.Instance.CurrentAsteroidFieldData = AsteroidFieldData;
        Debug.LogError("Clicked POI");
    }

    void CreateGraphics()
    {
        for (int i = 0; i < modelAmount; i++)
        {
            Vector3 spawnPos = Random.insideUnitSphere * modelSpawnArea + transform.position;
            spawnPos.y = transform.position.y;
            GameObject model = models[Random.Range(0, models.Length)];
            GameObject spawnedModel = Instantiate(model, spawnPos, Quaternion.identity, transform);
            spawnedModel.transform.localScale *= modelSize;

            // Flatten for world map
            spawnedModel.transform.localScale = new Vector3(
                spawnedModel.transform.localScale.x,
                spawnedModel.transform.localScale.y / 10f,
                spawnedModel.transform.localScale.z);
        }
    }

    void ApplyIcon()
    {
        foreach (TMP_Text iconText in iconTexts)
        {
            iconText.text = Icon;
        }
    }

    void EnableInfoPanel()
    {
        iconBig.SetActive(false);
        infoPanel.SetActive(true);
    }

    void DisableInfoPanel()
    {
        iconBig.SetActive(true);
        infoPanel.SetActive(false);
    }
}
