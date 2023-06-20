using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drill : MonoBehaviour
{
    public GameObject DrillBit;
    public Animator Animator;

    public ParticleSystem[] DrillParticles;

    void Start()
    {
        
    }

    public void Update()
    {
        //OnDrill();
    }

    public void OnDrill()
    {
        Animator.SetBool("Drill", true);
        DrillBit.transform.Rotate(0, 0, Time.deltaTime * 1200.0f);
    }

    public void OnNotDrill()
    {

        for (int i = 0; i < DrillParticles.Length; i++) 
        {
            if (DrillParticles[i].isPlaying)
            {
                DrillParticles[i].Stop();
            }
        }

        Animator.SetBool("Drill", false);
    }

    public void OnHittingRock()
    {
        for (int i = 0; i < DrillParticles.Length; i++) 
        {
            if (!DrillParticles[i].isPlaying)
            {
                DrillParticles[i].Play();
            }
        }

        Animator.SetBool("Shake", true);
    }

    public void OnNotHittingRock()
    {
        for (int i = 0; i < DrillParticles.Length; i++)
        {
            if (DrillParticles[i].isPlaying)
            {
                DrillParticles[i].Stop();
            }
        }

        Animator.SetBool("Shake", false);
    }
}
