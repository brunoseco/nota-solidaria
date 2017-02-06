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
    public partial class EntidadeCadastradorApp : IDisposable
    {
        private IRepository<EntidadeCadastrador> repEntidadeCadastrador;
        private ICache cache;
        private EntidadeCadastrador EntidadeCadastrador;
		public ValidationHelper ValidationHelper;

        public EntidadeCadastradorApp(string token)
        {
			this.Init(token);
			this.ValidationHelper = EntidadeCadastrador.ValidationHelper;
        }
			

		public SearchResult<EntidadeCadastradorDto> GetByFilters(EntidadeCadastradorFilter filter)
        {
			var result = default(SearchResult<EntidadeCadastradorDto>);
			using (var transaction = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
            {
				result = GetByFiltersWithCache(filter, MapperDomainToResult);
			}
			return result;
        }
				
		public SearchResult<dynamic> GetDataListCustom(EntidadeCadastradorFilter filters)
        {
			var result = default(SearchResult<dynamic>);
			using (var transaction = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
            {
				var pagingResult = this.EntidadeCadastrador.GetDataListCustomPaging(filters);
                result = new SearchResult<dynamic>
                {
                    DataList = pagingResult.ResultPaginatedData.ToList(),
                    Summary = this.EntidadeCadastrador.GetSummaryDataListCustom(pagingResult.Source)
                };
			}
			return result;
        }
		
		public dynamic GetDataCustom(EntidadeCadastradorFilter filters)
        {
			var result = default(dynamic);
			using (var transaction = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
            {
				result = this.EntidadeCadastrador.GetDataCustom(filters);
			}
			return result;
        }

		public EntidadeCadastradorDto Get(EntidadeCadastradorDto dto)
        {
			var result = default(EntidadeCadastradorDto);
			using (var transaction = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
            {
				var model =  AutoMapper.Mapper.Map<EntidadeCadastradorDto, EntidadeCadastrador>(dto);
				var data = this.EntidadeCadastrador.GetOne(model);
				result =  this.MapperDomainToDtoSpecialized(data);
			}
			return result;
        }
		
		public IEnumerable<DataItem> GetDataItem(EntidadeCadastradorFilter filters)
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

				result = this.EntidadeCadastrador.GetDataItem(filters);
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

		public int GetTotalByFilters(EntidadeCadastradorFilter filter)
        {
			var result = default(int);
			using (var transaction = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
            {
				result = this.EntidadeCadastrador.GetByFilters(filter).Count();
			}
			return result;
        }
		
		public EntidadeCadastradorDto Save(EntidadeCadastradorDtoSpecialized dto)
        {
			var result = default(EntidadeCadastradorDto);
			using (var transaction = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))
            {
				var model =  AutoMapper.Mapper.Map<EntidadeCadastradorDtoSpecialized, EntidadeCadastrador>(dto);
				var data = this.EntidadeCadastrador.Save(model);
				result =  this.MapperDomainToDtoOnSave(data, dto);
				transaction.Complete();
			}
			return result;
        }

		public EntidadeCadastradorDto SavePartial(EntidadeCadastradorDtoSpecialized dto)
        {
			var result = default(EntidadeCadastradorDto);
			using (var transaction = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))            
			{
				var model =  AutoMapper.Mapper.Map<EntidadeCadastradorDtoSpecialized, EntidadeCadastrador>(dto);
				var data = this.EntidadeCadastrador.SavePartial(model);
				result =  this.MapperDomainToDtoOnSave(data, dto);
				transaction.Complete();
			}
			return result;
        }

		public IEnumerable<EntidadeCadastradorDto> Save(IEnumerable<EntidadeCadastradorDtoSpecialized> dtos)
        {
			var result = default(IEnumerable<EntidadeCadastradorDto>);
			using (var transaction = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))
            {
				var models = AutoMapper.Mapper.Map<IEnumerable<EntidadeCadastradorDtoSpecialized>, IEnumerable<EntidadeCadastrador>>(dtos);
				var data = this.EntidadeCadastrador.Save(models);
				result =  this.MapperDomainToDtoOnSave(data, dtos);
				transaction.Complete();
			}
			return result;
        }

		public void Delete(EntidadeCadastradorDto dto)
        {
			using (var transaction = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))
            {
				var model =  AutoMapper.Mapper.Map<EntidadeCadastradorDto, EntidadeCadastrador>(dto);
				this.EntidadeCadastrador.Delete(model);
				transaction.Complete();
			}
        }
				
		private Common.Models.Summary Summary(IQueryable<EntidadeCadastrador> queryBase, PaginateResult<EntidadeCadastrador> dataList)
        {
			return this.EntidadeCadastrador.GetSummary(queryBase);
        }

		private SearchResult<EntidadeCadastradorDto> GetByFiltersWithCache(EntidadeCadastradorFilter filter, Func<EntidadeCadastradorFilter, PaginateResult<EntidadeCadastrador>, IEnumerable<EntidadeCadastradorDto>> MapperDomainToDto)
        {
            var filterKey = filter.CompositeKey();
            if (filter.ByCache)
                if (this.cache.ExistsKey(filterKey))
                    return (SearchResult<EntidadeCadastradorDto>)this.cache.Get(filterKey);

            var paginateResultOptimize = this.EntidadeCadastrador.GetByFiltersPaging(filter);
            var result = MapperDomainToDto(filter, paginateResultOptimize);
			this.EntidadeCadastrador.SummaryBehavior = filter.SummaryBehavior;
            this.EntidadeCadastrador.TotalCount = paginateResultOptimize.TotalCount;
            var summary = this.Summary(paginateResultOptimize.Source, paginateResultOptimize);

            var searchResult = new SearchResult<EntidadeCadastradorDto>
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
            var tags = this.cache.Get("EntidadeCadastrador") as List<string>;
            if (tags.IsNull()) tags = new List<string>();
            tags.Add(filterKey);
            this.cache.Add("EntidadeCadastrador", tags);
        }
		
        public void Dispose()
        {
            this.EntidadeCadastrador.Dispose();
        }
    }
}
