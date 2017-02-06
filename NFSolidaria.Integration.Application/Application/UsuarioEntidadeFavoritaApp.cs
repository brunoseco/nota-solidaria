using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Domain.Interfaces;
using Common.Infrastructure.Cache;
using System.Collections.Generic;
using Common.Models;
using NFSolidaria.Core.Filters;
using Common.Interfaces;
using NFSolidaria.Core.Dto;
using NFSolidaria.Core.Domain;
using Common.Domain;
using System.Transactions;
using Common.Infrastructure.Log;

namespace NFSolidaria.Integration.Application
{
    public partial class UsuarioEntidadeFavoritaApp : IDisposable
    {
        private IRepository<UsuarioEntidadeFavorita> repUsuarioEntidadeFavorita;
        private ICache cache;
        private UsuarioEntidadeFavorita UsuarioEntidadeFavorita;
		public ValidationHelper ValidationHelper;

        public UsuarioEntidadeFavoritaApp(string token)
        {
			this.Init(token);
			this.ValidationHelper = UsuarioEntidadeFavorita.ValidationHelper;
        }
			

		public SearchResult<UsuarioEntidadeFavoritaDto> GetByFilters(UsuarioEntidadeFavoritaFilter filter)
        {
			var result = default(SearchResult<UsuarioEntidadeFavoritaDto>);
			using (var transaction = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
            {
				result = GetByFiltersWithCache(filter, MapperDomainToResult);
			}
			return result;
        }
				
		public SearchResult<dynamic> GetDataListCustom(UsuarioEntidadeFavoritaFilter filters)
        {
			var result = default(SearchResult<dynamic>);
			using (var transaction = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
            {
				var pagingResult = this.UsuarioEntidadeFavorita.GetDataListCustomPaging(filters);
                result = new SearchResult<dynamic>
                {
                    DataList = pagingResult.ResultPaginatedData.ToList(),
                    Summary = this.UsuarioEntidadeFavorita.GetSummaryDataListCustom(pagingResult.Source)
                };
			}
			return result;
        }
		
		public dynamic GetDataCustom(UsuarioEntidadeFavoritaFilter filters)
        {
			var result = default(dynamic);
			using (var transaction = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
            {
				result = this.UsuarioEntidadeFavorita.GetDataCustom(filters);
			}
			return result;
        }

		public UsuarioEntidadeFavoritaDto Get(UsuarioEntidadeFavoritaDto dto)
        {
			var result = default(UsuarioEntidadeFavoritaDto);
			using (var transaction = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
            {
				var model =  AutoMapper.Mapper.Map<UsuarioEntidadeFavoritaDto, UsuarioEntidadeFavorita>(dto);
				var data = this.UsuarioEntidadeFavorita.GetOne(model);
				result =  this.MapperDomainToDtoSpecialized(data);
			}
			return result;
        }
		
		public IEnumerable<DataItem> GetDataItem(UsuarioEntidadeFavoritaFilter filters)
		{
			var result = default(IEnumerable<DataItem>);
			using (var transaction = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
            {
				var filterKey = filters.CompositeKey(); 

				if (filters.ByCache)
				{
					if (this.cache.ExistsKey(filterKey))
					{
						var resultCache = (IEnumerable<DataItemCache>)this.cache.Get(filterKey);
						return resultCache.Select(_ => new DataItem
						{
							Id = _.Id,
							Name = _.Name
						});
					}
				}

				result = this.UsuarioEntidadeFavorita.GetDataItem(filters);
				if (filters.ByCache)
				{
					if (!result.IsAny()) return result;
					this.cache.Add(filterKey, result.Select(_ => new DataItemCache
					{
						Id = _.Id,
						Name = _.Name
					}).ToList());
					this.AddTagCache(filterKey);
					FactoryLog.GetInstace().Debug(string.Format("DataItem: {0} - Key: {1} - CacheExpiresTime:{2}", filters.CacheGroup, filterKey, filters.CacheExpiresTime));
				}
			}
			return result;
		}

		public int GetTotalByFilters(UsuarioEntidadeFavoritaFilter filter)
        {
			var result = default(int);
			using (var transaction = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
            {
				result = this.UsuarioEntidadeFavorita.GetByFilters(filter).Count();
			}
			return result;
        }
		
		public UsuarioEntidadeFavoritaDto Save(UsuarioEntidadeFavoritaDtoSpecialized dto)
        {
			var result = default(UsuarioEntidadeFavoritaDto);
			using (var transaction = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))
            {
				var model =  AutoMapper.Mapper.Map<UsuarioEntidadeFavoritaDtoSpecialized, UsuarioEntidadeFavorita>(dto);
				var data = this.UsuarioEntidadeFavorita.Save(model);
				result =  this.MapperDomainToDtoOnSave(data, dto);
				transaction.Complete();
			}
			return result;
        }

		public UsuarioEntidadeFavoritaDto SavePartial(UsuarioEntidadeFavoritaDtoSpecialized dto)
        {
			var result = default(UsuarioEntidadeFavoritaDto);
			using (var transaction = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))            
			{
				var model =  AutoMapper.Mapper.Map<UsuarioEntidadeFavoritaDtoSpecialized, UsuarioEntidadeFavorita>(dto);
				var data = this.UsuarioEntidadeFavorita.SavePartial(model);
				result =  this.MapperDomainToDtoOnSave(data, dto);
				transaction.Complete();
			}
			return result;
        }

		public IEnumerable<UsuarioEntidadeFavoritaDto> Save(IEnumerable<UsuarioEntidadeFavoritaDtoSpecialized> dtos)
        {
			var result = default(IEnumerable<UsuarioEntidadeFavoritaDto>);
			using (var transaction = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))
            {
				var models = AutoMapper.Mapper.Map<IEnumerable<UsuarioEntidadeFavoritaDtoSpecialized>, IEnumerable<UsuarioEntidadeFavorita>>(dtos);
				var data = this.UsuarioEntidadeFavorita.Save(models);
				result =  this.MapperDomainToDtoOnSave(data, dtos);
				transaction.Complete();
			}
			return result;
        }

		public void Delete(UsuarioEntidadeFavoritaDto dto)
        {
			using (var transaction = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))
            {
				var model =  AutoMapper.Mapper.Map<UsuarioEntidadeFavoritaDto, UsuarioEntidadeFavorita>(dto);
				this.UsuarioEntidadeFavorita.Delete(model);
				transaction.Complete();
			}
        }
				
		private Common.Models.Summary Summary(IQueryable<UsuarioEntidadeFavorita> queryBase, PaginateResult<UsuarioEntidadeFavorita> dataList)
        {
			return this.UsuarioEntidadeFavorita.GetSummary(queryBase);
        }

		private SearchResult<UsuarioEntidadeFavoritaDto> GetByFiltersWithCache(UsuarioEntidadeFavoritaFilter filter, Func<UsuarioEntidadeFavoritaFilter, PaginateResult<UsuarioEntidadeFavorita>, IEnumerable<UsuarioEntidadeFavoritaDto>> MapperDomainToDto)
        {
            var filterKey = filter.CompositeKey();
            if (filter.ByCache)
                if (this.cache.ExistsKey(filterKey))
                    return (SearchResult<UsuarioEntidadeFavoritaDto>)this.cache.Get(filterKey);

            var paginateResultOptimize = this.UsuarioEntidadeFavorita.GetByFiltersPaging(filter);
            var result = MapperDomainToDto(filter, paginateResultOptimize);
			this.UsuarioEntidadeFavorita.SummaryBehavior = filter.SummaryBehavior;
            this.UsuarioEntidadeFavorita.TotalCount = paginateResultOptimize.TotalCount;
            var summary = this.Summary(paginateResultOptimize.Source, paginateResultOptimize);

            var searchResult = new SearchResult<UsuarioEntidadeFavoritaDto>
            {
                DataList = result,
                Summary = summary,
            };

            if (filter.ByCache)
			{
				if (!searchResult.DataList.IsAny()) return searchResult;
                this.cache.Add(filterKey, searchResult);
				this.AddTagCache(filterKey);
			}

            return searchResult;
        }
		
        private void AddTagCache(string filterKey)
        {
            var tags = this.cache.Get("UsuarioEntidadeFavorita") as List<string>;
            if (tags.IsNull()) tags = new List<string>();
            tags.Add(filterKey);
            this.cache.Add("UsuarioEntidadeFavorita", tags);
        }
		
        public void Dispose()
        {
            this.UsuarioEntidadeFavorita.Dispose();
        }
    }
}
