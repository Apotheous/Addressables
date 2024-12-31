using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

public class LoadWithLabels : MonoBehaviour
{
    public List<string> keys = new List<string>() { "characters", "animals" };
    AsyncOperationHandle<IList<GameObject>> loadHandle;

    public void BtnClick()
    {
        float x = 0, z = 0;
        Debug.Log("Starting to load addressable assets...");

        loadHandle = Addressables.LoadAssetsAsync<GameObject>(
            keys,
            addressable =>
            {
                if (addressable != null)
                {
                    var assetPath = addressable.name;
                    var loadInfo = Addressables.GetDownloadSizeAsync(assetPath);
                    loadInfo.Completed += (sizeOperation) =>
                    {
                        if (sizeOperation.Result == 0)
                        {
                            // Asset is in cache, let's find its location
                            string persistentDataPath = Application.persistentDataPath;
                            string cachePath = Path.Combine(persistentDataPath, "com.unity.addressables");

                            // Android specific cache path
#if UNITY_ANDROID
                            string androidCachePath = Path.Combine(cachePath, "Android");
                            Debug.Log("+-+-" + $"Asset '{assetPath}' loaded from cache");
                            DebugTextManager.WriteText("+-+-Using = " + $"Cache Directory: {androidCachePath}");

                            try
                            {
                                if (Directory.Exists(androidCachePath))
                                {
                                    var cachedFiles = Directory.GetFiles(androidCachePath, "*", SearchOption.AllDirectories);
                                    foreach (var file in cachedFiles)
                                    {
                                        if (file.Contains(assetPath))
                                        {
                                            Debug.Log("+-+-" + $"Found cached file at: {file}");
                                        }
                                    }
                                }
                            }
                            catch (System.Exception e)
                            {
                                Debug.LogWarning($"Could not search cache directory: {e.Message}");
                            }
#else
                            Debug.Log("+-+-" + $"Asset '{assetPath}' loaded from cache at: {cachePath}");
#endif
                        }
                        else
                        {
                            DebugTextManager.WriteText("+-+-Downloading" + $"Asset '{assetPath}' downloading from server. Size: {sizeOperation.Result / 1024f:F2} KB");
                        }

                        // Release the operation handle
                        Addressables.Release(loadInfo);
                    };
                    
                    GameObject instance = Instantiate<GameObject>(addressable,
                        new Vector3(x++ * 2.0f, 0, z * 2.0f),
                        Quaternion.identity,
                        null);

                    if (x > 9)
                    {
                        x = 0;
                        z++;
                    }
                }
            },
            Addressables.MergeMode.Union,
            false);

        loadHandle.Completed += LoadHandle_Completed;
    }

    private void LoadHandle_Completed(AsyncOperationHandle<IList<GameObject>> operation)
    {
        if (operation.Status != AsyncOperationStatus.Succeeded)
        {
            Debug.LogError($"Failed to load addressable assets. Status: {operation.Status}");
            if (operation.OperationException != null)
            {
                Debug.LogError($"Error details: {operation.OperationException.Message}");
            }
        }
        else
        {
            Debug.Log("Successfully loaded all addressable assets");

            if (operation.Result != null)
            {
                // Try to get the catalog location
                var catalogPath = Path.Combine(Application.persistentDataPath, "com.unity.addressables", "catalog.json");
                if (File.Exists(catalogPath))
                {
                    Debug.Log("+-+-" + $"Addressables catalog found at: {catalogPath}");
                }

                foreach (var result in operation.Result)
                {
                    var downloadSizeHandle = Addressables.GetDownloadSizeAsync(result.name);
                    downloadSizeHandle.Completed += handle =>
                    {
                        bool isCached = handle.Result == 0;
                        Debug.Log("+-+-" + $"Asset '{result.name}' is {(isCached ? "cached" : "not cached")}");
                        Addressables.Release(handle);
                    };
                }
            }
        }
    }

    private void OnDestroy()
    {
        if (loadHandle.IsValid())
        {
            Addressables.Release(loadHandle);
        }
    }
}

/*
 * 
 *         // Varsayýlan önbellek yolunu al
        string cachePath = Caching.currentCacheForWriting.path;
        
        DebugTextManager.WriteText( cachePath );
 * */
