using System;
using System.Linq;
using BackendBPR.Database;

namespace BackendBPR.Utils
{
    /// <summary>
    /// Class that contains utilities to be used in the controllers
    /// </summary>
    public static class ControllerUtilities
    {
        /// <summary>
        /// Verifies if the token is mismatched/malformed and outs the user that it belongs to with the result
        /// </summary>
        /// <param name="token">The user token to verify</param>
        /// <param name="dbContext">The database context to query</param>
        /// <param name="user">The user that the token matches to if they exist else null</param>
        /// <param name="isVerified">The result if the token matches the user</param>
        public static void TokenVerification(string token,OrangeBushContext dbContext, out User user, out bool isVerified)
        {
            isVerified = true;
            
            var splitToken = token.Split("=");
            if (splitToken.Length > 2)
               isVerified = false;

            user = null;
            User tempUser = null;
            try
            {
                var id = Int32.Parse(splitToken[0]);
                tempUser = dbContext.Users
                    .First(a => a.Id == id);
            }
            catch (InvalidOperationException)
            {
                isVerified = false;
            }

            if (isVerified)
            {
                user = tempUser;
                if (splitToken[1] != user.Token)
                {
                    isVerified = false;
                    user = null;
                }
            }
        }
        /// <summary>
        /// Verifies if the token is mismatched/malformed and outs the user that it belongs to with the result
        /// </summary>
        /// <param name="token">The user token to verify</param>
        /// <param name="dbContext">The database context to query</param>
        /// <returns>Whether or not the given user token matches a user</returns>
        public static bool TokenVerification(string token,OrangeBushContext dbContext)
        {
            var splitToken = token.Split("=");
            if (splitToken.Length > 2)
                return false;

            User user;
            try
            {
                var id = Int32.Parse(splitToken[0]);
                user = dbContext.Users
                .First(a => a.Id == id);
            }
            catch (InvalidOperationException)
            {
                return false;
            }

            if (splitToken[1] != user.Token)
                return false;

            return true;
        }
    }
}