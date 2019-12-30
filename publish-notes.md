1) Bump version in csproj
2) Run `dotnet pack -c Release` to create nuget package
	- produces a "Pioneer.Pagination.x.x.x.nupkg" file
3) Publish package

-----
Projects settings now ouputs a nuget packed on build. Above command can still work but is no longer needed.
Switch to release and build. 
