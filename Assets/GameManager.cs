using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [HideInInspector] public InputHandler InputHandler;

    public int resources = 0; // Resource count

    void Awake()
    {
        // Implement singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InputHandler = gameObject.AddComponent<InputHandler>();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Method to add resources
    public void AddResources(int amount)
    {
        resources += amount;
        Debug.Log("Resources: " + resources);
    }
}
