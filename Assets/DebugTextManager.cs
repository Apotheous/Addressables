using UnityEngine;
using TMPro;

public class DebugTextManager : MonoBehaviour
{
    [SerializeField] static TextMeshProUGUI m_TextMeshPro;

    private void Start()
    {
        m_TextMeshPro = GetComponent<TextMeshProUGUI>();
    }
    public static void WriteText(string text)
    {
        m_TextMeshPro.text = text;
    }


}
