namespace It270.MedicalSystem.Common.Application.Core.Contracts;

#region Message contracts

/// <summary>
/// General message contract (email)
/// </summary>
/// <param name="Name">Receiver name</param>
/// <param name="Email">Receiver email</param>
/// <param name="Subject">Message subject</param>
/// <param name="Content">Message content</param>
public record GeneralMessage(string Name, string Email, string Subject, string Content);

#endregion