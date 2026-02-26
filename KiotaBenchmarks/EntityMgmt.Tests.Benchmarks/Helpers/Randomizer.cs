using System;
using System.Collections.Generic;
using TypeCode = Aveva.Platform.EntityMgmt.Client.Api.Models.TypeCodeObject;

namespace Aveva.Platform.EntityMgmt.Tests.Benchmarks.Helpers;

internal static class Randomizer
{
    private const int MinArrayItems = 1;
    private const int MaxArrayItems = 15;

    private static readonly Random _rnd = new();

    public static object? RandomValueForTypeCode(TypeCode dataType)
    {
        return dataType switch
        {
            TypeCode.Int32 => RandomInt(),
            TypeCode.Int64 => RandomLong(),
            TypeCode.Double => RandomDouble(),
            TypeCode.Single => RandomFloat(),
            TypeCode.Boolean => RandomBool(),
            TypeCode.String => RandomString(),
            TypeCode.DateTime => RandomDateTime(),
            TypeCode.TimeSpan => RandomTimeSpan(),
            TypeCode.Int32Array => RandomIntArray(),
            TypeCode.Int64Array => RandomLongArray(),
            TypeCode.DoubleArray => RandomDoubleArray(),
            TypeCode.SingleArray => RandomFloatArray(),
            TypeCode.BooleanArray => RandomBoolArray(),
            TypeCode.StringArray => RandomStringArray(),
            TypeCode.DateTimeArray => RandomDateTimeArray(),
            TypeCode.TimeSpanArray => RandomTimeSpanArray(),
            TypeCode.None => null,
            _ => throw new InvalidOperationException($"Invalid data type '{dataType}'."),
        };
    }

    public static int RandomInt()
    {
        return _rnd.Next(int.MinValue, int.MaxValue);
    }

    public static long RandomLong()
    {
        return _rnd.NextInt64(long.MinValue, long.MaxValue);
    }

    public static double RandomDouble()
    {
        return _rnd.NextDouble();
    }

    public static float RandomFloat()
    {
        return _rnd.NextSingle();
    }

    public static bool RandomBool()
    {
        return _rnd.Next() > int.MaxValue / 2;
    }

    public static string RandomString()
    {
        return Guid.NewGuid().ToString();
    }

    public static DateTime RandomDateTime()
    {
        var dt = DateTime.UtcNow;
        var offset = _rnd.Next(-100, 100);
        return dt.AddDays(offset);
    }

    public static TimeSpan RandomTimeSpan()
    {
        if (RandomBool())
        {
            var dt = DateTime.UtcNow;
            var offset = _rnd.Next(-100, 100);
            return dt.AddDays(offset) - dt;
        }
        else
        {
            return TimeSpan.FromTicks(RandomInt());
        }
    }

    public static int[] RandomIntArray() => RandomArray(RandomInt);

    public static long[] RandomLongArray() => RandomArray(RandomLong);

    public static double[] RandomDoubleArray() => RandomArray(RandomDouble);

    public static float[] RandomFloatArray() => RandomArray(RandomFloat);

    public static bool[] RandomBoolArray() => RandomArray(RandomBool);

    public static string[] RandomStringArray() => RandomArray(RandomString);

    public static DateTime[] RandomDateTimeArray() => RandomArray(RandomDateTime);

    public static TimeSpan[] RandomTimeSpanArray() => RandomArray(RandomTimeSpan);

    private static T[] RandomArray<T>(Func<T> randomType)
    {
        var numberOfItems = _rnd.Next(MinArrayItems, MaxArrayItems);
        var items = new List<T>();

        for (var i = 0; i < numberOfItems; i++)
        {
            items.Add(randomType());
        }

        return [.. items];
    }
}
