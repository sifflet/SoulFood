using UnityEngine;
using UnityEngine.UI;

public class Button : MonoBehaviour {

    float totalTime = 2.0f; //Amount of time needed on the pad
    private float timer; 
	bool isTriggered = false;

	public bool IsTriggered { get { return this.isTriggered; } }

    private Slider slider;
    private Image fillImage;
    public Color fullTimerColor = Color.green;
    public Color emptyTimerColor = Color.red;

    void Awake()
    {
        this.slider = this.GetComponentInChildren<Slider>();
        this.fillImage = this.GetComponentsInChildren<Image>()[1];
        this.timer = this.totalTime;
    }

    void FixedUpdate () {
		if (isTriggered) {
			if (GetButtonStatus ())
				timer = 0f;
			else
				timer -= Time.deltaTime; //timer decreased overtime
		} else {
			if(timer >= 2f)
				timer = totalTime;
			else
				timer += Time.deltaTime; //timer recharged slowly
		}

        SetTimerUI();
    }

    public bool GetButtonStatus()
    {
        if (timer <= 0f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

	public SoulTree GetSoulTreeForCurrentButton() 
	{
		return this.transform.root.GetComponent<SoulTree>();
	}

	void OnTriggerStay(Collider col)
	{
		CollectorDriver collectorDriver = col.GetComponent<CollectorDriver>();
		if(collectorDriver)
			isTriggered = true;
	}

	void OnTriggerExit(Collider col)
	{
		CollectorDriver collectorDriver = col.GetComponent<CollectorDriver>();
		if(collectorDriver)
			isTriggered = false;
	}

    private void SetTimerUI()
    {
        this.slider.value = this.timer;
        this.fillImage.color = Color.Lerp(this.emptyTimerColor, this.fullTimerColor, this.timer / this.totalTime);
    }
}