#tool nuget:?package=NUnit.ConsoleRunner&version=3.4.0
//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var assemblyInfo = ParseAssemblyInfo(@".\winsvc\Properties\AssemblyInfo.cs");

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////


Task("Restore-NuGet-Packages")
    .Does(() =>
{
    NuGetRestore("./winsvc.sln");
});

Task("Debug-Build")
    .IsDependentOn("Restore-NuGet-Packages")
    .Does(() =>
{
    MSBuild("./winsvc.sln", settings => 
    {
        settings.SetConfiguration("Debug");
        settings.Verbosity = Verbosity.Quiet; 
	    settings.UseToolVersion(MSBuildToolVersion.VS2017);
    });
});

Task("Release-Build")
    .IsDependentOn("Restore-NuGet-Packages")
    .Does(() =>
{
    MSBuild("./winsvc.sln", settings => 
    {
        settings.SetConfiguration("Release");
        settings.Verbosity = Verbosity.Quiet; 
    });
});

Task("Build")
    .IsDependentOn("Debug-Build")
    .IsDependentOn("Release-Build");

Task("Run-Unit-Tests-Debug-x64")
    .IsDependentOn("Build")
    .Does(() =>
{
    NUnit3(@".\winsvc.tests\bin\Debug\winsvc.tests.dll", new NUnit3Settings {
        NoResults = true,
        X86 = false,
        });
});

Task("Run-Unit-Tests-Debug-x86")
    .IsDependentOn("Build")
    .Does(() =>
{
    NUnit3(@".\winsvc.tests\bin\Debug\winsvc.tests.dll", new NUnit3Settings {
        NoResults = true,
        X86 = true,
        });
});

Task("Run-Unit-Tests-Release-x64")
    .IsDependentOn("Build")
    .Does(() =>
{
    NUnit3(@".\winsvc.tests\bin\Release\winsvc.tests.dll", new NUnit3Settings {
        NoResults = true,
        X86 = false,
        });
});

Task("Run-Unit-Tests-Release-x86")
    .IsDependentOn("Build")
    .Does(() =>
{
    NUnit3(@".\winsvc.tests\bin\Release\winsvc.tests.dll", new NUnit3Settings {
        NoResults = true,
        X86 = true,
        });
});

Task("Run-Unit-Tests")
    .IsDependentOn("Run-Unit-Tests-Debug-x86")
    .IsDependentOn("Run-Unit-Tests-Debug-x64")
    .IsDependentOn("Run-Unit-Tests-Release-x86")
    .IsDependentOn("Run-Unit-Tests-Release-x64");

Task("NuGetPack")
    .IsDependentOn("Run-Unit-Tests")
    .Does(() => {
        var settings = new NuGetPackSettings {
            BasePath = @".\winsvc",
            OutputDirectory = @"C:\Data\Builds\nuget\winsvc",
            Files = new [] {
                new NuSpecContent {
                    Source = @".\bin\Release\winsvc.dll",
                    Target = @"lib\net40",
                },
                new NuSpecContent {
                    Source = @".\bin\Release\winsvc.xml",
                    Target = @"lib\net40",
                }
            },
            Version = assemblyInfo.AssemblyVersion,
        };
        NuGetPack("./winsvc/winsvc.nuspec", settings);
    });

Task("Default")
    .IsDependentOn("NuGetPack");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);
