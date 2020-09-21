﻿// SPDX-License-Identifier: Apache-2.0
// Licensed to the Ed-Fi Alliance under one or more agreements.
// The Ed-Fi Alliance licenses this file to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICES files in the project root for more information.

#if NETSTANDARD
using Autofac;
using EdFi.Ods.Common.Security.Authorization;
using EdFi.Ods.Common.Security.Claims;
using EdFi.Ods.Security.Authorization;
using EdFi.Ods.Security.AuthorizationStrategies;

namespace EdFi.Ods.Security.Container.Modules
{
    public class AuthorizationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<EdFiAuthorizationProvider>().As<IEdFiAuthorizationProvider>();
            builder.RegisterType<AuthorizationSegmentsVerifier>().As<IAuthorizationSegmentsVerifier>();
            builder.RegisterType<ResourceAuthorizationMetadataProvider>().As<IResourceAuthorizationMetadataProvider>();
        }
    }
}
#endif
