namespace It270.MedicalSystem.Common.Application.Core.Contracts;

#region User contracts

/// <summary>
/// User created contract
/// </summary>
/// <param name="Id">Entity id</param>
/// <param name="Name">User name</param>
/// <param name="Email">User email</param>
/// <param name="Subject">Message subject</param>
/// <param name="Content">Message content</param>
public record UserItemCreated(int Id, string Name, string Email, string Subject, string Content);

#endregion