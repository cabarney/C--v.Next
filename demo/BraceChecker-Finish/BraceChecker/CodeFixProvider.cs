using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Rename;
using Microsoft.CodeAnalysis.Text;
using Microsoft.CodeAnalysis.Formatting;

namespace BraceChecker
{
    [ExportCodeFixProvider(DiagnosticAnalyzer.DiagnosticId, LanguageNames.CSharp)]
    internal class CodeFixProvider : ICodeFixProvider
    {
        public IEnumerable<string> GetFixableDiagnosticIds()
        {
            return new[] { DiagnosticAnalyzer.DiagnosticId };
        }

        public async Task<IEnumerable<CodeAction>> GetFixesAsync(Document document, TextSpan span, IEnumerable<Diagnostic> diagnostics, CancellationToken cancellationToken)
        {
            var root = await document.GetSyntaxRootAsync(cancellationToken);
            var token = root.FindToken(span.Start);

            if (token.IsKind(SyntaxKind.IfKeyword))
            {
                var ifStatement = (IfStatementSyntax)token.Parent;
                var newStatement = ifStatement.WithStatement(SyntaxFactory.Block(ifStatement.Statement)).WithAdditionalAnnotations(Formatter.Annotation);

                var newRoot = root.ReplaceNode(ifStatement, newStatement);
                return new[] { CodeAction.Create("Add braces", document.WithSyntaxRoot(newRoot)) };
            }

            if (token.IsKind(SyntaxKind.ElseKeyword))
            {
                var elseClause = (ElseClauseSyntax)token.Parent;
                var newStatement = elseClause.WithStatement(SyntaxFactory.Block(elseClause.Statement)).WithAdditionalAnnotations(Formatter.Annotation);

                var newRoot = root.ReplaceNode(elseClause, newStatement);
                return new[] { CodeAction.Create("Add braces", document.WithSyntaxRoot(newRoot)) };
            }
            return null;
        }
    }
}