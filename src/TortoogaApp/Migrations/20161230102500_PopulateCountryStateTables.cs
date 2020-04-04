using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using System.IO;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNet.Http;
using Microsoft.AspNetCore.Hosting;

using System.IO;

using Microsoft.Extensions.Options;
using System.Runtime.Loader;

namespace TortoogaApp.Migrations
{
    public partial class PopulateCountryStateTables : Migration
    {
        private readonly string migrationScriptBaseDir = @"Migrations/SQLScripts/";

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(File.ReadAllText(migrationScriptBaseDir + "countries.sql"));
            migrationBuilder.Sql(File.ReadAllText(migrationScriptBaseDir + "states.sql"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}