using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Omu.ValueInjecter;
using Singularity;
using Singularity.Api;
using Singularity.DataService;
using Singularity.Injection.Ignores;

namespace Singularity.DataInjectionService
{
	public abstract class InjectedEntityAssembler<TEntity, TDto, TRequestDto> : EntityAssembler<TEntity, TDto, TRequestDto>
		where TDto : DtoBase, new()
		where TRequestDto : DtoBase, new()
		where TEntity : class, new()
	{
		protected override void InjectDtoFromEntity(TDto dto, TEntity entity)
		{
			dto.InjectFrom<IgnoreNulls>(entity);
		}

		protected sealed override void InjectEntityFromDto(TEntity entity, TRequestDto requestDto)
		{
			// Ignore the primary key coming in from the DTO - it'll most likely be Guid.Empty and the new domain entity will have a real GUID.
			String pkBaseName = typeof(TDto).Name.RemoveSuffix("Dto");
			IgnoreNulls ignoreNulls = new IgnoreNulls(new[] { pkBaseName + "ID", pkBaseName + "Id" });
			entity.InjectFrom(ignoreNulls, requestDto);
		}
	}
}
