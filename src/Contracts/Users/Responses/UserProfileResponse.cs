using Contracts.Users.DTOs;

namespace Contracts.Users.Responses;

public record UserProfileResponse(
    string Id,
    string Email,
    string? DisplayName,
    UserSchoolDto[] Schools);