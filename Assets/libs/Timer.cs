public class Timer
{
    public bool isRepeat = true;
    public float timer = 0;
    public float Cooldown = 0;
    public delegate void TimerEventHandler();
    public event TimerEventHandler OnTick;
    public Timer(float time)
    {
        Cooldown = time;
        timer = time;
    }
    public Timer(float time, float start)
    {
        Cooldown = time;
        timer = start;
    }
    public void Update(float deltaTime)
    {
        if (timer > 0)
            timer -= deltaTime;
        if (timer < 0)
            timer = 0;
        if (timer == 0)
        {
            if (isRepeat)
                timer = Cooldown;
            OnTick();
        }
    }
}