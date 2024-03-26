using UnityEngine;

public class AriHareketi : HareketliEngel
{
    bool sagaHareket = true;
    float baslangicX;

    void Start()
    {
        baslangicX = transform.position.x;
        hareketHizi = 3f;
    }

    void Update()
    {
        HareketEt();
    }

    protected override void HareketEt()
    {
        float hareketMiktari = hareketHizi * Time.deltaTime;

        if (sagaHareket)
        {
            transform.Translate(Vector3.right * hareketMiktari);

            if (transform.position.x >= baslangicX + 3f)
            {
                sagaHareket = false;
            }
        }
        else
        {
            transform.Translate(Vector3.left * hareketMiktari);

            if (transform.position.x <= baslangicX - 3f)
            {
                sagaHareket = true;
            }
        }
    }
}