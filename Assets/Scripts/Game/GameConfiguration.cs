using UnityEngine;


[CreateAssetMenu(fileName = "GameConfiguration", menuName = "Game/GameConfiguration", order = 1)]
public class GameConfiguration : ScriptableObject
{
    // Player
    [Range(-100, 0)] public float gravity = -9.81f;
    [Range(0, 50)] public float jumpForce = 3;
    [Range(0, 1)] public float maxJumpDuration = 0.3f;
    public Vector3 playerFallTreshold = new Vector3(-2f, -0.4f, 0f);

    // World
    [Range(0, 20)] public float movementSpeed = 4;

    [Range(1, 30)] public float minPlatformSize = 4;
    [Range(1, 30)] public float maxPlatformSize = 13;

    [Range(1, 10)] public float minPlatformDistance = 1;
    [Range(1, 10)] public float maxPlatformDistance = 5;

    public Color earthColor = new Color(0/255f, 237/255f, 15/255f);
    public Color waterColor = new Color(41/255f, 134/255f, 202/255f);
}
