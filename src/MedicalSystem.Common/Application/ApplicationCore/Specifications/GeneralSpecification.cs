using Ardalis.Specification;
using It270.MedicalSystem.Common.Application.Core.Interfaces;

namespace It270.MedicalSystem.Common.Application.ApplicationCore.Specifications;

/// <summary>
/// General Ardalis Specification
/// </summary>
/// <typeparam name="T">Entity identifier type</typeparam>
/// <typeparam name="E">Entity type</typeparam>
public abstract class GeneralSpecification<T, E> : Specification<E>
where T : notnull
where E : class, IAggregateRoot
{
    public GeneralSpecification()
    { }

    /// <summary>
    /// Constructor for one element query
    /// </summary>
    /// <param name="id">Element identifier</param>
    public GeneralSpecification(T id)
    { }

    /// <summary>
    /// Constructor for a paginated query
    /// </summary>
    /// <param name="skip">Page number</param>
    /// <param name="take">Page size</param>
    public GeneralSpecification(int skip, int take)
    { }
}