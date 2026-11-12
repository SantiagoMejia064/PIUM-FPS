
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(SplitScreenConfiguration))]
public class PlayerSpawner : MonoBehaviour
{
    [Header("Puntos de aparicion")]
    public Transform[] spawnPoints;
    [SerializeField] private GameObject scenePlayer;

    private SplitScreenConfiguration splitScreenManager;
    private MatchManager matchManager;

    private int nextRespawnIndex;

    private void Start()
    {
        splitScreenManager = GetComponent<SplitScreenConfiguration>();
        matchManager = GetComponent<MatchManager>();

        if (matchManager == null)
        {
            Debug.LogError("No se encuentra MatchManager en el objeto PlayerSpawner.");
            return;
        }

        PlayerInput sceneInput = scenePlayer != null ? scenePlayer.GetComponent<PlayerInput>() : null;
        if (scenePlayer != null && sceneInput == null)
        {
            Debug.LogError("El jugador colocado en la arena no tiene PlayerInput.");
            return;
        }

        if (!MatchConfiguration.IsConfigured && sceneInput != null)
        {
            RegisterPlayer(sceneInput, 0);
            return;
        }

        SpawnPlayers(sceneInput);
    }

    private void SpawnPlayers(PlayerInput sceneInput)
    {
        if (!MatchConfiguration.IsConfigured)
        {
            Debug.LogError("No existe una configuracion valida de partida.");
            return;
        }

        int playerCount = MatchConfiguration.PlayerCount;

        if (playerCount < 1 || playerCount > 4)
        {
            Debug.LogError("La cantidad de jugadores debe estar entre 1 y 4.");
            return;
        }

        if (spawnPoints == null || spawnPoints.Length < playerCount)
        {
            Debug.LogError("No hay suficientes puntos de aparicion.");
            return;
        }

        for (int i = 0; i < playerCount; i++)
        {
            GameObject playerPrefab = MatchConfiguration.SelectedPlayerPrefabs[i];
            Transform spawnPoint = spawnPoints[i];

            if (playerPrefab == null || spawnPoint == null)
            {
                Debug.LogError("Falta el prefab o el punto de aparicion del jugador " + (i + 1));
                return;
            }

            if (MatchConfiguration.SelectedDevices.Count != playerCount ||
                MatchConfiguration.SelectedDevices[i] == null || !MatchConfiguration.SelectedDevices[i].added)
            {
                Debug.LogError("Falta el dispositivo seleccionado del jugador " + (i + 1));
                return;
            }

            InputDevice device = MatchConfiguration.SelectedDevices[i];
            InputDevice[] devices = device is Keyboard && Mouse.current != null
                ? new InputDevice[] { device, Mouse.current } : new InputDevice[] { device };
            string controlScheme = device is Keyboard ? "Keyboard" : "Gamepad";
            PlayerInput input;
            if (i == 0 && sceneInput != null)
            {
                input = sceneInput;
                input.SwitchCurrentControlScheme(controlScheme, devices);
            }
            else
            {
                input = PlayerInput.Instantiate(playerPrefab, i, controlScheme, -1, devices);
            }

            input.neverAutoSwitchControlSchemes = true;
            input.transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);

            RegisterPlayer(input, i);
        }

        nextRespawnIndex = playerCount % spawnPoints.Length;
    }

    private void RegisterPlayer(PlayerInput input, int index)
    {
        GameObject player = input.gameObject;
        player.name = "Player" + (index + 1);

        PlayerRespawn playerRespawn = player.GetComponent<PlayerRespawn>();

        if (playerRespawn != null)
        {
            playerRespawn.SetSpawner(this);
        }
        else
        {
            Debug.LogError("El jugador " + (index + 1) + " no tiene PlayerRespawn.");
        }

        PlayerStats playerStats = player.GetComponent<PlayerStats>();

        if (playerStats != null)
        {
            matchManager.RegisterPlayer(playerStats);
        }
        else
        {
            Debug.LogError("El jugador " + (index + 1) + " no tiene PlayerStats.");
        }

        Camera playerCamera = input.camera != null ? input.camera : player.GetComponentInChildren<Camera>(true);

        if (playerCamera == null)
        {
            Debug.LogError("El prefab del jugador " + (index + 1) + " no contiene una camara.");
            return;
        }

        AudioListener[] listeners = player.GetComponentsInChildren<AudioListener>(true);

        for (int j = 0; j < listeners.Length; j++)
        {
            listeners[j].enabled = index == 0 && j == 0;
        }

        input.camera = playerCamera;
        splitScreenManager.RegisterCamera(playerCamera);

        Debug.Log("Jugador " + (index + 1) + " creado y registrado en la partida.");
    }

    public Transform GetNextSpawnPoint()
    {
        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            return null;
        }

        Transform spawnPoint = spawnPoints[nextRespawnIndex];

        nextRespawnIndex++;

        if (nextRespawnIndex >= spawnPoints.Length)
        {
            nextRespawnIndex = 0;
        }

        return spawnPoint;
    }
}
