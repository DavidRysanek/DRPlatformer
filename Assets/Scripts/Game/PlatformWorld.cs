using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;


public class PlatformWorld : MonoBehaviour
{
    GameDirector gameDirector => DependencyContainer.Instance.gameDirector;

    [SerializeField] GameObject initialPlatform;
    Transform initialTransform => initialPlatform.transform;

    SimpleObjectPool platformPool;
    List<GameObject> platforms = new List<GameObject>();

    // Platform properties
    float movementSpeed;

    float minSize;
    float maxSize;

    float minDistance;
    float maxDistance;


    #region Lifecycle

    void Awake()
    {
        platformPool = GetComponent<SimpleObjectPool>();
        Assert.IsNotNull(platformPool);
        initialPlatform.SetActive(false);
    }

    void Start()
    {
        UpdateConfiguration();
    }

    public void ResetWorld()
    {
        RemoveAllPlatforms();
    }

    private void UpdateConfiguration()
    {
        var configuration = DependencyContainer.Instance.gameConfiguration;
        minSize = configuration.minPlatformSize;
        maxSize = configuration.maxPlatformSize;
        minDistance = configuration.minPlatformDistance;
        maxDistance = configuration.maxPlatformDistance;
        movementSpeed = configuration.movementSpeed;
    }

    #endregion


    #region Update

    void Update()
    {
        if (!gameDirector.isPlaying) {
            return;
        }
        var dt = Time.deltaTime;
        foreach (var go in platforms) {
            go.transform.Translate(Vector3.left * movementSpeed * dt);
        }
    }

    void FixedUpdate()
    {
        if (!gameDirector.isPlaying) {
            return;
        }
        RemoveInvisiblePlatforms();
        GenerateRandomPlatformIfNeeded();
    }
        
    private void RemoveInvisiblePlatforms()
    {
        // Initialise the list only if it should contain any object
        List<GameObject> toRemove = null;

        foreach (var go in platforms) {
            // Remove platform is it is not visible by camera and if it is behind player
            if (go.transform.position.x < gameDirector.playerPosition.x && !gameDirector.IsObjectVisible(go)) {
                if (toRemove == null) {
                    toRemove = new List<GameObject>();
                }
                toRemove.Add(go);
            }
        }

        if (toRemove != null) {
            foreach (var go in toRemove) {
                RemovePlatform(go);
            }
        }
    }

    #endregion


    #region World/Platform generation

    public void GenerateWorld(Element initialElement)
    {
        GeneratePlatform(initialElement);
        while (NeedsGeneratePlatform()) {
            GenerateRandomPlatform();
        }
    }

    public void GenerateRandomPlatformIfNeeded()
    {
        if (NeedsGeneratePlatform()) {
            GenerateRandomPlatform();
        }
    }

    public void GenerateRandomPlatform()
    {
        var element = GetRandomElement();
        GeneratePlatform(element);
    }

    public void GeneratePlatform(Element element)
    {
        var lastPlatformTail = GetLastPlatformTail();

        var go = platformPool.GetObject();
        platforms.Add(go);

        var platform = go.GetComponent<Platform>();
        // Set element
        platform.SetElement(element);

        // SIZE
        // Scale each platform randomly in given range
        var scaleX = Mathf.Lerp(minSize, maxSize, Random.value);
        var scale = new Vector3(scaleX, 1, 1);

        // POSITION
        Vector3 position = initialTransform.position; // Initial platform

        if (platforms.Count == 1) {
            scale = new Vector3(maxSize, 1, 1);
            // Initial platform - move it a bit to the left so the player won't fall down
            position.x -= scale.x * 0.2f;
        } else {
            // Make random distance between each platform
            var dx = Mathf.Lerp(minDistance, maxDistance, Random.value);
            //position.x = lastPlatformTail.x + dx;
            position.x = lastPlatformTail + dx;
        }

        // Move the platform to the right by half of it's size
        position.x += scale.x * 0.5f;
        go.transform.position = position;
        go.transform.localScale = scale;
        
        // Keep nice hierarchy
        go.transform.parent = transform;
    }

    private Element GetRandomElement()
    {
        int i = Mathf.RoundToInt(Random.value);
        return (Element)i;
    }

    private bool NeedsGeneratePlatform()
    {
        if (platforms.Count <= 0) {
            return true;
        }
        // Return true if the last platform is visible from the camera.
        // If it is not, it means, that the platform wasn't in the camera's view yet (i.e. it is comming).
        var go = platforms[platforms.Count - 1];
        return gameDirector.IsObjectVisible(go);
    }

    private float GetLastPlatformTail()
    {
        if (platforms.Count <= 0) {
            return 0;
        }
        var go = platforms[platforms.Count - 1];
        return go.transform.position.x + go.transform.localScale.x * 0.5f;
    }


    public void RemoveAllPlatforms()
    {
        foreach (var go in platforms) {
            platformPool.ReturnObject(go);
        }
        platforms.RemoveRange(0, platforms.Count);
    }

    private void RemovePlatform(GameObject go)
    {
        var i = platforms.IndexOf(go);
        if (i >= 0 && i < platforms.Count) {
            platforms.RemoveAt(i);
            platformPool.ReturnObject(go);
        }
    }

    #endregion
}
