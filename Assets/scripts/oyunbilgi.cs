using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class oyunbilgi : MonoBehaviour
{
    public Button yenidenbasla;
  
  public void basadon(){
    Debug.Log("Yeniden başla butonuna basıldı.");
    SceneManager.LoadScene(1); 
}
public Text oyunBilgiText;

    void Start()
    {
        // Oyun bilgisi metnini boşalt
        oyunBilgiText.text = "";
    }

    // Sandık toplandığında bu fonksiyon çağrılır
    public void SandikToplandi(string sandikTuru, string lokasyonString)
    {
        string bilgi = sandikTuru + " toplandı! " + lokasyonString + " konumunda bulundu";
        // Önceki bilgileri koru yeni bilgiyi ekle
        oyunBilgiText.text += bilgi + "\n";
    }
}
