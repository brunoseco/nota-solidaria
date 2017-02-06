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
    public partial class CadastradorApp : IDisposable
    {
        private IRepository<Cadastrador> repCadastrador;
        private ICache cache;
        private Cadastrador Cadastrador;
		public ValidationHelper ValidationHelper;

        public CadastradorApp(string token)
        {
			this.Init(token);
			this.ValidationHelper = Cadastrador.ValidationHelper;
        }
			

		public SearchResult<CadastradorDto> GetByFilters(CadastradorFilter filter)
        {
			var result = default(SearchResult<CadastradorDto>);
			using (var transaction = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
            {
				result = GetByFiltersWithCache(filter, MapperDomainToResult);
			}
			return result;
        }
				
		public SearchResult<dynamic> GetDataListCustom(CadastradorFilter filters)
        {
			var result = default(SearchResult<dynamic>);
			using (var transaction = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
            {
				var pagingResult = this.Cadastrador.GetDataListCustomPaging(filters);
                result = new SearchResult<dynamic>
                {
                    DataList = pagingResult.ResultPaginatedData.ToList(),
                    Summary = this.Cadastrador.GetSummaryDataListCustom(pagingResult.Source)
                };
			}
			return result;
        }
		
		public dynamic GetDataCustom(CadastradorFilter filters)
        {
			var result = default(dynamic);
			using (var transaction = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
            {
				result = this.Cadastrador.GetDataCustom(filters);
			}
			return result;
        }

		public CadastradorDto Get(CadastradorDto dto)
        {
			var result = default(CadastradorDto);
			using (var transaction = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
            {
				var model =  AutoMapper.Mapper.Map<CadastradorDto, Cadastrador>(dto);
				var data = this.Cadastrador.GetOne(model);
				result =  this.MapperDomainToDtoSpecialized(data);
			}
			return result;
        }
		
		public IEnumerable<DataItem> GetDataItem(CadastradorFilter filters)
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

				result = this.Cadastrador.GetDataItem(filters);
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

		public int GetTotalByFilters(CadastradorFilter filter)
        {
			var result = default(int);
			using (var transaction = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
            {
				result = this.Cadastrador.GetByFilters(filter).Count();
			}
			return result;
        }
		
		public CadastradorDto Save(CadastradorDtoSpecialized dto)
        {
			var result = default(CadastradorDto);
			using (var transaction = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))
            {
				var model =  AutoMapper.Mapper.Map<CadastradorDtoSpecialized, Cadastrador>(dto);
				var data = this.Cadastrador.Save(model);
				result =  this.MapperDomainToDtoOnSave(data, dto);
				transaction.Complete();
			}
			return result;
        }

		public CadastradorDto SavePartial(CadastradorDtoSpecialized dto)
        {
			var result = default(CadastradorDto);
			using (var transaction = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))            
			{
				var model =  AutoMapper.Mapper.Map<CadastradorDtoSpecialized, Cadastrador>(dto);
				var data = this.Cadastrador.SavePartial(model);
				result =  this.MapperDomainToDtoOnSave(data, dto);
				transaction.Complete();
			}
			return result;
        }

		public IEnumerable<CadastradorDto> Save(IEnumerable<CadastradorDtoSpecialized> dtos)
        {
			var result = default(IEnumerable<CadastradorDto>);
			using (var transaction = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))
            {
				var models = AutoMapper.Mapper.Map<IEnumerable<CadastradorDtoSpecialized>, IEnumerable<Cadastrador>>(dtos);
				var data = this.Cadastrador.Save(models);
				result =  this.MapperDomainToDtoOnSave(data, dtos);
				transaction.Complete();
			}
			return result;
        }

		public void Delete(CadastradorDto dto)
        {
			using (var transaction = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))
            {
				var model =  AutoMapper.Mapper.Map<CadastradorDto, Cadastrador>(dto);
				this.Cadastrador.Delete(model);
				transaction.Complete();
			}
        }
				
		private Common.Models.Summary Summary(IQueryable<Cadastrador> queryBase, PaginateResult<Cadastrador> dataList)
        {
			return this.Cadastrador.GetSummary(queryBase);
        }

		private SearchResult<CadastradorDto> GetByFiltersWithCache(CadastradorFilter filter, Func<CadastradorFilter, PaginateResult<Cadastrador>, IEnumerable<CadastradorDto>> MapperDomainToDto)
        {
            var filterKey = filter.CompositeKey();
            if (filter.ByCache)
                if (this.cache.ExistsKey(filterKey))
                    return (SearchResult<CadastradorDto>)this.cache.Get(filterKey);

            var paginateResultOptimize = this.Cadastrador.GetByFiltersPaging(filter);
            var result = MapperDomainToDto(filter, paginateResultOptimize);
			this.Cadastrador.SummaryBehavior = filter.SummaryBehavior;
            this.Cadastrador.TotalCount = paginateResultOptimize.TotalCount;
            var summary = this.Summary(paginateResultOptimize.Source, paginateResultOptimize);

            var searchResult = new SearchResult<CadastradorDto>
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
            var tags = this.cache.Get("Cadastrador") as List<string>;
            if (tags.IsNull()) tags = new List<string>();
            tags.Add(filterKey);
            this.cache.Add("Cadastrador", tags);
        }
		
        public void Dispose()
        {
            this.Cadastrador.Dispose();
        }
    }
}
