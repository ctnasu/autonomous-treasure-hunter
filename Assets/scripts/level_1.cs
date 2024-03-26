using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class level_1 : MonoBehaviour
{
    public InputField boy_input;
    public InputField genislik_input;
    public Button buton;

    public void BoyuKaydet()
    {
        if (string.IsNullOrEmpty(boy_input.text))
        {
            Debug.LogError("Boyu girmelisiniz!");
            return;
        }

        if (int.TryParse(boy_input.text, out int boy))
        {
            PlayerPrefs.SetInt("HaritaBoy", boy);
            Debug.Log("Boy başarıyla kaydedildi: " + boy);
        }
        else
        {
            Debug.LogError("Hatalı giriş! Lütfen geçerli bir sayı girin.");
        }
    }

    public void GenisligiKaydet()
    {
         if (string.IsNullOrEmpty(genislik_input.text))
        {
            Debug.LogError("Genişliği girmelisiniz!");
            return;
        }

        if (int.TryParse(genislik_input.text, out int genislik))
        {
            PlayerPrefs.SetInt("HaritaGenislik", genislik);
            Debug.Log("Genişlik başarıyla kaydedildi: " + genislik);
        }
        else
        {
            Debug.LogError("Hatalı giriş! Lütfen geçerli bir sayı girin.");
        }
    }

   public void SonrakiSahneyeGec()
{
    int boy = PlayerPrefs.GetInt("HaritaBoy", 10);
    int genislik = PlayerPrefs.GetInt("HaritaGenislik", 10);

    PlayerPrefs.SetInt("SonrakiSahneBoy", boy);
    PlayerPrefs.SetInt("SonrakiSahneGenislik", genislik);

    SceneManager.LoadScene(2);
}

}
