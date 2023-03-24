using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PointOfInterest : MonoBehaviour
{
    [SerializeField]
    GameObject[] models;
    [SerializeField]
    float modelSpawnArea = 2f;
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

    int modelAmount = 3;
    // Start is called before the first frame update
    void Start()
    {
        CreateGraphics();
        ApplyIcon();
    }
    void CreateGraphics()
    {
        for (int i = 0; i < modelAmount; i++)
        {
            Vector3 spawnPos = Random.insideUnitSphere * modelSpawnArea + transform.position;
            spawnPos.y = transform.position.y;
            GameObject model = models[Random.Range(0, models.Length)];
            GameObject spawnedModel = Instantiate(model, spawnPos, Random.rotation, transform);
            spawnedModel.transform.localScale *= modelSize;
        }
    }

    void ApplyIcon()
    {
        foreach (TMP_Text iconText in iconTexts)
        {
            iconText.text = Icon;
        }
    }

    void Update()
    {
        //Vector3 screenPos = Camera.main.transform.InverseTransformPoint(transform.position);
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        screenPos.x -= Screen.width / 2;
        screenPos.y -= Screen.height / 2;
        screenPos += UIItemOffset;

        uiComponents.localPosition = screenPos;
        //for (int i = 0; i < infoCanvas.transform.childCount; i++)
        //{
        //    Transform child = infoCanvas.transform.GetChild(i);
        //    child.localPosition = screenPos;
        //}
    }
}
