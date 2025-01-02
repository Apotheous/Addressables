using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class LoadWithEvent : MonoBehaviour
{
    public string address;
    AsyncOperationHandle<GameObject> opHandle;
    
    public void BtnClick()
    {
        // Create operation
        opHandle = Addressables.LoadAssetAsync<GameObject>(address);
        // Add event handler
        opHandle.Completed += Operation_Completed;
    }

    private void Operation_Completed(AsyncOperationHandle<GameObject> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            Instantiate(obj.Result, null);
        }
        else
        {
            obj.Release();
        }
    }

    void OnDestroy()
    {
        opHandle.Release();
    }
}
