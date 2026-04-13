using UnityEngine;
using Esper.Freeloader;
using Crest; // Crest namespace
using System.Collections;
using UnityEngine.SceneManagement;
public class DrownDetector : MonoBehaviour
{
    private bool isDrowning = false;
    private SampleHeightHelper _sampleHeightHelper = new SampleHeightHelper();

    public GameObject Description;

    private void Update()
    {
        // Query the water height at player's position
        _sampleHeightHelper.Init(transform.position, 0f);

        if (_sampleHeightHelper.Sample(out float waterHeight))
        {
            // If player is below water surface
            if (transform.position.y < waterHeight && !isDrowning)
            {
                isDrowning = true;
                StartCoroutine(DrownPlayer());
            }
        }
    }

    private IEnumerator DrownPlayer()
    {
        Debug.Log("Player is below water surface!");
        yield return new WaitForSeconds(2f);
         GameOver();
    }

    private void GameOver()
    {
        // SceneManager.LoadScene(2);
        Description.SetActive(true);
    }
}