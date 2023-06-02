using SuperMaxim.Messaging;
using System;

public class Achievement : IPersistableObject
{
    public bool IsHalfTargetDone { get => _isHalfGoldTargetDone; }
    bool _isHalfGoldTargetDone = false;
    public bool IsGoldTargetDone { get => _isGoldTargetDone; }
    bool _isGoldTargetDone = false;

    public string halfTargetMessage = "Halfway to heaven bro, keep going <3";
    public string targetDoneMessage =
        "You are the richest man in the world! Well Done!";

    public Achievement()
    {
        // Subcribe to gold change topic
        Messenger.Default.Subscribe<GoldChangedPayLoad>(OnGoldChanged);
    }

    public void OnGoldChanged(GoldChangedPayLoad obj)
    {
        int gold = obj.TotalGold;
        if (!_isHalfGoldTargetDone && 
            gold >= ConfigManager.GetTargetGold() / 2)
        {
            _isHalfGoldTargetDone = true;
            NotifyNewAchievement(halfTargetMessage);
        }

        if (!_isGoldTargetDone &&
            gold >= ConfigManager.GetTargetGold())
        {
            _isGoldTargetDone = true;
            NotifyNewAchievement(targetDoneMessage);
        }
    }

    private void NotifyNewAchievement(string achievement)
    {
        var payload = new NewAchievementPayLoad { NewAchievement = achievement };
        Messenger.Default.Publish(payload);
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
