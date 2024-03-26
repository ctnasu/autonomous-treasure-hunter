using UnityEngine;

public class Karakter : MonoBehaviour
{
    public string ID { get; set; }
    public string Ad { get; set; }
    public Lokasyon Lokasyon { get; set; }

    // Karakterin hareket hızı
    private float hareketHizi = 30f; // Hareket hızını birim birim ayarladık

    void Update()
    {
        RastgeleHareketEt();
    }

   void RastgeleHareketEt()
{
    // Rastgele bir hedef pozisyon belirle
    Vector2 hedefPozisyon = transform.position + new Vector3(Random.Range(-1, 2), Random.Range(-1, 2), 0) * hareketHizi * Time.deltaTime;

    // Hedef pozisyonu harita sınırları içinde tut
    hedefPozisyon.x = Mathf.Clamp(hedefPozisyon.x, 0f, PlayerPrefs.GetInt("SonrakiSahneBoy", 10));
    hedefPozisyon.y = Mathf.Clamp(hedefPozisyon.y, 0f, PlayerPrefs.GetInt("SonrakiSahneGenislik", 10));

    // Engellerle çarpışma kontrolü
    if (!EngelVar(hedefPozisyon))
    {
        // Engel yoksa hareket et
        GetComponent<Rigidbody2D>().MovePosition(hedefPozisyon);
    }

    // Karakterin rotasyonunu sabit tut (dönmesini engelle)
    transform.rotation = Quaternion.identity;
}

bool EngelVar(Vector2 hedefPozisyon)
{
    // Hedef pozisyona birim çapında bir çember çevresindeki tüm çarpışmaları kontrol et
    Collider2D[] colliders = Physics2D.OverlapCircleAll(hedefPozisyon, 0.2f);

    // Eğer herhangi bir engel varsa true döndür
    foreach (Collider2D collider in colliders)
    {
        if (collider.CompareTag("Engel"))
        {
            return true;
        }
    }

    // Engel yoksa false döndür
    return false;
}

}