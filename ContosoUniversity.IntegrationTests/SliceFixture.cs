﻿using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ContosoUniversity.Domain.SeedWork;
using ContosoUniversity.Infrastructure;
using ContosoUniversity.Server;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Respawn;

namespace ContosoUniversity.IntegrationTests
{
    public class SliceFixture
    {
        private static readonly Checkpoint _checkpoint;
        private static readonly IConfigurationRoot _configuration;
        private static readonly IServiceScopeFactory _scopeFactory;

        private static int _courseNumber = 1;

        static SliceFixture()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .AddEnvironmentVariables();
            _configuration = builder.Build();

            var startup = new Startup(_configuration);
            var services = new ServiceCollection();
            services.AddLogging();
            startup.ConfigureServices(services);
            var provider = services.BuildServiceProvider();
            _scopeFactory = provider.GetService<IServiceScopeFactory>();
            _checkpoint = new Checkpoint();

            using(var context = provider.GetRequiredService<SchoolContext>())
            {
                context.Database.EnsureCreated();
            }
        }

        public static Task ResetCheckpoint()
        {
            return _checkpoint.Reset(_configuration["ConnectionString"]);
        }

        public static async Task ExecuteScopeAsync(Func<IServiceProvider, Task> action)
        {
            using var scope = _scopeFactory.CreateScope();

            await action(scope.ServiceProvider).ConfigureAwait(false);
            }

        public static async Task<T> ExecuteScopeAsync<T>(Func<IServiceProvider, Task<T>> action)
        {
            using var scope = _scopeFactory.CreateScope();

                var result = await action(scope.ServiceProvider).ConfigureAwait(false);
                
                return result;
            
        }

        public static Task ExecuteDbContextAsync(Func<SchoolContext, Task> action)
        {
            return ExecuteScopeAsync(sp => action(sp.GetService<SchoolContext>()));
        }

        public static Task ExecuteDbContextAsync(Func<SchoolContext, ValueTask> action)
        {
            return ExecuteScopeAsync(sp => action(sp.GetService<SchoolContext>()).AsTask());
        }

        public static Task ExecuteDbContextAsync(Func<SchoolContext, IMediator, Task> action)
        {
            return ExecuteScopeAsync(sp => action(sp.GetService<SchoolContext>(), sp.GetService<IMediator>()));
        }

        public static Task<T> ExecuteDbContextAsync<T>(Func<SchoolContext, Task<T>> action)
        {
            return ExecuteScopeAsync(sp => action(sp.GetService<SchoolContext>()));
        }

        public static Task<T> ExecuteDbContextAsync<T>(Func<SchoolContext, ValueTask<T>> action)
        {
            return ExecuteScopeAsync(sp => action(sp.GetService<SchoolContext>()).AsTask());
        }

        public static Task<T> ExecuteDbContextAsync<T>(Func<SchoolContext, IMediator, Task<T>> action)
        {
            return ExecuteScopeAsync(sp => action(sp.GetService<SchoolContext>(), sp.GetService<IMediator>()));
        }

        public static Task InsertAsync<T>(params T[] entities) where T : class
        {
            return ExecuteDbContextAsync(db =>
            {
                foreach (var entity in entities)
                {
                    db.Set<T>().Add(entity);
                }

                return db.SaveChangesAsync();
            });
        }

        public static Task InsertAsync<TEntity>(TEntity entity) where TEntity : class
        {
            return ExecuteDbContextAsync(db =>
            {
                db.Set<TEntity>().Add(entity);

                return db.SaveChangesAsync();
            });
        }

        public static Task InsertAsync<TEntity, TEntity2>(TEntity entity, TEntity2 entity2)
            where TEntity : class
            where TEntity2 : class
        {
            return ExecuteDbContextAsync(db =>
            {
                db.Set<TEntity>().Add(entity);
                db.Set<TEntity2>().Add(entity2);

                return db.SaveChangesAsync();
            });
        }

        public static Task InsertAsync<TEntity, TEntity2, TEntity3>(TEntity entity, TEntity2 entity2, TEntity3 entity3)
            where TEntity : class
            where TEntity2 : class
            where TEntity3 : class
        {
            return ExecuteDbContextAsync(db =>
            {
                db.Set<TEntity>().Add(entity);
                db.Set<TEntity2>().Add(entity2);
                db.Set<TEntity3>().Add(entity3);

                return db.SaveChangesAsync();
            });
        }

        public static Task InsertAsync<TEntity, TEntity2, TEntity3, TEntity4>(TEntity entity, TEntity2 entity2,
            TEntity3 entity3, TEntity4 entity4)
            where TEntity : class
            where TEntity2 : class
            where TEntity3 : class
            where TEntity4 : class
        {
            return ExecuteDbContextAsync(db =>
            {
                db.Set<TEntity>().Add(entity);
                db.Set<TEntity2>().Add(entity2);
                db.Set<TEntity3>().Add(entity3);
                db.Set<TEntity4>().Add(entity4);

                return db.SaveChangesAsync();
            });
        }

        public static Task<T> FindAsync<T>(int id)
            where T : Entity
        {
            return ExecuteDbContextAsync(db => db.Set<T>().FindAsync(id).AsTask());
        }

        public static Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
        {
            return ExecuteScopeAsync(sp =>
            {
                var mediator = sp.GetService<IMediator>();

                return mediator.Send(request);
            });
        }

        public static Task SendAsync(IRequest request)
        {
            return ExecuteScopeAsync(sp =>
            {
                var mediator = sp.GetService<IMediator>();

                return mediator.Send(request);
            });
        }

        public static int NextCourseNumber()
        {
            return Interlocked.Increment(ref _courseNumber);
        }
    }
}