using NHaml.Core.Parser;
using Xunit;

namespace NHaml.Core.Tests.Parser
{
    public class CharacterReaderTests
    {
        [Fact]
        public void Reads4Chars()
        {
            var reader = new CharacterReader("test");

            Assert.True(reader.Read());
            Assert.Equal('t', reader.Current);
            Assert.Equal('e', reader.Next);
            Assert.True(reader.Read());
            Assert.Equal('e', reader.Current);
            Assert.Equal('s', reader.Next);
            Assert.True(reader.Read());
            Assert.Equal('s', reader.Current);
            Assert.Equal('t', reader.Next);
            Assert.True(reader.Read());
            Assert.Equal('t', reader.Current);
            Assert.False(reader.Read());
        }

        [Fact]
        public void ReadToEndReadsTestOneRead()
        {
            var reader = new CharacterReader("test");
            reader.Read(); // Current = t

            Assert.Equal("test", reader.ReadToEnd());
        }

        [Fact]
        public void ReadToEndReadsTest()
        {
            var reader = new CharacterReader("1test");
            reader.Read(); // Current = 1
            reader.Read(); // Current = t
            
            Assert.Equal("test",reader.ReadToEnd());
        }

        [Fact]
        public void ReadWhileReadsTestOneRead()
        {
            var reader = new CharacterReader("test1234");
            reader.Read(); // Current = t

            var result = reader.ReadWhile(c => char.IsLetter(c));

            Assert.Equal("test", result);
        }

        [Fact]
        public void ReadWhileReadsTest()
        {
            var reader = new CharacterReader("1test1234");
            reader.Read(); // Current = 1
            reader.Read(); // Current = t

            var result = reader.ReadWhile(c => char.IsLetter(c));

            Assert.Equal("test",result);
        }

        [Fact]
        public void ReadWhileEndsOnTheRightChar()
        {
            var reader = new CharacterReader("test1234");

            reader.ReadWhile(c => char.IsLetter(c));

            Assert.Equal('1', reader.Current);
        }
    }
}