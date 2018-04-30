#tool nuget:?package=NUnit.ConsoleRunner&version=3.4.0
#tool nuget:?package=GitVersion.CommandLine

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
var artifactsBasePath = (DirectoryPath)Directory("./artifacts");
var baseBinPath = artifactsBasePath.Combine("bin");
var bin452Path = baseBinPath.Combine("lib/net452");
var nugetPath = artifactsBasePath.Combine("nuget");
var bin452FullPath = MakeAbsolute(bin452Path).FullPath;

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

        MSBuild(solution, settings => settings.SetConfiguration(configuration));
    });

Task("Copy-Build-Artifacts")
    .IsDependentOn("Build")
    .Does(() =>
    {
        EnsureDirectoryExists(bin452Path);

        CopyFiles(string.Format("{0}/*.dll", buildPath.ToString()), bin452Path);
        CopyFiles(string.Format("{0}/*.pdb", buildPath.ToString()), bin452Path);
        CopyFiles(string.Format("{0}/*.xml", buildPath.ToString()), bin452Path);
        CopyFiles(string.Format("{0}/*.config", buildPath.ToString()), bin452Path);
        CopyFileToDirectory("license", artifactsBasePath);
    });

Task("Create-NuGet-Package")
    .WithCriteria(() => package == true)
    .IsDependentOn("Copy-Build-Artifacts")
    .Does(() =>
    {
        var files = GetFiles(bin452Path.ToString());
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
            Authors = new []{"Albert L. Hives", "Chris Patterson"},
            Dependencies = new []
            {
                new NuSpecDependency { Id = "Newtonsoft.Json", Version = "11.0.2" }
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