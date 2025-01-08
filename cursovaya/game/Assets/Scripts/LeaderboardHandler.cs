using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class LeaderboardHandler : MonoBehaviour
{
    public TMP_InputField playerName;
    [SerializeField] private TMP_InputField score;

    [System.Serializable]
    public class PlayerScore
    {
        public string name;
        public int score;
    }

    public void PostPlayerScore()
    {
        StartCoroutine(PostScore());
    }

    IEnumerator PostScore()
    {
        if (string.IsNullOrEmpty(playerName.text) || string.IsNullOrEmpty(score.text))
        {
            Debug.LogError("Player name or score is empty.");
            yield break; // Exit if inputs are invalid
        }

        int parsedScore;
        if (!int.TryParse(score.text, out parsedScore))
        {
            Debug.LogError("Invalid score input");
            yield break; // Exit if parsing fails
        }

        PlayerScore playerScore = new PlayerScore
        {
            name = playerName.text,
            score = parsedScore
        };

        string json = JsonUtility.ToJson(playerScore); // Serialize to JSON

        using (UnityWebRequest www = new UnityWebRequest("http://localhost:3009/api/leaderboard", "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Error posting score: {www.error}");
            }
            else
            {
                Debug.Log("Score upload complete! Response: " + www.downloadHandler.text);
            }
        }
    }

    public void GetLeaderboard()
    {
        StartCoroutine(GetLeaderboardRoutine());
    }

    IEnumerator GetLeaderboardRoutine()
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
                Debug.Log("Leaderboard data: " + www.downloadHandler.text);
            }
        }
    }
}
