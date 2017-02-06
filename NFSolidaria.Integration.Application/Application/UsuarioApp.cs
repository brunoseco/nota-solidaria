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
    public partial class UsuarioApp : IDisposable
    {
        private IRepository<Usuario> repUsuario;
        private ICache cache;
        private Usuario Usuario;
		public ValidationHelper ValidationHelper;

        public UsuarioApp(string token)
        {
			this.Init(token);
			this.ValidationHelper = Usuario.ValidationHelper;
        }
			

		public SearchResult<UsuarioDto> GetByFilters(UsuarioFilter filter)
        {
			var result = default(SearchResult<UsuarioDto>);
			using (var transaction = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
            {
				result = GetByFiltersWithCache(filter, MapperDomainToResult);
			}
			return result;
        }
				
		public SearchResult<dynamic> GetDataListCustom(UsuarioFilter filters)
        {
			var result = default(SearchResult<dynamic>);
			using (var transaction = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
            {
				var pagingResult = this.Usuario.GetDataListCustomPaging(filters);
                result = new SearchResult<dynamic>
                {
                    DataList = pagingResult.ResultPaginatedData.ToList(),
                    Summary = this.Usuario.GetSummaryDataListCustom(pagingResult.Source)
                };
			}
			return result;
        }
		
		public dynamic GetDataCustom(UsuarioFilter filters)
        {
			var result = default(dynamic);
			using (var transaction = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
            {
				result = this.Usuario.GetDataCustom(filters);
			}
			return result;
        }

		public UsuarioDto Get(UsuarioDto dto)
        {
			var result = default(UsuarioDto);
			using (var transaction = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
            {
				var model =  AutoMapper.Mapper.Map<UsuarioDto, Usuario>(dto);
				var data = this.Usuario.GetOne(model);
				result =  this.MapperDomainToDtoSpecialized(data);
			}
			return result;
        }
		
		public IEnumerable<DataItem> GetDataItem(UsuarioFilter filters)
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

				result = this.Usuario.GetDataItem(filters);
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

		public int GetTotalByFilters(UsuarioFilter filter)
        {
			var result = default(int);
			using (var transaction = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
            {
				result = this.Usuario.GetByFilters(filter).Count();
			}
			return result;
        }
		
		public UsuarioDto Save(UsuarioDtoSpecialized dto)
        {
			var result = default(UsuarioDto);
			using (var transaction = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))
            {
				var model =  AutoMapper.Mapper.Map<UsuarioDtoSpecialized, Usuario>(dto);
				var data = this.Usuario.Save(model);
				result =  this.MapperDomainToDtoOnSave(data, dto);
				transaction.Complete();
			}
			return result;
        }

		public UsuarioDto SavePartial(UsuarioDtoSpecialized dto)
        {
			var result = default(UsuarioDto);
			using (var transaction = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))            
			{
				var model =  AutoMapper.Mapper.Map<UsuarioDtoSpecialized, Usuario>(dto);
				var data = this.Usuario.SavePartial(model);
				result =  this.MapperDomainToDtoOnSave(data, dto);
				transaction.Complete();
			}
			return result;
        }

		public IEnumerable<UsuarioDto> Save(IEnumerable<UsuarioDtoSpecialized> dtos)
        {
			var result = default(IEnumerable<UsuarioDto>);
			using (var transaction = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))
            {
				var models = AutoMapper.Mapper.Map<IEnumerable<UsuarioDtoSpecialized>, IEnumerable<Usuario>>(dtos);
				var data = this.Usuario.Save(models);
				result =  this.MapperDomainToDtoOnSave(data, dtos);
				transaction.Complete();
			}
			return result;
        }

		public void Delete(UsuarioDto dto)
        {
			using (var transaction = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))
            {
				var model =  AutoMapper.Mapper.Map<UsuarioDto, Usuario>(dto);
				this.Usuario.Delete(model);
				transaction.Complete();
			}
        }
				
		private Common.Models.Summary Summary(IQueryable<Usuario> queryBase, PaginateResult<Usuario> dataList)
        {
			return this.Usuario.GetSummary(queryBase);
        }

		private SearchResult<UsuarioDto> GetByFiltersWithCache(UsuarioFilter filter, Func<UsuarioFilter, PaginateResult<Usuario>, IEnumerable<UsuarioDto>> MapperDomainToDto)
        {
            var filterKey = filter.CompositeKey();
            if (filter.ByCache)
                if (this.cache.ExistsKey(filterKey))
                    return (SearchResult<UsuarioDto>)this.cache.Get(filterKey);

            var paginateResultOptimize = this.Usuario.GetByFiltersPaging(filter);
            var result = MapperDomainToDto(filter, paginateResultOptimize);
			this.Usuario.SummaryBehavior = filter.SummaryBehavior;
            this.Usuario.TotalCount = paginateResultOptimize.TotalCount;
            var summary = this.Summary(paginateResultOptimize.Source, paginateResultOptimize);

            var searchResult = new SearchResult<UsuarioDto>
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
            var tags = this.cache.Get("Usuario") as List<string>;
            if (tags.IsNull()) tags = new List<string>();
            tags.Add(filterKey);
            this.cache.Add("Usuario", tags);
        }
		
        public void Dispose()
        {
            this.Usuario.Dispose();
        }
    }
}
