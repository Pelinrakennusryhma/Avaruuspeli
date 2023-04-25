using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManagerByAnttiK : MonoBehaviour
{
    public static GameManagerByAnttiK Instance;
/*     public Text TextLifeLeft;
    public Text TextAmmonLeft; */
    public Text TextKullat;
    public Text TextTimantit;
    public Text TextPuut;
    public Text TextKivet;
    public Text TextHopeat;
 /*    public int Life = 5;
    public int Ammon = 10;
 */    
    public bool StopMove = false;

    public int Kullat;
    public int Timantit;
    public int Kivet;
    public int Puut;
    public int Hopeat;
    // public GameObject AvaruusAlus;

   // public GameObject pointsTextPrefabs, itemInstance;
   // public string textToDisplay;


    public void Awake()
    {
        
        if (Instance == null)
        {
            // RelaunchingTheSameScene = false;
            Instance = this;
            // DontDestroyOnLoad(gameObject);
            // SceneManager.sceneLoaded -= OnSceneLoaded;
            // SceneManager.sceneLoaded += OnSceneLoaded;
            // OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
        }

        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
 /*        PlayerPrefs.SetFloat("PlayerX", Alus.transform.position.x);
        PlayerPrefs.SetFloat("PlayerY", AvaruusAlus.transform.position.y);
        PlayerPrefs.SetFloat("PlayerZ", AvaruusAlus.transform.position.z);         */
    }

    // Update is called once per frame
    void Update()
    {
      //  TextLifeLeft.text = Life.ToString();
       // TextAmmonLeft.text = Ammon.ToString();
        TextKullat.text = Kullat.ToString();
        TextTimantit.text = Timantit.ToString();
        TextKivet.text = Kivet.ToString();
        TextPuut.text = Puut.ToString();
        TextHopeat.text = Hopeat.ToString();
    }

/*     public void AloituAsema(GameObject Alus)
    {
        PlayerPrefs.SetFloat("PlayerX", Alus.transform.position.x);
        PlayerPrefs.SetFloat("PlayerY", Alus.transform.position.y);
        PlayerPrefs.SetFloat("PlayerZ", Alus.transform.position.z);
        Debug.Log("Tallennettu " + PlayerPrefs.GetFloat("PlayerX"));
        Debug.Log("Tallennettu " + PlayerPrefs.GetFloat("PlayerY"));
        Debug.Log("Tallennettu " + PlayerPrefs.GetFloat("PlayerZ"));
    }   

    public void Save(GameObject Alus)
    {
        PlayerPrefs.SetFloat("PlayerX", Alus.transform.position.x);
        PlayerPrefs.SetFloat("PlayerY", Alus.transform.position.y);
        PlayerPrefs.SetFloat("PlayerZ", Alus.transform.position.z);
        Debug.Log("Tallennettu " + PlayerPrefs.GetFloat("PlayerX"));
        Debug.Log("Tallennettu " + PlayerPrefs.GetFloat("PlayerY"));
        Debug.Log("Tallennettu " + PlayerPrefs.GetFloat("PlayerZ"));
    }

    public void Load(GameObject Alus)
    {
        Alus.transform.position = new Vector3(PlayerPrefs.GetFloat("PlayerX"), PlayerPrefs.GetFloat("PlayerY"), PlayerPrefs.GetFloat("PlayerZ"));
        StartCoroutine(Reload(Alus));
        Debug.Log("Ladattu " + PlayerPrefs.GetFloat("PlayerX"));
        Debug.Log("Ladattu " + PlayerPrefs.GetFloat("PlayerY"));
        Debug.Log("Ladattu " + PlayerPrefs.GetFloat("PlayerZ"));
    }

    public void pointsFloat(string määrä) {
        Debug.Log("Floating");
        GameObject pointsTextInstance = Instantiate(pointsTextPrefabs, itemInstance.transform);
        pointsTextInstance.transform.GetChild(0).GetComponent<TextMeshPro>().SetText(määrä);
    }
    IEnumerator Reload(GameObject Alus)
    {
        StopMove = true;
        // yield return new WaitForSeconds(1);
        yield return new WaitForSeconds(0.2f);
        Alus.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        Alus.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        Alus.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        Alus.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        Alus.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        Alus.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        Alus.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        Alus.SetActive(true);
        // Resources.UnloadUnusedAssets();
        
        StopMove = false;
        
    } */
}
