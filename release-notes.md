## Release notes

A listing of what each [Nuget package](https://www.nuget.org/packages/Haystack) version represents.

### v1
* 1.0.0 - (2019-05-21) - Initial release containing:
  * `ITokenProvider` - an interface that must be implemented that represents how a token is retrieved from a token source.
  * `ITokenCache` / `TokenCache` - The thread-safe, mockable object that manages the cached JWT, which auto-refreshes when the token expires.
  * `TokenCachingHandler` - A `DelegatingHandler` that takes an `ITokenCache` that's designed to be used as the `innerHandler` on an `HttpClient`. This keeps your application code free of cache management boilerplate.
  