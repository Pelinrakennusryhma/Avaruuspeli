using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : Weapon
{
    public float CoolDown;
    public float CoolDownLength = 0.5f;

    public Animator Animator;

    public const string Hit1 = "Hit1";
    public const string Hit2 = "Hit2";
    public const string Hit3 = "Hit3";
    public const string Hit4 = "Hit4";

    public int LastRandom;

    public bool HasDealtDamageDuringThisSwing;

    private RaycastHit[] RaycastHits = new RaycastHit[48];
    private Collider[] HitResults = new Collider[48];

    public void Awake()
    {
        TypeOfWeapon = WeaponType.MeleeWeapon;

        Animator = GetComponent<Animator>();
    }

    public override void OnFire1Down()
    {
        if (CoolDown > 0)
        {
            //Debug.Log("Tried to fire melee weapon, but cooldown has not yet been cleared");
            return;
        }

        base.OnFire1Down();

        CoolDown = CoolDownLength;

        int random = -1;

        while (true) 
        {
            random = Random.Range(0, 4);

            if (random != LastRandom)
            {
                LastRandom = random;
                break;
            }
        }

        if (random == 0)
        {
            Animator.SetTrigger(Hit1);
        }

        else if (random == 1)
        {
            Animator.SetTrigger(Hit2);
        }

        else if (random == 2)
        {
            Animator.SetTrigger(Hit3);
        }

        else
        {
            Animator.SetTrigger(Hit4);
        }

        LastRandom = random;
        HasDealtDamageDuringThisSwing = false;

        //Debug.LogWarning("Fire 1 called on melee weapon " + Time.time);
    }

    private void Update()
    {
        CoolDown -= Time.deltaTime;

        if (CoolDown > 0
            && !HasDealtDamageDuringThisSwing)
        {

            // Overlap tests did not work for some weird reason. Nothing was detected. Same with raycasts
            // Relaucnhing Unity fixed this problem. Weird.

            //int amountOfHits = Physics.OverlapSphereNonAlloc(Camera.main.transform.position
            //                                                 + Camera.main.transform.forward * 1.4f,
            //                                                 2.0f,
            //                                                 HitResults);

            //int amountOfHits = Physics.OverlapBoxNonAlloc(Camera.main.transform.position
            //                                                 + Camera.main.transform.forward * 1.4f,
            //                                                 new Vector3(2, 2, 2),
            //                                                 hitResults,
            //                                                 Quaternion.identity);

            int amountOfHits = Physics.SphereCastNonAlloc(Camera.main.transform.position,
                                                          1.5f,
                                                          Camera.main.transform.forward,
                                                          RaycastHits,
                                                          0.1f);

            //int amountOfHits = Physics.RaycastNonAlloc(Camera.main.transform.position,
            //                                           Camera.main.transform.forward * 1.4f,
            //                                           RaycastHits);


            for (int i = 0; i < amountOfHits; i++)
            {
                if (!RaycastHits[i].collider.gameObject.CompareTag("Player")
                    && !RaycastHits[i].collider.gameObject.CompareTag("PickUpTrigger")) 
                {
                    //Debug.Log("Hit an object " + RaycastHits[i].collider.gameObject.name);

                    HitDetector hitDetector = RaycastHits[i].collider.gameObject.GetComponent<HitDetector>();

                    if (hitDetector != null)
                    {
                        hitDetector.OnHit(Health.DamageType.PlasmaCutter);

                        RaycastHit hitInfo;
                        Ray ray = new Ray(Camera.main.transform.position,
                                          RaycastHits[i].collider.gameObject.transform.position - Camera.main.transform.position);

                        bool hit = RaycastHits[i].collider.Raycast(ray,
                                                                   out hitInfo,
                                                                   2.0f);

                        Vector3 dir = Camera.main.transform.forward;

                        if (LastRandom == 1)
                        {
                            dir = Quaternion.AngleAxis(15, Vector3.up) * dir;
                        }

                        else if(LastRandom == 2)
                        {
                            dir = Quaternion.AngleAxis(25, Vector3.up) * dir;
                        }

                        else if (LastRandom == 3)
                        {
                            dir = Quaternion.AngleAxis(-25, Vector3.up) * dir;
                        }

                        else if (LastRandom == 4)
                        {
                            dir = Quaternion.AngleAxis(-15, Vector3.up) * dir;
                        }

                        hitDetector.AddForceToPos(hitInfo.point,
                                                  (dir).normalized * 150.0f,
                                                  ForceMode.Impulse);

                        //hitDetector.AddForceToPos(hitInfo.point, 
                        //                          (hitInfo.point - Camera.main.transform.position).normalized * 300.0f, 
                        //                          ForceMode.Impulse);

                        Debug.DrawRay(hitInfo.point, Vector3.up * 10, Color.green, 10.0f);

                        Debug.LogWarning("Non-null hit detector. Preparing to deal damage. Hit point is " + RaycastHits[i].point);
                    }

                    else
                    {
                        DestroyableRock rock = RaycastHits[i].collider.gameObject.GetComponent<DestroyableRock>();

                        if (rock != null)
                        {
                            rock.ReduceHealth(0.12f, 
                                              Health.DamageType.PlasmaCutter,
                                              true, 
                                              1.0f,
                                              4.4f);
                            //Debug.LogWarning("Non-null rock. Preparing to shake, shake, shake");
                        }
                    }
                }
            }

            HasDealtDamageDuringThisSwing = true; 
        }

        // Perhaps some sort of overlap test?
        // Or maybe use a trigger that is activated for a short while??
    }
}
