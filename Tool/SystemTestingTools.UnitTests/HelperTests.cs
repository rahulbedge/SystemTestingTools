﻿using Shouldly;
using System.Text;
using Xunit;

namespace SystemTestingTools.UnitTests
{
    [Trait("Project", "SystemTestingTools Unit Tests (others)")]
    public class HelperTests
    {
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("  ")]
        public void ParseContentType_Unhappy(string format)
        {
            var response = Helper.ParseContentType(format);
            response.encoding.ShouldBe(Encoding.UTF8);
            response.mediaType.ShouldBe("text/plain");
        }

        [Theory]
        [InlineData("application/json; charset=utf-8", "utf-8", "application/json")]        
        [InlineData("application/html; charset=utf-16", "unicode", "application/html")]
        [InlineData("application/json", "utf-8", "application/json")]
        [InlineData("text/plain; charset=UTF-8", "utf-8", "text/plain")]
        [InlineData("text/html; charset=GBK", "utf-8", "text/html")]
        [InlineData("text/html", "utf-8", "text/html")]
        [InlineData("text/html;charset=UTF-8", "utf-8", "text/html")]        
        public void ParseContentType_Happy(string format, string encoding, string mediaType)
        {
            var response = Helper.ParseContentType(format);
            response.encoding.ShouldBe(Encoding.GetEncoding(encoding));
            response.mediaType.ShouldBe(mediaType);
        }

        [Theory]
        [InlineData("")]
        [InlineData("null")]
        [InlineData("gibberish")]
        [InlineData("text/plain")]
        public void GetKnownContentType_tests(string value)
        {
            Helper.GetKnownContentType(value).ShouldBe(Helper.KnownContentTypes.Other);
        }

        [Theory]
        [InlineData("application/xml")]
        [InlineData("TEXT/xml")]
        public void GetKnownContentType_Tests_Xml(string value)
        {
            Helper.GetKnownContentType(value).ShouldBe(Helper.KnownContentTypes.Xml);
        }

        [Theory]
        [InlineData("application/json")]
        [InlineData("TEXT/json")]
        public void GetKnownContentType_Tests_Json(string value)
        {
            Helper.GetKnownContentType(value).ShouldBe(Helper.KnownContentTypes.Json);
        }
    }
}
