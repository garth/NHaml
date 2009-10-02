using NHaml.Core.Ast;

namespace NHaml.Core.Parser.Rules
{
    public class FilterRule : MarkupRuleBase
    {
        public override string[] Signifiers
        {
            get { return new[]{":"};}
        }

        public override AstNode Process(ParserReader parserReader)
        {
            var reader = new CharacterReader(parserReader.Text);
            reader.Read(1); // eat :

            var name = reader.ReadWhile(IsNameChar);

            var node = new FilterNode(name);

            node.Child = parserReader.ParseChildren(parserReader.Indent, node.Child);
            
            return node;
        }
    }
}