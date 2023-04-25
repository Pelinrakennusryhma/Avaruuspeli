using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetOnWorldMap : MonoBehaviour
{
    public PlanetData PlanetData;

    public StarSystemOnFocus ParentStarSystem;

    public CenterStarOnWorldMap CenterStar;

    public LineRenderer LineRenderer;

    public WorldMapClickDetector ClickDetector;


    public Vendor Vendor;

    [SerializeField]
    GameObject POIPrefab;


    public void Init(CenterStarOnWorldMap centerStar,
                     StarSystemOnFocus parentStarSystem)
    {
        CenterStar = centerStar;
        ParentStarSystem = parentStarSystem;

        ClickDetector = GetComponent<WorldMapClickDetector>();
        ClickDetector.OnObjectClicked -= OnPlanetClicked;
        ClickDetector.OnObjectClicked += OnPlanetClicked;

        Instantiate(POIPrefab, transform);
    }

    public void OnPlanetClicked(WorldMapClickDetector.ClickableObjectType type)
    {
        Debug.Log("clicked planet?");
        GameManager.Instance.CurrentPlanet = this;
        GameManager.Instance.CurrentPlanetData = PlanetData;
        SetVendor();

        //Debug.Log("Planet listens to a click now");
    }

    public void SetPlanetData(PlanetData data)
    {
        PlanetData = data;
    }

    public void SetInitialStartingPosition(bool setInitialStartingPos)
    {
        if (setInitialStartingPos)
        {
            float radius = (CenterStar.transform.position - transform.position).magnitude;
            transform.position = CenterStar.transform.position;

            transform.Rotate(new Vector3(0, Random.Range(0, 360), 0));
            transform.position = CenterStar.transform.position + -transform.forward * radius;

            PlanetData.SetPosRotAndScale(transform.position,
                                         transform.rotation,
                                         transform.localScale);  
            
            GameManager.Instance.SaverLoader.SavePlanet(ParentStarSystem.ParentGalaxy.GalaxyData.ID,
                                                        ParentStarSystem.StarSystemData.ID,
                                                        PlanetData);

        }

        else
        {
            transform.position = PlanetData.GetPos();
            transform.rotation = PlanetData.GetRot();
            transform.localScale = PlanetData.GetLocalScale();

            //Debug.Log("We are loading and all that jazz");
            //float radius = (CenterStar.transform.position - transform.position).magnitude;
            //transform.position = CenterStar.transform.position;

            //transform.Rotate(new Vector3(0, Random.Range(0, 360), 0));
            //transform.position = CenterStar.transform.position + -transform.forward * radius;

            //PlanetData.SetPosRotAndScale(transform.position,
            //                             transform.rotation,
            //                             transform.localScale);
        }


    }

    public void DrawOrbit()
    {

        LineRenderer.useWorldSpace = false;

        float radius = (CenterStar.transform.position - transform.position).magnitude;

        LineRenderer.material = UniverseController.Instance.PlanetOrbitMaterial;

        //DrawPolygon(360, radius, CenterStar.transform.position, 0.01f, 0.01f);
        DrawCircle(CenterStar.gameObject, radius, 0.005f);
        //Debug.Log("Should draw orbit " + gameObject.name);
    }

    public void HideOrbit()
    {
        //Debug.Log("Should hide orbit " + gameObject.name);
    }

    // https://www.codinblack.com/how-to-draw-lines-circles-or-anything-else-using-linerenderer/
    // This function draws the line on x and y axis, so it doesn't work on our purposes
    void DrawPolygon(int vertexNumber, float radius, Vector3 centerPos, float startWidth, float endWidth)
    {
        LineRenderer.startWidth = startWidth;
        LineRenderer.endWidth = endWidth;
        LineRenderer.loop = true;
        float angle = 2 * Mathf.PI / vertexNumber;
        LineRenderer.positionCount = vertexNumber;

        for (int i = 0; i < vertexNumber; i++)
        {
            Matrix4x4 rotationMatrix = new Matrix4x4(new Vector4(Mathf.Cos(angle * i), Mathf.Sin(angle * i), 0, 0),
                                                     new Vector4(-1 * Mathf.Sin(angle * i), Mathf.Cos(angle * i), 0, 0),
                                                     new Vector4(0, 0, 1, 0),
                                                     new Vector4(0, 0, 0, 1));



            Vector3 initialRelativePosition = new Vector3(0, radius, 0);
            
            LineRenderer.SetPosition(i, centerPos + rotationMatrix.MultiplyPoint(initialRelativePosition));

        }
    }

    // https://www.loekvandenouweland.com/content/use-linerenderer-in-unity-to-draw-a-circle.html
    public void DrawCircle(GameObject container, float radius, float lineWidth)
    {
        int segments = 360;

        LineRenderer.useWorldSpace = true;
        LineRenderer.loop = true;
        LineRenderer.startWidth = lineWidth;
        LineRenderer.endWidth = lineWidth;
        LineRenderer.positionCount = segments + 1;

        int pointCount = segments + 1; // add extra point to make startpoint and endpoint the same to close the circle
        Vector3[] points = new Vector3[pointCount];

        for (int i = 0; i < pointCount; i++)
        {
            float rad = Mathf.Deg2Rad * (i * 360f / segments);
            points[i] = container.transform.position + new Vector3(Mathf.Sin(rad) * radius, 0, Mathf.Cos(rad) * radius);
        }

        LineRenderer.SetPositions(points);
    }

    public void SetVendor()
    {
        if (ParentStarSystem == null)
        {
            Debug.LogError("Null parent star system");
        }

        else if (ParentStarSystem.ParentGalaxy == null)
        {
            Debug.LogError("Null parent galaxy");
        }


        if (PlanetData == null)
        {
            Debug.LogError("Null planet data");
        }

       // Debug.Log("Setting vendor");

        Vendor = Vendor.GetVendor(ParentStarSystem.ParentGalaxy.GalaxyData.ID,
                                  ParentStarSystem.StarSystemData.ID,
                                  PlanetData.ID);

        if (Vendor != null
            && Vendor.Items.Count == 0)
        {
            //Debug.LogWarning("Refilling vendor inventory");
            Vendor.InitializeVendor(ParentStarSystem.ParentGalaxy.GalaxyData.ID,
                                    ParentStarSystem.StarSystemData.ID,
                                    PlanetData.ID);
        }

        if (Vendor == null)
        {
            //Debug.LogError("Vendor is null");
        }

        if (Vendor == null
            || (Vendor != null
                && Vendor.SellMultiplierss.Length != GameManager.Instance.InventoryController.ItemDataBaseWithScriptables.ItemDataBaseSO.AllItems.Count))
        {



            if ((Vendor != null
                && Vendor.SellMultiplierss.Length != GameManager.Instance.InventoryController.ItemDataBaseWithScriptables.ItemDataBaseSO.AllItems.Count))
            {
                Debug.LogError("Buy and sell multiplier counts do not match. Sell multipliers is " + Vendor.SellMultiplierss.Length + " all items count is " + GameManager.Instance.InventoryController.ItemDataBaseWithScriptables.ItemDataBaseSO.AllItems.Count);
            }

            else
            {
                Debug.LogWarning("Counts match and vendor is not null");
            }

            Vendor = new Vendor();
            Vendor.InitializeVendor(ParentStarSystem.ParentGalaxy.GalaxyData.ID,
                                    ParentStarSystem.StarSystemData.ID,
                                    PlanetData.ID);




            Debug.LogError("Null vendor. A new one is created....");
        }
    }
}
