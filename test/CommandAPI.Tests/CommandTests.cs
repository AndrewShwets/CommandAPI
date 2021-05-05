using System;
using Xunit;
using CommandAPI.Models;

namespace CommandAPI.Tests
{
    public class CommandTests : IDisposable
    {
        Command command;

        public CommandTests()
        {
            command = new Command
            {
                HowTo = "Do something awesome",
                Platform = "xUnit",
                CommandLine = "dotnet test"
            };
        }

        [Fact]
        public void CanChangeHowTo()
        {
            //Act
            command.HowTo = "Execute Unit Tests";
            //Assert
            Assert.Equal("Execute Unit Tests", command.HowTo);        
        }

        [Fact]
        public void CanChangePlatform()
        {
            //Act
            command.Platform = "node js";
            //Assert
            Assert.Equal("node js", command.Platform);
        }

        [Fact]
        public void CanChangeCommandLine()
        {
            command.CommandLine = "npm i && npm start";

            Assert.Equal("npm i && npm start", command.CommandLine);
        }

        public void Dispose()
        {
            command = null;
        }
    }
}
