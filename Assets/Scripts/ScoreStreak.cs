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
            " Streak ended", " Not Bad!", " Well Done!", " Nice!", " Cool!",
            " AWESOME!", " Killin' It!", " Wow!", " Impossible!", " DAYUMMM!",
            " R U HUMAN?!", " WTF?!?!", " NO FREAKIN' WAY!", " STOP CHEATING!"
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