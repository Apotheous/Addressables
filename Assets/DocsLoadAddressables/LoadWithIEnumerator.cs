using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class LoadWithIEnumerator : MonoBehaviour
{
    public string address;
    AsyncOperationHandle<GameObject> opHandle;
    public void BtnClick() { StartCoroutine(StartFonc()); }
    public IEnumerator StartFonc()
    {
        opHandle = Addressables.LoadAssetAsync<GameObject>(address);

        // yielding when already done still waits until the next frame
        // so don't yield if done.
        if (!opHandle.IsDone)
            yield return opHandle;

        if (opHandle.Status == AsyncOperationStatus.Succeeded)
        {
            Instantiate(opHandle.Result, null);
        }
        else
        {
            opHandle.Release();
        }
    }

    void OnDestroy()
    {
        opHandle.Release();
    }
}
