using UnityEngine;
using System.Collections;

public class StealthMiniGameManager : MonoBehaviour
{
    public static StealthMiniGameManager Instance;

    [Header("References")]
    [SerializeField] private GateController gateController;
    [SerializeField] private GateTrigger gateTrigger;
    [SerializeField] private Transform player;
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private NPCWatcher npcWatcher;
    [SerializeField] private MiniGameUI miniGameUI;

    [SerializeField] private MiniGameCameraController cameraController;

    [Header("Game State")]
    public bool gameActive = false;
    public bool gameCompleted = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        if (npcWatcher != null)
        {
            npcWatcher.SetMiniGameManager(this);
            npcWatcher.enabled = false;
        }
    }

    public void StartMiniGame()
    {
        
        if (gameCompleted || gameActive) return;

        CancelInvoke(nameof(RestartMiniGame));
        gameActive = true;

        if (gateController != null)
        {
            gateController.OpenGate();
        }

        if (npcWatcher != null)
        {
            npcWatcher.enabled = true;
            npcWatcher.BeginWatchingCycle();
        }
        
        if (cameraController != null)
        {
            cameraController.SwitchToMiniGameView();
        }


        if (miniGameUI != null)
        {
            miniGameUI.ShowMessage("Sneak past the NPC and find the key!", 2f);
        }

        Debug.Log("Mini-game started.");
    }

    public void FailMiniGame()
    {
        if (!gameActive || gameCompleted) return;

        gameActive = false;
        Debug.Log("Player failed mini-game.");

        if (npcWatcher != null)
        {
            npcWatcher.StopWatchingCycle();
            npcWatcher.enabled = false;
        }

        if (miniGameUI != null)
        {
            miniGameUI.ShowMessage("The NPC saw you!", 2f);
        }

        StartCoroutine(FailSequence());
    }

    private IEnumerator FailSequence()
    {
        yield return new WaitForSeconds(0.5f);

        RespawnPlayerOutsideGate();

        if (gateController != null)
        {
            gateController.CloseGate();
        }

        if (gateTrigger != null)
        {
            gateTrigger.ResetTrigger();
        }

        yield return new WaitForSeconds(1f);

        RestartMiniGame();
    }

    private void RestartMiniGame()
    {
        StartMiniGame();
    }

    public void CompleteMiniGame()
    {
        if (!gameActive || gameCompleted) return;

        CancelInvoke(nameof(RestartMiniGame));

        gameCompleted = true;
        gameActive = false;

        if (npcWatcher != null)
        {
            npcWatcher.StopWatchingCycle();
            npcWatcher.enabled = false;
        }

        if (gateController != null)
        {
            gateController.OpenGate();
        }

        if (cameraController != null)
        {
            cameraController.SwitchToNormalView();
        }

        if (miniGameUI != null)
        {
            miniGameUI.ShowMessage("You got the key!", 2f);
        }

        Debug.Log("Mini-game completed.");
    }

    public void CloseGateOnly()
    {
        if (gateController != null)
        {
            gateController.CloseGate();
        }
    }

    private void RespawnPlayerOutsideGate()
    {
        if (player == null || respawnPoint == null)
        {
            Debug.LogWarning("Player or RespawnPoint is not assigned.");
            return;
        }

        CharacterController cc = player.GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;

        Rigidbody rb = player.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        player.position = respawnPoint.position;
        player.rotation = respawnPoint.rotation;

        if (cc != null) cc.enabled = true;
        Debug.Log("Respawning player to: " + respawnPoint.position);
    }
}