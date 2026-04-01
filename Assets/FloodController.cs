using UnityEngine;

public class FloodController : MonoBehaviour
{
    [Header("Flood Settings")]
    public float startHeight = -2f;       // Water starts below street level
    public float maxHeight = 3f;          // Max flood height (above ground)
    public float riseSpeed = 0.5f;        // Metres per second rise rate
    public bool isFlooding = false;

    private void Start()
    {
        // Start water below ground
        Vector3 pos = transform.position;
        pos.y = startHeight;
        transform.position = pos;
    }

    private void Update()
    {
        if (!isFlooding) return;

        if (transform.position.y < maxHeight)
        {
            transform.Translate(Vector3.up * riseSpeed * Time.deltaTime);
        }
    }

    // Call this to start the flood (e.g. from a UI button or trigger)
    public void StartFlood()
    {
        isFlooding = true;
    }

    public void StopFlood()
    {
        isFlooding = false;
    }
}