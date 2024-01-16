using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HoldButton : MonoBehaviour
{
    public float currentPressTime = 0f;
    public float pressDuration=2.5f;
    public bool pressing = false;
    public bool pressComplete = false;

    public UnityEvent OnPressComplete;

    private void Update()
    {
        if (pressing)
        {
            currentPressTime += Time.deltaTime;
            if (currentPressTime > pressDuration)
            {
                pressComplete = true;
                OnPressComplete?.Invoke();
            }
        }
    }

    public void OnHold()
    {
        pressing = true;
    }

    public void OnRelease()
    {

    }
}
