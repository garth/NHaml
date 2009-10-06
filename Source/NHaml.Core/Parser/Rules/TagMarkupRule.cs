using System;
using NHaml.Core.Ast;

namespace NHaml.Core.Parser.Rules
{
    public class TagMarkupRule : MarkupRuleBase
    {
        public override string[] Signifiers
        {
            get { return new[] {"%", ".", "#"}; }
        }

        public override AstNode Process(ParserReader parser)
        {
            var reader = new CharacterReader(parser.Text);
            var baseIndent = parser.Indent;

            if(!reader.Read()) // initial read
                return null;

            var node = CreateTagNode(reader);

            while(!reader.IsEndOfStream)
                switch(reader.Current)
                {
                    case '#':
                    {
                        reader.Read(); // eat #

                        var attribute = node.Attributes.Find(a => a.Name.Equals("id", StringComparison.InvariantCultureIgnoreCase));

                        if(attribute == null)
                        {
                            attribute = new AttributeNode("id");
                            node.Attributes.Add(attribute);
                        }

                        attribute.Value = parser.ParseText(reader.ReadWhile(IsNameChar));

                        continue;
                    }
                    case '.':
                    {
                        reader.Read(); // eat .

                        node.Attributes.Add(new AttributeNode("class")
                        {
                            Value = parser.ParseText(reader.ReadWhile(IsNameChar))
                        });

                        continue;
                    }
                    case '=':
                    {
                        reader.Read(); // eat =

                        reader.ReadWhile(c => char.IsWhiteSpace(c));

                        node.Child = new CodeNode(reader.ReadToEnd());

                        break;
                    }
                    case '(':
                    {
                        ParseAttributes(node, ref reader, parser);
                        break;
                    }
                    case '{':
                    {
                        ParseRubyLikeAttributes(node, ref reader, parser);
                        break;
                    }
                    default:
                    {
                        var text = reader.ReadToEnd();
                        node.Child = parser.ParseText(text.TrimStart());

                        break;
                    }
                }

            node.Child = parser.ParseChildren(baseIndent, node.Child);

            return node;
        }

        private static TagNode CreateTagNode(CharacterReader reader)
        {
            TagNode node;
            if(reader.Current == '%')
            {
                reader.Read(); // eat %

                var name = reader.ReadWhile(IsNameChar);

                node = new TagNode(name);
            }
            else
                node = new TagNode("div");
            return node;
        }

        public void ParseAttributes(TagNode node, ref CharacterReader reader, ParserReader parser)
        {
            reader.Read(); // eat (

            while(reader.Current != ')')
            {
                reader.ReadWhiteSpaces();

                if(reader.IsEndOfStream)
                {
                    if(!parser.Read())
                        break;

                    reader = new CharacterReader(parser.Text);
                    reader.Read();
                }

                var name = reader.ReadName();

                reader.ReadWhile(c => char.IsWhiteSpace(c));

                //Todo: report error when there is no =
                reader.Read(); // =

                reader.ReadWhiteSpaces();

                var attribute = new AttributeNode(name);
                node.Attributes.Add(attribute);
                switch(reader.Current)
                {
                    case '\'':
                    {
                        reader.Read(); // skip '
                        attribute.Value = parser.ParseText(reader.ReadWhile(c => c != '\''));
                        reader.Read(); // skip '
                        break;
                    }
                    case '"':
                    {
                        reader.Read(); // skip "
                        attribute.Value = parser.ParseText(reader.ReadWhile(c => c != '"'));
                        reader.Read(); // skip "
                        break;
                    }
                    default:
                    {
                        attribute.Value = new CodeNode(reader.ReadWhile(IsNameChar));
                        break;
                    }
                }
            }
        }

        public void ParseRubyLikeAttributes(TagNode node, ref CharacterReader reader, ParserReader parser)
        {
            reader.Read(); // eat {

            while(reader.Current != '}')
            {
                reader.ReadWhiteSpaces();

                if(reader.IsEndOfStream)
                {
                    if(!parser.Read())
                        break;

                    reader = new CharacterReader(parser.Text);
                    reader.Read();
                }

                string name = null;
                switch(reader.Current)
                {
                    case ':':
                        reader.Read(); // eat :
                        name = reader.ReadName();
                        break;
                    case '\'':
                        reader.Read(); // eat '
                        name = reader.ReadWhile(c => c != '\'');
                        reader.Read(); // eat '
                        break;
                    default:
                        reader.Read(); // eat char
                        break;
                }

                reader.ReadWhiteSpaces();

                //Todo: report error when there is no =>
                reader.Read(2); // =>

                reader.ReadWhiteSpaces();

                var attribute = new AttributeNode(name);
                node.Attributes.Add(attribute);
                switch(reader.Current)
                {
                    case '\'':
                    {
                        reader.Read(); // skip '
                        attribute.Value = parser.ParseText(reader.ReadWhile(c => c != '\''));
                        reader.Read(); // skip '
                        break;
                    }
                    case '"':
                    {
                        reader.Read(); // skip "
                        attribute.Value = parser.ParseText(reader.ReadWhile(c => c != '"'));
                        reader.Read(); // skip "
                        break;
                    }
                    default:
                    {
                        attribute.Value = new CodeNode(reader.ReadWhile(IsNameChar));
                        break;
                    }
                }

                reader.ReadWhiteSpaces();

                //if(reader.Current!='}'&&reader.Current!=',')
                // report error here

                if(reader.Current != '}')
                    reader.Read(); // eat ,
            }
        }
    }
}