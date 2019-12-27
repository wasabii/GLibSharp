using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

using GObject.Introspection.CodeGen.Model;
using GObject.Introspection.CodeGen.Model.Expressions;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Editing;

namespace GObject.Introspection.CodeGen.Syntax
{

    /// <summary>
    /// Passes some contextual information down to each syntax node builder.
    /// </summary>
    class ModuleContext
    {

        readonly SyntaxGenerator syntax;
        readonly ConcurrentDictionary<Model.Type, SyntaxTypeBuilderBase> typeBuilders;
        readonly ConcurrentDictionary<Model.Member, SyntaxMemberBuilderBase> memberBuilders;
        readonly ConcurrentDictionary<Model.Statement, SyntaxStatementBuilderBase> statementBuilders;
        readonly ConcurrentDictionary<Model.Expression, SyntaxExpressionBuilderBase> expressionBuilders;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="syntax"></param>
        public ModuleContext(SyntaxGenerator syntax)
        {
            this.syntax = syntax ?? throw new ArgumentNullException(nameof(syntax));

            typeBuilders = new ConcurrentDictionary<Model.Type, SyntaxTypeBuilderBase>();
            memberBuilders = new ConcurrentDictionary<Member, SyntaxMemberBuilderBase>();
            statementBuilders = new ConcurrentDictionary<Statement, SyntaxStatementBuilderBase>();
            expressionBuilders = new ConcurrentDictionary<Expression, SyntaxExpressionBuilderBase>();
        }

        /// <summary>
        /// Gets a reference to the syntax generator.
        /// </summary>
        public SyntaxGenerator Syntax => syntax;

        /// <summary>
        /// Builds the syntax for the given type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public IEnumerable<SyntaxNode> Build(Model.Type type)
        {
            return GetBuilder(type).Build();
        }
        /// <summary>
        /// Builds the specified node.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public SyntaxTypeBuilderBase GetBuilder(Model.Type type)
        {
            if (type is null)
                throw new ArgumentNullException(nameof(type));

            return typeBuilders.GetOrAdd(type, _ => CreateBuilder(_));
        }

        SyntaxTypeBuilderBase CreateBuilder(Model.Type type)
        {
            return type switch
            {
                ClassType t => new ClassTypeBuilder(this, t),
                StructureType t => new StructureTypeBuilder(this, t),
                EnumType t => new EnumTypeBuilder(this, t),
                DelegateType t => new DelegateTypeBuilder(this, t),
                _ => throw new NotSupportedException(type.GetType().ToString()),
            };
        }

        /// <summary>
        /// Builds the syntax for the given expression.
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        public IEnumerable<SyntaxNode> Build(Model.Member member)
        {
            return GetBuilder(member).Build();
        }

        /// <summary>
        /// Builds the specified node.
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        public SyntaxMemberBuilderBase GetBuilder(Model.Member member)
        {
            if (member is null)
                throw new ArgumentNullException(nameof(member));

            return memberBuilders.GetOrAdd(member, _ => CreateBuilder(_));
        }

        /// <summary>
        /// Builds the specified node.
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        SyntaxMemberBuilderBase CreateBuilder(Model.Member member)
        {
            return member switch
            {
                FieldMember m => new FieldMemberBuilder(this, m),
                ConstructorMember m => new ConstructorMemberBuilder(this, m),
                MethodMember m => new MethodMemberBuilder(this, m),
                PropertyMember m => new PropertyMemberBuilder(this, m),
                TypeMember m => new TypeMemberBuilder(this, m),
                EnumMember m => new EnumMemberBuilder(this, m),
                _ => throw new NotSupportedException(member.GetType().ToString()),
            };
        }

        /// <summary>
        /// Builds the syntax for the given statement.
        /// </summary>
        /// <param name="statement"></param>
        /// <returns></returns>
        public SyntaxNode Build(Model.Statement statement)
        {
            return GetBuilder(statement).Build();
        }

        /// <summary>
        /// Builds the specified statement.
        /// </summary>
        /// <param name="statement"></param>
        /// <returns></returns>
        public SyntaxStatementBuilderBase GetBuilder(Model.Statement statement)
        {
            if (statement is null)
                throw new ArgumentNullException(nameof(statement));

            return statementBuilders.GetOrAdd(statement, _ => CreateBuilder(_));
        }

        /// <summary>
        /// Builds the specified statement.
        /// </summary>
        /// <param name="statement"></param>
        /// <returns></returns>
        SyntaxStatementBuilderBase CreateBuilder(Model.Statement statement)
        {
            return statement switch
            {
                ExpressionStatement e => new ExpressionStatementBuilder(this, e),
                ReturnStatement e => new ReturnStatementBuilder(this, e),
                PInvokeStatement e => new PInvokeStatementBuilder(this, e),
                _ => throw new NotSupportedException(statement.GetType().ToString()),
            };
        }

        /// <summary>
        /// Builds the syntax for the given expression.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public SyntaxNode Build(Model.Expression expression)
        {
            return GetBuilder(expression).Build();
        }

        /// <summary>
        /// Builds the specified node.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public SyntaxExpressionBuilderBase GetBuilder(Model.Expression expression)
        {
            if (expression is null)
                throw new ArgumentNullException(nameof(expression));

            return expressionBuilders.GetOrAdd(expression, _ => CreateBuilder(_));
        }

        /// <summary>
        /// Builds the specified node.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        SyntaxExpressionBuilderBase CreateBuilder(Model.Expression expression)
        {
            return expression switch
            {
                LiteralExpression e => new LiteralExpressionBuilder(this, e),
                ParameterExpression e => new ParameterExpressionBuilder(this, e),
                VariableExpression e => new VariableExpressionBuilder(this, e),
                BinaryExpression e => new BinaryExpressionBuilder(this, e),
                PropertyOrFieldExpression e => new PropertyOrFieldExpressionBuilder(this, e),
                AssignExpression e => new AssignExpressionBuilder(this, e),
                InvokeExpression e => new InvokeExpressionBuilder(this, e),
                NotExpression e => new NotExpressionBuilder(this, e),
                ConvertExpression e => new ConvertExpressionBuilder(this, e),
                ConditionalExpression e => new ConditionalExpressionBuilder(this, e),
                IsTypeExpression e => new IsTypeExpressionBuilder(this, e),
                PInvokeExpression e => new PInvokeExpressionBuilder(this, e),
                _ => throw new NotSupportedException(expression.GetType().ToString()),
            };
        }

    }

}
