using System;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    private int kills;
    private int deaths;

    public event Action<int, int> StatsChanged;

    public int Kills
    {
        get { return kills; }
    }

    public int Deaths
    {
        get { return deaths; }
    }

    public void AddKill()
    {
        kills++;

        NotifyStatsChanged();
    }

    public void AddDeath()
    {
        deaths++;

        NotifyStatsChanged();
    }

    private void NotifyStatsChanged()
    {
        if (StatsChanged != null)
        {
            StatsChanged(kills, deaths);
        }

        Debug.Log(name + " - Eliminaciones: " + kills + " | Muertes: " + deaths);
    }
}
