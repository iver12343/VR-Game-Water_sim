using UnityEngine;
using TMPro;
using BNG;

public class PhoneNoSignal : MonoBehaviour
{
    [Header("UI Text Elements")]
    public GameObject hintText;
    public GameObject noSignalText;
    public GameObject callingText;      // New calling text object

    [Header("Audio")]
    public AudioSource noSignalSound;
    public AudioSource voiceHint;
    public AudioSource callingSound;    // Looping ring sound

    [Header("Settings")]
    public float messageDuration = 2f;
    public float callingDotSpeed = 0.5f; // Speed of dot animation

    private Grabbable grabbable;
    private InputBridge input;
    private float hideTimer = 0f;
    private bool isShowing = false;
    private bool voicePlayed = false;
    private bool wasHeld = false;
    private bool atDestination = false; // True when player is in destination zone

    // Dot animation variables
    private TMP_Text callingTMP;
    private float dotTimer = 0f;
    private int dotCount = 0;
    private bool isCalling = false;

    void Start()
    {
        grabbable = GetComponent<Grabbable>();
        input = InputBridge.Instance;

        // Get TMP component from CallingText
        if (callingText != null)
            callingTMP = callingText.GetComponent<TMP_Text>();

        // Initial states
        if (hintText != null) hintText.SetActive(true);
        if (noSignalText != null) noSignalText.SetActive(false);
        if (callingText != null) callingText.SetActive(false);
    }

    void Update()
    {
        bool isHeld = grabbable.BeingHeld;

        if (isHeld && !wasHeld) OnPhonePickedUp();
        if (!isHeld && wasHeld) OnPhonePutDown();

        wasHeld = isHeld;

        if (!isHeld) return;

        Grabber holder = grabbable.GetPrimaryGrabber();
        if (holder == null) return;

        bool triggerPressed = false;

        if (holder.HandSide == ControllerHand.Left)
            triggerPressed = input.LeftTrigger > 0.5f;
        else if (holder.HandSide == ControllerHand.Right)
            triggerPressed = input.RightTrigger > 0.5f;

        if (triggerPressed && !isShowing && !isCalling)
        {
            // Check if player is at destination
            if (atDestination)
                ShowCalling();      // Show calling animation
            else
                ShowNoSignal();     // Show no signal
        }

        // Auto hide no signal timer
        if (isShowing)
        {
            hideTimer -= Time.deltaTime;
            if (hideTimer <= 0f)
                HideNoSignal();
        }

        // Animate calling dots
        if (isCalling)
            AnimateCallingDots();
    }

    // --- Destination Detection ---

    // Called when player/phone enters destination zone
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Destination"))
        {
            atDestination = true;
            Debug.Log("Phone: Reached destination zone");
        }
    }

    // Called when player/phone exits destination zone
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Destination"))
        {
            atDestination = false;
            // If calling and player leaves zone, stop calling
            if (isCalling) StopCalling();
            Debug.Log("Phone: Left destination zone");
        }
    }

    // --- Phone State ---

    void OnPhonePickedUp()
    {
        if (hintText != null) hintText.SetActive(false);

        if (voiceHint != null && !voicePlayed)
        {
            voiceHint.Play();
            voicePlayed = true;
        }
    }

    void OnPhonePutDown()
    {
        HideNoSignal();
        StopCalling();
    }

    // --- No Signal ---

    void ShowNoSignal()
    {
        isShowing = true;
        hideTimer = messageDuration;

        if (noSignalText != null) noSignalText.SetActive(true);
        if (noSignalSound != null && !noSignalSound.isPlaying)
            noSignalSound.Play();
    }

    void HideNoSignal()
    {
        isShowing = false;
        if (noSignalText != null) noSignalText.SetActive(false);
    }

    // --- Calling ---

    void ShowCalling()
    {
        isCalling = true;
        dotCount = 0;
        dotTimer = 0f;

        if (callingText != null)
        {
            callingText.SetActive(true);
            if (callingTMP != null)
                callingTMP.text = "Calling.";
        }

        if (callingSound != null)
            callingSound.Play();
    }

    void StopCalling()
    {
        isCalling = false;

        if (callingText != null) callingText.SetActive(false);

        if (callingSound != null && callingSound.isPlaying)
            callingSound.Stop();
    }

    // --- Dot Animation ---

    void AnimateCallingDots()
    {
        dotTimer += Time.deltaTime;

        if (dotTimer >= callingDotSpeed)
        {
            dotTimer = 0f;
            dotCount = (dotCount + 1) % 4; // Cycles 0,1,2,3

            if (callingTMP != null)
            {
                // Builds dot string: "" / "." / ".." / "..."
                string dots = new string('.', dotCount);
                callingTMP.text = "Calling" + dots;
            }
        }
    }
}