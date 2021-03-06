using Shouldly;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using SystemTestingTools;
using Xunit;

namespace IsolatedTests.ComponentTestings
{
    [Collection("SharedServer collection")]
    [Trait("Project", "MovieProject Component Tests (Happy)")]
    public class GetMovieHappyTests
    {
        private readonly TestServerFixture Fixture;

        private static string MovieUrl = "http://www.omdbapi.com/?apikey=863d6589&type=movie";        

        public GetMovieHappyTests(TestServerFixture fixture)
        {        
            Fixture = fixture;
        }

        [Fact(Skip = "example of how NOT write Component tests, because it doesn't assert logs and outgoing requests")]
        public async Task When_UserAsksForMovieWithMostFields_Then_ReturnMovieProperly_Incomplete()
        {
            // arrange
            var client = Fixture.Server.CreateClient();
            client.CreateSession();
            var response = ResponseFactory.FromFiddlerLikeResponseFile($"{Fixture.StubsFolder}/OmdbApi/Real_Responses/Happy/200_ContainsMostFields_TheMatrix.txt");
            var matrixMovieUrl = $"{MovieUrl}&t=matrix";
            client.AppendHttpCallStub(HttpMethod.Get, new System.Uri(matrixMovieUrl), response);

            // act
            var httpResponse = await client.GetAsync("/api/movie/matrix");

            // assert
            httpResponse.ShouldNotBeNull();
            httpResponse.StatusCode.ShouldBe(HttpStatusCode.OK);

            var movie = JsonSerializer.Deserialize<MovieProject.Logic.DTO.Media>(await httpResponse.Content.ReadAsStringAsync());
            movie.ShouldNotBeNull();

            movie.Id.ShouldBe("tt0133093");
            movie.Name.ShouldBe("The Matrix");
            movie.Year.ShouldBe("1999");
            movie.Runtime.ShouldBe("136 min");
            movie.Plot.ShouldBe("A computer hacker learns from mysterious rebels about the true nature of his reality and his role in the war against its controllers.");
        }

        [Fact]
        public async Task When_UserAsksForMovieWithMostFields_Then_ReturnMovieProperly()
        {
            // arrange
            var client = Fixture.Server.CreateClient();
            client.CreateSession();
            var response = ResponseFactory.FromFiddlerLikeResponseFile($"{Fixture.StubsFolder}/OmdbApi/Real_Responses/Happy/200_ContainsMostFields_TheMatrix.txt");
            var matrixMovieUrl = $"{MovieUrl}&t=matrix";
            client.AppendHttpCallStub(HttpMethod.Get, new System.Uri(matrixMovieUrl), response);

            // act
            var httpResponse = await client.GetAsync("/api/movie/matrix");

            // assert logs
            var logs = client.GetSessionLogs();
            logs.ShouldBeEmpty();

            // assert outgoing
            var outgoingRequests = client.GetSessionOutgoingRequests();
            outgoingRequests.Count.ShouldBe(1);
            outgoingRequests[0].GetEndpoint().ShouldBe($"GET {matrixMovieUrl}");
            outgoingRequests[0].GetHeaderValue("Referer").ShouldBe(MovieProject.Logic.Constants.Website);

            // assert return
            httpResponse.StatusCode.ShouldBe(HttpStatusCode.OK);

            var movie = await httpResponse.ReadJsonBody<MovieProject.Logic.DTO.Media>();
            movie.Id.ShouldBe("tt0133093");
            movie.Name.ShouldBe("The Matrix");
            movie.Year.ShouldBe("1999");
            movie.Runtime.ShouldBe("2.3h");
            movie.Plot.ShouldBe("A computer hacker learns from mysterious rebels about the true nature of his reality and his role in the war against its controllers.");
            movie.Language.ShouldBe(MovieProject.Logic.DTO.Media.Languages.English);
        }

        [Fact]
        public async Task When_UserAsksForMovieWithMostFields_With_ResponseBodyOny_Then_ReturnMovieProperly()
        {
            // arrange
            var client = Fixture.Server.CreateClient();
            client.CreateSession();
            // FromBodyOnlyFile work well, but if possible it's nicer to read from a more comprehensive file format (the one that FromFiddlerLikeResponseFile uses)
            var response = ResponseFactory.FromBodyOnlyFile($"{Fixture.StubsFolder}/OmdbApi/Real_Responses/Happy/200_ContainsMostFields_TheMatrix.json", HttpStatusCode.OK);
            var matrixMovieUrl = $"{MovieUrl}&t=matrix";
            client.AppendHttpCallStub(HttpMethod.Get, new System.Uri(matrixMovieUrl), response);

            // act
            var httpResponse = await client.GetAsync("/api/movie/matrix");

            // assert logs
            var logs = client.GetSessionLogs();
            logs.ShouldBeEmpty();

            // assert outgoing
            var outgoingRequests = client.GetSessionOutgoingRequests();
            outgoingRequests.Count.ShouldBe(1);
            outgoingRequests[0].GetEndpoint().ShouldBe($"GET {matrixMovieUrl}");
            outgoingRequests[0].GetHeaderValue("Referer").ShouldBe(MovieProject.Logic.Constants.Website);

            // assert return
            httpResponse.StatusCode.ShouldBe(HttpStatusCode.OK);

            var movie = await httpResponse.ReadJsonBody<MovieProject.Logic.DTO.Media>();
            movie.Id.ShouldBe("tt0133093");
            movie.Name.ShouldBe("The Matrix");
            movie.Year.ShouldBe("1999");
            movie.Runtime.ShouldBe("2.3h");
            movie.Plot.ShouldBe("A computer hacker learns from mysterious rebels about the true nature of his reality and his role in the war against its controllers.");
            movie.Language.ShouldBe(MovieProject.Logic.DTO.Media.Languages.English);
        }

        [Fact]
        public async Task When_UserAsksForMovieThatDoesntExist_Then_Return400Status()
        {
            // arrange
            var client = Fixture.Server.CreateClient();
            client.CreateSession();
            var response = ResponseFactory.FromFiddlerLikeResponseFile($"{Fixture.StubsFolder}/OmdbApi/Real_Responses/Happy/200_MovieNotFound.txt");
            client.AppendHttpCallStub(HttpMethod.Get, new System.Uri($"{MovieUrl}&t=some_weird_title"), response);

            // act
            var httpResponse = await client.GetAsync("/api/movie/some_weird_title");

            // assert logs
            var logs = client.GetSessionLogs();
            logs.ShouldBeEmpty();

            // assert outgoing
            var outgoingRequests = client.GetSessionOutgoingRequests();
            outgoingRequests.Count.ShouldBe(1);
            outgoingRequests[0].GetEndpoint().ShouldBe($"GET {MovieUrl}&t=some_weird_title");
            outgoingRequests[0].GetHeaderValue("Referer").ShouldBe(MovieProject.Logic.Constants.Website);

            // assert return
            httpResponse.StatusCode.ShouldBe(HttpStatusCode.NotFound);
            var message = await httpResponse.ReadBody();
            message.ShouldBe("Search terms didn't match any movie");
        }

        [Fact]
        public async Task When_UserAsksForMoviFewFields_Then_ReturnMovieProperly()
        {
            // arrange
            var client = Fixture.Server.CreateClient();
            client.CreateSession();
            var response = ResponseFactory.FromFiddlerLikeResponseFile($"{Fixture.StubsFolder}/OmdbApi/Real_Responses/Happy/200_FewFields_OldMovie.txt");
            var comeAlongMovieUrl = $"{MovieUrl}&t=come along, do";
            client.AppendHttpCallStub(HttpMethod.Get, new System.Uri(comeAlongMovieUrl), response);

            // act
            var httpResponse = await client.GetAsync("/api/movie/come along, do");

            // assert logs
            var logs = client.GetSessionLogs();
            logs.ShouldBeEmpty();

            // assert outgoing
            var outgoingRequests = client.GetSessionOutgoingRequests();
            outgoingRequests.Count.ShouldBe(1);
            outgoingRequests[0].GetEndpoint().ShouldBe($"GET {comeAlongMovieUrl}");
            outgoingRequests[0].GetHeaderValue("Referer").ShouldBe(MovieProject.Logic.Constants.Website);

            // assert return
            httpResponse.StatusCode.ShouldBe(HttpStatusCode.OK);

            var movie = await httpResponse.ReadJsonBody<MovieProject.Logic.DTO.Media>();
            movie.Id.ShouldBe("tt0000182");
            movie.Name.ShouldBe("Come Along, Do!");
            movie.Year.ShouldBe("1898");
            movie.Runtime.ShouldBe("1 min");
            movie.Plot.ShouldBe("A couple look at a statue while eating in an art gallery.");
        }

        [Fact]
        public async Task When_UserAsksForMovieWithSomeInvalidValues_Then_ReturnMovieProperly()
        {
            // arrange
            var client = Fixture.Server.CreateClient();
            client.CreateSession();
            var response = ResponseFactory.FromFiddlerLikeResponseFile($"{Fixture.StubsFolder}/OmdbApi/Fake_Responses/Happy/200_NoRunTime_NoPlot_YearTooOld.txt");
            var comeAlongMovieUrl = $"{MovieUrl}&t=fantastika";
            client.AppendHttpCallStub(HttpMethod.Get, new System.Uri(comeAlongMovieUrl), response);

            // act
            var httpResponse = await client.GetAsync("/api/movie/fantastika");

            // assert logs
            var logs = client.GetSessionLogs();
            logs.ShouldBeEmpty();

            // assert outgoing
            var outgoingRequests = client.GetSessionOutgoingRequests();
            outgoingRequests.Count.ShouldBe(1);
            outgoingRequests[0].GetEndpoint().ShouldBe($"GET {comeAlongMovieUrl}");
            outgoingRequests[0].GetHeaderValue("Referer").ShouldBe(MovieProject.Logic.Constants.Website);

            // assert return
            httpResponse.StatusCode.ShouldBe(HttpStatusCode.OK);

            var movie = await httpResponse.ReadJsonBody<MovieProject.Logic.DTO.Media>();
            movie.Id.ShouldBe("tt1185643");
            movie.Name.ShouldBe("Fantastika vs. Wonderwoman");
            movie.Runtime.ShouldBe("Unknown");
            movie.Year.ShouldBe("Unknown");            
            movie.Plot.ShouldBe("");
        }
    }
}
