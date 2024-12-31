using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class LoadAddress : MonoBehaviour
{
    public string key;
    AsyncOperationHandle<GameObject> opHandle;


    public void BtnClick()
    {
        StartCoroutine(LoadAndInstantiate());
    }

    public IEnumerator LoadAndInstantiate()
    {
        opHandle = Addressables.LoadAssetAsync<GameObject>(key);
        yield return opHandle;

        if (opHandle.Status == AsyncOperationStatus.Succeeded)
        {
            GameObject obj = opHandle.Result;
            
            Instantiate(obj, null);
        }
    }

    void OnDestroy()
    {
        opHandle.Release();
    }
}
