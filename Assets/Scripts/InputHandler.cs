using UnityEngine;

public class InputHandler : MonoBehaviour
{
    private InputSystem_Actions inputActions;

    // Unity's InputActionAsset for handling input
    private void Awake()
    {
        inputActions = new InputSystem_Actions();
    }

    public void OnEnable()
    {
        inputActions.UI.Enable();
        inputActions.Player.Enable();
    }

    private void OnDisable()
    {
        // Disable the input actions
        inputActions.UI.Disable();
        inputActions.Player.Disable();
    }

    public bool MouseClick()
    {
        return inputActions.UI.Click.triggered;
    }
    public bool ShootInput()
    {
        return inputActions.Player.Attack.triggered;
    }

    public bool MainMenu()
    {
        return inputActions.UI.MainMenu.triggered;
    }
}
