using UnityEngine;
using UnityEngine.Assertions;

public class DependencyContainer : SceneSingletonBehaviour<DependencyContainer>
{
    public GameConfiguration gameConfiguration;
    [HideInInspector] public GameDirector gameDirector;
    [HideInInspector] public PlatformWorld platformWorld;
    [HideInInspector] public Player player;
    [HideInInspector] public PlayerController playerController;
    [HideInInspector] public UIManager uiManager;
    

    override protected void Awake()
    {
        base.Awake();
        Assert.IsNotNull(gameConfiguration);
        gameDirector = FindObjectOfType<GameDirector>();
        Assert.IsNotNull(gameDirector);
        platformWorld = FindObjectOfType<PlatformWorld>();
        Assert.IsNotNull(platformWorld);
        player = FindObjectOfType<Player>();
        Assert.IsNotNull(player);
        playerController = FindObjectOfType<PlayerController>();
        Assert.IsNotNull(playerController);
        uiManager = FindObjectOfType<UIManager>();
        Assert.IsNotNull(uiManager);
    }
}
