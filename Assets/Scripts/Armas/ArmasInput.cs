
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class ArmasInput : MonoBehaviour
{
    private PlayerInput playerInput;
    private ArmasController weaponController;
    private Transform aimCamera;

    private InputAction fireAction;
    private InputAction reloadAction;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        weaponController = GetComponent<ArmasController>();

        Camera[] playerCameras = GetComponentsInChildren<Camera>(true);

        for (int i = 0; i < playerCameras.Length; i++)
        {
            if (!playerCameras[i].name.Contains("Weapon"))
            {
                aimCamera = playerCameras[i].transform;
                break;
            }
        }

        fireAction = playerInput.actions.FindAction("Fire", false);
        reloadAction = playerInput.actions.FindAction("Reload", false);

        if (weaponController == null || fireAction == null || reloadAction == null)
        {
            Debug.LogError("Falta WeaponController, Fire o Reload en " + name);
            enabled = false;
            return;
        }

        if (aimCamera == null)
        {
            Debug.LogError("No se encontro la camara de apuntado en " + name);
            enabled = false;
        }
    }

    private void Update()
    {
        if (fireAction.WasPressedThisFrame())
        {
            weaponController.Fire(aimCamera);
        }

        if (reloadAction.WasPressedThisFrame())
        {
            weaponController.Reload();
        }
    }
}
