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

namespace NFSolidaria.Core.Application
{
    public partial class EntidadeApp : IDisposable
    {
        private IRepository<Entidade> repEntidade;
        private ICache cache;
        private Entidade Entidade;
		public ValidationHelper ValidationHelper;

        public EntidadeApp(string token)
        {
			this.Init(token);
			this.ValidationHelper = Entidade.ValidationHelper;
        }
			

		public SearchResult<EntidadeDto> GetByFilters(EntidadeFilter filter)
        {
			var result = default(SearchResult<EntidadeDto>);
			using (var transaction = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
            {
				result = GetByFiltersWithCache(filter, MapperDomainToResult);
			}
			return result;
        }
				
		public SearchResult<dynamic> GetDataListCustom(EntidadeFilter filters)
        {
			var result = default(SearchResult<dynamic>);
			using (var transaction = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
            {
				var pagingResult = this.Entidade.GetDataListCustomPaging(filters);
                result = new SearchResult<dynamic>
                {
                    DataList = pagingResult.ResultPaginatedData.ToList(),
                    Summary = this.Entidade.GetSummaryDataListCustom(pagingResult.Source)
                };
			}
			return result;
        }
		
		public dynamic GetDataCustom(EntidadeFilter filters)
        {
			var result = default(dynamic);
			using (var transaction = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
            {
				result = this.Entidade.GetDataCustom(filters);
			}
			return result;
        }

		public EntidadeDto Get(EntidadeDto dto)
        {
			var result = default(EntidadeDto);
			using (var transaction = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
            {
				var model =  AutoMapper.Mapper.Map<EntidadeDto, Entidade>(dto);
				var data = this.Entidade.GetOne(model);
				result =  this.MapperDomainToDtoSpecialized(data);
			}
			return result;
        }
		
		public IEnumerable<DataItem> GetDataItem(EntidadeFilter filters)
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

				result = this.Entidade.GetDataItem(filters);
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

		public int GetTotalByFilters(EntidadeFilter filter)
        {
			var result = default(int);
			using (var transaction = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
            {
				result = this.Entidade.GetByFilters(filter).Count();
			}
			return result;
        }
		
		public EntidadeDto Save(EntidadeDtoSpecialized dto)
        {
			var result = default(EntidadeDto);
			using (var transaction = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))
            {
				var model =  AutoMapper.Mapper.Map<EntidadeDtoSpecialized, Entidade>(dto);
				var data = this.Entidade.Save(model);
				result =  this.MapperDomainToDtoOnSave(data, dto);
				transaction.Complete();
			}
			return result;
        }

		public EntidadeDto SavePartial(EntidadeDtoSpecialized dto)
        {
			var result = default(EntidadeDto);
			using (var transaction = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))            
			{
				var model =  AutoMapper.Mapper.Map<EntidadeDtoSpecialized, Entidade>(dto);
				var data = this.Entidade.SavePartial(model);
				result =  this.MapperDomainToDtoOnSave(data, dto);
				transaction.Complete();
			}
			return result;
        }

		public IEnumerable<EntidadeDto> Save(IEnumerable<EntidadeDtoSpecialized> dtos)
        {
			var result = default(IEnumerable<EntidadeDto>);
			using (var transaction = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))
            {
				var models = AutoMapper.Mapper.Map<IEnumerable<EntidadeDtoSpecialized>, IEnumerable<Entidade>>(dtos);
				var data = this.Entidade.Save(models);
				result =  this.MapperDomainToDtoOnSave(data, dtos);
				transaction.Complete();
			}
			return result;
        }

		public void Delete(EntidadeDto dto)
        {
			using (var transaction = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))
            {
				var model =  AutoMapper.Mapper.Map<EntidadeDto, Entidade>(dto);
				this.Entidade.Delete(model);
				transaction.Complete();
			}
        }
				
		private Common.Models.Summary Summary(IQueryable<Entidade> queryBase, PaginateResult<Entidade> dataList)
        {
			return this.Entidade.GetSummary(queryBase);
        }

		private SearchResult<EntidadeDto> GetByFiltersWithCache(EntidadeFilter filter, Func<EntidadeFilter, PaginateResult<Entidade>, IEnumerable<EntidadeDto>> MapperDomainToDto)
        {
            var filterKey = filter.CompositeKey();
            if (filter.ByCache)
                if (this.cache.ExistsKey(filterKey))
                    return (SearchResult<EntidadeDto>)this.cache.Get(filterKey);

            var paginateResultOptimize = this.Entidade.GetByFiltersPaging(filter);
            var result = MapperDomainToDto(filter, paginateResultOptimize);
			this.Entidade.SummaryBehavior = filter.SummaryBehavior;
            this.Entidade.TotalCount = paginateResultOptimize.TotalCount;
            var summary = this.Summary(paginateResultOptimize.Source, paginateResultOptimize);

            var searchResult = new SearchResult<EntidadeDto>
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
            var tags = this.cache.Get("Entidade") as List<string>;
            if (tags.IsNull()) tags = new List<string>();
            tags.Add(filterKey);
            this.cache.Add("Entidade", tags);
        }
		
        public void Dispose()
        {
            this.Entidade.Dispose();
        }
    }
}
