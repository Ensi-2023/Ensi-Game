using UnityEngine;

public static class RecoilCalculator 
{
    /// <summary>
    /// Плавное изменение угла с демпфированием
    /// </summary>
    public static float CalculateSmoothAngle(
        float current,
        float target,
        ref float velocity,
        float smppthTime)
    {
        
        return Mathf.SmoothDampAngle(current, target, ref velocity, smppthTime);
    }
    
    /// <summary>
    /// Плавное изменение позиции
    /// </summary>
    public static float CalculateSmoothPosition(
        float current, 
        float target, 
        ref float velocity, 
        float smoothTime
    ) {
        return Mathf.SmoothDamp(
            current, 
            target, 
            ref velocity, 
            smoothTime
        );
    }

    /// <summary>
    /// Генерация случайного разброса на основе шума Перлина
    /// </summary>
    public static Vector2 GeneratePerlinSpread(float intensity, float time)
    {
        float perlinX = Mathf.PerlinNoise(time * 1.2f, 0) * 2 - 1;
        float perlinY = Mathf.PerlinNoise(0, time * 1.2f) * 2 - 1;
        return new Vector2(perlinX, perlinY) * intensity;
    }
    
    
}
