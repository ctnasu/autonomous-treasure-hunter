public class Lokasyon
{
    private float x;
    private float y;

    // Constructor
    public Lokasyon(float x, float y)
    {
        this.x = x;
        this.y = y;
    }

    // Get metotları
    public float GetX()
    {
        return x;
    }

    public float GetY()
    {
        return y;
    }

    // Set metotları
    public void SetX(float x)
    {
        this.x = x;
    }

    public void SetY(float y)
    {
        this.y = y;
    }
}