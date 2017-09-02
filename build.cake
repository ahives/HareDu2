#tool nuget:?package=NUnit.ConsoleRunner&version=3.4.0
#tool nuget:?package=GitVersion.CommandLine

// Script arguments
var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var buildType = Argument("type", "local");

// Directory paths
var solution = "./src/HareDu.sln";
var buildPath = Directory("./src/HareDu/bin") + Directory(configuration);
var artifactsBasePath = (DirectoryPath)Directory("./artifacts");
var baseBinPath = artifactsBasePath.Combine("bin");
var bin452Path = baseBinPath.Combine("net452");
var nuGetPath = artifactsBasePath.Combine("nuget");
var bin452FullPath = MakeAbsolute(bin452Path).FullPath;

// Perform task initialization before and after Build Tasks
TaskSetup(context =>
    {
        var message = string.Format("Task {0} started", context.Task.Name);

        Information(message);
    });

TaskTeardown(context =>
    {
        var message = string.Format("Task {0} finished", context.Task.Name);

        Information(message);
    });

/*
    Perform the following build tasks
    1. Clean the build folder
    2. Restore all NuGet packages
    3. Build solution
    4. Create NuGet package
*/

Task("Clean-Build-Folder")
    .Does(() =>
    {
        CleanDirectory(artifactsBasePath);
    });

Task("Restore-NuGet-Packages")
    .IsDependentOn("Clean-Build-Folder")
    .Does(() =>
    {
        NuGetRestore(solution);
    });

Task("Build")
    .IsDependentOn("Restore-NuGet-Packages")
    .Does(() =>
    {
        // Add support for .NET Standard, .NET Core

        if (IsRunningOnWindows())
        {
            MSBuild(solution, settings => settings.SetConfiguration(configuration));
        }
        else
        {
            XBuild(solution, c => c.SetConfiguration(configuration)
                .SetVerbosity(Verbosity.Minimal)
                .UseToolVersion(XBuildToolVersion.NET40));
        }
    });

Task("Create-NuGet-Package")
    .WithCriteria(() => buildType == "nuget")
    .IsDependentOn("Build")
    .Does(() =>
    {
        var nuspec = "./nuspec/HareDu.nuspec";

        // Add semver here using GitVersion

        var packageSettings = new NuGetPackSettings
        {
            Id = "HareDu",
            Title = "HareDu",
            Description = "HareDu is a .NET API that can be used to manage and monitor a RabbitMQ server or cluster.",
            Copyright = "Copyright 2013-2017 Albert L. Hives, Chris Patterson",
            ProjectUrl = new Uri("http://github.com/ahives/HareDu2"),
            LicenseUrl = new Uri("http://www.apache.org/licenses/LICENSE-2.0"),
            Owners = new []{"Albert L. Hives"},
            Authors = new []{"Albert L. Hives", "Chris Patterson"},
            Dependencies = new []
            {
                new NuSpecDependency { Id = "Common.Logging", Version = "3.3.1" },
                new NuSpecDependency { Id = "Microsoft.AspNet.WebApi.Client", Version = "5.2.3" },
                new NuSpecDependency { Id = "Microsoft.Net.Http", Version = "4.3.2" },
                new NuSpecDependency { Id = "Newtonsoft.Json", Version = "10.0.3" },
                new NuSpecDependency { Id = "Polly-Signed", Version = "5.3.0" }
            },
        //    Version = ,
            BasePath = bin452Path,
            OutputDirectory = nuGetPath
        };

        NuGetPack(nuspec, packageSettings);
    });

Task("Default")
    .IsDependentOn("Create-NuGet-Package");

RunTarget(target);