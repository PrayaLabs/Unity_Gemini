using System;
using System.Threading.Tasks;
using UnityEngine;

public class PlanetInfoProvider : MonoBehaviour
{
    [Header("References")]
    public GeminiApiClient gemini;

    [Serializable]
    public class PlanetInfo
    {
        public string name;
        public string info;
    }

    [Serializable]
    public class PlanetList
    {
        public PlanetInfo[] planets;
    }

    private PlanetList cached;
    public bool HasData => cached != null && cached.planets != null && cached.planets.Length > 0;

    public PlanetList GetCached() => cached;

    public async Task<bool> LoadPlanetsFromGemini()
    {
        if (gemini == null)
        {
            Debug.LogError("PlanetInfoProvider: GeminiApiClient reference missing!");
            return false;
        }

        string prompt =
            "Return ONLY valid JSON. Do NOT use ``` or ```json. No extra words.\n" +
            "Schema:\n" +
            "{\n" +
            "  \"planets\": [\n" +
            "    {\"name\":\"Mercury\",\"info\":\"...\"},\n" +
            "    {\"name\":\"Venus\",\"info\":\"...\"},\n" +
            "    {\"name\":\"Earth\",\"info\":\"...\"},\n" +
            "    {\"name\":\"Mars\",\"info\":\"...\"},\n" +
            "    {\"name\":\"Jupiter\",\"info\":\"...\"},\n" +
            "    {\"name\":\"Saturn\",\"info\":\"...\"},\n" +
            "    {\"name\":\"Uranus\",\"info\":\"...\"},\n" +
            "    {\"name\":\"Neptune\",\"info\":\"...\"}\n" +
            "  ]\n" +
            "}\n\n" +
            "Rules:\n" +
            "- Each planet info must be maximum 100 words.\n" +
            "- Student-friendly language.\n" +
            "- Include 3–5 short facts within the 100 words.\n";

        string raw = await gemini.GenerateText(prompt);
        if (string.IsNullOrWhiteSpace(raw))
        {
            Debug.LogError("PlanetInfoProvider: Gemini returned empty text.");
            return false;
        }

        string json = GeminiApiClient.ExtractJson(raw);
        if (string.IsNullOrWhiteSpace(json))
        {
            Debug.LogError("PlanetInfoProvider: Could not extract JSON. Raw:\n" + raw);
            return false;
        }

        try
        {
            cached = JsonUtility.FromJson<PlanetList>(json);
            if (!HasData)
            {
                Debug.LogError("PlanetInfoProvider: Parsed JSON but no planets found.\nJSON:\n" + json);
                return false;
            }
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError("PlanetInfoProvider: JSON parse failed.\nJSON:\n" + json + "\nRAW:\n" + raw);
            Debug.LogException(e);
            return false;
        }
    }
}