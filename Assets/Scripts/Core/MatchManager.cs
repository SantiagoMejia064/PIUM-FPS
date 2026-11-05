using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MatchManager : MonoBehaviour
{
    [Header("Configuracion de partida")]
    public int killLimit = 20;
    public float matchDuration = 600f;

    private List<PlayerStats> players = new List<PlayerStats>();

    private float remainingTime;
    private bool matchFinished;

    public event Action<float> TimeChanged;
    public event Action<PlayerStats> MatchEnded;

    public float RemainingTime
    {
        get { return remainingTime; }
    }

    public bool MatchFinished
    {
        get { return matchFinished; }
    }

    private void Awake()
    {
        remainingTime = matchDuration;
        matchFinished = false;
    }

    private void Start()
    {
        NotifyTimeChanged();
    }

    private void Update()
    {
        if (matchFinished)
        {
            return;
        }

        remainingTime -= Time.deltaTime;

        if (remainingTime < 0f)
        {
            remainingTime = 0f;
        }

        NotifyTimeChanged();

        if (remainingTime <= 0f)
        {
            FinishByTime();
        }
    }

    public void RegisterPlayer(PlayerStats playerStats)
    {
        if (playerStats == null || players.Contains(playerStats))
        {
            return;
        }

        players.Add(playerStats);
        playerStats.StatsChanged += OnPlayerStatsChanged;

        Debug.Log("Jugador registrado en la partida: " + playerStats.name);
    }

    private void OnPlayerStatsChanged(int kills, int deaths)
    {
        if (matchFinished)
        {
            return;
        }

        PlayerStats playerStats = null;

        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].Kills >= killLimit)
            {
                playerStats = players[i];
                break;
            }
        }

        if (playerStats != null)
        {
            FinishMatch(playerStats);
        }
    }

    private void FinishByTime()
    {
        PlayerStats winner = null;
        int highestKills = -1;
        bool tied = false;

        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].Kills > highestKills)
            {
                highestKills = players[i].Kills;
                winner = players[i];
                tied = false;
            }
            else if (players[i].Kills == highestKills)
            {
                tied = true;
            }
        }

        if (tied)
        {
            winner = null;
        }

        FinishMatch(winner);
    }

    private void FinishMatch(PlayerStats winner)
    {
        if (matchFinished)
        {
            return;
        }

        matchFinished = true;

        for (int i = 0; i < players.Count; i++)
        {
            PlayerMovement movement = players[i].GetComponent<PlayerMovement>();
            ArmasInput weaponInput = players[i].GetComponent<ArmasInput>();
            PlayerInput playerInput = players[i].GetComponent<PlayerInput>();

            if (movement != null)
            {
                movement.enabled = false;
            }

            if (weaponInput != null)
            {
                weaponInput.enabled = false;
            }

            if (playerInput != null)
            {
                playerInput.DeactivateInput();
            }

            players[i].StatsChanged -= OnPlayerStatsChanged;
        }

        Debug.Log("PARTIDA FINALIZADA.");

        if (winner != null)
        {
            Debug.Log("GANADOR: " + winner.name + " | Eliminaciones: " + winner.Kills);
        }
        else
        {
            Debug.Log("PARTIDA EMPATADA.");
        }

        if (MatchEnded != null)
        {
            MatchEnded(winner);
        }

        Time.timeScale = 0f;
    }

    private void NotifyTimeChanged()
    {
        if (TimeChanged != null)
        {
            TimeChanged(remainingTime);
        }
    }

    private void OnDestroy()
    {
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i] != null)
            {
                players[i].StatsChanged -= OnPlayerStatsChanged;
            }
        }

        Time.timeScale = 1f;
    }

    public PlayerStats[] GetPlayers()
    {
        return players.ToArray();
    }
}
