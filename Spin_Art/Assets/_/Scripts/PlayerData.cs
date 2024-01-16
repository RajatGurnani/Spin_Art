using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int sessionsCompleted;
    public int _currency;
    public int level;
    public List<Color32> colors = new();
    public int Currency
    {
        get
        {
            return _currency;
        }
        set
        {
            _currency = value;
        }
    }

    public void AddCurrency(int value)
    {
        Currency += value;
    }

    public PlayerData()
    {
        level = 0;
        Currency = 0;
        colors = new();
    }

    public void AddColor(Color32 color)
    {
        if (!colors.Contains(color))
        {
            colors.Add(color);
        }
    }

    public bool HasColor(Color32 color) => colors.Contains(color);
}
