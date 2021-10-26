# MSBuild Summary Files
Creates a human readable summary of the inputs and outputs of your MSBuild-based build which can be used to make sense of changes and assist with migrations.  This includes source files, compiler constants, reference assemblies, and files placed during the build.  The summary files are commited to source control so that changes to build logic show how the change alters the build inputs and outputs.

**Seeing changes in a pull request**
When someone sends a pull request that changes build logic, you will see rich diffs showing that that altered a build.

For example, this is the result of adding a new `<PackageReference />`:

![image](https://user-images.githubusercontent.com/17556515/138954657-ed4612d3-5dd6-4cf9-b1be-70fd6cd8ca0e.png)

**Assisting with migrations**
Sometimes when projects are migrated from one system to another, it can be difficult to understand how the migration affected the outcome of a build.

For example, this project was migrated from .NET Framework to .NET Core:

![image](https://user-images.githubusercontent.com/17556515/138954611-5b0b4b32-294c-439f-8cf2-e57cae9b5c79.png)

This project was migrated from Legacy CSPROJ to an SDK-style project:

![image](https://user-images.githubusercontent.com/17556515/138954567-50665c04-e159-4206-aec6-ec783e14d5e9.png)
