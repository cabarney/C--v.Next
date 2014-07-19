using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace BraceChecker
{
    // TODO: Consider implementing other interfaces that implement IDiagnosticAnalyzer instead of or in addition to ISymbolAnalyzer

    [DiagnosticAnalyzer]
    [ExportDiagnosticAnalyzer(DiagnosticId, LanguageNames.CSharp)]
    public class DiagnosticAnalyzer : ISyntaxNodeAnalyzer<SyntaxKind>
    {
        internal const string DiagnosticId = "BraceAnalysisTest";
        internal const string Description = "If and Else Statements should contain braces";
        internal const string MessageFormat = "{0} bodies should be contained in braces";
        internal const string Category = "Naming";

        internal static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Description, MessageFormat, Category, DiagnosticSeverity.Warning);

        public ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

        public ImmutableArray<SyntaxKind> SyntaxKindsOfInterest
        {
            get
            {
                return ImmutableArray.Create(SyntaxKind.IfStatement, SyntaxKind.ElseClause);
            }
        }

        public void AnalyzeNode(SyntaxNode node, SemanticModel semanticModel, Action<Diagnostic> addDiagnostic, CancellationToken cancellationToken)
        {
            var ifStatment = node as IfStatementSyntax;
            if (ifStatment != null && 
                ifStatment.Statement != null && 
                !ifStatment.Statement.IsKind(SyntaxKind.Block))
            {
                addDiagnostic(Diagnostic.Create(Rule, ifStatment.IfKeyword.GetLocation(), "if statement"));
            }

            var elseClause = node as ElseClauseSyntax;
            if (elseClause != null && elseClause.Statement != null && !elseClause.Statement.IsKind(SyntaxKind.Block) && !elseClause.Statement.IsKind(SyntaxKind.IfStatement))
            {
                addDiagnostic(Diagnostic.Create(Rule, elseClause.ElseKeyword.GetLocation(), "else clauses"));
            }
        }
    }
}
