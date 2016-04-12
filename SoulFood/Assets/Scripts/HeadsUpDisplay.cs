using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HeadsUpDisplay : MonoBehaviour {

    private Text[] headsUpDisplay;
    private static Text SoulsCollectedText;
    private static Text CollectorRemainingLivesText;
    private static Text GameTimerText;

    public GameObject indicator;
    private static GameObject indicatorInstance;
    private float indicatorLifeTime = 5.0f;
    private float indicatorTimer = 5.0f;

    /**
     *  Acquiring handle on text components.
     */
    void Awake()
    {
        this.headsUpDisplay = this.gameObject.GetComponentsInChildren<Text>();
        SoulsCollectedText = this.headsUpDisplay[0];
        CollectorRemainingLivesText = this.headsUpDisplay[1];
        GameTimerText = this.headsUpDisplay[2];

        indicatorInstance = (GameObject)Instantiate(indicator);
        indicatorInstance.transform.SetParent(this.transform);
        indicatorInstance.transform.localScale = Vector2.one;
        indicatorInstance.SetActive(false);
    }

    void Update()
    {
        if(indicatorInstance.activeInHierarchy)
        {
            indicatorTimer -= Time.deltaTime;

            if(indicatorTimer <= 0.0f)
            {
                indicatorInstance.SetActive(false);
                indicatorTimer = indicatorLifeTime;
            }
        }
    }

    public static void Initialize(int soulsCollected, int soulLimit, int collectorRemainingLives, float gameTimeRemaining)
    {
        UpdateHUDSoulsCollected(soulsCollected, soulLimit);
        UpdateHUDCollectorRemainingLives(collectorRemainingLives);
        UpdateHUDGameTimer(gameTimeRemaining);
    }

    public static void UpdateHUDSoulsCollected(int soulsCollected, int soulLimit)
    {
        SoulsCollectedText.text = "Souls Collected: " + soulsCollected + " / " + soulLimit;
    }

    public static void UpdateHUDCollectorRemainingLives(int collectorRemainingLives)
    {
        CollectorRemainingLivesText.text = "Collector Lives: " + collectorRemainingLives;
    }

    public static void UpdateHUDGameTimer(float gameTimeRemaining)
    {
        GameTimerText.text = timeFormat(gameTimeRemaining);
    }

    private static string timeFormat(float time)
    {
        string minutes = Mathf.Floor(time / 60.0f).ToString("00");
        string seconds = (time % 60.0f).ToString("00");
        return minutes + ":" + seconds;
    }

    public static void CreateIndicator(Vector2 targetLocation)
    {
        indicatorInstance.SetActive(true);
    }
}