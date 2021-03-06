## 1.3.7
- Bugfixes
    - Recorder when receiving 204 (no content), would produce a file that could not be ready by function FromFiddlerLikeResponseFile    
    - FromBodyOnlyFile() would set content type header as text/plain, which caused problems when trying to parse to common types (json and xml), now the ending of the file (the extension) is used to guess the content type

## 1.3.6
- New feature
    - New Added HttpClient extension GetSessionId(), can be useful to interact with other tools

## 1.3.5
- Breaking change
    - Renamed mentions of "mock" to "stub", because it's a more precise word to define what is being done
    - Renamed MockInstrumentation to ContextRepo [example](/Examples/MovieProject/MovieProject.Tests/MovieProject.IsolatedTests/ComponentTesting/TestServerFixture.cs#L67)
- Bug fix
    - Interpret content type "text/json" as JSON (not only "application/json"), so recorder will try to format it nicely

## 1.3.4
- Bugfixes
    - ReadJsonBody<T> now parses enums as strings
- Change
    - Recorded file now shows assembly name and version and url of initial request like MovieProject.Web 1.0.0.0 (GET https://localhost:44374/api/movie/matrix)

## 1.3.3
- New feature
    - New HttpRequestMessageWrapper methods: GetEndpoint() will return "HttpMethod FullUrl", GetHeaderValue("headerName") will return header values separated by comma

## 1.3.2
- Bug fix
    - ReadJsonBody<T> is now case insensitive
    - XML responses saved by Recorder are now properly indented for easy visualization

## 1.3.1
- Bug fix
    - Fixed bug where generated recorded files, the displayed source file was incorrect

## 1.3.0
- New feature
    - Introduced support for interception WCF HTTP calls

## 1.2.0
- Breaking change
    - Migrated from .net core 2.2 to 3.1 (LTS), therefore removing dependency on Newtonsoft.Json
- New feature
    - Created new extension methods for HttpResponseMessage: ReadBody, ReadJsonBody<T> and ModifyJsonBody<T>

## 1.1.0
- Breaking change
    - Improvements to how interception of HTTP calls happen, to be easier to configure and less intrusive, inspired by https://github.com/justeat/httpclient-interception.

## 1.0.0
- Change
    - After extensive real life testing, and with minor improvements to documentation and other non breaking changes, I decided the library was stable enough to become version 1

## 0.3.0
- Bug fix
    - Fixed bug that only allowed one downstream dependency, otherwise code threw exception "DelegatingHandler instances provided to HttpMessageHandlerBuilder must not be reused or cached"
- New feature
    -  When 'RequestResponseRecorder' creates a file, now the first line of metadata will be "Observations: !! EXPLAIN WHY THIS EXAMPLE IS IMPORTANT HERE !!" to encourage better documentation

## 0.2.0
- New feature
    - Removed dependency on NLog, to enable it to work with any logging framework

## 0.1.0
- New feature
    - First version