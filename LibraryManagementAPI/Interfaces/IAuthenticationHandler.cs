﻿using LibraryManagementAPI.Models;

namespace LibraryManagementAPI.Interfaces;

public interface IAuthenticationHandler
{
    string GenerateJWTToken(LibraryMember user);
}
