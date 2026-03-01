using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlanetScrollUIRotator : MonoBehaviour
{
    [Header("References")]
    public PlanetInfoProvider provider;
    public Text uiText;                 // Your "Scrollbar Text"
    public float changeEverySeconds = 5f;

    private int index = 0;

    private IEnumerator Start()
    {
        if (uiText != null)
            uiText.text = "Loading planets...";

        if (provider == null)
        {
            if (uiText != null) uiText.text = "Provider missing!";
            yield break;
        }

        // Load once from Gemini
        var loadTask = provider.LoadPlanetsFromGemini();
        while (!loadTask.IsCompleted) yield return null;

        if (!loadTask.Result || !provider.HasData)
        {
            if (uiText != null) uiText.text = "Failed to load planets.\nCheck API key / internet.";
            yield break;
        }

        // Rotate display
        while (true)
        {
            ShowCurrent();
            yield return new WaitForSeconds(changeEverySeconds);
            index = (index + 1) % provider.GetCached().planets.Length;
        }
    }

    private void ShowCurrent()
    {
        var planets = provider.GetCached().planets;
        var p = planets[index];

        if (uiText != null)
            uiText.text = p.name + "\n\n" + p.info;
    }
}