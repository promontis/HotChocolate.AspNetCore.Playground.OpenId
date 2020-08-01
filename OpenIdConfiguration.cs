namespace HotChocolate.AspNetCore.Playground
{
    public class OpenIdConfiguration
    {
        /// <summary>
        /// The URL of the OIDC/OAuth2 provider.
        /// </summary>
        public string Authority { get; }

        /// <summary>
        /// Your client application's identifier as registered with the OIDC/OAuth2 provider.
        /// </summary>
        public string ClientId { get; }

        /// <summary>
        /// The type of response desired from the OIDC/OAuth2 provider.
        /// </summary>
        public string ResponseType { get; }

        /// <summary>
        /// The scope being requested from the OIDC/OAuth2 provider.
        /// </summary>
        public string Scope { get; }

        public OpenIdConfiguration(string authority, string clientId, string responseType = "id_token", string scope = "openid")
        {
            Authority = authority;
            ClientId = clientId;
            ResponseType = responseType;
            Scope = scope;
        }
    }
}
