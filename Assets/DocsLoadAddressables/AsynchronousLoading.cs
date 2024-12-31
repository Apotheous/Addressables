using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AsynchronousLoading : MonoBehaviour
{
    private string address ;
    private Dictionary<string, AsyncOperationHandle<GameObject>> handles = new Dictionary<string, AsyncOperationHandle<GameObject>>();

    public void BtnClick(string key)
    {
        address = key;
        if (!handles.ContainsKey(address))
        {
            var handle = Addressables.LoadAssetAsync<GameObject>(address);
            handles[address] = handle;
            handle.Completed += HandleLoadComplete;
        }
    }

    private void HandleLoadComplete(AsyncOperationHandle<GameObject> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            GameObject loadedAsset = handle.Result;
                       
            Debug.Log("Asset Loaded: " + loadedAsset.name);
        }
        else
        {
            Debug.LogError("Failed to load asset: " + handle.OperationException?.Message);
        }
    }

    private void OnDestroy()
    {
        foreach (var handle in handles.Values)
        {
            if (handle.IsValid())
            {
                handle.Release();
            }
        }
    }
}
