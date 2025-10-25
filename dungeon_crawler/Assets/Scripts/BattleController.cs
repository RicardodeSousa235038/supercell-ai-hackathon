using UnityEngine;

public class BattleController : MonoBehaviour
{
    void Update()
    {
        // Press ESC to return to world map (you can change this to go back to the last exploration area)
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameStateManager.Instance.SwitchState(GameStateManager.GameState.WorldMap);
        }
    }
}