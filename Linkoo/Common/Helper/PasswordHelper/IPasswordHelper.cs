﻿namespace ReportApp.Common.Helper.PasswordServices
{
    public interface IPasswordHelper
    {
         (string salt, string hash) createPasswordHash(string password);
         bool verifyPasswordHash(string password, string passwordHash, string salted);
    }
}
