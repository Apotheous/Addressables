using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class LoadWithAddress : MonoBehaviour
{
    // Assign in Editor or in code
    public string address;

    // Retain handle to release asset and operation
    private AsyncOperationHandle<GameObject> handle;

    // Start the load operation on start
    public void LoadWithAdressMeth()
    {
        handle = Addressables.LoadAssetAsync<GameObject>(address);
        handle.Completed += Handle_Completed;
   
    }

    // Instantiate the loaded prefab on complete
    private void Handle_Completed(AsyncOperationHandle<GameObject> operation)
    {
        if (operation.Status == AsyncOperationStatus.Succeeded)
        {
            Instantiate(operation.Result, null);
        }
        else
        {
            Debug.LogError($"Asset for {address} failed to load.");
        }
    }

    // Release asset when parent object is destroyed
    private void OnDestroy()
    {
        handle.Release();
    }

    public void PosCenter(GameObject[] go)
    {
        foreach (GameObject go2 in go) 
        {
            if (go2 != null) 
            {
                go2.transform.position = Vector3.zero; 
                go2.transform.SetParent(null); 
            }
        }
        
    }
}
