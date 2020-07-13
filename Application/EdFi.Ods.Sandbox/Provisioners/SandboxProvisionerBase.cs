// SPDX-License-Identifier: Apache-2.0
// Licensed to the Ed-Fi Alliance under one or more agreements.
// The Ed-Fi Alliance licenses this file to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICES files in the project root for more information.

#if NETSTANDARD
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using EdFi.Admin.DataAccess;
using EdFi.Admin.DataAccess.Utils;
using EdFi.Ods.Common.Database;
using Microsoft.Extensions.Configuration;

namespace EdFi.Ods.Sandbox.Provisioners
{
    public abstract class SandboxProvisionerBase : ISandboxProvisioner
    {
        private readonly IAdminDatabaseConnectionStringProvider _connectionStringProvider;
        private ObjectContext _context;

        protected SandboxProvisionerBase(IAdminDatabaseConnectionStringProvider connectionStringProvider, IConfigurationRoot configurationRoot)
        {
            _connectionStringProvider = connectionStringProvider;

            CommandTimeout = int.TryParse(configurationRoot.GetSection("SandboxAdminSQLCommandTimeout").Value, out int timeout)
                ? timeout
                : 30;
        }

        public int CommandTimeout { get; set; }

        protected ObjectContext ObjectContext
        {
            get
            {
                if (_context == null)
                {
                    var tmp = new DbContext(_connectionStringProvider.GetConnectionString());
                    _context = (tmp as IObjectContextAdapter).ObjectContext;

                    _context.CommandTimeout = CommandTimeout;
                }

                return _context;
            }
        }

        public void AddSandbox(string sandboxKey, SandboxType sandboxType)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteSandboxes(params string[] deletedClientKeys)
        {
            throw new System.NotImplementedException();
        }

        public SandboxStatus GetSandboxStatus(string clientKey) => throw new System.NotImplementedException();

        public void ResetDemoSandbox()
        {
            throw new System.NotImplementedException();
        }

        public async Task<string[]> GetSandboxDatabasesAsync()
        {
            object[] parameters = {new SqlParameter("@name", DatabaseNameBuilder.SandboxNameForKey("%"))};

            var result = await ObjectContext.ExecuteStoreQueryAsync<string>(
                "SELECT name FROM sys.databases WHERE name like @name;",
                parameters);

            return result.ToArray();
        }
    }
}
#endif