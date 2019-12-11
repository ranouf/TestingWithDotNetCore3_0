# TestingWithDotNetCore3_0

I'm trying to make Xunit integration testing working on .Net Core 3.0. I need to be able to:
- use Dependency Injection
- seed data in a _InMemoryDataBase_
- redirect log in _ITestOuputHelper_

I finally succeeded to upgrade to .Net Core 3.0 in this commit:
=> **commit** https://github.com/ranouf/TestingWithDotNetCore3_0/commit/42db1bfdeaa04baa7f3d99541ac304a60c3e2132

##History##

First I had an issue with the ITestOutputHelper:
- Github: https://github.com/aspnet/AspNetCore/issues/17586 
- StackOverflow: https://stackoverflow.com/questions/59166798/net-core-3-0-issue-with-iclassfixture-unresolved-constructor-arguments-ites/

=> **Fix**: https://github.com/ranouf/TestingWithDotNetCore3_0/commit/3e588106f3588ac671e5bc8dbcb7b17d416fc1ee

Then I got an issue when I move the test integration projet in a subfolder which generates a _DirectoryNotFoundException_
- Github: https://github.com/aspnet/AspNetCore/issues/17707
- StackOverflow: https://stackoverflow.com/questions/59253641/net-core-3-0-directorynotfoundexception-with-xunit-integration-test

=> **Fix**: https://github.com/ranouf/TestingWithDotNetCore3_0/commit/7ef98f17e557c9d727c547f01ce43eed9e614e59

I added Autofac and I can inject a service on the API project and the Test project.


