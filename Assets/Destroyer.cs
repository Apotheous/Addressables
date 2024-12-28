using UnityEngine;

public class Destroyer : MonoBehaviour
{
    // Sahneye eklendiði anda baþlar
    void Start()
    {
        StartCoroutine(DeactivateAfterTime(2)); // 5 saniyelik geri sayýmý baþlat
    }

    // Coroutine ile zamanlayýcý iþlevi
    private System.Collections.IEnumerator DeactivateAfterTime(float countdown)
    {
        while (countdown > 0)
        {
            Debug.Log($"Remaining time: {countdown} seconds"); // Geri sayýmý konsola yaz
            yield return new WaitForSeconds(1); // 1 saniye bekle
            countdown--; // Geri sayýmý azalt
        }

        Debug.Log("Destroyer is now inactive!");
        gameObject.SetActive(false); // Nesneyi pasif yap
    }
}
