using System;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class GeminiApiClient : MonoBehaviour
{
    [Header("Gemini Settings")]
    [SerializeField] private string apiKey = "PASTE_YOUR_API_KEY";
    [SerializeField] private string model = "gemini-2.5-flash";

    [Serializable] private class GeminiRequest { public Content[] contents; }
    [Serializable] private class Content { public string role; public Part[] parts; }
    [Serializable] private class Part { public string text; }

    [Serializable] private class GeminiResponse { public Candidate[] candidates; }
    [Serializable] private class Candidate { public Content content; }

    public async Task<string> GenerateText(string prompt)
    {
        string url = $"https://generativelanguage.googleapis.com/v1beta/models/{model}:generateContent?key={apiKey}";

        var reqObj = new GeminiRequest
        {
            contents = new[]
            {
                new Content
                {
                    role = "user",
                    parts = new[] { new Part { text = prompt } }
                }
            }
        };

        string json = JsonUtility.ToJson(reqObj);

        using var req = new UnityWebRequest(url, "POST");
        req.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
        req.downloadHandler = new DownloadHandlerBuffer();
        req.SetRequestHeader("Content-Type", "application/json");

        var op = req.SendWebRequest();
        while (!op.isDone) await Task.Yield();

        if (req.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"Gemini request failed: {req.error}\n{req.downloadHandler.text}");
            return null;
        }

        string responseJson = req.downloadHandler.text;

        try
        {
            var parsed = JsonUtility.FromJson<GeminiResponse>(responseJson);
            if (parsed?.candidates != null && parsed.candidates.Length > 0)
            {
                var parts = parsed.candidates[0].content?.parts;
                if (parts != null && parts.Length > 0)
                    return parts[0].text;
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Gemini response parsing failed:\n" + responseJson);
            Debug.LogException(e);
        }

        return null;
    }

    // Utility to remove ```json fences and extract { ... }
    public static string ExtractJson(string text)
    {
        if (string.IsNullOrWhiteSpace(text)) return null;

        text = text.Replace("```json", "").Replace("```JSON", "").Replace("```", "").Trim();

        int first = text.IndexOf('{');
        int last = text.LastIndexOf('}');

        if (first < 0 || last < 0 || last <= first)
            return null;

        return text.Substring(first, last - first + 1).Trim();
    }
}