using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using FakeRepo.Core.Impl;

namespace FakeRepo.Core;

public abstract class RepositoryFake<TId, TEntity>
    where TEntity : class
{
    private readonly ISerializer _serializer;
    private readonly RepositoryConfiguration _config;

    private readonly IDictionary<TId, TEntity> _table = new Dictionary<TId, TEntity>();

    protected RepositoryFake(ISerializer serializer, Action<RepositoryConfiguration> configAction)
    {
        _serializer = serializer;
        var config = new RepositoryConfiguration();
        configAction(config);
        _config = config;
    }

    protected Func<TEntity, TId> GetId => _config.GetIdFunc.Compile();
    protected Action<TEntity, TId> SetId => (entity, id) =>
    {
        var propertyName = _config.GetIdFunc.GetMemberInfo().Name;
        var propertyInfo = typeof(TEntity).GetProperty(propertyName);
        if (propertyInfo == null)
            throw new InvalidOperationException($"No property '{propertyName}' found on {typeof(TEntity).Name}");
        propertyInfo.SetValue(entity, id);
    };

    public Task<TEntity> GetByIdAsync(TId id, CancellationToken ct) => Task.FromResult(GetById(id));
    public Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken ct) => Task.FromResult(GetAll());

    protected Task<TEntity> GetByAsync(Expression<Func<TEntity, bool>> filterExpr, CancellationToken ct) => Task.FromResult(GetBy(filterExpr));
    protected Task<IReadOnlyList<TEntity>> GetAllByAsync(Expression<Func<TEntity, bool>> filterExpr, CancellationToken ct) => Task.FromResult(GetAllBy(filterExpr));

    protected Task InsertAsync(TId id, TEntity entity, CancellationToken ct)
    {
        Insert(id, entity);
        return Task.CompletedTask;
    }

    protected Task UpdateAsync(TId oldId, TEntity entity, CancellationToken ct)
    {
        Update(oldId, entity);
        return Task.CompletedTask;
    }

    protected Task UpsertAsync(TId id, TEntity entity, CancellationToken ct)
    {
        Upsert(id, entity);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Convenience methods to be used in tests to avoid awaiting
    /// </summary>
    public TEntity GetById(TId id)
        => _table.ContainsKey(id)
            ? _serializer.Clone(_table[id])
            : null;

    /// <summary>
    /// Convenience methods to be used in tests to avoid awaiting
    /// </summary>
    public TEntity GetBy(Expression<Func<TEntity, bool>> filterExpr)
    {
        var found = GetAllKeyPairsBy(filterExpr).ToArray();
        if (!found.Any()) return null;
        if (found.Length == 1) return found[0].Value;

        throw new Exception($"Expected SingleOrDefault for expression {ExpressionToString(filterExpr)} but found:{ItemsAsString(found)}");
    }

    /// <summary>
    /// Convenience methods to be used in tests to avoid awaiting
    /// </summary>
    public IReadOnlyList<TEntity> GetAll() => _table.Values.Select(e => _serializer.Clone(e)).ToArray();

    private IReadOnlyList<TEntity> GetAllBy(Expression<Func<TEntity, bool>> filterExpr)
        => GetAllKeyPairsBy(filterExpr).Select(kvp => kvp.Value).ToArray();

    private IEnumerable<KeyValuePair<TId, TEntity>> GetAllKeyPairsBy(Expression<Func<TEntity, bool>> filterExpr)
    {
        var filterFunc = filterExpr.Compile();
        return _table.Where(kvp => filterFunc(kvp.Value)).Select(x => new KeyValuePair<TId, TEntity>(x.Key, _serializer.Clone(x.Value))).ToArray();
    }

    private void Insert(TId id, TEntity entity)
    {
        if (_table.ContainsKey(id))
            throw new InvalidOperationException($"Entity of type {typeof(TEntity).Name} with id {id} already exists.{Environment.NewLine}Insert:{ItemAsString(entity)}{Environment.NewLine}Contents:{ContentsAsString()}");
        _table[id] = _serializer.Clone(entity);
    }

    private void Update(TId oldId, TEntity entity)
    {
        if (_table.ContainsKey(oldId))
        {
            var newId = GetId(entity);
            if (!oldId.Equals(newId)) _table.Remove(oldId);
            _table[newId] = _serializer.Clone(entity);
        }
    }

    private void Upsert(TId id, TEntity entity)
    {
        if (_table.ContainsKey(id))
        {
            Update(id, entity);
        }
        else
        {
            Insert(id, entity);
        }
    }

    protected TEntity Add(TEntity entity)
    {
        Insert(GetId(entity), entity);
        return _serializer.Clone(entity);
    }

    private string ContentsAsString() => ItemsAsString(_table);

    private string ItemsAsString(IEnumerable<KeyValuePair<TId, TEntity>> allItems)
    {
        var itemsAsArray = allItems.ToArray();
        return itemsAsArray.Length == 0
            ? " <empty>"
            : Environment.NewLine + string.Join(Environment.NewLine, itemsAsArray.Select(kvp => $"[{kvp.Key}]{ItemAsString(kvp.Value)}"));
    }

    private string ItemAsString(TEntity entity)
        => entity == null
            ? " <null>"
            : $" {_serializer.Serialize(entity)}";

    private static string ExpressionToString(Expression<Func<TEntity, bool>> filterExpr)
        => filterExpr.Parameters
            .Aggregate(filterExpr.Body.ToString(), (current, param) => current.Replace(param.Name, param.Type.Name))
            .Replace("AndAlso", "&&")
            .Replace("OrElse", "||");

    protected class RepositoryConfiguration
    {
        public Expression<Func<TEntity, TId>> GetIdFunc { get; set; }
    }
}
