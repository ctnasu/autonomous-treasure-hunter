using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static System.Net.Mime.MediaTypeNames;

public class HaritaOlusturucu : MonoBehaviour
{
 
    public GameObject karakterPrefab; // Sınıf düzeyinde bir karakter prefab'ı tanımlayın
    public GameObject karePrefab;
    public GameObject agacPrefab;
    public GameObject kayaPrefab;
    public GameObject duvarPrefab;
    public GameObject altinSandikPrefab;
    public GameObject gumusSandikPrefab;
    public GameObject zumrutSandikPrefab;
    public GameObject bakirSandikPrefab;
    public GameObject ariPrefab;
    public GameObject kusPrefab;
    private GameObject karakterObjesi;
public float karakterHizi = 10.0f; // Karakter hızı, varsayılan olarak 5 birim/saniye olarak ayarlanmıştır
private DijkstraAlgorithm dijkstraAlgorithm; 
    void Start()
    {
        HaritayiOlustur();
        AyarlariUygula();
        EngelleriOlustur();
        SandiklariOlustur();
        KareleriOlustur();
        OlusturKarakter();
        GameObject[] sandiklar = GameObject.FindGameObjectsWithTag("SandikPrefab");

      
dijkstraAlgorithm = new DijkstraAlgorithm();
        
    }
    void OlusturKarakter()
    {
        int boy = PlayerPrefs.GetInt("SonrakiSahneBoy", 10);
        int genislik = PlayerPrefs.GetInt("SonrakiSahneGenislik", 10);

        float birimKareBoyutX = 1f;
        float birimKareBoyutY = 1f;

        float x = Random.Range(0f, boy - birimKareBoyutX);
        float y = Random.Range(birimKareBoyutY, genislik - birimKareBoyutY);

        Vector3 pozisyon = new Vector3(x, y, 0f);

        karakterObjesi = Instantiate(karakterPrefab, pozisyon, Quaternion.identity);
    }
GameObject EnYakinSandikBul(GameObject karakter, GameObject[] sandiklar)
{
    GameObject enYakinSandik = null;
    float enKisaMesafe = Mathf.Infinity;
    Vector3 karakterPozisyon = karakter.transform.position;

    foreach (GameObject sandik in sandiklar)
    {
        Vector3 sandikPozisyon = sandik.transform.position;
        float mesafe = Vector3.Distance(karakterPozisyon, sandikPozisyon);

        if (mesafe < enKisaMesafe)
        {
            enKisaMesafe = mesafe;
            enYakinSandik = sandik;
        }
    }

    return enYakinSandik;
}int silinenSandikSayisi = 0; // Silinen sandık sayısını takip etmek için sayaç
void KarakteriSandigaGit(GameObject karakter, GameObject sandik)
{
    if (sandik != null)
    {
        Vector3 hedefPozisyon = sandik.transform.position;
        Vector3 yon = (hedefPozisyon - karakter.transform.position).normalized;

        yon.x = Mathf.Round(yon.x);
        yon.y = Mathf.Round(yon.y);
        karakter.transform.position += yon * karakterHizi * Time.deltaTime;

        if (Vector3.Distance(karakter.transform.position, hedefPozisyon) < 0.1f && sandik.activeSelf)
        {
            // Sandık türünü ve konumunu al
            string sandikTuru = sandik.name;
            Lokasyon lokasyon = new Lokasyon(sandik.transform.position.x, sandik.transform.position.y);
            string lokasyonString = "(" + lokasyon.GetX() + ", " + lokasyon.GetY() + ")";

            // Sandık toplandı mesajını yazdır
            Debug.Log(sandikTuru + " toplandı! " + lokasyonString + " konumunda bulundu");

            // Sandığı yok et
            Destroy(sandik);

            // Silinen sandık sayısını artır
            silinenSandikSayisi++;

            // Eğer silinen sandık sayısı 4 ise, karakterin hareketini durdur
            if (silinenSandikSayisi >= 4)
            {
                SceneManager.LoadScene(3);
                karakter.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
        }
    }
    else
    {
        // Engellere takılmayı engelleyecek kontrol
        float algilamaMesafesi = 10.0f;
        Vector3 yon = karakter.transform.right;
        Vector3 pozisyon = karakter.transform.position;
        RaycastHit2D hit = Physics2D.Raycast(pozisyon, yon, algilamaMesafesi);
        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Engel"))
            {
                float engelGenislik = hit.collider.bounds.size.x;
                karakter.transform.position += yon * (engelGenislik / 2) * Time.deltaTime;
            }
        }
    }
}


    void Update()
    {
        if (karakterObjesi != null)
        {
            // Karakterin bulunduğu karenin pozisyonunu al
            Vector3 karakterPozisyon = karakterObjesi.transform.position;

            // Karelerin silineceği alanı belirle
            float minX = karakterPozisyon.x - 3.5f;
            float maxX = karakterPozisyon.x + 3.5f;
            float minY = karakterPozisyon.y - 3.5f;
            float maxY = karakterPozisyon.y + 3.5f;

            // Belirlenen alandaki birim kareleri bul ve sil
            GameObject[] birimKareler = GameObject.FindGameObjectsWithTag("BirimKare");
            foreach (GameObject birimKare in birimKareler)
            {
                Vector3 karePozisyon = birimKare.transform.position;

                if (karePozisyon.x >= minX && karePozisyon.x <= maxX && karePozisyon.y >= minY && karePozisyon.y <= maxY)
                {
                    Destroy(birimKare);
                }
            }
             GameObject[] sandiklar = GameObject.FindGameObjectsWithTag("SandikPrefab"); // Sandıkları bul
        GameObject enYakinSandik = EnYakinSandikBul(karakterObjesi, sandiklar); // En yakın sandığı bul

        // En yakın sandığa git
        KarakteriSandigaGit(karakterObjesi, enYakinSandik);

        // Diğer güncelleme işlemleri devam eder...
     
    }
    }

    void HaritayiOlustur()
    {
        int boy = PlayerPrefs.GetInt("SonrakiSahneBoy", 10);
        int genislik = PlayerPrefs.GetInt("SonrakiSahneGenislik", 10);

        GameObject harita = new GameObject("Harita");
        MeshRenderer meshRenderer = harita.AddComponent<MeshRenderer>();
        MeshFilter meshFilter = harita.AddComponent<MeshFilter>();

        meshFilter.mesh = duzlemolustur(boy, genislik);
        // Bembeyaz rengini kullan
        meshRenderer.material.color = Color.white;
    }

    void AyarlariUygula()
    {
        Camera.main.orthographic = true;
        Camera.main.orthographicSize = Mathf.Max(PlayerPrefs.GetInt("SonrakiSahneBoy", 10) / 2, PlayerPrefs.GetInt("SonrakiSahneGenislik", 10) / 2);
        Camera.main.transform.position = new Vector3(PlayerPrefs.GetInt("SonrakiSahneBoy", 10) / 2, PlayerPrefs.GetInt("SonrakiSahneGenislik", 10) / 2, -10f);
    }

    void EngelleriOlustur()
    {
        int engelSayisi = 7; // Oluşturulacak engel sayısı

        for (int i = 0; i < engelSayisi; i++)
        {
            EngelOlustur(agacPrefab, Color.green); // Ağaç prefab'ı, yeşil renkte
            EngelOlustur(kayaPrefab, Color.gray); // Kaya prefab'ı, gri renkte
            EngelOlustur(duvarPrefab, Color.white); // Duvar prefab'ı, beyaz renkte
        }
        EngelOlustur(ariPrefab, Color.red);
        EngelOlustur(ariPrefab, Color.red);
        EngelOlustur(kusPrefab, Color.blue);
    }

    void SandiklariOlustur()
    {
        // Hazine sandıklarını oluştur
        SandikOlustur(altinSandikPrefab);
        SandikOlustur(gumusSandikPrefab);
        SandikOlustur(zumrutSandikPrefab);
        SandikOlustur(bakirSandikPrefab);
    }

    void KareleriOlustur()
    {
        int boy = PlayerPrefs.GetInt("SonrakiSahneBoy", 10);
        int genislik = PlayerPrefs.GetInt("SonrakiSahneGenislik", 10);

        for (int x = 0; x < boy; x++)
        {
            for (int y = 0; y < genislik; y++)
            {
                float xPos = x + 0.5f; // Karelerin ortası pozisyona alınır
                float yPos = y + 0.5f;

                Vector3 pozisyon = new Vector3(xPos, yPos, 0f);
                GameObject kare = Instantiate(karePrefab, pozisyon, Quaternion.identity);
                kare.tag = "BirimKare"; // Tag'ı tanımla
            }
        }
    }

  void SandikOlustur(GameObject sandikPrefab)
{
    int boy = PlayerPrefs.GetInt("SonrakiSahneBoy", 10);
    int genislik = PlayerPrefs.GetInt("SonrakiSahneGenislik", 10);

    float birimKareBoyutX = 1f;
    float birimKareBoyutY = 1f;
    float minimumUzaklik = 10f; // Minimum uzaklık değeri düşürüldü

    float x = Random.Range(0f, boy - birimKareBoyutX);
    float y = Random.Range(birimKareBoyutY, genislik - birimKareBoyutY);

    Vector3 pozisyon = new Vector3(x, y, 0f);
    Vector2 boyut = sandikPrefab.GetComponent<SpriteRenderer>().bounds.size;

    // Çakışma kontrolü ve minimum uzaklık kontrolü yap
    Collider2D[] colliders = Physics2D.OverlapBoxAll(pozisyon, boyut, 0f);

    // Bu düzenleme ile, çakışan sandıkların ve engellerin yeniden oluşturulması sağlanır
    while (!MinimumUzaklikKontrolu(pozisyon, boyut, minimumUzaklik, boy, genislik))
    {
        x = Random.Range(0f, boy - birimKareBoyutX);
        y = Random.Range(birimKareBoyutY, genislik - birimKareBoyutY);
        pozisyon = new Vector3(x, y, 0f);
        colliders = Physics2D.OverlapBoxAll(pozisyon, boyut, 0f);
    }

    GameObject sandik = Instantiate(sandikPrefab, pozisyon, Quaternion.identity);
    sandik.GetComponent<Renderer>().material.color = Color.yellow; // Örnek olarak sarı renk atandı, isteğe bağlı
    sandik.tag = "SandikPrefab"; // Sandık tag'ı atanarak sandıkları diğer nesnelerden ayırmak için kullanılabilir
}

    void EngelOlustur(GameObject engelPrefab, Color renk)
    {
        int boy = PlayerPrefs.GetInt("SonrakiSahneBoy", 10);
        int genislik = PlayerPrefs.GetInt("SonrakiSahneGenislik", 10);

        float birimKareBoyutX = 1f;
        float birimKareBoyutY = 1f;
        float minimumUzaklik = 10f; // Minimum uzaklık değeri düşürüldü

        float x = Random.Range(0f, boy - birimKareBoyutX);
        float y = Random.Range(birimKareBoyutY, genislik - birimKareBoyutY);

        Vector3 pozisyon = new Vector3(x, y, 0f);
        Vector2 boyut = engelPrefab.GetComponent<SpriteRenderer>().bounds.size;

        // Çakışma kontrolü ve minimum uzaklık kontrolü yap
        Collider2D[] colliders = Physics2D.OverlapBoxAll(pozisyon, boyut, 0f);

        // Bu düzenleme ile, çakışan engellerin yeniden oluşturulması sağlanır
        while (colliders.Length > 0 || !MinimumUzaklikKontrolu(pozisyon, boyut, minimumUzaklik, boy, genislik))
        {
            x = Random.Range(0f, boy - birimKareBoyutX);
            y = Random.Range(birimKareBoyutY, genislik - birimKareBoyutY);
            pozisyon = new Vector3(x, y, 0f);
            colliders = Physics2D.OverlapBoxAll(pozisyon, boyut, 0f);
        }

        GameObject engel = Instantiate(engelPrefab, pozisyon, Quaternion.identity);
        engel.GetComponent<Renderer>().material.color = renk;

        // Hareketli engellere özel script ekle
        if (engelPrefab == ariPrefab)
        {
            engel.AddComponent<AriHareketi>();
        }
        else if (engelPrefab == kusPrefab)
        {
            engel.AddComponent<KusHareketi>();
        }
    }

    bool MinimumUzaklikKontrolu(Vector3 pozisyon, Vector2 boyut, float minimumUzaklik, float haritaBoy, float haritaGenislik)
    {
        // Harita sınırlarını kontrol et
        if (pozisyon.x - boyut.x / 2f < 0f || pozisyon.x + boyut.x / 2f > haritaBoy ||
            pozisyon.y - boyut.y / 2f < 0f || pozisyon.y + boyut.y / 2f > haritaGenislik)
        {
            return false;
        }

        Collider2D[] colliders = Physics2D.OverlapBoxAll(pozisyon, boyut, 0f);
        foreach (var collider in colliders)
        {
            if (Vector3.Distance(pozisyon, collider.transform.position) < minimumUzaklik)
            {
                return false;
            }
        }
        return true;
    }

    Mesh duzlemolustur(int boy, int genislik)
    {
        Mesh mesh = new Mesh();
        Vector3[] vertices = new Vector3[4];

        vertices[0] = new Vector3(0, 0, 0);
        vertices[1] = new Vector3(0, genislik, 0);
        vertices[2] = new Vector3(boy, 0, 0);
        vertices[3] = new Vector3(boy, genislik, 0);

        int[] triangles = { 0, 1, 2, 2, 1, 3 };

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        return mesh;
    }
}