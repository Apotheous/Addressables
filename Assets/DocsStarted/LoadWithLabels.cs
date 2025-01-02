using System.Collections;
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
    [SerializeField] TextManager textManager;
    public void BtnClick()
    {
        float x = 0, z = 0;
        textManager.PublicWriteText("Starting to load addressable assets...");

        loadHandle = Addressables.LoadAssetsAsync<GameObject>(keys,addressable =>{
                if (addressable != null)
                {
                var assetPath = addressable.name;
                    var loadInfo = Addressables.GetDownloadSizeAsync(assetPath);
                    textManager.PublicWriteText("+-+-Using = " + $"Cache Directory: {loadInfo}");
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
                            textManager.PublicWriteText("+-+-" + $"Asset '{assetPath}' loaded from cache");
                            //DebugTextManager.WriteText("+-+-Using = " + $"Cache Directory: {cachePath}");

                            try
                            {
                                if (Directory.Exists(androidCachePath))
                                {
                                    var cachedFiles = Directory.GetFiles(androidCachePath, "*", SearchOption.AllDirectories);
                                    foreach (var file in cachedFiles)
                                    {
                                        if (file.Contains(assetPath))
                                        {
                                            textManager.PublicWriteText("+-+-" + $"Found cached file at: {file}");
                                        }
                                    }
                                }
                            }
                            catch (System.Exception e)
                            {
                                textManager.PublicWriteText($"Could not search cache directory: {e.Message}");
                            }
#else
                            Debug.Log("+-+-" + $"Asset '{assetPath}' loaded from cache at: {cachePath}");
#endif
                        }
                        else
                        {
                            textManager.PublicWriteText("+-+-Downloading" + $"Asset '{assetPath}' downloading from server. Size: {sizeOperation.Result / 1024f:F2} KB");
                        }

                        // Release the operation handle
                        Addressables.Release(loadInfo);
                    };
                    
                    GameObject instance = Instantiate<GameObject>(addressable,new Vector3(x++ * 2.0f, 0, z * 2.0f),Quaternion.identity,null);

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

        StartCoroutine(DelayedRelease());

    }

    private void LoadHandle_Completed(AsyncOperationHandle<IList<GameObject>> operation)
    {
        if (operation.Status != AsyncOperationStatus.Succeeded)
        {
            textManager.PublicWriteText($"Failed to load addressable assets. Status: {operation.Status}");
            if (operation.OperationException != null)
            {
                textManager.PublicWriteText($"Error details: {operation.OperationException.Message}");
            }
        }
        else
        {
            textManager.PublicWriteText("Successfully loaded all addressable assets");

            if (operation.Result != null)
            {
                // Try to get the catalog location
                var catalogPath = Path.Combine(Application.persistentDataPath, "com.unity.addressables", "catalog.json");
                if (File.Exists(catalogPath))
                {
                    textManager.PublicWriteText("+-+-" + $"Addressables catalog found at: {catalogPath}");
                }

                foreach (var result in operation.Result)
                {
                    var downloadSizeHandle = Addressables.GetDownloadSizeAsync(result.name);
                    downloadSizeHandle.Completed += handle =>
                    {
                        bool isCached = handle.Result == 0;
                        textManager.PublicWriteText("+-+-" + $"Asset '{result.name}' is {(isCached ? "cached" : "not cached")}");
                        Addressables.Release(handle);
                    };
                }
            }
        }
    }

    private IEnumerator DelayedRelease()
    {
        yield return new WaitForSeconds(5f); // 5 saniye bekle
        if (loadHandle.IsValid())
        {
            Addressables.Release(loadHandle);
        }
    }
}


