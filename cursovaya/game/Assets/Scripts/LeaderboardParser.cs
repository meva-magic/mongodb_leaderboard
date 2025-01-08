using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class LeaderboardDisplay : MonoBehaviour
{
    public TMP_Text[] leaderboardEntries = new TMP_Text[13]; // Array to hold references to the text fields

    private void Start()
    {
        // Automatically fetch leaderboard data on start
        GetLeaderboard();
    }

    public void GetLeaderboard()
    {
        StartCoroutine(GetLeaderboardRoutine());
    }

    private IEnumerator GetLeaderboardRoutine()
    {
        using (UnityWebRequest www = UnityWebRequest.Get("http://localhost:3009/api/leaderboard"))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(www.error);
            }
            else
            {
                // Parse and display the leaderboard data
                DisplayLeaderboard(www.downloadHandler.text);
            }
        }
    }

    private void DisplayLeaderboard(string json)
    {
        // Assuming the JSON response is an array of player scores
        PlayerScore[] playerScores = JsonUtility.FromJson<PlayerScoreArray>(json).playerScores;

        // Clear previous entries
        for (int i = 0; i < leaderboardEntries.Length; i++)
        {
            leaderboardEntries[i].text = ""; // Clear the text fields
        }

        // Fill the text fields with leaderboard data
        for (int i = 0; i < playerScores.Length && i < leaderboardEntries.Length; i++)
        {
            leaderboardEntries[i].text = $"{playerScores[i].name}: {playerScores[i].score}";
        }
    }

    [System.Serializable]
    public class PlayerScore
    {
        public string name;
        public int score;
    }

    [System.Serializable]
    public class PlayerScoreArray
    {
        public PlayerScore[] playerScores; // This should match the structure of your JSON response
    }
}
