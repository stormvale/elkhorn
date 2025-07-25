﻿using Contracts.Users.DTOs;

namespace Contracts.Users.Responses;

public record UserProfileResponse(
    Guid Id,
    string Email,
    string? DisplayName,
    UserSchoolDto[] Schools);