using UnityEngine;
using System.Collections;

public class BossDoor : MonoBehaviour
{
    public float earthquakeDuration = 3f;
    public float earthquakeMagnitude = 0.3f;

    private bool bossSequenceStarted = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !bossSequenceStarted)
        {
            bossSequenceStarted = true;
            StartCoroutine(BossBattleSequence());
        }
    }

    IEnumerator BossBattleSequence()
    {
        Debug.Log("Player entered the boss door!");

        // Step 1: Switch to Battle Background
        GameStateManager.Instance.SwitchState(GameStateManager.GameState.Battle);
        yield return new WaitForSeconds(0.5f);

        // Step 2: Earthquake!
        Debug.Log("Earthquake starting!");
        yield return StartCoroutine(CameraShake.Instance.Shake(earthquakeDuration, earthquakeMagnitude));

        // Step 3: Door opened (background already shows open door version)
        Debug.Log("Door opened! Boss battle begins!");
        yield return new WaitForSeconds(0.5f);

        // Boss battle begins!
    }
}