namespace It270.MedicalSystem.Common.Core.Entities;

public abstract class BaseEntity<T>
    where T : notnull
{
    public virtual T Id { get; protected set; }
}