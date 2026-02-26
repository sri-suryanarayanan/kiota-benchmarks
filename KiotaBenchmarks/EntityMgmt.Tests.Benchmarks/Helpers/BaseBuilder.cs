using System;
using System.Collections.Generic;
using System.Linq;
using Aveva.Platform.EntityMgmt.Client.Api.Models;
using Aveva.Platform.EntityMgmt.Tests.Benchmarks.Extensions;
using Microsoft.Kiota.Abstractions.Serialization;
using TypeCode = Aveva.Platform.EntityMgmt.Client.Api.Models.TypeCodeObject;

namespace Aveva.Platform.EntityMgmt.Tests.Benchmarks.Helpers;

/// <summary>
/// Builder for shared Component and Entity properties.
/// </summary>
public abstract class BaseBuilder<T> where T : BaseBuilder<T>
{
    protected abstract T This { get; }

    /// <summary>
    /// Adds a relationship to the model.
    /// </summary>
    public abstract T WithRelationship(RelationshipPayload relationship);

    /// <summary>
    /// Adds a property to the model.
    /// </summary>
    public abstract T WithProperty(PropertyPayload modelProperty);

    /// <summary>
    /// Adds a relationship to the <typeparamref name="T" />.
    /// </summary>
    public T WithRelationship(string id, TargetBase targetType, RelationshipType relationshipType, params string[] values)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id, nameof(id));

        var relationship = new RelationshipPayload
        {
            Id = id,
            Targets = [.. values.Select(destination => new Target() { Id = destination, TargetBase = targetType, RelationshipType = relationshipType })],
        };

        WithRelationship(relationship);

        return (T)this;
    }

    /// <summary>
    /// Adds variations of each relationship type to the model.
    /// </summary>
    public T WithAllRelationshipTypes()
    {
        foreach (var targetType in Enum.GetValues<TargetBase>().Except([TargetBase.Unknown]))
        {
            foreach (var relationshipType in Enum.GetValues<RelationshipType>().Except([RelationshipType.Unknown]))
            {
                var value = $"{targetType}__{relationshipType}";
                WithRelationship(Guid.NewGuid().ToString(), targetType, relationshipType, value);
            }
        }

        return This;
    }

    /// <summary>
    /// Adds a property to the model.
    /// </summary>
    public T WithProperty(string id, string category, TypeCode dataType, object value, string uom)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id, nameof(id));

        var property = new PropertyPayload
        {
            Id = id,
            Category = category.EmptyIfNull(),
            TypeCode = dataType,
            Value = ConvertValueForTypeCode(value, dataType),

            // setting the uomid value if datatype has one of the numeric type
            Uom = (dataType is not TypeCode.String &&
                        dataType is not TypeCode.StringArray &&
                        dataType is not TypeCode.Boolean &&
                        dataType is not TypeCode.BooleanArray &&
                        dataType is not TypeCode.DateTime &&
                        dataType is not TypeCode.DateTimeArray &&
                        dataType is not TypeCode.Int32Enum &&
                        dataType is not TypeCode.Int32EnumArray &&
                        dataType is not TypeCode.None) ? "lb" : uom.EmptyIfNull(),
        };

        WithProperty(property);

        return This;
    }

    /// <summary>
    /// Adds a property to the model.
    /// </summary>
    public T WithProperty(TypeCode dataType, object value)
    {
        WithProperty(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), dataType, value, null!);

        return This;
    }

    /// <summary>
    /// Adds variations of each property type to the entity.
    /// </summary>
    public T WithAllPropertyTypes()
    {
        var dataTypes = Enum.GetValues<TypeCode>();
        foreach (var dataType in dataTypes)
        {
            if (dataType == TypeCode.None
                || dataType == TypeCode.Int32Enum
                || dataType == TypeCode.Int32EnumArray)
            {
                continue;
            }

            WithProperty(dataType, Randomizer.RandomValueForTypeCode(dataType)!);
        }

        return This;
    }

    private static UntypedNode ConvertValueForTypeCode(object obj, TypeCode dataType)
    {
        if (obj == null || dataType == TypeCode.None)
        {
            return new UntypedNull();
        }

        return dataType switch
        {
            TypeCode.Int32Array => ConvertArrayTypes<int>(obj, TypeCode.Int32),
            TypeCode.Int64Array => ConvertArrayTypes<long>(obj, TypeCode.Int64),
            TypeCode.DoubleArray => ConvertArrayTypes<double>(obj, TypeCode.Double),
            TypeCode.SingleArray => ConvertArrayTypes<float>(obj, TypeCode.Single),
            TypeCode.BooleanArray => ConvertArrayTypes<bool>(obj, TypeCode.Boolean),
            TypeCode.StringArray => ConvertArrayTypes<string>(obj, TypeCode.String),
            TypeCode.DateTimeArray => ConvertArrayTypes<DateTime>(obj, TypeCode.DateTime),
            TypeCode.TimeSpanArray => ConvertArrayTypes<TimeSpan>(obj, TypeCode.TimeSpan),
            _ => ConvertBasicTypeCodes(obj, dataType),
        };
    }

    private static UntypedArray ConvertArrayTypes<TArray>(object obj, TypeCode nodeTypeCode)
    {
        var nodes = new List<UntypedNode>();
        if (obj is IEnumerable<TArray> objList)
        {
            foreach (var objItem in objList)
            {
                nodes.Add(ConvertBasicTypeCodes(objItem!, nodeTypeCode));
            }
        }

        return new UntypedArray(nodes);
    }

    private static UntypedNode ConvertBasicTypeCodes(object obj, TypeCode dataType)
    {
        try
        {
            return dataType switch
            {
                TypeCode.Boolean => new UntypedBoolean(Convert.ToBoolean(obj)),
                TypeCode.DateTime => new UntypedString(Convert.ToDateTime(obj).ToString()),
                TypeCode.Double => new UntypedDouble(Convert.ToDouble(obj)),
                TypeCode.Int32 => new UntypedInteger(Convert.ToInt32(obj)),
                TypeCode.Int64 => new UntypedLong(Convert.ToInt64(obj)),
                TypeCode.Single => new UntypedDecimal(Convert.ToDecimal(obj)),
                TypeCode.String => obj is string objString ? new UntypedString(objString) : new UntypedString(Convert.ToString(obj)),
                TypeCode.TimeSpan => new UntypedString(TimeSpan.Parse(obj.ToString()!).ToString()),
                TypeCode.Int32Enum => new UntypedInteger(Convert.ToInt32(obj)),
                _ => throw new NotImplementedException(),
            };
        }
        catch (Exception)
        {
            throw;
        }
    }
}
