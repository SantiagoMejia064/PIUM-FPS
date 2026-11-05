
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public static class MatchConfiguration
{
    public static List<GameObject> SelectedPlayerPrefabs { get; private set; } = new List<GameObject>();

    public static string SelectedMapScene { get; private set; } = string.Empty;

    public static List<InputDevice> SelectedDevices { get; private set; } = new List<InputDevice>();

    public static void SetDevices(List<InputDevice> devices)
    {
        SelectedDevices = new List<InputDevice>(devices);
    }

    public static int PlayerCount
    {
        get
        {
            return SelectedPlayerPrefabs.Count;
        }
    }

    public static bool HasValidPlayers
    {
        get
        {
            if (PlayerCount < 2 || PlayerCount > 4)
            {
                return false;
            }

            for (int i = 0; i < SelectedPlayerPrefabs.Count; i++)
            {
                if (SelectedPlayerPrefabs[i] == null)
                {
                    return false;
                }
            }

            return true;
        }
    }

    public static bool IsConfigured
    {
        get
        {
            return HasValidPlayers && !string.IsNullOrEmpty(SelectedMapScene);
        }
    }

    public static bool TrySetPlayers(List<GameObject> playerPrefabs)
    {
        if (playerPrefabs == null || playerPrefabs.Count < 2 || playerPrefabs.Count > 4)
        {
            Debug.LogWarning("Se necesitan entre 2 y 4 jugadores para preparar la partida.");

            return false;
        }

        for (int i = 0; i < playerPrefabs.Count; i++)
        {
            if (playerPrefabs[i] == null)
            {
                Debug.LogWarning("Falta el prefab del jugador " + (i + 1));

                return false;
            }
        }

        SelectedPlayerPrefabs = new List<GameObject>(playerPrefabs);
        SelectedDevices.Clear();
        SelectedMapScene = string.Empty;

        Debug.Log("Selección guardada. Total de jugadores: " + PlayerCount);

        return true;
    }

    public static bool TrySetMap(string mapSceneName)
    {
        if (string.IsNullOrWhiteSpace(mapSceneName))
        {
            Debug.LogWarning("El nombre de la escena del mapa está vacío.");
            return false;
        }

        SelectedMapScene = mapSceneName;

        return true;
    }

    public static bool TrySetConfiguration(List<GameObject> playerPrefabs, string mapSceneName)
    {
        if (!TrySetPlayers(playerPrefabs))
        {
            return false;
        }

        if (!TrySetMap(mapSceneName))
        {
            SelectedPlayerPrefabs.Clear();
            return false;
        }

        return true;
    }

    public static void Clear()
    {
        SelectedPlayerPrefabs.Clear();
        SelectedMapScene = string.Empty;
        SelectedDevices.Clear();
    }
}
