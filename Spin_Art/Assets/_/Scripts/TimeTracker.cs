using UnityEngine;
using UnityEngine.EventSystems;

public class TimeTracker : MonoBehaviour
{
    public float timeSpentOnBoard = 0;
    public float timeSpentOffBoard = 0;
    public float timeSpentIdle = 0;
    public float totalTime = 0;

    public bool paintStarted = false;
    public bool paintDone = false;

    public void IncrementOnTime()
    {
        timeSpentOnBoard += Time.deltaTime;
    }

    public void IncrementOffTime()
    {
        timeSpentOffBoard += Time.deltaTime;
    }

    public void TimeSpentIdle()
    {
        timeSpentIdle += Time.deltaTime;
    }

    private void Update()
    {
        if (paintDone)
        {
            return;
        }

        if (paintStarted)
        {
            totalTime += Time.deltaTime;
        }

        if (Input.GetMouseButton(0) && EventSystem.current.IsPointerOverGameObject())
        {
            IncrementOffTime();
        }
    }
}
