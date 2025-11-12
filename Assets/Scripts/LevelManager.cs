using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    public Transform startPoint;
    public Transform endPoint;
    public Transform[] path;

    private void Awake()
    {
        instance = this;
    }
}
