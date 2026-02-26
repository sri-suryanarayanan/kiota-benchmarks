using System;
using Aveva.Platform.EntityMgmt.Client.Api.Models;
using Microsoft.Kiota.Abstractions.Serialization;
using TypeCode = Aveva.Platform.EntityMgmt.Client.Api.Models.TypeCodeObject;

namespace Aveva.Platform.EntityMgmt.Tests.Benchmarks.Helpers;

/// <summary>
/// Helper class for building entities.
/// </summary>
public class EntityBuilder : BaseBuilder<EntityBuilder>
{
    private readonly Entity _entity = new() { Id = Guid.NewGuid().ToString(), Properties = [], Relationships = [], Aliases = [] };

    protected override EntityBuilder This => this;

    /// <summary>
    /// Adds the specified name to the entity.
    /// </summary>
    public EntityBuilder WithName(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name, nameof(name));

        _entity.Name = name;

        return this;
    }

    /// <summary>
    /// Adds the specified description to the entity.
    /// </summary>
    public EntityBuilder WithDescription(string description)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(description, nameof(description));

        _entity.Description = description;

        return this;
    }

    /// <summary>
    /// Adds the specified id to the entity.
    /// </summary>
    public EntityBuilder WithId(string id)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id, nameof(id));

        _entity.Id = id;

        return this;
    }

    /// <summary>
    /// Adds the specified relationship to the entity.
    /// </summary>
    public override EntityBuilder WithRelationship(RelationshipPayload relationship)
    {
        _entity.Relationships?.Add(relationship);

        return this;
    }

    /// <summary>
    /// Adds the specified property to the entity.
    /// </summary>
    public override EntityBuilder WithProperty(PropertyPayload modelProperty)
    {
        _entity.Properties?.Add(modelProperty);

        return this;
    }

    /// <summary>
    /// Shortcut to include name, description, security tags,all property types,
    /// relationship types and populated components.
    /// </summary>
    public EntityBuilder FullyPopulated()
    {
        return WithName(Guid.NewGuid().ToString())
            .WithDescription(Guid.NewGuid().ToString())
            .WithAllPropertyTypes()
            .WithAllRelationshipTypes();
    }

    /// <summary>
    /// Builds an EntityPayload.
    /// </summary>
    public Entity BuildEntityRequest()
    {
        var destEntity = new Entity()
        {
            Id = _entity.Id,
            Name = _entity.Id,
            Description = _entity.Description,
            Properties = _entity.Properties,
            Relationships = _entity.Relationships,
            Aliases = _entity.Aliases,
        };

        // TO DO - temporarily adding a single test property, enable the following for loop when _entity.Properties.TypeCode bug has been fixed by entity store
        destEntity.Properties?.Add(new PropertyPayload
        {
            Id = "propertyId",
            TypeCode = TypeCode.String,
            Value = new UntypedString("test"),
        });

        destEntity.Relationships?.Add(new RelationshipPayload()
        {
            Id = "hasTank",
            Targets = [new Target() { Id = "tank001", TargetBase = TargetBase.Entity, RelationshipType = RelationshipType.Related }],
        });

        return destEntity;
    }
}
