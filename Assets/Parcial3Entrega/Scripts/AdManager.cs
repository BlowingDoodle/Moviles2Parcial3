using System;
using UnityEngine;

public class AdManager : MonoBehaviour
{
    public static AdManager Instance { get; private set; }

    private string _gameId = "203326405"; // Reemplaza con tu Game ID real
    private Action<bool> _callback;
    private static bool _isInitialized = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (_isInitialized)
        {
            Destroy(gameObject);
            return;
        }

        IronSource.Agent.init(_gameId);
        IronSource.Agent.shouldTrackNetworkState(true);
        DontDestroyOnLoad(gameObject);
        _isInitialized = true;

#if DEVELOPMENT_BUILD
        IronSource.Agent.validateIntegration();
#endif

        // Suscribirse a eventos de IronSource
        IronSourceRewardedVideoEvents.onAdRewardedEvent += RewardedVideoOnAdRewardedEvent;
        IronSourceRewardedVideoEvents.onAdShowFailedEvent += RewardedVideoOnAdShowFailedEvent;
    }

    private void RewardedVideoOnAdRewardedEvent(IronSourcePlacement placement, IronSourceAdInfo adInfo)
    {
        _callback?.Invoke(true); // Notificar que el anuncio se completó
        _callback = null;
    }

    private void RewardedVideoOnAdShowFailedEvent(IronSourceError error, IronSourceAdInfo adInfo)
    {
        _callback?.Invoke(false); // Notificar que el anuncio falló
        _callback = null;
    }

    public void ShowRewardedVideo(Action<bool> callback)
    {
        _callback = callback;

        if (IronSource.Agent.isRewardedVideoAvailable())
        {
            Debug.Log("Showing ad...");
            IronSource.Agent.showRewardedVideo();
        }
        else
        {
            Debug.Log("Ad not available.");
            _callback = null;
            callback(false);
        }
    }

    private void OnApplicationPause(bool isPaused)
    {
        IronSource.Agent.onApplicationPause(isPaused);
    }
}
