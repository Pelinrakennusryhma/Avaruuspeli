using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ButtonHeldAction : UITrackable
{
    [SerializeField]
    GameObject successObject;
    [SerializeField]
    GameObject failureObject;
    [SerializeField]
    protected float delay;
    bool leavingScene = false;
    float buttonHeldFor = 0f;
    protected virtual void Awake()
    {
        successObject.SetActive(false);
    }

    protected abstract bool CanTrigger();
    protected abstract void OnSuccess();

    public void OnButtonPressed()
    {
        leavingScene = true;

        if (CanTrigger())
        {
            successObject.SetActive(true);
            StartCoroutine(CompletionRoutine(delay));
        } else
        {
            failureObject.SetActive(true);
        }

    }

    public void OnButtonReleased()
    {
        leavingScene = false;
        buttonHeldFor = 0f;

        successObject.SetActive(false);
        failureObject.SetActive(false);

        StopAllCoroutines();
    }

    // Update is called once per frame
    void Update()
    {
        if (leavingScene)
        {
            buttonHeldFor += Time.deltaTime;
        }
    }

    IEnumerator CompletionRoutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        OnSuccess();
    }

    public override float MaxValue
    {
        get
        {
            return delay;
        }
    }

    public override float CurrentValue
    {
        get
        {
            return buttonHeldFor;
        }
    }
}
