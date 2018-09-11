using UnityEngine;

public class ScoreStreak {
    private GameManagerScript _gameManager;
    private AudioManager _audioManager;
    public int CurrentScoreStreak;
    public int CurrentScoreTier;
    public string[] TextArray;

    public ScoreStreak() {
        _gameManager = GameObject.Find("Game Manager")
            .GetComponent<GameManagerScript>();
        _audioManager = GameObject.Find("Audio Manager")
            .GetComponent<AudioManager>();
        CurrentScoreStreak = 0;
        CurrentScoreTier = 0;
        TextArray = new string[] {
            " Streak ended",
            " 1K STREAK\n Not Bad!",
            " 2K STREAK\n Well Done!",
            " 3K STREAK\n Nice!",
            " 4K STREAK\n Cool!",
            " 5K STREAK\n AWESOME!",
            " 6K STREAK\n Killin' It!",
            " 7K STREAK\n Wow!",
            " 8K STREAK\n Impossible!",
            " 9K STREAK\n DAYUMMM!",
            " 10K STREAK\n R U HUMAN?!",
            " 11K STREAK\n WTF?!?!",
            " 12K STREAK\n NO FREAKIN' WAY!",
            " 13K STREAK\n STOP CHEATING!"
        };
    }

    public void IncreaseStreakAndTryAdvanceToNextTier(int addedAmount) {
        CurrentScoreStreak += addedAmount;
        if (CurrentScoreStreak < (CurrentScoreTier + 1) * 1000) return;
        _audioManager.Play("ScoreTier");
        CurrentScoreTier++;
        if (CurrentScoreTier > TextArray.Length - 1)
            CurrentScoreStreak = TextArray.Length - 1;
        _gameManager.BroadcastMessageOrScore(TextArray[CurrentScoreTier],
            false);
    }

    public void StreakEnd() {
        if (CurrentScoreTier != 0)
            _gameManager.BroadcastMessageOrScore(TextArray[0], false);
        CurrentScoreStreak = 0;
        CurrentScoreTier = 0;
    }
}