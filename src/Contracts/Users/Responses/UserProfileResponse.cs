using Contracts.Users.DTOs;

namespace Contracts.Users.Responses;

public record UserProfileResponse(
    string Id,
    string Email,
    string? FirstName,
    string? LastName,
    string? DisplayName,
    UserSchoolDto[] Schools,
    DateTime CreatedAt);