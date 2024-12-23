using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private static MusicManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Make the GameObject persistent across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instance
        }
    }
}
