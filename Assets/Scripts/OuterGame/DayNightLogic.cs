using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightLogic : MonoBehaviour
{
    public int dayLimit = 3; // Initial day limit

    public void DecreaseDayLimit()
    {
        if (dayLimit > 0)
        {
            dayLimit--;
            Debug.Log($"Day limit decreased. Days remaining: {dayLimit}");
        }
        else
        {
            Debug.Log("No more days left to interact with islands.");
        }
    }

    public void ResetDayLimit()
    {
        dayLimit = 3;
        Debug.Log("Day limit reset to 3.");
    }

    public bool CanInteractWithIsland()
    {
        return dayLimit > 0;
    }
}
