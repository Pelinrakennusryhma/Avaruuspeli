using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HakkuIsku : MonoBehaviour
{

    // Tavaroiden määrä

    public int kultaa = 5;
    public int timantteja = 2;
    public int kiviä = 10;
    public int puita = 20;
    public int hopeaa = 15;

    // Kainettu

    public int kultaaKaivettu = 0;
    public int timanttejaKaivettu = 0;
    public int kiviäKaivettu = 0;
    public int hopeaaKaivettu = 0;
    public int puitaHakattu = 0;

    // Iskua varten

    public Transform attackPoint;
    public float attackRange = 0.5f;
    // public LayerMask enemyLayers;
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire1")) {
            anim.SetBool("attacking", true);
        Collider[] hitColliders = Physics.OverlapSphere(attackPoint.position, attackRange);
        foreach (var hitCollider in hitColliders)
        {
            if(hitCollider.tag == "Kulta") {
                
                Debug.Log("Toimiiii");
                GameManagerByAnttiK.Instance.Kullat += kultaa;
                HakkuOsuu text = hitCollider.GetComponent<HakkuOsuu>();
                if(text != null) {
                    text.ShowFloatingText(kultaa.ToString());
                }
                
                kultaaKaivettu++;
                if(kultaaKaivettu > 4) {
                   hitCollider.gameObject.SetActive(false);
                   kultaaKaivettu = 0; 
                }
                
            }
    
            if(hitCollider.tag == "Timantti") {
                Debug.Log("Toimiiii");
                GameManagerByAnttiK.Instance.Timantit += timantteja;
                HakkuOsuu text = hitCollider.GetComponent<HakkuOsuu>();
                if(text != null) {
                    text.ShowFloatingText(timantteja.ToString());
                }
                
                timanttejaKaivettu++;
                if(timanttejaKaivettu > 4) {
                   hitCollider.gameObject.SetActive(false);
                   timanttejaKaivettu = 0; 
                }
            }
            
            if(hitCollider.tag == "Kivi") {
                Debug.Log("Toimiiii");
                GameManagerByAnttiK.Instance.Kivet += kiviä;
                HakkuOsuu text = hitCollider.GetComponent<HakkuOsuu>();
                if(text != null) {
                    text.ShowFloatingText(kiviä.ToString());
                }
                
                kiviäKaivettu++;
                if(kiviäKaivettu > 4) {
                   hitCollider.gameObject.SetActive(false);
                   kiviäKaivettu = 0; 
                }
            }
            if(hitCollider.tag == "Puu") {
                Debug.Log("Toimiiii");
                GameManagerByAnttiK.Instance.Puut += puita;
                HakkuOsuu text = hitCollider.GetComponent<HakkuOsuu>();
                if(text != null) {
                    text.ShowFloatingText(puita.ToString());
                }
                
                puitaHakattu++;
                if(puitaHakattu > 4) {
                   hitCollider.gameObject.SetActive(false);
                   puitaHakattu = 0; 
                }
            }

            if(hitCollider.tag == "Hopea") {
                Debug.Log("Toimiiii");
                GameManagerByAnttiK.Instance.Hopeat += hopeaa;
                HakkuOsuu text = hitCollider.GetComponent<HakkuOsuu>();
                if(text != null) {
                    text.ShowFloatingText(hopeaa.ToString());
                }
                
                hopeaaKaivettu++;
                if(hopeaaKaivettu > 4) {
                   hitCollider.gameObject.SetActive(false);
                   hopeaaKaivettu = 0; 
                }
            }
            // hitCollider.SendMessage("AddDamage");
            
        }
        } else if(Input.GetButtonUp("Fire1")) {
            anim.SetBool("attacking", false);
        }
            
    }
}
