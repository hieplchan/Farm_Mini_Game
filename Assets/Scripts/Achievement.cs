using System;

public class Achievement : IPersistableObject
{
    public event Action<string> NewAchievement;

    public bool IsHalfTargetDone { get => _isHalfGoldTargetDone; }
    bool _isHalfGoldTargetDone = false;
    public bool IsGoldTargetDone { get => _isGoldTargetDone; }
    bool _isGoldTargetDone = false;

    public string halfTargetMessage = "Halfway to heaven bro, keep going <3";
    public string targetDoneMessage =
        "You are the richest man in the world! Well Done!";

    public void OnGoldChanged(int gold)
    {
        if (!_isHalfGoldTargetDone && 
            gold >= ConfigManager.targetGold / 2)
        {
            _isHalfGoldTargetDone = true;
            NewAchievement?.Invoke(halfTargetMessage);
        }

        if (!_isGoldTargetDone &&
            gold >= ConfigManager.targetGold)
        {
            _isGoldTargetDone = true;
            NewAchievement?.Invoke(targetDoneMessage);
        }
    }

    public void Save(GameDataWriter writer)
    {
        writer.Write(_isHalfGoldTargetDone);
        writer.Write(_isGoldTargetDone);
    }

    public void Load(GameDataReader reader)
    {
        _isHalfGoldTargetDone = reader.ReadBool();
        _isGoldTargetDone = reader.ReadBool();
        MLog.Log("Achievement", string.Format(
            "Load half done {0}, done {1}",
            _isHalfGoldTargetDone, _isGoldTargetDone));
    }
}
