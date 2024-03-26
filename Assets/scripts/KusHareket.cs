using UnityEngine;

public abstract class HareketliEngel : MonoBehaviour
{
    protected float hareketHizi;

    protected abstract void HareketEt();
}

public class KusHareketi : HareketliEngel
{
    float baslangicY;
    bool yukariHareket = true;

    void Start()
    {
        baslangicY = transform.position.y;
        hareketHizi = 5f;
    }

    void Update()
    {
        HareketEt();
    }

    protected override void HareketEt()
    {
        float hareketMiktari = hareketHizi * Time.deltaTime;

        if (yukariHareket)
        {
            transform.Translate(Vector3.up * hareketMiktari);

            if (transform.position.y >= baslangicY + 5f)
            {
                yukariHareket = false;
            }
        }
        else
        {
            transform.Translate(Vector3.down * hareketMiktari);

            if (transform.position.y <= baslangicY - 5f)
            {
                yukariHareket = true;
            }
        }
    }
}