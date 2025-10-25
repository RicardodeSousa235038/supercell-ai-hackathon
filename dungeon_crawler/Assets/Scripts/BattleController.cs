using UnityEngine;

public class BattleController : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameStateManager.Instance.SwitchState(GameStateManager.GameState.TownExploration);
        }
    }
}