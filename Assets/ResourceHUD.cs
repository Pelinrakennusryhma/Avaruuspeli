using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResourceHUD : MonoBehaviour
{

    public AsteroidSurface AsteroidSurface;
    public PlanetSurface PlanetSurface;

    public RaycastHit[] RaycastHits;

    public Collider[] IgnoreColliders;

    public TextMeshProUGUI HUDText;

    public Vector3 OriginalHUDScale;

    [SerializeField]
    float scanDistance = 20f;

    public void Awake()
    {
        AsteroidSurface = FindObjectOfType<AsteroidSurface>();

        if (AsteroidSurface == null)
        {
            PlanetSurface = FindObjectOfType<PlanetSurface>();
        }

        RaycastHits = new RaycastHit[32];

        IgnoreColliders = transform.parent.GetComponentsInChildren<Collider>();

        OriginalHUDScale = HUDText.transform.localScale;
        HUDText.transform.localScale = new Vector3(0, OriginalHUDScale.y, 0);
    }

    void Scan()
    {
        Debug.Log("scanning..");
        // Ignore pickup trigger and character collider layers
        int layerMask = 1 << 3;
        layerMask |= (1 << 2);
        layerMask = ~layerMask;

        RaycastHit hit;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, scanDistance, layerMask))
        {
            Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * hit.distance, Color.yellow);
            DestroyableRock hitRock = hit.collider.GetComponent<DestroyableRock>();
            if (hitRock != null)
            {
                ShowHUD(hitRock.ResourceType, hitRock.ResourceAmount);
            }
            else
            {
                HideHUD();
            }
        }
        else
        {
            HideHUD();
        }
    }

    void ShowHUD(ResourceInventory.ResourceType resource, int amount)
    {
        if (HUDText != null
            && amount > 0
            && resource != ResourceInventory.ResourceType.None)
        {
            HUDText.gameObject.SetActive(true);
            HUDText.text = "Rock contains " + resource.ToString();

            Vector3 currentScale = HUDText.transform.localScale;

            HUDText.transform.localScale = new Vector3(Mathf.Lerp(currentScale.x, OriginalHUDScale.x, Time.deltaTime * 12.0f),
                                                       Mathf.Lerp(currentScale.y, OriginalHUDScale.y, Time.deltaTime * 12.0f),
                                                       OriginalHUDScale.z);
        }
        else
        {
            HideHUD();
        }
    }

    void HideHUD()
    {
        Vector3 currentScale = HUDText.transform.localScale;

        HUDText.transform.localScale = new Vector3(Mathf.Lerp(currentScale.x, 0, Time.deltaTime * 20.0f),
                                                   Mathf.Lerp(currentScale.y, 0, Time.deltaTime * 20.0f),
                                                   OriginalHUDScale.z);
    }

    void Update()
    {
        Scan();
        //RaycastHits = Physics.RaycastAll(Camera.main.transform.position,
        //                                 Camera.main.transform.forward,                                  
        //                                 20.0f);

        //bool closestIsDestroyableRock = false;
        //float distanceToClosest = 10000.0f;

        //Collider collider = null;

        //for (int i = 0; i < RaycastHits.Length; i++)
        //{
        //    bool hitAnIgnoreCollider = false;

        //    for (int j = 0; j < IgnoreColliders.Length; j++) 
        //    {
        //        if (RaycastHits[i].collider.gameObject == IgnoreColliders[j].gameObject)
        //        {
        //            hitAnIgnoreCollider = true;
        //            break;
        //        }
        //    }

        //    if (!hitAnIgnoreCollider)
        //    {
        //        if (RaycastHits[i].distance <= distanceToClosest)
        //        {
        //            distanceToClosest = RaycastHits[i].distance;

        //            if (RaycastHits[i].collider.gameObject.layer == 6)
        //            {
        //                closestIsDestroyableRock = true;

        //            }

        //            else
        //            {
        //                closestIsDestroyableRock = false;
        //            }

        //            if (closestIsDestroyableRock) 
        //            {
        //                collider = RaycastHits[i].collider;
        //                //Debug.Log("Closest is destroyable rock");
        //            }

        //            else
        //            {
        //                collider = null;
        //                //Debug.Log("Closest IS NOT destroyable rock");
        //            }
        //        }

        //        //Debug.Log("hit collider " + RaycastHits[i].collider.gameObject.name);
        //    }


        //}
                        
        //ResourceInventory.ResourceType resource = ResourceInventory.ResourceType.None;
        //int amount = 0;

        //if (AsteroidSurface != null)
        //{
        //    AsteroidSurface.CompareCollider(collider, out resource, out amount);
        //}

        //else if(PlanetSurface != null)
        //{
        //    PlanetSurface.CompareCollider(collider, out resource, out amount);
        //}


        //if (HUDText != null
        //    && amount > 0
        //    && resource != ResourceInventory.ResourceType.None)
        //{
        //    ShowingResourceOnHUD = true;
        //    HUDText.gameObject.SetActive(true);

        //    string text = "";

        //    switch (resource)
        //    {
        //        case ResourceInventory.ResourceType.None:
        //            break;
        //        case ResourceInventory.ResourceType.TestDice:
        //            text = "Dice";
        //            break;
        //        case ResourceInventory.ResourceType.Gold:
        //            text = "Gold";
        //            break;
        //        case ResourceInventory.ResourceType.Silver:
        //            text = "Silver";
        //            break;
        //        case ResourceInventory.ResourceType.Copper:
        //            text = "Copper";
        //            break;
        //        case ResourceInventory.ResourceType.Iron:
        //            text = "Iron";
        //            break;
        //        case ResourceInventory.ResourceType.Diamond:
        //            text = "Diamonds";
        //            break;
        //        default:
        //            break;
        //    }

        //    text = "Rock contains " + text;
        //    HUDText.text = text;
        //    //HUDText.text = text + " " + amount.ToString();

        //    //Debug.Log(resource.ToString() + " " + amount.ToString());   
        //}


        //else
        //{
        //    ShowingResourceOnHUD = false;
        //    //HUDText.gameObject.SetActive(false);
        //}

        //if (ShowingResourceOnHUD)
        //{
        //    Vector3 currentScale = HUDText.transform.localScale;

        //    HUDText.transform.localScale = new Vector3(Mathf.Lerp(currentScale.x ,OriginalHUDScale.x, Time.deltaTime * 12.0f), 
        //                                               Mathf.Lerp(currentScale.y, OriginalHUDScale.y, Time.deltaTime * 12.0f),
        //                                               OriginalHUDScale.z);
        //}

        //else
        //{
        //    Vector3 currentScale = HUDText.transform.localScale;

        //    HUDText.transform.localScale = new Vector3(Mathf.Lerp(currentScale.x, 0, Time.deltaTime * 20.0f),
        //                                               Mathf.Lerp(currentScale.y, 0, Time.deltaTime * 20.0f),
        //                                               OriginalHUDScale.z);
        //}
    }


}
