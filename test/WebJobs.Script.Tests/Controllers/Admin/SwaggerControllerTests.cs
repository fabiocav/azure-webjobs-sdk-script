﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http.Results;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Script.Config;
using Microsoft.Azure.WebJobs.Script.Description;
using Microsoft.Azure.WebJobs.Script.WebHost;
using Microsoft.Azure.WebJobs.Script.WebHost.Controllers;
using Microsoft.Azure.WebJobs.Script.WebHost.Filters;
using Moq;
using Newtonsoft.Json.Linq;
using WebJobs.Script.Tests;
using Xunit;

namespace Microsoft.Azure.WebJobs.Script.Tests.Controllers.Admin
{
    public class SwaggerControllerTests
    {
        private readonly TempDirectory _secretsDirectory = new TempDirectory();
        private Mock<ScriptHost> _hostMock;
        private Mock<WebScriptHostManager> _managerMock;
        private Collection<FunctionDescriptor> _testFunctions;
        private SwaggerController _testController;
        private Mock<ISwaggerDocumentManager> _swaggerDocumentManagerMock;

        public SwaggerControllerTests()
        {
            _testFunctions = new Collection<FunctionDescriptor>();

            var testData = GetTestTargets(_testFunctions, true);
            _hostMock = testData.HostMock;
            _managerMock = testData.HostManagerMock;
            _swaggerDocumentManagerMock = testData.DocumentManagerMock;
            _testController = testData.Controller;
        }

        [Fact]
        public void HasAuthorizationLevelAttribute()
        {
            SystemAuthorizationLevelAttribute attribute = typeof(SwaggerController).GetCustomAttribute<SystemAuthorizationLevelAttribute>();
            Assert.Equal(AuthorizationLevel.System, attribute.Level);
        }

        [Fact]
        public void GetGeneratedSwaggerDocument_ReturnsSwaggerDocument()
        {
            JObject json = new JObject();
            _testController.Request = new HttpRequestMessage(HttpMethod.Get, "https://local/admin/host/swagger/default");
            _swaggerDocumentManagerMock.Setup(p => p.GenerateSwaggerDocument(null)).Returns(json);
            var result = (OkNegotiatedContentResult<JObject>)_testController.GetGeneratedSwaggerDocument();
            Assert.Equal(json, result.Content);
        }

        [Fact]
        public void GetGeneratedSwaggerDocument_ReturnsInternalServerError()
        {
            _testController.Request = new HttpRequestMessage(HttpMethod.Get, "https://local/admin/host/swagger/default");
            _swaggerDocumentManagerMock.Setup(p => p.GenerateSwaggerDocument(null)).Throws(new Exception("TestException"));
            Exception result = Assert.Throws<Exception>(() => _testController.GetGeneratedSwaggerDocument());
            Assert.Equal("TestException", result.Message);
        }

        [Fact]
        public async Task GetSwaggerDocumentAsync_ReturnsSwaggerDocument()
        {
            JObject json = new JObject();
            _testController.Request = new HttpRequestMessage(HttpMethod.Get, "https://local/admin/host/swagger/default");
            _swaggerDocumentManagerMock.Setup(p => p.GetSwaggerDocumentAsync()).ReturnsAsync(json);
            var result = (OkNegotiatedContentResult<JObject>)await _testController.GetSwaggerDocumentAsync();
            Assert.Equal(json, result.Content);
        }

        [Fact]
        public async Task GetSwaggerDocumentAsync_ReturnsInternalServerError()
        {
            JObject json = new JObject();
            _testController.Request = new HttpRequestMessage(HttpMethod.Get, "https://local/admin/host/swagger/default");
            _swaggerDocumentManagerMock.Setup(p => p.GetSwaggerDocumentAsync()).Throws(new Exception("TestException"));
            Exception result = await Assert.ThrowsAsync<Exception>(() => _testController.GetSwaggerDocumentAsync());
            Assert.Equal("TestException", result.Message);
        }

        [Fact]
        public async Task GetSwaggerDocumentAsync_ReturnsNotFound_WhenDocumentNotPresent()
        {
            JObject json = null;
            _testController.Request = new HttpRequestMessage(HttpMethod.Get, "https://local/admin/host/swagger/default");
            _swaggerDocumentManagerMock.Setup(p => p.GetSwaggerDocumentAsync()).ReturnsAsync(json);
            var result = (NotFoundResult)await _testController.GetSwaggerDocumentAsync();
            Assert.IsAssignableFrom(typeof(NotFoundResult), result);
        }

        [Fact]
        public async Task GetSwaggerDocumentAsync_ReturnsNotFound_WhenDisabled()
        {
            var testData = GetTestTargets(_testFunctions, false);
            JObject json = new JObject();
            testData.Controller.Request = new HttpRequestMessage(HttpMethod.Get, "https://local/admin/host/swagger/default");
            testData.DocumentManagerMock.Setup(p => p.GetSwaggerDocumentAsync()).ReturnsAsync(json);
            var result = (NotFoundResult)await testData.Controller.GetSwaggerDocumentAsync();
            Assert.IsAssignableFrom(typeof(NotFoundResult), result);
        }

        [Fact]
        public async Task AddOrUpdateSwaggerDocumentAsync_ReturnsSwaggerDocument()
        {
            JObject json = null;
            _testController.Request = new HttpRequestMessage(HttpMethod.Post, "https://local/admin/host/swagger/default");
            _swaggerDocumentManagerMock.Setup(p => p.AddOrUpdateSwaggerDocumentAsync(json)).ReturnsAsync(json);
            var result = (OkNegotiatedContentResult<JObject>)await _testController.AddOrUpdateSwaggerDocumentAsync(json);
            Assert.Equal(json, result.Content);
        }

        [Fact]
        public async Task AddOrUpdateSwaggerDocumentAsync_ReturnsInternalServerError()
        {
            JObject json = new JObject();
            _testController.Request = new HttpRequestMessage(HttpMethod.Post, "https://local/admin/host/swagger/default");
            _swaggerDocumentManagerMock.Setup(p => p.AddOrUpdateSwaggerDocumentAsync(It.IsAny<JObject>())).Throws(new Exception("TestException"));
            Exception result = await Assert.ThrowsAsync<Exception>(() => _testController.AddOrUpdateSwaggerDocumentAsync(json));
            Assert.Equal("TestException", result.Message);
        }

        [Fact]
        public async Task DeleteSwaggerDocumentAsync_ReturnsNoContent()
        {
            _testController.Request = new HttpRequestMessage(HttpMethod.Delete, "https://local/admin/host/swagger/default");
            _swaggerDocumentManagerMock.Setup(p => p.DeleteSwaggerDocumentAsync()).ReturnsAsync(true);
            var result = (StatusCodeResult)await _testController.DeleteSwaggerDocumentAsync();
            Assert.Equal(HttpStatusCode.NoContent, result.StatusCode);
        }

        [Fact]
        public async Task DeleteSwaggerDocumentAsync_ReturnsNoFound()
        {
            _testController.Request = new HttpRequestMessage(HttpMethod.Delete, "https://local/admin/host/swagger/default");
            _swaggerDocumentManagerMock.Setup(p => p.DeleteSwaggerDocumentAsync()).ReturnsAsync(false);
            var result = (NotFoundResult)await _testController.DeleteSwaggerDocumentAsync();
            Assert.IsAssignableFrom(typeof(NotFoundResult), result);
        }

        [Fact]
        public async Task DeleteSwaggerDocumentAsync_ReturnsInternalServerError()
        {
            JObject json = new JObject();
            _testController.Request = new HttpRequestMessage(HttpMethod.Delete, "https://local/admin/host/swagger/default");
            _swaggerDocumentManagerMock.Setup(p => p.DeleteSwaggerDocumentAsync()).Throws(new Exception("TestException"));
            Exception result = await Assert.ThrowsAsync<Exception>(() => _testController.DeleteSwaggerDocumentAsync());
            Assert.Equal("TestException", result.Message);
        }

        private(SwaggerController Controller, Mock<ISwaggerDocumentManager> DocumentManagerMock, Mock<ScriptHost> HostMock, Mock<WebScriptHostManager> HostManagerMock) GetTestTargets(Collection<FunctionDescriptor> testFunctions, bool swaggerEnabled)
        {
            var builder = new ScriptHostConfiguration.Builder();
            if (swaggerEnabled)
            {
                builder.EnableSwagger();
            }
            ScriptHostConfiguration config = builder.Build();

            var settingsManager = new ScriptSettingsManager();
            var environment = new NullScriptHostEnvironment();
            var hostMock = new Mock<ScriptHost>(MockBehavior.Strict, new object[] { environment, config, null });
            hostMock.Setup(p => p.Functions).Returns(testFunctions);

            WebHostEnvironmentSettings settings = new WebHostEnvironmentSettings();
            settings.SecretsPath = _secretsDirectory.Path;
            var managerMock = new Mock<WebScriptHostManager>(MockBehavior.Strict, new object[] { new TestSecretManagerFactory(), settingsManager, settings });
            managerMock.SetupGet(p => p.Instance).Returns(hostMock.Object);
            var swaggerDocumentManagerMock = new Mock<ISwaggerDocumentManager>(MockBehavior.Strict);
            var traceWriter = new TestTraceWriter(TraceLevel.Verbose);
            var testController = new SwaggerController(swaggerDocumentManagerMock.Object, managerMock.Object, traceWriter);

            return (testController, swaggerDocumentManagerMock, hostMock, managerMock);
        }
    }
}