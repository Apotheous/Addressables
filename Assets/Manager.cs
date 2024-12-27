using UnityEngine;
using UnityEngine.AddressableAssets;

public class Manager : MonoBehaviour
{
    [SerializeField] private GameObject go;

    public void OnClickBtn()
    {
        Instantiate(go);
    }

    private void Update()
    {

    }
}
