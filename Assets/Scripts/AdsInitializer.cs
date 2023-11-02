using UnityEngine;
using UnityEngine.Advertisements;

public class AdsInitializer : MonoBehaviour, IUnityAdsInitializationListener
{
    [SerializeField] private string androidGameId;
    [SerializeField] private string iOSGameId;
    [SerializeField] private bool testMode = true;
    private string gameId;

    private void Awake()
    {
        InitializeAds();
    }

    private void InitializeAds()
    {
        // Determine the appropriate game ID based on the platform
        #if UNITY_IOS
            gameId = iOSGameId;
        #elif UNITY_ANDROID
            gameId = androidGameId;
        #elif UNITY_EDITOR
            // Use the Android game ID for testing in the Editor
            gameId = androidGameId;
        #endif

        // Initialize Unity Ads if it hasn't been initialized and is supported
        if (!Advertisement.isInitialized && Advertisement.isSupported)
        {
            Advertisement.Initialize(gameId, testMode, this);
        }
    }

    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads initialization complete.");
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
    }
}
