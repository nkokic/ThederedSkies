using UnityEngine;
using System;

public class MathExpanded
{
    public static float Ramp(float value, float threshold)
    {
        value -= threshold;
        if (value < 0)
            return 0;
        return value;
    }

    public static float GetRatioWithInfinites(float antecedent, float consequent)
    {
        if (float.IsInfinity(antecedent) && float.IsInfinity(consequent))
            return .5f;
        if (float.IsInfinity(antecedent))
            return 1;

        return antecedent / consequent;
    }

    public static float CalculateAngleFromLine(Vector2 start, Vector2 end)
    {
        Vector2 direction = end - start;
        return Vector2.SignedAngle(Vector2.up, direction);
    }

    public static Vector3[] CalculateCatenary(Vector2 A, Vector2 B, float lenght, int count)
    {
        Vector3[] y = new Vector3[count];
        float h = (B.x - A.x);
        float v = (B.y - A.y);
        float deltaX = h / count;
        lenght = Mathf.Max(lenght, (B - A).magnitude);

        const float IntervalStep = 0.01f;
        float a = 0;
        do
        {
            a += IntervalStep;
        }
        while (Math.Sqrt(Math.Pow(lenght, 2) - Math.Pow(v, 2)) < 2 * a * Sinh(h / (2 * a)));

        float p = (A.x + B.x - a * Mathf.Log((lenght + v) / (lenght - v), Mathf.Exp(1))) / 2;
        float q = ((A + B).y - lenght * Coth(h / (2 * a))) / 2;

        for (int i = 0; i < count; i++)
            y[i] = new Vector3(i * deltaX + A.x, a * Cosh((i*deltaX - p) / a) + q, 0);

        return y;
    }

    public static float Cosh(float f)
    {
        return (float)(Math.Exp(f)+ Math.Exp(-f)) / 2;
    }

    public static float Sinh(float f)
    {
        return (float)(Math.Exp(f) - Math.Exp(-f)) / 2;
    }

    public static float Coth(float f)
    {
        return Cosh(f) / Sinh(f);
    }

    public static int SolveQuadraticEquasion(float a, float b, float c, out float x1, out float x2)
    {
        float D = b * b - 4 * a * c;

        if(D < 0)
        {
            x1 = Mathf.Infinity;
            x2 = Mathf.NegativeInfinity;
            return 0;
        }

        x1 = (-b + Mathf.Sqrt(D)) / (2 * a);
        x2 = (-b - Mathf.Sqrt(D)) / (2 * a);

        return D == 0 ? 1 : 2;
    }

    public static Vector2 Angle2Direction(float angle)
    {
        return new Vector2(x: Mathf.Cos(angle), y: Mathf.Sin(angle));
    }
}

