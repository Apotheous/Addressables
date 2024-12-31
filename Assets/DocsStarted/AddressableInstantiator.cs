using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressableInstantiator : MonoBehaviour
{
    [SerializeField] AssetReferenceGameObject go;
    [SerializeField] GameObject _instanceReference;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D)) { go.ReleaseInstance(_instanceReference); }
    }
    public void OnClickAddressables()
    {
        go.InstantiateAsync().Completed += OnAddressableInstantiated;
    }


    void OnAddressableInstantiated(AsyncOperationHandle<GameObject> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            _instanceReference = handle.Result;
        }

    }
}
