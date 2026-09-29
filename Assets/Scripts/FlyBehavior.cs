using UnityEngine;

public class FlyBehavior : BaseBehavior
{
    Vector2[] points;
    int currentIndex;

    protected override void Start()
    {
        base.Start();
        points = new Vector2[4];
        points[0] = new Vector2(5.0f, 3.0f);
        points[1] = new Vector2(5.0f, -3.0f);
        points[2] = new Vector2(-5.0f, -3.0f);
        points[3] = new Vector2(-5.0f, 3.0f);
        currentIndex = 0;
    }

    protected override void Act()
    {
        this.transform.position = MoveTo(this.transform, points[currentIndex], Time.deltaTime);
        if ((transform.position - (Vector3)points[currentIndex]).magnitude <= 0.1f )
        {
            currentIndex = (currentIndex + 1) % points.Length;
        }
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        
    }
}
