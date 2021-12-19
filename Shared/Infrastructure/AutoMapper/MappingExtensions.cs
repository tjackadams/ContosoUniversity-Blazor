﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using ContosoUniversity.Shared.Infrastructure.Collections;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.Shared.Infrastructure.AutoMapper
{
    public static class MappingExtensions
    {
        public static Task<PaginatedList<TDestination>> PaginatedListAsync<TDestination>(
            this IQueryable<TDestination> queryable, int pageNumber, int pageSize)
        {
            return PaginatedList<TDestination>.CreateAsync(queryable, pageNumber, pageSize);
        }

        public static Task<List<TDestination>> ProjectToListAsync<TDestination>(this IQueryable queryable,
            IConfigurationProvider configuration)
        {
            return queryable.ProjectTo<TDestination>(configuration).ToListAsync();
        }

        public static Task<TDestination[]> ProjectToArrayAsync<TDestination>(this IQueryable queryable,
            IConfigurationProvider configuration)
        {
            return queryable.ProjectTo<TDestination>(configuration).ToArrayAsync();
        }
    }
}