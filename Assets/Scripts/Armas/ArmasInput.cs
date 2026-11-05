using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class ArmasInput : MonoBehaviour
{
    private PlayerInput playerInput;
    private ArmasController weaponController;

    private InputAction fireAction;
    private InputAction reloadAction;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        weaponController = GetComponent<ArmasController>();

        fireAction = playerInput.actions.FindAction("Fire", false);
        reloadAction = playerInput.actions.FindAction("Reload", false);

        if (weaponController == null || fireAction == null || reloadAction == null)
        {
            Debug.LogError("Falta WeaponController, Fire o Reload en " + name);
            enabled = false;
        }
    }

    private void Update()
    {
        if (fireAction.WasPressedThisFrame())
        {
            weaponController.Fire();
        }

        if (reloadAction.WasPressedThisFrame())
        {
            weaponController.Reload();
        }
    }
}
