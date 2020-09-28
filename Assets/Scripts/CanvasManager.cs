using TMPro;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    private TextMeshProUGUI speedIndicator;
    private RectTransform turretIndicator;
    private RectTransform jetsIndicator;
    private GameObject[] lifeIndicators;
    private RectTransform targetingReticle;
    private RectTransform radarTarget;

    private const float TURRET_INDICATOR_MAX_DISPLACEMENT = 150f;

    private float canvasScale;

    public void Init()
    {
        speedIndicator = Utils.FindChildByNameRecursively(transform, "SpeedIndicator").GetComponent<TextMeshProUGUI>();
        turretIndicator = Utils.FindChildByNameRecursively(transform, "TurretIndicator").GetComponent<RectTransform>();
        jetsIndicator = Utils.FindChildByNameRecursively(transform, "JetsIndicator").GetComponent<RectTransform>();

        var lifeIndicatorParent = Utils.FindChildByNameRecursively(transform, "LifeIndicator").transform.GetChild(0);
        lifeIndicators = new GameObject[lifeIndicatorParent.childCount];
        for (int i = 0; i < lifeIndicatorParent.childCount; i++)
        {
            lifeIndicators[i] = lifeIndicatorParent.GetChild(i).gameObject;
        }

        targetingReticle = Utils.FindChildByNameRecursively(transform, "TargetingReticle").GetComponent<RectTransform>();
        radarTarget = Utils.FindChildByNameRecursively(transform, "RadarTarget").GetComponent<RectTransform>();
        canvasScale = GetComponent<Canvas>().scaleFactor;

        SetRadarTargetVisible(false);
    }

    public void SetRadarTargetVisible(bool value)
    {
        radarTarget.gameObject.SetActive(value);
    }

    public void UpdateRadarTarget(Vector3 radarTargetPosition)
    {
        Vector2 myPositionOnScreen = Camera.main.WorldToScreenPoint(radarTargetPosition);
        Vector2 finalPosition = new Vector2(myPositionOnScreen.x / canvasScale, myPositionOnScreen.y / canvasScale);
        radarTarget.anchoredPosition = finalPosition;
    }

    public void UpdateTargetingReticle(Vector3 targetingReticlePosition)
    {
        Vector2 myPositionOnScreen = Camera.main.WorldToScreenPoint(targetingReticlePosition);
        Vector2 finalPosition = new Vector2(myPositionOnScreen.x / canvasScale, myPositionOnScreen.y / canvasScale);
        targetingReticle.anchoredPosition = finalPosition;
    }

    public void SetSpeedIndicator(int value)
    {
        speedIndicator.text = string.Format("{0} KPH", value);
    }

    public void SetTurretIndicatorPosition(float percent)
    {
        turretIndicator.localPosition = new Vector3(percent * 150, 0, 0);
    }

    public void SetJetsIndicator(float percent)
    {
        jetsIndicator.localScale = new Vector3(1, percent, 1);
    }

    public void UpdateLifeIndicator(float percent)
    {
        int numVisibleBars = Mathf.CeilToInt(lifeIndicators.Length * percent);

        for (int i = 0; i < lifeIndicators.Length; i++)
        {
            if (i < numVisibleBars)
            {
                lifeIndicators[i].SetActive(true);
            }
            else
            {
                lifeIndicators[i].SetActive(false);
            }
        }
    }
}
