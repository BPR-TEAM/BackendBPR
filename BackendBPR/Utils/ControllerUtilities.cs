using System;
using System.Linq;
using BackendBPR.Database;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;
using Zxcvbn;

namespace BackendBPR.Utils
{
    /// <summary>
    /// Class that contains utilities to be used in the controllers
    /// </summary>
    public static class ControllerUtilities
    {
        /// <summary>
        /// Ascii encoding for the bmp image format
        /// </summary>
        /// <value>Ascii byte value of "BM"</value>
        public static byte[] bmp = Encoding.ASCII.GetBytes("BM");
        /// <summary>
        /// Ascii encoding for the gif image format
        /// </summary>
        /// <returns>Ascii byte value of "GIF"</returns>
        public static byte[] gif    = Encoding.ASCII.GetBytes("GIF");
        /// <summary>
        /// Ascii encoding for the png image format
        /// </summary>
        /// <value>137, 80, 78, 71</value>
        public static byte[] png    = new byte[] { 137, 80, 78, 71 };
        /// <summary>
        /// Ascii encoding for the tiff image format
        /// </summary>
        /// <value>73, 73, 42</value>
        public static byte[] tiff   = new byte[] { 73, 73, 42 };
        /// <summary>
        /// Ascii encoding for the other tiff image format
        /// </summary>
        /// <value>77, 77, 42</value>
        public static byte[] tiff2  = new byte[] { 77, 77, 42 };
        /// <summary>
        /// Ascii encoding for the jpeg image format
        /// </summary>
        /// <value>255, 216, 255, 224</value>
        public static byte[] jpeg   = new byte[] { 255, 216, 255, 224 };
        /// <summary>
        /// Ascii encoding for the jpeg canon image format
        /// </summary>
        /// <value>255, 216, 255, 225</value>
        public static byte[] jpeg2  = new byte[] { 255, 216, 255, 225 };

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
        /// <summary>
        /// Function for getting a password result using Zxcvbn library
        /// takes into consideration meta data if supplied like email, name etc.
        /// </summary>
        /// <param name="_password">The password to be evaluated</param>
        /// <param name="_metaData">The meta data for context</param>
        /// <returns>A Zxcvbn result object with score, cracktime, feedback etc.</returns>
        public static Zxcvbn.Result passwordResult(string _password, List<string> _metaData)
        {
            if(_metaData.Equals(null))
            {
                return Zxcvbn.Core.EvaluatePassword(_password);
            }
            else
            {
                return Zxcvbn.Core.EvaluatePassword(_password, _metaData);
            }
        }
        /// <summary>
        /// Function for getting a password score using Zxcvbn library
        /// takes into consideration meta data if supplied like email, name etc.
        /// </summary>
        /// <param name="_password">The password to be evaluated</param>
        /// <param name="_metaData">The meta data for context</param>
        /// <returns>An integer ranging 0-4, 0 being the weakest</returns>
        public static int passwordScore(string _password, List<string> _metaData)
        {
             if(_metaData.Equals(null))
            {
                return Zxcvbn.Core.EvaluatePassword(_password).Score;
            }
            else
            {
                return Zxcvbn.Core.EvaluatePassword(_password, _metaData).Score;
            }
        }

        /// <summary>
        /// Checks if an image is actually an image
        /// </summary>
        /// <param name="_image">The image to check in byte array format</param>
        /// <param name="_maxSizeBytes">The maximum size of said image</param>
        /// <returns>Whether or not the image is actually an image</returns>
        public static bool isImage(byte[] _image, int _maxSizeBytes)
        {
            using(var stream = new MemoryStream(_image))
            {
                try
                {
                    //Check if you can even read the stream
                    if (!stream.CanRead)
                        return false;

                    //Check if over the max size
                    if (_image.Length > _maxSizeBytes)
                        return false;
                    
                    //Check if bytes correspond to an image format encoding
                    if(!firstByteChecking(_image))
                        return false;

                    //Regex check
                    byte[] buffer = new byte[_maxSizeBytes];
                    stream.Read(buffer, 0, _maxSizeBytes);
                    string content = System.Text.Encoding.UTF8.GetString(buffer);
                    if (Regex.IsMatch(content, @"<script|<html|<head|<title|<body|<pre|<table|<a\s+href|<img|<plaintext|<cross\-domain\-policy",
                        RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline))
                        return false;
                }
                catch (Exception)
                {
                    return false;
                }

                try
                {
                    using (var bitmap = new System.Drawing.Bitmap(stream))
                    {
                    }
                }
                catch (Exception)
                {
                    return false;
                }
                finally
                {
                    stream.Position = 0;
                }
            }
            return true;
        }

        /// <summary>
        /// Checks whether the first bytes correspond to an image
        /// </summary>
        /// <param name="_image">The image to check</param>
        /// <returns>Whether or not the first bytes correspond to an image</returns>
        public static bool firstByteChecking(byte[] _image)
        {
            return bmp.SequenceEqual(_image.Take(bmp.Length)) || gif.SequenceEqual(_image.Take(gif.Length))
                || png.SequenceEqual(_image.Take(png.Length)) || tiff.SequenceEqual(_image.Take(tiff.Length))
                || tiff2.SequenceEqual(_image.Take(tiff2.Length)) || jpeg.SequenceEqual(_image.Take(jpeg.Length))
                || jpeg2.SequenceEqual(_image.Take(jpeg2.Length));
        }
    }
}