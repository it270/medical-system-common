using Ardalis.Specification;
using It270.MedicalSystem.Common.Application.Core.Interfaces;

namespace It270.MedicalSystem.Common.Application.ApplicationCore.Interfaces.General;

/// <summary>
/// General repository interface
/// </summary>
/// <typeparam name="T">Entity class type</typeparam>
public interface IRepository<T> : IRepositoryBase<T> where T : class, IAggregateRoot
{ }