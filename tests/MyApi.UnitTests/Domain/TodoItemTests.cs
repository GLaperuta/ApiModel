using MyApi.Domain.Models;

namespace MyApi.UnitTests.Domain
{
    public class TodoItemTests
    {
        [Fact]
        public void Rename_ShouldChangeTitle_AndVersion()
        {
            var todo = new TodoItem("a");
            var oldVersion = todo.Version;

            todo.Rename("b");

            Assert.Equal("b", todo.Title);
            Assert.NotEqual(oldVersion, todo.Version);
        }
        [Fact]
        public void Constructor_ShouldThrow_WhenTitleIsInvalid()
        {
            Assert.Throws<ArgumentException>(() => new TodoItem(""));
            Assert.Throws<ArgumentException>(() => new TodoItem("   "));
        }

        [Fact]
        public void MarkDone_ShouldSetDoneAndChangeVersion()
        {
            var todo = new TodoItem("teste");
            var oldVersion = todo.Version;

            todo.MarkDone();

            Assert.True(todo.Done);
            Assert.NotEqual(oldVersion, todo.Version);
        }

        [Fact]
        public void MarkDone_ShouldNotChangeVersion_WhenAlreadyDone()
        {
            var todo = new TodoItem("teste");
            todo.MarkDone();
            var versionAfterFirstDone = todo.Version;

            todo.MarkDone();

            Assert.Equal(versionAfterFirstDone, todo.Version);
        }

        [Fact]
        public void Rename_ShouldChangeTitleAndVersion()
        {
            var todo = new TodoItem("old");
            var oldVersion = todo.Version;

            todo.Rename("new");

            Assert.Equal("new", todo.Title);
            Assert.NotEqual(oldVersion, todo.Version);
        }

        [Fact]
        public void Rename_ShouldThrow_WhenTitleIsInvalid()
        {
            var todo = new TodoItem("teste");

            Assert.Throws<ArgumentException>(() => todo.Rename(""));
            Assert.Throws<ArgumentException>(() => todo.Rename("   "));
        }
    }
}
