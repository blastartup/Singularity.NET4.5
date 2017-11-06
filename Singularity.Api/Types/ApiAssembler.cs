using System;
using System.Collections.Generic;

namespace Singularity.Api
{
	public abstract class ApiAssembler
	{
	}

	public abstract class EntityAssembler<TEntity, TDto, TRequestDto> : ApiAssembler
		where TDto : DtoBase, new()
		where TRequestDto : DtoBase, new()
		where TEntity : class, new()
	{
		/// <summary>
		/// Populates a Data Transfer Object response with values from its associated entity.
		/// </summary>
		/// <param name="entity">Domain entity (database object)</param>
		/// <returns>Data Transfer Object injected with values from the given entity.</returns>
		protected TDto PopulateDto(TEntity entity)
		{
			return EntityToDto(entity);
		}

		/// <summary>
		/// Populates a collection Data Transfer Object with values from its associated entity collection.
		/// </summary>
		/// <param name="entity">Collection of domain entities (database objects)</param>
		/// <returns>List of Data Transfer Object injected with values from the given entity collection.</returns>
		protected List<TDto> PopulateDtos(IEnumerable<TEntity> entityList)
		{
			return EntitiesToDtos(entityList);
		}

		/// <summary>
		/// Creates a new Entity with values from a Data Transfer Object request.
		/// </summary>
		/// <param name="requestDto">Data Transfer Object</param>
		/// <returns>Domain entity (database object) injected with values from the given DTO.</returns>
		protected TEntity CreateEntity(TRequestDto requestDto = null)
		{
			return DtoToNewEntity(requestDto);
		}

		/// <summary>
		/// Updates an existing Entity with values from a Data Transfer Object request.
		/// </summary>
		/// <param name="requestDto">Data Transfer Object</param>
		/// <returns>Domain entity (database object) injected with values from the given DTO.</returns>
		protected void UpdateEntity(TEntity entity, TRequestDto requestDto)
		{
			DtoToExistingEntity(requestDto, entity);
		}

		private TDto EntityToDto(TEntity entity)
		{
			if (entity.IsEmpty())
			{
				return null;
			}

			TDto dto = new TDto();
			InjectDtoFromEntity(dto, entity);
			return dto;
		}

		private TEntity DtoToNewEntity(TRequestDto requestDto)
		{
			TEntity entity = new TEntity();
			if (requestDto.IsEmpty())
			{
				return entity;
			}

			InjectNewEntityFromDto(entity, requestDto);
			return entity;
		}

		private void DtoToExistingEntity(TRequestDto requestDto, TEntity entity)
		{
			if (requestDto.IsEmpty())
			{
				return;
			}

			InjectExistingEntityFromDto(entity, requestDto);
		}

		protected abstract void InjectDtoFromEntity(TDto dto, TEntity entity);

		protected virtual void InjectNewEntityFromDto(TEntity entity, TRequestDto requestDto)
		{
			InjectEntityFromDto(entity, requestDto);
		}

		protected virtual void InjectExistingEntityFromDto(TEntity entity, TRequestDto requestDto)
		{
			InjectEntityFromDto(entity, requestDto);
		}

		protected abstract void InjectEntityFromDto(TEntity entity, TRequestDto requestDto);

		private List<TDto> EntitiesToDtos(IEnumerable<TEntity> entityList)
		{
			List<TDto> dtoList = Activator.CreateInstance<List<TDto>>();
			foreach (TEntity entity in entityList)
			{
				dtoList.Add(EntityToDto(entity));
			}
			return dtoList;
		}

		//protected Action<TEntity> PreAssemble { get; set; }

		//public void AddPostAssembly(Action<TDto, TEntity> postAssembly)
		//{
		//	PostAssemblies.Add(postAssembly);
		//}

		//private List<Action<TDto, TEntity>> PostAssemblies
		//{
		//	get { return _postAssemblies ?? (_postAssemblies = new List<Action<TDto, TEntity>>()); }
		//}
		//private List<Action<TDto, TEntity>> _postAssemblies;
	}
}
