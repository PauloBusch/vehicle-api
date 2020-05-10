using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Questor.Vehicle.Domain.Utils.Metadata
{
    public class TokenData
    {
        public string UserId { get; private set; }

        private JwtSecurityToken _token;

        public TokenData() { }

        public TokenData(string userId) : this()
        {
            this.UserId = userId;
        }

        public TokenData(JwtSecurityToken token)
        {
            this._token = token;
            var payload = token.Payload;
            this.UserId = payload["userId"].ToString();
        }
        
        public string Generate()
        {
            var symmetricKey = Encoding.ASCII.GetBytes(VehicleStartup.Secret);
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("userId", UserId),
                }),
                Expires = DateTime.UtcNow.AddHours(12),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(symmetricKey), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public static TokenData Decode(string tokenStr)
        {
            if (string.IsNullOrWhiteSpace(tokenStr)) return null;

            var tokenResult = tokenStr.Split(' ').ElementAt(1);
            var token = new JwtSecurityToken(tokenResult);
            if (token == null) return null;
            return new TokenData(token);
        } 

        public static bool Validate(string tokenStr)
        {
            if (string.IsNullOrWhiteSpace(tokenStr)) return false;
            var tokenResult = tokenStr.Split(' ').ElementAt(1);
            var token = new JwtSecurityToken(tokenResult);
            if (token == null) throw new InvalidOperationException(nameof(token));

            var now = DateTime.UtcNow;
            if (now < token.ValidFrom || now > token.ValidTo) return false;

            var symmetricKey = Encoding.ASCII.GetBytes(VehicleStartup.Secret);
            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(symmetricKey), 
                SecurityAlgorithms.HmacSha256Signature, 
                SecurityAlgorithms.Sha256Digest
            );
            var signature = CreateEncodedSignature(string.Concat(token.RawHeader, ".", token.RawPayload), signingCredentials);
            if (signature != token.RawSignature) return false;

            return true;
        }

        public Claim[] GetClaims()
        {
            return _token.Claims.ToArray();
        }

        private static string CreateEncodedSignature(string input, SigningCredentials signingCredentials)
        {
            if (string.IsNullOrWhiteSpace(input)) return null;
            if (signingCredentials == null) return null;

            var cryptoProviderFactory = signingCredentials.CryptoProviderFactory ?? signingCredentials.Key.CryptoProviderFactory;
            var signatureProvider = cryptoProviderFactory.CreateForSigning(signingCredentials.Key, signingCredentials.Algorithm);
            if (signatureProvider == null) return null;
            try
            {
                return Base64UrlEncoder.Encode(signatureProvider.Sign(Encoding.UTF8.GetBytes(input)));
            }
            finally
            {
                cryptoProviderFactory.ReleaseSignatureProvider(signatureProvider);
            }
        }
    }
}
