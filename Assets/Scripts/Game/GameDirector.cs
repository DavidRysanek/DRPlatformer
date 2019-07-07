using System.Collections;
using UnityEngine;


public class GameDirector : MonoBehaviour
{
    Player player => DependencyContainer.Instance.player;
    PlayerController playerController => DependencyContainer.Instance.playerController;
    UIManager uiManager => DependencyContainer.Instance.uiManager;
    PlatformWorld platformWorld => DependencyContainer.Instance.platformWorld;
    // If not set within the editor, it will be set to Camera.main
    public new Camera camera;

    private int defaultScoreIncrease = 1;
    private Element defaultElement = Element.water;

    public bool isPlaying { get; private set; }

    public Vector3 playerPosition => player.gameObject.transform.position;
    private Vector3 playerFallTreshold = Vector3.zero;
    // For tweaking
    private bool isElementInvincibilityEnabled = false;


    #region Lifecycle

    void Awake()
    {
        if (camera == null) {
            camera = Camera.main;
        }
        Pause();
    }

    void Start()
    {
        UpdateConfiguration();
        Application.quitting += OnIsApplicationQuitting;
        StartNewGameAfterDelay(0.5f);
    }

    void OnIsApplicationQuitting()
    {
        Pause();
    }

    private void UpdateConfiguration()
    {
        var configuration = DependencyContainer.Instance.gameConfiguration;
        Physics.gravity = Vector3.up * configuration.gravity;
        playerFallTreshold = configuration.playerFallTreshold;
    }

    #endregion


    #region Updates

    void Update()
    {
        // Pause/Resume game
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (isPlaying) {
                Pause();
            } else {
                Resume();
            }
        }

        // Restart game
        if (Input.GetKeyDown(KeyCode.R)) {
            Pause();
            StartNewGame();
        }
    }

    void FixedUpdate()
    {
        if (!isPlaying) {
            return;
        }

        if (playerPosition.x < playerFallTreshold.x || playerPosition.y < playerFallTreshold.y) {
            GameOver();
        }
    }

    #endregion


    #region Callbacks
    
    public void OnChangePlayerElement()
    {
        player.ChangeElement();
    }

    public void OnPlayerHitPlatform(Platform platform)
    {
        if (player.element == platform.element) {
            // Increase score
            IncreaseScore(defaultScoreIncrease);
        } else {
            if (!isElementInvincibilityEnabled) {
                // Game over
                GameOver();
            }
        }
    }

    #endregion


    #region Game state

    void Pause()
    {
        isPlaying = false;
        Time.timeScale = 0;
    }

    void Resume()
    {
        isPlaying = true;
        Time.timeScale = 1;
    }
    
    public void GameOver()
    {
        //Debug.Log("Game over player position: " + playerPosition);
        Pause();
        StartNewGameAfterDelay(1.0f);
    }
    
    private void StartNewGameAfterDelay(float delay)
    {
        StartCoroutine(StartNewGameCoroutine(delay));
    }

    IEnumerator StartNewGameCoroutine(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        StartNewGame();
    }

    private void StartNewGame()
    {
        ResetAll();
        Resume();
    }

    private void ResetAll()
    {
        ResetPlayer();
        ResetWorld();
        ResetScore();
    }

    private void ResetPlayer()
    {
        playerController.ResetState();
        player.ResetState();
        player.SetElement(defaultElement, false);
    }

    private void ResetWorld()
    {
        platformWorld.ResetWorld();
        platformWorld.GenerateWorld(defaultElement);
    }

    #endregion


    #region Score

    private void IncreaseScore(int diff)
    {
        Score score = player.score;
        score.current += diff;
        if (score.current > score.maximum) {
            score.maximum = score.current;
        }
        SetScore(score);
    }

    private void ResetScore()
    {
        Score score = player.score;
        score.current = 0;
        SetScore(score);
    }

    private void SetScore(Score score)
    {
        player.score = score;
        uiManager.scoreUIController.SetScore(player.score);
    }

    #endregion

    // Discussion
    // Why not to use IsVisible or OnBecameInvisible?
    // IsVisible is called by the Editor camera as well.
    // Even if the object isn't visible in the Game View, if it's visible in the Editor, it will be marked as visible.
    // OnBecameInvisible is also called when the GameObject is deactivated.
    public bool IsObjectVisible(GameObject go)
    {
        var renderer = go.GetComponent<Renderer>();
        if (renderer == null) {
            return false;
        } else {
            return renderer.IsVisibleFrom(camera);
        }
    }
}
