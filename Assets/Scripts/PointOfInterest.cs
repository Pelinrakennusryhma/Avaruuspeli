using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

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
    Button enterButton;
    [SerializeField]
    WorldMapClickDetector worldMapClickDetector;
    [SerializeField]
    Canvas canvas;
    int modelAmount = 3;
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
        CreateGraphics();
        ApplyIcon();
    }
    void Update()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        screenPos += UIItemOffset;
        uiComponents.position = screenPos;

        CheckIfMothershipInVicinity();
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
        if(distance < 0.003)
        {
            EnableInfoPanel();
            GameManager.Instance.currentPOI = this;
        } else
        {
            DisableInfoPanel();
        }
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
