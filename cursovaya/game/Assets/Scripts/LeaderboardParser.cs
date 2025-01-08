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
        // Wrap the JSON array in an object for deserialization
        string wrappedJson = "{\"playerScores\":" + json + "}";
        
        // Deserialize the JSON response into the PlayerScoreArray
        PlayerScoreArray playerScoreArray = JsonUtility.FromJson<PlayerScoreArray>(wrappedJson);

        // Clear previous entries
        for (int i = 0; i < leaderboardEntries.Length; i++)
        {
            leaderboardEntries[i].text = ""; // Clear the text fields
        }

        // Fill the text fields with leaderboard data
        for (int i = 0; i < playerScoreArray.playerScores.Length && i < leaderboardEntries.Length; i++)
        {
            leaderboardEntries[i].text = $"{playerScoreArray.playerScores[i].name}: {playerScoreArray.playerScores[i].score}";
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
