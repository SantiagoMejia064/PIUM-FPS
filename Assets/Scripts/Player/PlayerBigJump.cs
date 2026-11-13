using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerInput))]
public class PlayerBigJump : MonoBehaviour
{
    [SerializeField, Min(0.1f)] private float jumpHeight = 8f;
    [SerializeField, Min(0f)] private float cooldown = 10f;
    [SerializeField] private Slider cooldownSlider;

    private InputAction bigJumpAction;
    private float nextUseTime;

    private void Start()
    {
        bigJumpAction = GetComponent<PlayerInput>().actions?.FindAction("Player/BigJump", false);
    }

    private void LateUpdate()
    {
        if (cooldownSlider != null)
        {
            float charge = cooldown > 0f ? 1f - (nextUseTime - Time.time) / cooldown : 1f;
            cooldownSlider.SetValueWithoutNotify(Mathf.Clamp01(charge));
        }
    }

    public bool TryUse(out float height)
    {
        height = jumpHeight;
        if (bigJumpAction == null || !bigJumpAction.enabled ||
            Time.time < nextUseTime || !bigJumpAction.IsPressed())
        {
            return false;
        }

        nextUseTime = Time.time + cooldown;
        return true;
    }
}
