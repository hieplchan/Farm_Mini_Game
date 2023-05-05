using System;

public class Achievement
{
    public event Action<string> NewAchievement;

    bool isHalfGoldTargetDone = false;
    bool isGoldTargetDone = false;

    public string halfTargetMessage = "Halfway to heaven bro, keep going <3";
    public string targetDoneMessage =
        "You are the richest man in the world! Well Done!\n" +
        "You can keep playing.";

    public void OnGoldChanged(int gold)
    {
        if (!isHalfGoldTargetDone && 
            gold >= ConfigManager.targetGold / 2)
        {
            NewAchievement?.Invoke(halfTargetMessage);
            isHalfGoldTargetDone = true;
        }

        if (!isGoldTargetDone &&
            gold >= ConfigManager.targetGold)
        {
            NewAchievement?.Invoke(targetDoneMessage);
            isGoldTargetDone = true;
        }
    }
}
