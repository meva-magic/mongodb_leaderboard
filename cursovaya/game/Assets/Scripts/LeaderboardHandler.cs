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
        PlayerScore playerScore = new PlayerScore
        {
            name = playerName.text,
            score = int.Parse(score.text) // Преобразование строки в целое число
        };

        string json = JsonUtility.ToJson(playerScore); // Сериализация объекта в JSON

        using (UnityWebRequest www = new UnityWebRequest("http://localhost:3009/api/leaderboard", "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(www.error);
            }
            else
            {
                Debug.Log("Form upload complete!");
            }
        }
    }


    public void GetLeaderboard()
    {
        StartCoroutine(GetLeaderboardRoutine());
    }

    IEnumerator GetLeaderboardRoutine()
    {
        using(UnityWebRequest www = UnityWebRequest.Get("http://localhost:3009/api/leaderboard"))
        {
            yield return www.SendWebRequest();

            if(www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(www.error);
            }

            else{Debug.Log(www.downloadHandler.text);}
        }
    }
}
