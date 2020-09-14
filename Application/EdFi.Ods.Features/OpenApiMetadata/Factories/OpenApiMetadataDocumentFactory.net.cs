// SPDX-License-Identifier: Apache-2.0
// Licensed to the Ed-Fi Alliance under one or more agreements.
// The Ed-Fi Alliance licenses this file to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICES files in the project root for more information.

#if NETFRAMEWORK
using System;
using System.Collections.Generic;
using System.Linq;
using EdFi.Ods.Api.Constants;
using EdFi.Ods.Features.OpenApiMetadata.Dtos;
using EdFi.Ods.Features.OpenApiMetadata.Models;
using EdFi.Ods.Features.OpenApiMetadata.Strategies.ResourceStrategies;
using log4net;
using Newtonsoft.Json;

namespace EdFi.Ods.Features.OpenApiMetadata.Factories
{
    public class OpenApiMetadataDocumentFactory
    {
        private readonly OpenApiMetadataDefinitionsFactory _definitionsFactory;
        private readonly OpenApiMetadataDocumentContext _documentContext;

        private readonly ILog _logger = LogManager.GetLogger(typeof(OpenApiMetadataDocumentFactory));
        private readonly OpenApiMetadataParametersFactory _parametersFactory;
        private readonly OpenApiMetadataPathsFactory _pathsFactory;
        private readonly OpenApiMetadataResponsesFactory _responsesFactory;
        private readonly OpenApiMetadataTagsFactory _tagsFactory;

        public OpenApiMetadataDocumentFactory(OpenApiMetadataDocumentContext openApiMetadataDocumentContext)
            : this(
                new OpenApiMetadataParametersFactory(),
                OpenApiMetadataDocumentFactoryHelper.CreateOpenApiMetadataDefinitionsFactory(openApiMetadataDocumentContext),
                new OpenApiMetadataResponsesFactory(),
                OpenApiMetadataDocumentFactoryHelper.CreateOpenApiMetadataPathsFactory(openApiMetadataDocumentContext),
                OpenApiMetadataDocumentFactoryHelper.CreateOpenApiMetadataTagsFactory(openApiMetadataDocumentContext),
                openApiMetadataDocumentContext)
        { }

        internal OpenApiMetadataDocumentFactory(
            OpenApiMetadataParametersFactory parametersFactory,
            OpenApiMetadataDefinitionsFactory definitionsFactory,
            OpenApiMetadataResponsesFactory responsesFactory,
            OpenApiMetadataPathsFactory pathsFactory,
            OpenApiMetadataTagsFactory tagsFactory,
            OpenApiMetadataDocumentContext documentContext)
        {
            _parametersFactory = parametersFactory;
            _definitionsFactory = definitionsFactory;
            _responsesFactory = responsesFactory;
            _pathsFactory = pathsFactory;
            _tagsFactory = tagsFactory;
            _documentContext = documentContext;
        }

        public string Create(IOpenApiMetadataResourceStrategy resourceStrategy)
        {
            try
            {
                var resources = resourceStrategy.GetFilteredResources(_documentContext)
                                                .ToList();

                var openApiMetadataDocument = new OpenApiMetadataDocument
                {
                    info = new Info
                    {
                        title = "Ed-Fi Operational Data Store API",
                        version = $"{ApiVersionConstants.Ods}",
                        description =
                                                         "The Ed-Fi ODS / API enables applications to read and write education data stored in an Ed-Fi ODS through a secure REST interface. \n***\n > *Note: Consumers of ODS / API information should sanitize all data for display and storage. The ODS / API provides reasonable safeguards against cross-site scripting attacks and other malicious content, but the platform does not and cannot guarantee that the data it contains is free of all potentially harmful content.* \n***\n"
                    },
                    host = "%HOST%",
                    basePath = "%BASE_PATH%",
                    securityDefinitions =
                                              new Dictionary<string, SecurityScheme>
                                              {
                                                  {
                                                      "oauth2_client_credentials", new SecurityScheme
                                                                                   {
                                                                                       type = "oauth2", description =
                                                                                           "Ed-Fi ODS/API OAuth 2.0 Client Credentials Grant Type authorization",
                                                                                       flow = "application", tokenUrl = "%TOKEN_URL%",
                                                                                       scopes = new Dictionary<string, string>()
                                                                                   }
                                                  }
                                              },
                    security =
                                              new List<IDictionary<string, IEnumerable<string>>>
                                              {
                                                  new Dictionary<string, IEnumerable<string>>
                                                  {
                                                      {
                                                          "oauth2_client_credentials", new string[0]
                                                      }
                                                  }
                                              },
                    consumes = _documentContext.IsCompositeContext
                                              ? null
                                              : OpenApiMetadataDocumentHelper.GetConsumes(),
                    produces = OpenApiMetadataDocumentHelper.GetProduces(),
                    tags = _tagsFactory.Create(resources),
                    paths = _pathsFactory.Create(resources, _documentContext.IsCompositeContext),
                    definitions = _definitionsFactory.Create(resources),
                    parameters = _parametersFactory.Create(_documentContext.IsCompositeContext),
                    responses = _responsesFactory.Create()
                };

                return JsonConvert.SerializeObject(
                    openApiMetadataDocument,
                    new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    });
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw;
            }
        }
    }
}
#endif