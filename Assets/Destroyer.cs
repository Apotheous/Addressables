using UnityEngine;

public class Destroyer : MonoBehaviour
{
    // Sahneye eklendi�i anda ba�lar
    void Start()
    {
        StartCoroutine(DeactivateAfterTime(2)); // 5 saniyelik geri say�m� ba�lat
    }

    // Coroutine ile zamanlay�c� i�levi
    private System.Collections.IEnumerator DeactivateAfterTime(float countdown)
    {
        while (countdown > 0)
        {
            Debug.Log($"Remaining time: {countdown} seconds"); // Geri say�m� konsola yaz
            yield return new WaitForSeconds(1); // 1 saniye bekle
            countdown--; // Geri say�m� azalt
        }

        Debug.Log("Destroyer is now inactive!");
        gameObject.SetActive(false); // Nesneyi pasif yap
    }
}
