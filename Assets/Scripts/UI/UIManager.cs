using UnityEngine;


public class UIManager : MonoBehaviour
{
    [HideInInspector] public ScoreUIController scoreUIController;


    void Awake()
    {
        scoreUIController = FindObjectOfType<ScoreUIController>();
    }
}
