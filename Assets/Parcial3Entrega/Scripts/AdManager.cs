using System;
using UnityEngine;
using Unity.VisualScripting;

public class AdManager : MonoBehaviour
{
    public static AdManager Instance { get; private set; }

    private string _gameid = "203326405";
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
        IronSource.Agent.init(_gameid);
        IronSource.Agent.shouldTrackNetworkState(true);
        DontDestroyOnLoad(gameObject);
        _isInitialized = true;

#if DEVELOPMENT_BUILD 
        IronSource.Agent.validateIntegration();
#endif
        IronSourceRewardedVideoEvents.onAdOpenedEvent += RewardedVideoOnAdOpenedEvent;
        IronSourceRewardedVideoEvents.onAdClosedEvent += RewardedVideoOnAdClosedEvent;
        IronSourceRewardedVideoEvents.onAdAvailableEvent += RewardedVideoOnAdAvailable;
        IronSourceRewardedVideoEvents.onAdUnavailableEvent += RewardedVideoOnAdUnavailable;
        IronSourceRewardedVideoEvents.onAdShowFailedEvent += RewardedVideoOnAdShowFailedEvent;
        IronSourceRewardedVideoEvents.onAdRewardedEvent += RewardedVideoOnAdRewardedEvent;
        IronSourceRewardedVideoEvents.onAdClickedEvent += RewardedVideoOnAdClickedEvent;


    }

    void RewardedVideoOnAdAvailable(IronSourceAdInfo adInfo)
    {
    }

    void RewardedVideoOnAdUnavailable()
    {
    }

    void RewardedVideoOnAdOpenedEvent(IronSourceAdInfo adInfo)
    {

    }

    void RewardedVideoOnAdClosedEvent(IronSourceAdInfo adInfo)
    {
    }
    void RewardedVideoOnAdRewardedEvent(IronSourcePlacement placement, IronSourceAdInfo adInfo)
    {
        _callback?.Invoke(true);
        _callback = null;
    }

    void RewardedVideoOnAdShowFailedEvent(IronSourceError error, IronSourceAdInfo adInfo)
    {
        _callback?.Invoke(false);
        _callback = null;
    }

    void RewardedVideoOnAdClickedEvent(IronSourcePlacement placement, IronSourceAdInfo adInfo)
    {
    }

    public void ShowRewardedVideo(Action<bool> callback)
    {
        _callback = callback;
        if (IronSource.Agent.isRewardedVideoAvailable())
        {
            Debug.Log("showing ad");
            IronSource.Agent.showRewardedVideo();
        }
        else
        {
            Debug.Log("ad not available");
            _callback = null;
            callback(false);

        }
    }

    

    void OnApplicationPause(bool isPaused)
    {
        IronSource.Agent.onApplicationPause(isPaused);
    }




}


