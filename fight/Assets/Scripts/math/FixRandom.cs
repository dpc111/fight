using System;

public class FixRandom
{
    public static int num = 0;
    private ulong randSeed = 1;

    public FixRandom(uint seed)
    {
        randSeed = seed;
    }

    public uint Next()
    {
        randSeed = randSeed * 1103515245 + 12345;
        return (uint)(randSeed / 65536);
    }

    public uint Next(uint max)
    {
        return Next() % max;
    }

    public uint Range(uint min, uint max)
    {
        if (min > max)
            throw new ArgumentOutOfRangeException("Range");
        uint num = max - min;
        return Next(num) + min;
    }

    public Fix Range(Fix min, Fix max)
    {
        if (min > max)
            throw new ArgumentOutOfRangeException("Range");
        uint num = (uint)(max.rawValue - min.rawValue);
        return Fix.FromRaw(Next(num) + min.rawValue);
    }
}