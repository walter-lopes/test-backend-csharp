using System;
using System.Collections.Generic;
using Easynvest.Infohub.Parse.Api.Helpers;
using Easynvest.Infohub.Parse.Infra.CrossCutting.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;

namespace Easynvest.InfoHub.Parse.Test.Api.Helpers
{
    public class HtmlDecoderFilterTests
    {
        private HtmlDecoderFilter _decoderFilter;
        private ActionContext _actionContext;
        private ResourceExecutingContext _resourceFilter;

        private ILogger<HtmlDecoderFilter> _logger;
        private AuthenticatedUser _authenticatedUser;
        private Infohub.Parse.Infra.CrossCutting.Log.Logger _log;

        [SetUp]
        public void SetUp()
        {
            _logger = Substitute.For<ILogger<HtmlDecoderFilter>>();
            _authenticatedUser = new AuthenticatedUser(Substitute.For<IHttpContextAccessor>());
            _log = new Infohub.Parse.Infra.CrossCutting.Log.Logger(_authenticatedUser);

            _decoderFilter = new HtmlDecoderFilter(_logger, _authenticatedUser);
            _actionContext = new ActionContext(new DefaultHttpContext(), new RouteData(), new ActionDescriptor());
            _resourceFilter = new ResourceExecutingContext(_actionContext, Substitute.For<IList<IFilterMetadata>>(),
                Substitute.For<IList<IValueProviderFactory>>());
        }

        [Test]
        public void Should_Decode_Html_String_BondParse()
        {
            var dictionary = new RouteData();
            dictionary.Values.Add("bondType", "Banco%20ABC%20-%20D%20%2F%20%C3%89");
            dictionary.Values.Add("bondIndex", "Banco%20ABC%20-%20D%20%2F%20%C3%89");
            dictionary.Values.Add("isAntecipatedSell", "Banco%20ABC%20-%20D%20%2F%20%C3%89");
            dictionary.Values.Add("action", "DeleteBondParser");
            dictionary.Values.Add("controller", "BondParser");
            _resourceFilter.RouteData = dictionary;

            Assert.DoesNotThrow(() => _decoderFilter.OnResourceExecuting(_resourceFilter));

            var resourceDecodeValues = _resourceFilter.RouteData.Values;

            Assert.Multiple(() =>
            {
                Assert.AreEqual(resourceDecodeValues["bondType"], "Banco ABC - D / É");
                Assert.AreEqual(resourceDecodeValues["bondIndex"], "Banco ABC - D / É");
                Assert.AreEqual(resourceDecodeValues["isAntecipatedSell"], "Banco ABC - D / É");
                Assert.AreEqual(resourceDecodeValues["action"], "DeleteBondParser");
                Assert.AreEqual(resourceDecodeValues["controller"], "BondParser");
            });
        }

        [Test]
        public void Should__Decode_Html_String_IndexParse()
        {
            var dictionary = new RouteData();
            dictionary.Values.Add("CetipIndex", "Banco%20ABC%20-%20D%20%2F%20%C3%89");
            dictionary.Values.Add("action", "DeleteIndexParser");
            dictionary.Values.Add("controller", "IndexParser");
            _resourceFilter.RouteData = dictionary;

            Assert.DoesNotThrow(() => _decoderFilter.OnResourceExecuting(_resourceFilter));

            var resourceDecodeValues = _resourceFilter.RouteData.Values;

            Assert.Multiple(() =>
            {
                Assert.AreEqual(resourceDecodeValues["CetipIndex"], "Banco ABC - D / É");
                Assert.AreEqual(resourceDecodeValues["action"], "DeleteIndexParser");
                Assert.AreEqual(resourceDecodeValues["controller"], "IndexParser");
            });
        }

        [Test]
        public void Should__Decode_Html_String_IssuerParse()
        {
            var dictionary = new RouteData();
            dictionary.Values.Add("issuerNameCetip", "Banco%20ABC%20-%20D%20%2F%20%C3%89");
            dictionary.Values.Add("action", "DeleteIssuerParser");
            dictionary.Values.Add("controller", "IssuerParser");
            _resourceFilter.RouteData = dictionary;

            Assert.DoesNotThrow(() => _decoderFilter.OnResourceExecuting(_resourceFilter));

            var resourceDecodeValues = _resourceFilter.RouteData.Values;

            Assert.Multiple(() =>
            {
                Assert.AreEqual(resourceDecodeValues["issuerNameCetip"], "Banco ABC - D / É");
                Assert.AreEqual(resourceDecodeValues["action"], "DeleteIssuerParser");
                Assert.AreEqual(resourceDecodeValues["controller"], "IssuerParser");
            });
        }

        [Test]
        public void Should_Throws_Exception_When_RouteData_Is_Null()
        {
            _resourceFilter.RouteData = null;
            Assert.Throws<NullReferenceException>(() => _decoderFilter.OnResourceExecuting(_resourceFilter));
        }
    }
}
