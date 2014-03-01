﻿using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Thinktecture.IdentityServer.Core.Connect.Models;

namespace Thinktecture.IdentityServer.Core.Protocols.Connect.Results
{
    public class TokenResult : IHttpActionResult
    {
        public TokenResponse Response { get; set; }

        public TokenResult(TokenResponse response)
        {
            Response = response;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult<HttpResponseMessage>(Execute());
        }

        private HttpResponseMessage Execute()
        {
            var dto = new TokenResponseDto
            {
                id_token = Response.IdentityToken,
                access_token = Response.AccessToken,
                expires_in = Response.AccessTokenLifetime,
                token_type = Constants.TokenTypes.Bearer
            };

            var formatter = new JsonMediaTypeFormatter();
            formatter.SerializerSettings.DefaultValueHandling = DefaultValueHandling.Ignore;

            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ObjectContent<TokenResponseDto>(dto, formatter)
            };

            return response;
        }

        internal class TokenResponseDto
        {
            public string id_token { get; set; }
            public string access_token { get; set; }
            public int expires_in { get; set; }
            public string token_type { get; set; }
        }    
    }
}