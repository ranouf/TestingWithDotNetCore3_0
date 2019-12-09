# TestingWithDotNetCore3_0

I'm trying to make Xunit integration testing working on .Net Core 3.0.

First I had an issue with the ITestOutputHelper:
- Github: https://github.com/aspnet/AspNetCore/issues/17586 
- StackOverflow: https://stackoverflow.com/questions/59166798/net-core-3-0-issue-with-iclassfixture-unresolved-constructor-arguments-ites/

=> **Fix**: https://github.com/ranouf/TestingWithDotNetCore3_0/commit/3e588106f3588ac671e5bc8dbcb7b17d416fc1ee

I now get a new issue when I move the test integration projet in a subfolder which generates a _DirectoryNotFoundException_
- Github: https://github.com/aspnet/AspNetCore/issues/17707
- StackOverflow: https://stackoverflow.com/questions/59253641/net-core-3-0-directorynotfoundexception-with-xunit-integration-test

=> **Fix**: https://github.com/ranouf/TestingWithDotNetCore3_0/commit/7ef98f17e557c9d727c547f01ce43eed9e614e59
