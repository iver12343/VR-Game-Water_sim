using UnityEngine;
using TMPro;
using BNG;

public class PhoneNoSignal : MonoBehaviour
{
    [Header("UI Text Elements")]
    public GameObject hintText;
    public GameObject noSignalText;
    public GameObject callingText;

    [Header("Audio")]
    public AudioSource noSignalSound;
    public AudioSource voiceHint;
    public AudioSource callingSound;

    [Header("Settings")]
    public float messageDuration = 2f;
    public float callingDotSpeed = 0.5f;

    [Header("Helicopter")]
    public GameObject helicopter;              // Drag your helicopter GameObject here
    public float helicopterSpeed = 5f;         // Movement speed along Z axis
    public float helicopterMoveDistance = 50f; // How far it travels before stopping (0 = infinite)
    public bool stopAtDestination = true;      // Stop moving after reaching distance

    private Grabbable grabbable;
    private InputBridge input;
    private float hideTimer = 0f;
    private bool isShowing = false;
    private bool voicePlayed = false;
    private bool wasHeld = false;
    private bool atDestination = false;

    private TMP_Text callingTMP;
    private float dotTimer = 0f;
    private int dotCount = 0;
    private bool isCalling = false;

    // Helicopter tracking
    private Vector3 helicopterStartPos;
    private bool helicopterMoving = false;

    void Start()
    {
        grabbable = GetComponent<Grabbable>();
        input = InputBridge.Instance;

        if (callingText != null)
            callingTMP = callingText.GetComponent<TMP_Text>();

        if (hintText != null) hintText.SetActive(true);
        if (noSignalText != null) noSignalText.SetActive(false);
        if (callingText != null) callingText.SetActive(false);

        // Make sure helicopter starts hidden
        if (helicopter != null)
        {
            helicopter.SetActive(false);
            helicopterStartPos = helicopter.transform.position;
        }
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
            if (atDestination)
                ShowCalling();
            else
                ShowNoSignal();
        }

        if (isShowing)
        {
            hideTimer -= Time.deltaTime;
            if (hideTimer <= 0f)
                HideNoSignal();
        }

        if (isCalling)
            AnimateCallingDots();

        // Move helicopter if active
        if (helicopterMoving)
            MoveHelicopter();
    }

    // --- Helicopter ---

    void ActivateHelicopter()
    {
        if (helicopter == null) return;

        helicopter.SetActive(true);
        helicopter.transform.position = helicopterStartPos; // Reset to start position
        helicopterMoving = true;
        Debug.Log("Helicopter activated and moving.");
    }

    void DeactivateHelicopter()
    {
        if (helicopter == null) return;

        helicopterMoving = false;
        helicopter.SetActive(false);
        Debug.Log("Helicopter deactivated.");
    }

    void MoveHelicopter()
    {
        // Move along positive Z axis
        helicopter.transform.Translate(Vector3.forward * helicopterSpeed * Time.deltaTime, Space.World);

        // Optionally stop after a set distance
        if (stopAtDestination && helicopterMoveDistance > 0f)
        {
            float distanceTravelled = Vector3.Distance(helicopterStartPos, helicopter.transform.position);
            if (distanceTravelled >= helicopterMoveDistance)
            {
                helicopterMoving = false;
                Debug.Log("Helicopter reached destination, stopping.");
            }
        }
    }

    // --- Destination Detection ---

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Destination"))
        {
            atDestination = true;
            Debug.Log("Phone: Reached destination zone");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Destination"))
        {
            atDestination = false;
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

        // Activate helicopter when calling starts
        ActivateHelicopter();
    }

    void StopCalling()
    {
        isCalling = false;

        if (callingText != null) callingText.SetActive(false);

        if (callingSound != null && callingSound.isPlaying)
            callingSound.Stop();

        // Deactivate helicopter when calling stops
        DeactivateHelicopter();
    }

    // --- Dot Animation ---

    void AnimateCallingDots()
    {
        dotTimer += Time.deltaTime;

        if (dotTimer >= callingDotSpeed)
        {
            dotTimer = 0f;
            dotCount = (dotCount + 1) % 4;

            if (callingTMP != null)
            {
                string dots = new string('.', dotCount);
                callingTMP.text = "Calling" + dots;
            }
        }
    }
}