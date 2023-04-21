using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using It270.MedicalSystem.Common.Application.Interfaces.General;
using It270.MedicalSystem.Common.Application.Specifications;
using It270.MedicalSystem.Common.Core.Helpers.General;
using It270.MedicalSystem.Common.Core.Interfaces;
using Serilog;

namespace It270.MedicalSystem.Common.Application.Services;

/// <summary>
/// General service for only read functions
/// </summary>
/// <typeparam name="E">Entity type</typeparam>
/// <typeparam name="DTO">DTO class type</typeparam>
/// <typeparam name="ET">Entity identifier type</typeparam>
/// <typeparam name="GS">General Specification type</typeparam>
public abstract class ServiceRead<E, DTO, ET, GS> : IServiceRead<DTO, ET>
    where DTO : class, IDto
    where ET : notnull
    where E : class, IAggregateRoot
    where GS : GeneralSpecification<ET, E>
{
    protected readonly IRepository<E> _entityRepository;
    protected readonly IMapper _mapper;
    protected readonly ILogger _logger;

    public ServiceRead(IRepository<E> entityRepository,
        IMapper mapper,
        ILogger logger)
    {
        _entityRepository = entityRepository;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// Get element
    /// </summary>
    /// <param name="id">Element identifier</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Process result</returns>
    public virtual async Task<CustomWebResponse> Get(ET id, CancellationToken ct = default)
    {
        var specification = (GS)Activator.CreateInstance(typeof(GS), new object[] { id });
        var dataEntity = await _entityRepository.FirstOrDefaultAsync(specification, ct);

        if (dataEntity != null)
        {
            var dataDto = _mapper.Map<DTO>(dataEntity);
            return new CustomWebResponse()
            {
                ResponseBody = dataDto
            };
        }
        else
        {
            return new CustomWebResponse(true)
            {
                StatusCode = HttpStatusCode.NotFound,
                Message = "Not found",
            };
        }
    }

    /// <summary>
    /// Get all elements
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Process result</returns>
    public virtual async Task<CustomWebResponse> GetAll(CancellationToken ct = default)
    {
        var dataListEntity = await _entityRepository.ListAsync(ct);
        var dataListDto = _mapper.Map<List<DTO>>(dataListEntity);

        return new CustomWebResponse()
        {
            ResponseBody = dataListDto
        };
    }

    /// <summary>
    /// Get elements list (paginated)
    /// </summary>
    /// <param name="skip">Page</param>
    /// <param name="take">Page size</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Process result</returns>
    public virtual async Task<CustomWebResponse> GetList(int skip, int take, CancellationToken ct = default)
    {
        var specification = (GS)Activator.CreateInstance(typeof(GS), new object[] { skip, take });
        var dataListEntity = await _entityRepository.ListAsync(specification, ct);
        var dataListDto = _mapper.Map<List<DTO>>(dataListEntity);

        return new CustomWebResponse()
        {
            ResponseBody = dataListDto
        };
    }
}