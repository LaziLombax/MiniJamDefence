using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public static InputHandler Instance { get; private set; }
    private InputSystem_Actions inputActions;

    // Unity's InputActionAsset for handling input
    private void Awake()
    {
            // Implement singleton pattern
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        
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
        return inputActions.Player.Attack.ReadValue<float>() > 0;
    }

    public bool MainMenu()
    {
        return inputActions.UI.MainMenu.triggered;
    }
}
