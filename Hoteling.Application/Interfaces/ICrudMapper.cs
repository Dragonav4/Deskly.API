using System.Security.Claims;
using Hoteling.Application.Views.Common;
using Hoteling.Domain.Interfaces;

namespace Hoteling.Application.Interfaces;

public interface ICrudMapper<TDomain, in TCreateDto, TViewDto> where TViewDto : TCreateDto, IHasId
{
    Task<ActionListView<TViewDto>> MapDomainModelsToListView(IEnumerable<TDomain> domains, int totalCount, ClaimsPrincipal user);
    TDomain MapCreateDtoToDomain(TCreateDto createDto);
    TDomain MapViewToDomain(Guid id, TViewDto viewDto);
    Task<TViewDto> MapDomainToView(TDomain domain, ClaimsPrincipal user);
}
