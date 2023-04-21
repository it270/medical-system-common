using It270.MedicalSystem.Common.Application.Core.Interfaces;

namespace It270.MedicalSystem.Common.Application.ApplicationCore.Interfaces.General;

/// <summary>
/// CRUD service interface 
/// </summary>
/// <typeparam name="DTO">DTO class type</typeparam>
/// <typeparam name="ET">Entity identifier type</typeparam>
public interface IServiceCrud<DTO, ET> : IServiceAdd<DTO>, IServiceRead<DTO, ET>, IServiceUpdate<DTO, ET>, IServiceDelete<ET>
    where DTO : class, IDto
    where ET : notnull
{ }