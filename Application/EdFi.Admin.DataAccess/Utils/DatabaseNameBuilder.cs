// SPDX-License-Identifier: Apache-2.0
// Licensed to the Ed-Fi Alliance under one or more agreements.
// The Ed-Fi Alliance licenses this file to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICES files in the project root for more information.

using System.Configuration;
using System.Data.SqlClient;

namespace EdFi.Admin.DataAccess.Utils
{
    public static class DatabaseNameBuilder
    {
        private const string TemplatePrefix = "EdFi_Ods_";
        private const string SandboxPrefix = TemplatePrefix + "Sandbox_";

        private const string TemplateEmptyDatabase = TemplatePrefix + "Empty_Template";
        private const string TemplateMinimalDatabase = TemplatePrefix + "Minimal_Template";
        private const string TemplateSampleDatabase = TemplatePrefix + "Populated_Template";
        public const string CodeGenDatabase = "EdFi_Ods";

        public static string EmptyDatabase
        {
            get => TemplateEmptyDatabase;
        }

        public static string MinimalDatabase
        {
            get => TemplateMinimalDatabase;
        }

        public static string SampleDatabase
        {
            get => TemplateSampleDatabase;
        }

        public static string SandboxNameForKey(string key) => SandboxPrefix + key;
    }
}
