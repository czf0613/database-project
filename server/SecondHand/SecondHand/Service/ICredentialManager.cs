using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SecondHand.model;
using SecondHand.Model;

namespace SecondHand.Service
{
    public interface ICredentialManager
    {
        Task<bool> CheckTokenAsync(User user, string token);

        Task<ValueTuple<LoginResult, IdentityCredential>> CreateLoginRecordAsync(string userName, string password,
            Role role);

        Task LogOutAsync(string token);
        Task LogOutAnywayAsync(string userName);
    }

    public enum LoginResult
    {
        SUCCESS,
        BADCRIDENTIAL,
        LOCKED,
        TOO_MUCH
    }

    public class CredentialManager : ICredentialManager
    {
        private readonly TokenDatabase tokenDatabase;
        private readonly Databases databases;

        public CredentialManager(TokenDatabase tokenDatabase, Databases databases)
        {
            this.tokenDatabase = tokenDatabase;
            this.databases = databases;
        }

        public async Task<bool> CheckTokenAsync(User user, string token)
        {
            var cnt = tokenDatabase.LoginRecords.CountAsync(l => l.User.Equals(user) && l.Token == token);
            if (await cnt == 0)
                return false;
            return true;
        }

        public async Task<ValueTuple<LoginResult, IdentityCredential>> CreateLoginRecordAsync(string userName,
            string password, Role role)
        {
            var cnt = await databases.Users.CountAsync(u => u.UserName == userName);
            var user = await databases.Users.FirstAsync(u => u.UserName == userName);
            var records = tokenDatabase.LoginRecords.CountAsync(l => l.User.Equals(user));
            if (cnt == 0)
                return new ValueTuple<LoginResult, IdentityCredential>(LoginResult.BADCRIDENTIAL, null);

            if (await records >= 5)
                return new ValueTuple<LoginResult, IdentityCredential>(LoginResult.TOO_MUCH, null);

            if (!BCrypt.Net.BCrypt.EnhancedVerify(password, user.Password))
                return new ValueTuple<LoginResult, IdentityCredential>(LoginResult.BADCRIDENTIAL, null);

            var loginRecord = new LoginRecord
            {
                Role = role,
                Token = "12345678",
                User = user
            };
            var record = tokenDatabase.LoginRecords.AddAsync(loginRecord);

            var credential = new IdentityCredential
            {
                ExpireDate = (await record).Entity.Time.AddDays(15),
                Token = (await record).Entity.Token
            };
            if (role == Role.STUDENT)
            {
                credential.Credential = await databases.Students.FirstAsync(s => s.UserName == userName);
            }
            else
            {
                credential.Credential = await databases.Admins.FirstAsync(a => a.UserName == userName);
            }

            await tokenDatabase.SaveChangesAsync();
            return new ValueTuple<LoginResult, IdentityCredential>(LoginResult.SUCCESS, credential);
        }

        public async Task LogOutAsync(string token)
        {
            var list = tokenDatabase.LoginRecords.Where(l => l.Token == token)
                .ToListAsync();
            tokenDatabase.LoginRecords.RemoveRange(await list);
        }

        public async Task LogOutAnywayAsync(string userName)
        {
            Console.WriteLine(
                $"A dangerous method invoked: delete all login records of {userName} at {DateTimeOffset.Now}");
            var list = tokenDatabase.LoginRecords.Where(l => l.User.UserName == userName).ToListAsync();
            tokenDatabase.LoginRecords.RemoveRange(await list);
        }
    }
}