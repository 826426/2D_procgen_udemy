using UnityEngine;

public static class PlayerHealth
{
    private static float health = 5.0f;
    public static int clampHealth = 20;
    public static float Health
    {
        get => health;
        set => health = Mathf.Clamp(value, 0.0f, clampHealth);
    }

}
