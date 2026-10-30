using UnityEngine;

public class GameBootstrapper : MonoBehaviour
{
    //void Start()
    //{
    //    // 1. Kick off your daily round feed database arrays
    //    if (GameScrollManager.Instance != null)
    //    {
    //        GameScrollManager.Instance.InitializeDayLoadout(1);
    //    }

    //    // 2. Clear out overlay canvases and spin up your 7-bar round clock instantly
    //    if (BatteryTimeManager.Instance != null)
    //    {
    //        BatteryTimeManager.Instance.LaunchTimerEngine();
    //        Debug.Log("[BATTERY ENGINE] Timer successfully initialized! 7 Bars ticking.");
    //    }
    //}

    void Start()
    {
        // Give components 1 structural frame step to wake up cleanly
        Invoke(nameof(ExecuteSafeSystemBoot), 0.1f);
    }

    void ExecuteSafeSystemBoot()
    {
        if (BatteryTimeManager.Instance != null)
        {
            BatteryTimeManager.Instance.LaunchTimerEngine();
            Debug.Log("[BATTERY ENGINE] Timer kicked off successfully!");
        }

        if (GameScrollManager.Instance != null)
        {
            GameScrollManager.Instance.InitializeDayLoadout(1);
        }
    }

}
