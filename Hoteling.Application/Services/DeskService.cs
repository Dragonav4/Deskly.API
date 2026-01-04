using Hoteling.Application.Interfaces.IRepository;
using Hoteling.Application.Interfaces.IService;
using Hoteling.Domain.Entities;

namespace Hoteling.Application.Services;

public class DeskService(
    IDeskRepository desksRepository)
    : CrudService<Desk>(desksRepository), IDeskService
{
}
