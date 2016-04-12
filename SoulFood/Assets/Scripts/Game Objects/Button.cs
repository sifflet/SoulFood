using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Button : NetworkBehaviour {

	private float totalTime = 2.0f; //Amount of time needed on the pad
    [SyncVar (hook = "SetTimerUI")] private float timer; 
	bool isTriggered = false;
	private CollectorDriver collectorCurrentlyOnTheButton;

	public bool IsTriggered { get { return this.isTriggered; } }
	public CollectorDriver CollectorCurrentlyOnTheButton { get { return this.collectorCurrentlyOnTheButton; } }
	public bool IsTargettedForTriggering { get; set; }

    private Slider slider;
    private Image fillImage;
    public Color fullTimerColor = Color.green;
    public Color emptyTimerColor = Color.red;

    void Awake()
    {
        this.slider = this.GetComponentInChildren<Slider>();
        this.fillImage = this.GetComponentsInChildren<Image>()[1];
        this.timer = this.totalTime;
		this.IsTargettedForTriggering = false;
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
		{
			isTriggered = true;
			collectorCurrentlyOnTheButton = collectorDriver;
		}
	}

	void OnTriggerExit(Collider col)
	{
		CollectorDriver collectorDriver = col.GetComponent<CollectorDriver>();
		if(collectorDriver)
		{
			isTriggered = false;
			collectorCurrentlyOnTheButton = null;
		}
	}

    private void SetTimerUI(float time)
    {
        this.slider.value = time;
        this.fillImage.color = Color.Lerp(this.emptyTimerColor, this.fullTimerColor, this.timer / this.totalTime);
    }
}