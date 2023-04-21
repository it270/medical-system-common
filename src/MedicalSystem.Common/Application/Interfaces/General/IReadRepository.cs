using Ardalis.Specification;
using It270.MedicalSystem.Common.Core.Interfaces;

namespace It270.MedicalSystem.Common.Application.Interfaces.General;

/// <summary>
/// Read-only repository interface
/// </summary>
/// <typeparam name="T">Entity class type</typeparam>
public interface IReadRepository<T> : IReadRepositoryBase<T> where T : class, IAggregateRoot
{ }