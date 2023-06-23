using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Amazon;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.Runtime;
using It270.MedicalSystem.Common.Application.ApplicationCore.Interfaces.ExternalServices;
using It270.MedicalSystem.Common.Application.ApplicationCore.Services;
using It270.MedicalSystem.Common.Application.Core.Constants;
using It270.MedicalSystem.Common.Application.Core.Helpers.UserNS;
using Serilog;
using static It270.MedicalSystem.Common.Application.Core.Enums.UserEnums;

namespace It270.MedicalSystem.Common.Infrastructure.Infrastructure.Services;

/// <summary>
/// IAM Service
/// </summary>
public class IamService : IIamService
{
    private readonly IAmazonCognitoIdentityProvider _client;
    private readonly string _userPoolId;
    private readonly string _clientId;
    private readonly ILogger _logger;

    /// <summary>
    /// Service constructor
    /// </summary>
    /// <param name="logger">Serilog logger</param>
    public IamService(ILogger logger)
    {
        _logger = logger;
        _client = InitClient();
        _userPoolId = Environment.GetEnvironmentVariable("AWS_COGNITO_DEFAULT_POOL");
        _clientId = Environment.GetEnvironmentVariable("AWS_COGNITO_CLIENT_ID");
    }

    /// <summary>
    /// Initialize AWS Cognito client
    /// </summary>
    /// <returns>AWS Cognito client</returns>
    private static AmazonCognitoIdentityProviderClient InitClient()
    {
        var accessKey = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY");
        var secretkey = Environment.GetEnvironmentVariable("AWS_SECRET_KEY");
        var credentials = new BasicAWSCredentials(accessKey, secretkey);

        var region = RegionEndpoint.GetBySystemName(Environment.GetEnvironmentVariable("AWS_COGNITO_REGION"));

        return new(credentials, region);
    }

    /// <summary>
    /// Asynchronous IAM initialization
    /// </summary>
    public async Task Init()
    {
        try
        {
            // Add basic roles
            var roles = Enum.GetValues(typeof(MedicalRecordsRole))
                .Cast<MedicalRecordsRole>()
                .Select(t => t.ToString())
                .ToArray();

            foreach (var role in roles)
            {
                bool roleExists = (await GetRole(role)) != null;

                if (!roleExists)
                    await AddRole(role);
            }

            // Add main administrator
            bool adminExists = (await GetUser(GeneralConstants.SuperAdminUserName)) != null;

            if (!adminExists)
            {
                IamUser admin = new()
                {
                    UserName = GeneralConstants.SuperAdminUserName,
                    Email = Environment.GetEnvironmentVariable("GENERAL_ADMIN_EMAIL")
                };
                await AddUser(admin, Environment.GetEnvironmentVariable("GENERAL_ADMIN_PASSWORD"));

                // Add main administrator role
                await AddUserRole(GeneralConstants.SuperAdminUserName, nameof(MedicalRecordsRole.Admin));

            }
        }
        catch (Exception) { }
    }

    #region User functions

    /// <summary>
    /// Get users (paginated)
    /// </summary>
    /// <param name="code">Page code (token)</param>
    /// <param name="take">Page size</param>
    /// <returns>List of users data</returns>
    public async Task<List<IamUser>> GetUsers(string code, int take)
    {
        List<IamUser> users = null;

        try
        {
            var request = new ListUsersRequest
            {
                UserPoolId = _userPoolId,
                Limit = take,
                PaginationToken = code
            };

            var usersPaginator = _client.Paginators.ListUsers(request);
            var iamUsers = new List<UserType>();

            await foreach (var response in usersPaginator.Responses)
            {
                iamUsers.AddRange(response.Users);
            }

            users = iamUsers
                .Select(u => CastUserData(u))
                .ToList();
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error: Get IAM users");
        }

        return users;
    }

    /// <summary>
    /// Get user
    /// </summary>
    /// <param name="userName">User name</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Process result</returns>
    public async Task<IamUser> GetUser(string userName, CancellationToken ct = default)
    {
        IamUser user = null;

        try
        {
            var request = new AdminGetUserRequest()
            {
                Username = userName,
                UserPoolId = _userPoolId,
            };

            var response = await _client.AdminGetUserAsync(request, ct);

            user = new()
            {
                Sub = GetAttributeValue(response.UserAttributes, IamConstants.Sub),
                UserName = response.Username,
                Enabled = response.Enabled,
                Email = GetAttributeValue(response.UserAttributes, IamConstants.Email),
                PhoneNumber = GetAttributeValue(response.UserAttributes, IamConstants.PhoneNumber),
                CreationDate = response.UserCreateDate,
                EditionDate = response.UserLastModifiedDate,
            };
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error: Get IAM user {@userName}", userName);
        }

        return user;
    }

    /// <summary>
    /// Add user
    /// </summary>
    /// <param name="user">User data</param>
    /// <param name="key">User password</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Process result</returns>
    public async Task<IamUser> AddUser(IamUser user, string key = null, CancellationToken ct = default)
    {
        try
        {
            var userAttrsList = new List<AttributeType>()
            {
                new()
                {
                    Name = "email_verified",
                    Value = "true"
                },
                new()
                {
                    Name = "email",
                    Value = user.Email,
                },
                new()
                {
                    Name = "nickname",
                    Value = user.UserName,
                }
            };

            var request = new AdminCreateUserRequest
            {
                Username = user.UserName,
                UserPoolId = _userPoolId,
                UserAttributes = userAttrsList,
                TemporaryPassword = key,
                MessageAction = MessageActionType.SUPPRESS,
            };

            var response = await _client.AdminCreateUserAsync(request, ct);

            if (response.HttpStatusCode == HttpStatusCode.OK)
            {
                _logger.Information("Added IAM user {@user}", user);

                var iamSub = response.User.Attributes
                    .Where(a => a.Name == IamConstants.Sub)
                    .Select(a => a.Value)
                    .FirstOrDefault();

                user.Sub = iamSub;
            }
            else
                user = null;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error: Add IAM user {@user}", user);
        }

        return user;
    }

    /// <summary>
    /// Add role to user
    /// </summary>
    /// <param name="userName">User name</param>
    /// <param name="role">Role name</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Process result</returns>
    public async Task<bool> AddUserRole(string userName, string role, CancellationToken ct = default)
    {
        bool successResult = false;

        try
        {
            var request = new AdminAddUserToGroupRequest()
            {
                Username = userName,
                UserPoolId = _userPoolId,
                GroupName = role,
            };

            var response = await _client.AdminAddUserToGroupAsync(request, ct);

            successResult = response.HttpStatusCode == HttpStatusCode.OK;

            if (successResult)
            {
                var requestData = new { userName, role };
                _logger.Information("Added IAM user role {@requestData}", requestData);
            }
        }
        catch (Exception ex)
        {
            var requestData = new { userName, role };
            _logger.Error(ex, "Error: Add IAM user role {@requestData}", requestData);
        }

        return successResult;
    }

    /// <summary>
    /// Remove user role
    /// </summary>
    /// <param name="userName">User name</param>
    /// <param name="role">Role name</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Process result</returns>
    public async Task<bool> RemoveUserRole(string userName, string role, CancellationToken ct = default)
    {
        bool successResult = false;

        try
        {
            var request = new AdminRemoveUserFromGroupRequest()
            {
                Username = userName,
                UserPoolId = _userPoolId,
                GroupName = role,
            };

            var response = await _client.AdminRemoveUserFromGroupAsync(request, ct);

            successResult = response.HttpStatusCode == HttpStatusCode.OK;

            if (successResult)
            {
                var requestData = new { userName, role };
                _logger.Information("Removed IAM user role {@requestData}", requestData);
            }
        }
        catch (Exception ex)
        {
            var requestData = new { userName, role };
            _logger.Error(ex, "Error: Remove IAM user role {@requestData}", requestData);
        }

        return successResult;
    }

    /// <summary>
    /// Change user password
    /// </summary>
    /// <param name="userName">User name</param>
    /// <param name="key">New user password</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Process result</returns>
    public async Task<bool> SetUserPassword(string userName, string key, CancellationToken ct = default)
    {
        bool successResult = false;

        try
        {
            var request = new AdminSetUserPasswordRequest()
            {
                Username = userName,
                UserPoolId = _userPoolId,
                Password = key,
                Permanent = false,
            };

            var response = await _client.AdminSetUserPasswordAsync(request, ct);

            successResult = response.HttpStatusCode == HttpStatusCode.OK;

            if (successResult)
                _logger.Information("Changed IAM user password {@userName}", userName);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error: Changed IAM user password {@userName}", userName);
        }

        return successResult;
    }

    /// <summary>
    /// Reset user password
    /// </summary>
    /// <param name="userName">User name</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Process result</returns>
    public async Task<bool> ResetUserPassword(string userName, CancellationToken ct = default)
    {
        bool successResult = false;

        try
        {
            string key = CryptoTools.PasswordGenerator();

            var request = new AdminSetUserPasswordRequest()
            {
                Username = userName,
                UserPoolId = _userPoolId,
                Password = key,
                Permanent = false,
            };

            var response = await _client.AdminSetUserPasswordAsync(request, ct);

            successResult = response.HttpStatusCode == HttpStatusCode.OK;

            if (successResult)
                _logger.Information("Reset IAM user password {@userName}", userName);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error: Reset IAM user password {@userName}", userName);
        }

        return successResult;
    }

    /// <summary>
    /// Disable/Enable user
    /// </summary>
    /// <param name="userName">User name</param>
    /// <param name="disable">Disable flag</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Process result</returns>
    public async Task<bool> DisableUser(string userName, bool disable, CancellationToken ct = default)
    {
        bool successResult = false;

        try
        {
            if (disable)
            {
                var request = new AdminDisableUserRequest()
                {
                    Username = userName,
                    UserPoolId = _userPoolId,
                };

                var response = await _client.AdminDisableUserAsync(request, ct);

                successResult = response.HttpStatusCode == HttpStatusCode.OK;

                if (successResult)
                    _logger.Information("Disabled IAM user {@userName}", userName);
            }
            else
            {
                var request = new AdminEnableUserRequest()
                {
                    Username = userName,
                    UserPoolId = _userPoolId,
                };

                var response = await _client.AdminEnableUserAsync(request, ct);

                successResult = response.HttpStatusCode == HttpStatusCode.OK;

                if (successResult)
                    _logger.Information("Enabled IAM user {@userName}", userName);
            }

        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error: Reset IAM user password {@userName}", userName);
        }

        return successResult;
    }

    /// <summary>
    /// Delete user
    /// </summary>
    /// <param name="userName">User name</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Process result</returns>
    public async Task<bool> DeleteUser(string userName, CancellationToken ct = default)
    {
        bool successResult = false;

        try
        {
            var request = new AdminDeleteUserRequest()
            {
                Username = userName,
                UserPoolId = _userPoolId,
            };

            var response = await _client.AdminDeleteUserAsync(request, ct);

            successResult = response.HttpStatusCode == HttpStatusCode.OK;

            if (successResult)
                _logger.Information("Deleted IAM user {@userName}", userName);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error: Reset IAM user password {@userName}", userName);
        }

        return successResult;
    }

    #endregion

    #region Role functions

    /// <summary>
    /// Get roles by user
    /// </summary>
    /// <param name="userName">User name</param>
    /// <param name="code">Page code (token)</param>
    /// <param name="take">Page size</param>
    /// <returns>Process result</returns>
    public async Task<List<IamRole>> GetRolesByUser(string userName, string code, int take)
    {
        List<IamRole> roles = null;

        try
        {
            var request = new AdminListGroupsForUserRequest
            {
                Username = userName,
                UserPoolId = _userPoolId,
                Limit = take,
                NextToken = code
            };

            var rolesPaginator = _client.Paginators.AdminListGroupsForUser(request);
            roles = new();

            await foreach (var group in rolesPaginator.Groups)
            {
                roles.Add(new()
                {
                    Name = group.GroupName,
                    Description = group.Description
                });
            }
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error: Get IAM users");
        }

        return roles;
    }

    /// <summary>
    /// Get role
    /// </summary>
    /// <param name="name">Element identifier</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Process result</returns>
    public async Task<IamRole> GetRole(string name, CancellationToken ct = default)
    {
        IamRole role = null;

        try
        {
            var request = new GetGroupRequest
            {
                UserPoolId = _userPoolId,
                GroupName = name,
            };

            var response = await _client.GetGroupAsync(request, ct);
            GroupType group = response.Group;

            role = new()
            {
                Name = group.GroupName,
                Description = group.Description,
            };
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error: Get IAM role {@role}", name);
        }

        return role;
    }

    /// <summary>
    /// Add role
    /// </summary>
    /// <param name="name">Role name</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Added role data</returns>
    public async Task<IamRole> AddRole(string name, CancellationToken ct = default)
    {
        IamRole role = null;

        try
        {
            // Validate roles with the same name
            var tmpRole = await GetRole(name, ct);

            if (tmpRole != null)
                return tmpRole;

            // Save new role
            var request = new CreateGroupRequest()
            {
                UserPoolId = _userPoolId,
                GroupName = name
            };

            var response = await _client.CreateGroupAsync(request, ct);

            if (response != null && response.ContentLength > 0 && response.HttpStatusCode == HttpStatusCode.OK)
            {
                var group = response.Group;
                role = new()
                {
                    Name = group.GroupName,
                    Description = group.Description,
                };
            }
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error: Get IAM role {@role}", name);
        }

        return role;
    }

    #endregion

    #region General functions

    /// <summary>
    /// Cast AWS user data to DTO
    /// </summary>
    /// <param name="userData">AWS user data</param>
    /// <returns>User DTO data</returns>
    private static IamUser CastUserData(UserType userData)
    {
        return new()
        {
            Sub = GetAttributeValue(userData.Attributes, IamConstants.Sub),
            UserName = userData.Username,
            Enabled = userData.Enabled,
            Email = GetAttributeValue(userData.Attributes, IamConstants.Email),
            PhoneNumber = GetAttributeValue(userData.Attributes, IamConstants.PhoneNumber),
            CreationDate = userData.UserCreateDate,
            EditionDate = userData.UserLastModifiedDate,
        };
    }

    /// <summary>
    /// Get AWS user attribute
    /// </summary>
    /// <param name="attributes">AWS attribute list</param>
    /// <param name="name">AWS attribute name</param>
    /// <returns>AWS attribute value</returns>
    private static string GetAttributeValue(List<AttributeType> attributes, string name)
    {
        if (attributes == null || attributes.Count <= 0)
            return null;

        return attributes
            .Where(a => a.Name == name)
            .Select(a => a.Value)
            .FirstOrDefault();
    }

    #endregion
}