﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace IdentityServer.Models;

public class User : IdentityUser<int>
{
}

public class Role : IdentityRole<int>
{
}

public class UserRole : IdentityUserRole<int>
{
}

public class UserLogin : IdentityUserLogin<int>
{
}

public class UserClaim : IdentityUserClaim<int>
{
}

public class RoleClaim : IdentityRoleClaim<int>
{
}

public class UserToken : IdentityUserToken<int>
{
}

public class ApplicationDbContext : IdentityDbContext<User, Role, int, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public override DatabaseFacade Database => base.Database;

    public override ChangeTracker ChangeTracker => base.ChangeTracker;

    public override IModel Model => base.Model;

    public override DbContextId ContextId => base.ContextId;

    public override DbSet<User> Users { get => base.Users; set => base.Users = value; }
    public override DbSet<UserClaim> UserClaims { get => base.UserClaims; set => base.UserClaims = value; }
    public override DbSet<UserLogin> UserLogins { get => base.UserLogins; set => base.UserLogins = value; }
    public override DbSet<UserToken> UserTokens { get => base.UserTokens; set => base.UserTokens = value; }
    public override DbSet<Role> Roles { get => base.Roles; set => base.Roles = value; }
    public override DbSet<RoleClaim> RoleClaims { get => base.RoleClaims; set => base.RoleClaims = value; }
    public override DbSet<UserRole> UserRoles { get => base.UserRoles; set => base.UserRoles = value; }

    protected override Version SchemaVersion => base.SchemaVersion;

    public override EntityEntry<TEntity> Add<TEntity>(TEntity entity)
    {
        return base.Add(entity);
    }

    public override EntityEntry Add(object entity)
    {
        return base.Add(entity);
    }

    public override ValueTask<EntityEntry<TEntity>> AddAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default)
    {
        return base.AddAsync(entity, cancellationToken);
    }

    public override ValueTask<EntityEntry> AddAsync(object entity, CancellationToken cancellationToken = default)
    {
        return base.AddAsync(entity, cancellationToken);
    }

    public override void AddRange(params object[] entities)
    {
        base.AddRange(entities);
    }

    public override void AddRange(IEnumerable<object> entities)
    {
        base.AddRange(entities);
    }

    public override Task AddRangeAsync(params object[] entities)
    {
        return base.AddRangeAsync(entities);
    }

    public override Task AddRangeAsync(IEnumerable<object> entities, CancellationToken cancellationToken = default)
    {
        return base.AddRangeAsync(entities, cancellationToken);
    }

    public override EntityEntry<TEntity> Attach<TEntity>(TEntity entity)
    {
        return base.Attach(entity);
    }

    public override EntityEntry Attach(object entity)
    {
        return base.Attach(entity);
    }

    public override void AttachRange(params object[] entities)
    {
        base.AttachRange(entities);
    }

    public override void AttachRange(IEnumerable<object> entities)
    {
        base.AttachRange(entities);
    }

    public override void Dispose()
    {
        base.Dispose();
    }

    public override ValueTask DisposeAsync()
    {
        return base.DisposeAsync();
    }

    public override EntityEntry<TEntity> Entry<TEntity>(TEntity entity)
    {
        return base.Entry(entity);
    }

    public override EntityEntry Entry(object entity)
    {
        return base.Entry(entity);
    }

    public override bool Equals(object? obj)
    {
        return base.Equals(obj);
    }

    public override object? Find([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields | DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties | DynamicallyAccessedMemberTypes.Interfaces)] Type entityType, params object?[]? keyValues)
    {
        return base.Find(entityType, keyValues);
    }

    public override TEntity? Find<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields | DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties | DynamicallyAccessedMemberTypes.Interfaces)] TEntity>(params object?[]? keyValues) where TEntity : class
    {
        return base.Find<TEntity>(keyValues);
    }

    public override ValueTask<object?> FindAsync([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields | DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties | DynamicallyAccessedMemberTypes.Interfaces)] Type entityType, params object?[]? keyValues)
    {
        return base.FindAsync(entityType, keyValues);
    }

    public override ValueTask<object?> FindAsync([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields | DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties | DynamicallyAccessedMemberTypes.Interfaces)] Type entityType, object?[]? keyValues, CancellationToken cancellationToken)
    {
        return base.FindAsync(entityType, keyValues, cancellationToken);
    }

    public override ValueTask<TEntity?> FindAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields | DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties | DynamicallyAccessedMemberTypes.Interfaces)] TEntity>(params object?[]? keyValues) where TEntity : class
    {
        return base.FindAsync<TEntity>(keyValues);
    }

    public override ValueTask<TEntity?> FindAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields | DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties | DynamicallyAccessedMemberTypes.Interfaces)] TEntity>(object?[]? keyValues, CancellationToken cancellationToken) where TEntity : class
    {
        return base.FindAsync<TEntity>(keyValues, cancellationToken);
    }

    public override IQueryable<TResult> FromExpression<TResult>(Expression<Func<IQueryable<TResult>>> expression)
    {
        return base.FromExpression(expression);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override EntityEntry<TEntity> Remove<TEntity>(TEntity entity)
    {
        return base.Remove(entity);
    }

    public override EntityEntry Remove(object entity)
    {
        return base.Remove(entity);
    }

    public override void RemoveRange(params object[] entities)
    {
        base.RemoveRange(entities);
    }

    public override void RemoveRange(IEnumerable<object> entities)
    {
        base.RemoveRange(entities);
    }

    public override int SaveChanges()
    {
        return base.SaveChanges();
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return base.SaveChangesAsync(cancellationToken);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    public override DbSet<TEntity> Set<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields | DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties | DynamicallyAccessedMemberTypes.Interfaces)] TEntity>()
    {
        return base.Set<TEntity>();
    }

    public override DbSet<TEntity> Set<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields | DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties | DynamicallyAccessedMemberTypes.Interfaces)] TEntity>(string name)
    {
        return base.Set<TEntity>(name);
    }

    public override string? ToString()
    {
        return base.ToString();
    }

    public override EntityEntry<TEntity> Update<TEntity>(TEntity entity)
    {
        return base.Update(entity);
    }

    public override EntityEntry Update(object entity)
    {
        return base.Update(entity);
    }

    public override void UpdateRange(params object[] entities)
    {
        base.UpdateRange(entities);
    }

    public override void UpdateRange(IEnumerable<object> entities)
    {
        base.UpdateRange(entities);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<User>(b => b.ToTable("Users"));
        builder.Entity<Role>(b => b.ToTable("Roles"));
        builder.Entity<UserRole>(b => b.ToTable("UserRoles"));
        builder.Entity<UserClaim>(b => b.ToTable("UserClaims"));
        builder.Entity<UserLogin>(b => b.ToTable("UserLogins"));
        builder.Entity<UserToken>(b => b.ToTable("UserTokens"));
        builder.Entity<RoleClaim>(b => b.ToTable("RoleClaims"));
    }
}