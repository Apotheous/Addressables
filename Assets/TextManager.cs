using TMPro;
using UnityEngine;

public class TextManager : MonoBehaviour
{
    public TextMeshProUGUI[] textArry;
    int t_number;
    public void PublicWriteText(string text)
    {
        
        textArry[t_number].text = text;
        t_number++;
    }
}
