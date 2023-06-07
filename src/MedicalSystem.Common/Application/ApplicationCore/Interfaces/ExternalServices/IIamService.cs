using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using It270.MedicalSystem.Common.Application.Core.Helpers.UserNS;

namespace It270.MedicalSystem.Common.Application.ApplicationCore.Interfaces.ExternalServices;

/// <summary>
/// IAM service interface
/// </summary>
public interface IIamService
{
    /// <summary>
    /// Asynchronous IAM initialization
    /// </summary>
    Task Init();

    #region User functions

    /// <summary>
    /// Get users (paginated)
    /// </summary>
    /// <param name="code">Page code (token)</param>
    /// <param name="take">Page size</param>
    /// <returns>List of users data</returns>
    Task<List<IamUser>> GetUsers(string code, int take);

    /// <summary>
    /// Get user
    /// </summary>
    /// <param name="userName">User name</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Process result</returns>
    Task<IamUser> GetUser(string userName, CancellationToken ct = default);

    /// <summary>
    /// Add user
    /// </summary>
    /// <param name="user">User data</param>
    /// <param name="key">User password</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Process result</returns>
    Task<IamUser> AddUser(IamUser user, string key = null, CancellationToken ct = default);

    /// <summary>
    /// Add role to user
    /// </summary>
    /// <param name="userName">User name</param>
    /// <param name="role">Role name</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Process result</returns>
    Task<bool> AddUserRole(string userName, string role, CancellationToken ct = default);

    /// <summary>
    /// Remove user role
    /// </summary>
    /// <param name="userName">User name</param>
    /// <param name="role">Role name</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Process result</returns>
    Task<bool> RemoveUserRole(string userName, string role, CancellationToken ct = default);

    /// <summary>
    /// Change user password
    /// </summary>
    /// <param name="userName">User name</param>
    /// <param name="key">New user password</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Process result</returns>
    Task<bool> SetUserPassword(string userName, string key, CancellationToken ct = default);

    /// <summary>
    /// Reset user password
    /// </summary>
    /// <param name="userName">User name</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Process result</returns>
    Task<bool> ResetUserPassword(string userName, CancellationToken ct = default);

    /// <summary>
    /// Disable/Enable user
    /// </summary>
    /// <param name="userName">User name</param>
    /// <param name="disable">Disable flag</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Process result</returns>
    Task<bool> DisableUser(string userName, bool disable, CancellationToken ct = default);

    /// <summary>
    /// Delete user
    /// </summary>
    /// <param name="userName">User name</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Process result</returns>
    Task<bool> DeleteUser(string userName, CancellationToken ct = default);

    #endregion

    #region Role functions

    /// <summary>
    /// Get roles by user
    /// </summary>
    /// <param name="userName">User name</param>
    /// <param name="code">Page code (token)</param>
    /// <param name="take">Page size</param>
    /// <returns>Process result</returns>
    Task<List<IamRole>> GetRolesByUser(string userName, string code, int take);

    /// <summary>
    /// Get role
    /// </summary>
    /// <param name="name">Element identifier</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Process result</returns>
    Task<IamRole> GetRole(string name, CancellationToken ct = default);

    /// <summary>
    /// Add role
    /// </summary>
    /// <param name="name">Role name</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Added role data</returns>
    Task<IamRole> AddRole(string name, CancellationToken ct = default);

    #endregion
}