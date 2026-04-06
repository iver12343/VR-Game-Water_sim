using UnityEngine;

public class FloodController : MonoBehaviour
{
    public static FloodController Instance { get; private set; }

    [System.Serializable]
    public class FloodStage
    {
        public int stageNumber;
        public string stageName;
        public float startHeight;
        public float endHeight;
        public float duration;
    }

    [Header("Flood Stages")]
    public FloodStage[] floodStages;

    [Header("Control")]
    public bool isFlooding = false;

    private int _currentStageIndex = 0;
    private float _elapsed = 0f;

    // Read from anywhere to check current stage number
    public int CurrentStageNumber => floodStages.Length > 0 ? floodStages[_currentStageIndex].stageNumber : -1;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        if (floodStages.Length > 0)
        {
            Vector3 pos = transform.position;
            pos.y = floodStages[0].startHeight;
            transform.position = pos;
        }

        StartFlood();
    }

    private void Update()
    {
        if (!isFlooding) return;
        if (floodStages.Length == 0) return;
        if (_currentStageIndex >= floodStages.Length) return;

        FloodStage stage = floodStages[_currentStageIndex];

        _elapsed += Time.deltaTime;
        float t = Mathf.Clamp01(_elapsed / stage.duration);

        Vector3 pos = transform.position;
        pos.y = Mathf.Lerp(stage.startHeight, stage.endHeight, t);
        transform.position = pos;

        if (_elapsed >= stage.duration)
        {
            Debug.Log($"FloodController: Stage {stage.stageNumber} '{stage.stageName}' completed.");
            _elapsed = 0f;
            _currentStageIndex++;

            if (_currentStageIndex >= floodStages.Length)
            {
                Debug.Log("FloodController: All stages completed.");
                isFlooding = false;
            }
            else
            {
                Debug.Log($"FloodController: Starting stage {floodStages[_currentStageIndex].stageNumber} '{floodStages[_currentStageIndex].stageName}'");
            }
        }
    }

    public void StartFlood()
    {
        if (!isFlooding)
        {
            isFlooding = true;
            _currentStageIndex = 0;
            _elapsed = 0f;
            Debug.Log($"FloodController: Starting stage {floodStages[0].stageNumber} '{floodStages[0].stageName}'");
        }
    }

    public void StopFlood()
    {
        isFlooding = false;
    }

    // Returns true if the given stage number is currently running
    public bool IsStage(int stageNumber)
    {
        return isFlooding && CurrentStageNumber == stageNumber;
    }

    // Moves to the next stage immediately
    public void GoToNextStage()
    {
        if (!isFlooding) return;

        int nextIndex = _currentStageIndex + 1;

        if (nextIndex >= floodStages.Length)
        {
            Debug.Log("FloodController: No next stage, flood is complete.");
            isFlooding = false;
            return;
        }

        _elapsed = 0f;
        _currentStageIndex = nextIndex;
        Debug.Log($"FloodController: Moved to stage {floodStages[_currentStageIndex].stageNumber} '{floodStages[_currentStageIndex].stageName}'");
    }

    // Skip by stage name (kept from before)
    public void SkipToNextStage(string currentStageName)
    {
        int foundIndex = -1;
        for (int i = 0; i < floodStages.Length; i++)
        {
            if (floodStages[i].stageName == currentStageName)
            {
                foundIndex = i;
                break;
            }
        }

        if (foundIndex == -1)
        {
            Debug.LogWarning($"FloodController: Stage '{currentStageName}' not found.");
            return;
        }

        if (foundIndex != _currentStageIndex)
        {
            Debug.LogWarning($"FloodController: Stage '{currentStageName}' is not the current active stage.");
            return;
        }

        GoToNextStage();
    }
}