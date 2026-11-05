
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRespawn : MonoBehaviour
{
    [Header("Configuracion de respawn")]
    public float respawnTime = 3f;

    private PlayerHealth playerHealth;
    private CharacterController characterController;
    private PlayerMovement playerMovement;
    private ArmasInput weaponInput;
    private PlayerInput playerInput;

    private PlayerSpawner playerSpawner;

    private Vector3 initialPosition;
    private Quaternion initialRotation;

    private bool respawning;

    private void Awake()
    {
        playerHealth = GetComponent<PlayerHealth>();
        characterController = GetComponent<CharacterController>();
        playerMovement = GetComponent<PlayerMovement>();
        weaponInput = GetComponent<ArmasInput>();
        playerInput = GetComponent<PlayerInput>();

        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    private void OnEnable()
    {
        if (playerHealth != null)
        {
            playerHealth.PlayerDied += OnPlayerDied;
        }
    }

    private void OnDisable()
    {
        if (playerHealth != null)
        {
            playerHealth.PlayerDied -= OnPlayerDied;
        }
    }

    public void SetSpawner(PlayerSpawner spawner)
    {
        playerSpawner = spawner;
    }

    private void OnPlayerDied()
    {
        if (respawning)
        {
            return;
        }

        respawning = true;

        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }

        if (weaponInput != null)
        {
            weaponInput.enabled = false;
        }

        if (playerInput != null)
        {
            playerInput.DeactivateInput();
        }

        if (characterController != null)
        {
            characterController.enabled = false;
        }

        Debug.Log(name + " eliminado. Respawn en " + respawnTime + " segundos.");

        StartCoroutine(RespawnCoroutine());
    }

    private IEnumerator RespawnCoroutine()
    {
        yield return new WaitForSeconds(respawnTime);

        Transform spawnPoint = null;

        if (playerSpawner != null)
        {
            spawnPoint = playerSpawner.GetNextSpawnPoint();
        }

        if (spawnPoint != null)
        {
            transform.position = spawnPoint.position;
            transform.rotation = spawnPoint.rotation;
        }
        else
        {
            transform.position = initialPosition;
            transform.rotation = initialRotation;
        }

        playerHealth.RestoreHealth();

        if (characterController != null)
        {
            characterController.enabled = true;
        }

        if (playerMovement != null)
        {
            playerMovement.enabled = true;
        }

        if (weaponInput != null)
        {
            weaponInput.enabled = true;
        }

        if (playerInput != null)
        {
            playerInput.ActivateInput();
        }

        respawning = false;

        Debug.Log(name + " ha reaparecido.");
    }
}
