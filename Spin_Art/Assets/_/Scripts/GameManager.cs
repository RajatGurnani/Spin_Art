using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public PlayerData playerData;

    public override void Awake()
    {
        base.Awake();
        name = nameof(GameManager);
        playerData = SaveSystem.LoadPlayerData();
    }

    private void Start()
    {
        Application.targetFrameRate = 60;
    }

    [EasyButtons.Button]
    public void ResetDate()
    {
        SaveSystem.SavePlayerData(playerData);
    }
}
