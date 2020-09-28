using UnityEngine;
using RedBlueGames.Tools.TextTyper;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;

public class MainMenuCanvasManager : MonoBehaviour
{
    public static MainMenuCanvasManager Instance { get; private set; }

    private TextTyper pilotName_TextTyper;
    private TextTyper pilotAge_TextTyper;
    private Image pilotCountry_Image;
    private TextTyper description_TextTyper;
    private TextTyper vehicleName_TextTyper;
    private RawImage blackFade;

    private Queue<string> dialogueLines = new Queue<string>();

    private Statistic speedStats;
    private Statistic armorStats;
    private Statistic attackStats;


    public void Init()
    {
        Instance = this;

        pilotName_TextTyper = Utils.FindChildByNameRecursively(transform, "PilotName_Text (TMP)").GetComponent<TextTyper>();
        pilotAge_TextTyper = Utils.FindChildByNameRecursively(transform, "PilotAge_Text (TMP)").GetComponent<TextTyper>();
        pilotCountry_Image = Utils.FindChildByNameRecursively(transform, "PilotCountry_Image").GetComponent<Image>();
        description_TextTyper = Utils.FindChildByNameRecursively(transform, "VehicleDescription_Text (TMP)").GetComponent<TextTyper>();
        vehicleName_TextTyper = Utils.FindChildByNameRecursively(transform, "VehicleName_Text (TMP)").GetComponent<TextTyper>();
        speedStats = Utils.FindChildByNameRecursively(transform, "Speed_Statistic").GetComponent<Statistic>();
        armorStats = Utils.FindChildByNameRecursively(transform, "Armor_Statistic").GetComponent<Statistic>();
        attackStats = Utils.FindChildByNameRecursively(transform, "Attack_Statistic").GetComponent<Statistic>();
        blackFade = Utils.FindChildByNameRecursively(transform, "BlackFade_RawImage").GetComponent<RawImage>();
    }

    public void UpdateStatistics(PlayerScriptableObject pso)
    {
        speedStats.SetStars(pso.speedValue);
        armorStats.SetStars(pso.armorValue);
        attackStats.SetStars(pso.attackValue);
        vehicleName_TextTyper.TypeText(pso.vehicleName);
        description_TextTyper.TypeText(pso.vehicleDescription, 0.01f);
        pilotName_TextTyper.TypeText(pso.pilotName);
        pilotAge_TextTyper.TypeText(pso.pilotAge.ToString());
        pilotCountry_Image.sprite = pso.pilotCountryFlag;
    }

    public void SetFadeToBlack()
    {
        blackFade.color = new Color(0, 0, 0, 1);
    }

    public void SetFadeToClear()
    {
        blackFade.color = new Color(0, 0, 0, 0);
    }

    public void FadeToClear()
    {
        StartCoroutine(FadeToAlpha(0));
    }

    public void FadeToBlack()
    {
        StartCoroutine(FadeToAlpha(1));
    }

    private IEnumerator FadeToAlpha(int endAlpha)
    {
        float duration = 1;
        var endTime = Time.time + duration;
        float startAlpha = endAlpha == 0 ? 1 : 0;

        float t = 0;

        while (Time.time < endTime)
        {
            var alpha = Mathf.Lerp(startAlpha, endAlpha, t);
            blackFade.color = new Color(0, 0, 0, alpha);
            yield return null;
            t = (duration - (endTime - Time.time)) / duration;
        }
        blackFade.color = new Color(0, 0, 0, endAlpha);
    }
}
