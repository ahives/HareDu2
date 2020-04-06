//#tool nuget:?package=NUnit.ConsoleRunner&version=3.4.0
//#tool nuget:?package=GitVersion.CommandLine

//using Cake.Common.Tools.DotNetCore.Pack;

var target = Argument<string>("target", "Default");
var configuration = Argument<string>("configuration", "Release");
var package = Argument<bool>("package", true);
var prerelease = Argument<bool>("develop", false);
var semver = Argument<string>("semver");

var product = "HareDu2";
var copyright = "Copyright (c) 2013-2020 Albert L. Hives, et al.";
var solutionInfo = "./src/HareDu/SolutionVersion.cs";

var solution = "./src/HareDu.sln";
var brokerBuildPath = (DirectoryPath)(Directory("./src/HareDu/bin") + Directory(configuration));
var coreBuildPath = (DirectoryPath)(Directory("./src/HareDu.Core/bin") + Directory(configuration));
var snapshotBuildPath = (DirectoryPath)(Directory("./src/HareDu.Snapshotting/bin") + Directory(configuration));
var diagnosticsBuildPath = (DirectoryPath)(Directory("./src/HareDu.Diagnostics/bin") + Directory(configuration));
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

Task("Clean")
    .Does(() =>
    {
        CleanDirectory("./artifacts");
    });

Task("Restore")
    .Does(() =>
{
    DotNetCoreRestore("./src");
});


Task("Build")
    .IsDependentOn("Restore")
    .Does(() =>
    {
        var config = new DotNetCoreBuildSettings
        {
            Configuration = configuration,
            MSBuildSettings = new DotNetCoreMSBuildSettings()
        };
        
        DotNetCoreBuild("./src", config);
    });


/*
Task("Copy-Build-Artifacts")
    .IsDependentOn("Build")
    .Does(() =>
    {
        EnsureDirectoryExists(bin462Path);

        CopyFiles(string.Format("{0}/net462/*.dll", coreBuildPath.ToString()), bin462Path);
        CopyFiles(string.Format("{0}/net462/*.pdb", coreBuildPath.ToString()), bin462Path);
        CopyFiles(string.Format("{0}/net462/*.xml", coreBuildPath.ToString()), bin462Path);
        CopyFiles(string.Format("{0}/net462/*.yaml", coreBuildPath.ToString()), bin462Path);
        CopyFiles(string.Format("{0}/net462/*.config", coreBuildPath.ToString()), bin462Path);

        CopyFiles(string.Format("{0}/net462/*.dll", brokerBuildPath.ToString()), bin462Path);
        CopyFiles(string.Format("{0}/net462/*.pdb", brokerBuildPath.ToString()), bin462Path);
        CopyFiles(string.Format("{0}/net462/*.xml", brokerBuildPath.ToString()), bin462Path);
        CopyFiles(string.Format("{0}/net462/*.yaml", brokerBuildPath.ToString()), bin462Path);
        CopyFiles(string.Format("{0}/net462/*.config", brokerBuildPath.ToString()), bin462Path);

        CopyFiles(string.Format("{0}/net462/*.dll", snapshotBuildPath.ToString()), bin462Path);
        CopyFiles(string.Format("{0}/net462/*.pdb", snapshotBuildPath.ToString()), bin462Path);
        CopyFiles(string.Format("{0}/net462/*.xml", snapshotBuildPath.ToString()), bin462Path);
        CopyFiles(string.Format("{0}/net462/*.yaml", snapshotBuildPath.ToString()), bin462Path);
        CopyFiles(string.Format("{0}/net462/*.config", snapshotBuildPath.ToString()), bin462Path);

        CopyFiles(string.Format("{0}/net462/*.dll", diagnosticsBuildPath.ToString()), bin462Path);
        CopyFiles(string.Format("{0}/net462/*.pdb", diagnosticsBuildPath.ToString()), bin462Path);
        CopyFiles(string.Format("{0}/net462/*.xml", diagnosticsBuildPath.ToString()), bin462Path);
        CopyFiles(string.Format("{0}/net462/*.yaml", diagnosticsBuildPath.ToString()), bin462Path);
        CopyFiles(string.Format("{0}/net462/*.config", diagnosticsBuildPath.ToString()), bin462Path);

        EnsureDirectoryExists(binCorePath);

        CopyFiles(string.Format("{0}/netstandard2.0/*.dll", coreBuildPath.ToString()), binCorePath);
        CopyFiles(string.Format("{0}/netstandard2.0/*.pdb", coreBuildPath.ToString()), binCorePath);
        CopyFiles(string.Format("{0}/netstandard2.0/*.xml", coreBuildPath.ToString()), binCorePath);
        CopyFiles(string.Format("{0}/netstandard2.0/*.yaml", coreBuildPath.ToString()), binCorePath);
        CopyFiles(string.Format("{0}/netstandard2.0/*.config", coreBuildPath.ToString()), binCorePath);

        CopyFiles(string.Format("{0}/netstandard2.0/*.dll", brokerBuildPath.ToString()), binCorePath);
        CopyFiles(string.Format("{0}/netstandard2.0/*.pdb", brokerBuildPath.ToString()), binCorePath);
        CopyFiles(string.Format("{0}/netstandard2.0/*.xml", brokerBuildPath.ToString()), binCorePath);
        CopyFiles(string.Format("{0}/netstandard2.0/*.yaml", brokerBuildPath.ToString()), binCorePath);
        CopyFiles(string.Format("{0}/netstandard2.0/*.config", brokerBuildPath.ToString()), binCorePath);

        CopyFiles(string.Format("{0}/netstandard2.0/*.dll", snapshotBuildPath.ToString()), binCorePath);
        CopyFiles(string.Format("{0}/netstandard2.0/*.pdb", snapshotBuildPath.ToString()), binCorePath);
        CopyFiles(string.Format("{0}/netstandard2.0/*.xml", snapshotBuildPath.ToString()), binCorePath);
        CopyFiles(string.Format("{0}/netstandard2.0/*.yaml", snapshotBuildPath.ToString()), binCorePath);
        CopyFiles(string.Format("{0}/netstandard2.0/*.config", snapshotBuildPath.ToString()), binCorePath);

        CopyFiles(string.Format("{0}/netstandard2.0/*.dll", diagnosticsBuildPath.ToString()), binCorePath);
        CopyFiles(string.Format("{0}/netstandard2.0/*.pdb", diagnosticsBuildPath.ToString()), binCorePath);
        CopyFiles(string.Format("{0}/netstandard2.0/*.xml", diagnosticsBuildPath.ToString()), binCorePath);
        CopyFiles(string.Format("{0}/netstandard2.0/*.yaml", diagnosticsBuildPath.ToString()), binCorePath);
        CopyFiles(string.Format("{0}/netstandard2.0/*.config", diagnosticsBuildPath.ToString()), binCorePath);
        
        CopyFileToDirectory("license", artifactsBasePath);
    });
*/


Task("Pack")
    .IsDependentOn("Build")
    .Does(() =>
    {
        var settings = new DotNetCorePackSettings{
            OutputDirectory = "./artifacts",
            Configuration = configuration,
            MSBuildSettings = new DotNetCoreMSBuildSettings()
        };

        DotNetCorePack("./src/*", settings);
    });


Task("Default")
    .IsDependentOn("Clean")
    .IsDependentOn("Restore")
    .IsDependentOn("Build");
//    .IsDependentOn("Pack");

RunTarget(target);