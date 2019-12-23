#tool nuget:?package=NUnit.ConsoleRunner&version=3.4.0
#tool nuget:?package=GitVersion.CommandLine

//using Cake.Common.Tools.DotNetCore.Pack;

var target = Argument<string>("target", "Default");
var configuration = Argument<string>("configuration", "Release");
var package = Argument<bool>("package", true);
var prerelease = Argument<bool>("develop", false);
var semver = Argument<string>("semver");

var product = "HareDu2";
var copyright = string.Format("Copyright (c) 2013-{0} Albert L. Hives, et al.", DateTime.Now.Year);
var solutionInfo = "./src/HareDu/SolutionVersion.cs";

var solution = "./src/HareDu.sln";
var buildPath = (DirectoryPath)(Directory("./src/HareDu/bin") + Directory(configuration));
var coreBuildPath = (DirectoryPath)(Directory("./src/HareDu.Core/bin") + Directory(configuration));
var snapshottingBuildPath = (DirectoryPath)(Directory("./src/HareDu.Snapshotting/bin") + Directory(configuration));
var coreSnapshottingBuildPath = (DirectoryPath)(Directory("./src/HareDu.Snapshotting/bin") + Directory(configuration));
// var coreBuildPath = (DirectoryPath)(Directory("./src/HareDu/bin") + Directory(configuration));
var artifactsBasePath = (DirectoryPath)Directory("./artifacts");
var baseBinPath = artifactsBasePath.Combine("bin");
var bin462Path = baseBinPath.Combine("lib/net462");
var binCorePath = baseBinPath.Combine("lib/netstandard2.0");
var nugetPath = artifactsBasePath.Combine("nuget");
var bin462FullPath = MakeAbsolute(bin462Path).FullPath;

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

Task("Perform-Versioning")
    .IsDependentOn("Restore-NuGet-Packages")
    .Does(() =>
    {
        var asmInfoSettings = new AssemblyInfoSettings
        {
            Product = product,
            Copyright = copyright,
            InformationalVersion = semver,
            FileVersion = semver,
            Version = prerelease ? string.Format("{0}-prerelease", semver) : semver
        };

        CreateAssemblyInfo(solutionInfo, asmInfoSettings);
    });

Task("Build")
    .IsDependentOn("Perform-Versioning")
    .Does(() =>
    {
        // Add support for .NET Standard, .NET Core
        var config = new DotNetCoreBuildSettings
        {
            Framework = "netstandard2.0",
            Configuration = configuration,
        }
        DotNetCoreBuild("./src/*", config);
        //MSBuild(solution, settings => settings.SetConfiguration(configuration));
    });

Task("Copy-Build-Artifacts")
    .IsDependentOn("Build")
    .Does(() =>
    {
        EnsureDirectoryExists(bin462Path);

        CopyFiles(string.Format("{0}/net462/*.dll", snapshottingBuildPath.ToString()), bin462Path);
        CopyFiles(string.Format("{0}/net462/*.pdb", snapshottingBuildPath.ToString()), bin462Path);
        CopyFiles(string.Format("{0}/net462/*.xml", snapshottingBuildPath.ToString()), bin462Path);
        CopyFiles(string.Format("{0}/net462/*.yaml", snapshottingBuildPath.ToString()), bin462Path);
        CopyFiles(string.Format("{0}/net462/*.config", snapshottingBuildPath.ToString()), bin462Path);

        EnsureDirectoryExists(binCorePath);

        CopyFiles(string.Format("{0}/netstandard2.0/*.dll", coreBuildPath.ToString()), binCorePath);
        CopyFiles(string.Format("{0}/netstandard2.0/*.pdb", coreBuildPath.ToString()), binCorePath);
        CopyFiles(string.Format("{0}/netstandard2.0/*.xml", coreBuildPath.ToString()), binCorePath);
        CopyFiles(string.Format("{0}/netstandard2.0/*.yaml", coreBuildPath.ToString()), binCorePath);
        CopyFiles(string.Format("{0}/netstandard2.0/*.config", coreBuildPath.ToString()), binCorePath);
        
        CopyFileToDirectory("license", artifactsBasePath);
    });

Task("Create-NuGet-Package")
    .WithCriteria(() => package == true)
    .IsDependentOn("Copy-Build-Artifacts")
    .Does(() =>
    {
        var nuGetPackSettings = new NuGetPackSettings
            {
                OutputDirectory = rootAbsoluteDir + @"\artifacts\",
                IncludeReferencedProjects = true,
                Properties = new Dictionary<string, string>
                {
                    { "Configuration", "Release" }
                }
            };
        var files = GetFiles(bin462Path.ToString());
        var nuspecBasePath = new DirectoryPath("./artifacts/nuspec");
        //var nuspec = string.Format("{0}.nuspec", product);
        var nuspec = "HareDu.nuspec";

        EnsureDirectoryExists(nuspecBasePath);

        var assemblyInfo = ParseAssemblyInfo(solutionInfo);

        var packageSettings = new NuGetPackSettings
        {
            Id = product,
            Title = product,
            Description = "HareDu is a .NET API that can be used to manage and monitor a RabbitMQ server or cluster.",
            Copyright = copyright,
            ProjectUrl = new Uri("http://github.com/ahives/HareDu2"),
            LicenseUrl = new Uri("http://www.apache.org/licenses/LICENSE-2.0"),
            Owners = new []{"Albert L. Hives"},
            Authors = new []{"Albert L. Hives"},
            Dependencies = new []
            {
                new NuSpecDependency { Id = "Newtonsoft.Json", Version = "12.0.2" }
            },
            Version = assemblyInfo.AssemblyVersion,
            BasePath = bin452Path,
            OutputDirectory = nuspecBasePath,
            RequireLicenseAcceptance = true,
            Symbols = false,
            Files = files
                .Select(file => new NuSpecContent { Source = file.ToString(), Target = "lib" })
                .ToArray()
        };

        NuGetPack(nuspec, packageSettings);
    });

Task("Default")
    .IsDependentOn("Create-NuGet-Package");

RunTarget(target);