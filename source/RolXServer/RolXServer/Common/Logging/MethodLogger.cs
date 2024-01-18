// -----------------------------------------------------------------------
// <copyright file="MethodLogger.cs" company="Christian Ewald">
// Copyright (c) Christian Ewald. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System.Runtime.CompilerServices;

using Serilog.Events;

namespace RolXServer.Common.Logging;

/// <summary>
/// Helper class to log method calls.
/// </summary>
/// <param name="Logger">The logger to log to.</param>
/// <param name="MethodName">The name of the logged method.</param>
/// <param name="LogEventLevel">The log level.</param>
public record struct MethodLogger(ILogger Logger, string MethodName, LogEventLevel LogEventLevel)
{
    /// <summary>
    /// Logs the entering of the method.
    /// </summary>
    public void Enter()
        => this.Logger.Write(this.LogEventLevel, this.MethodName + "()");

    /// <summary>
    /// Logs the entering of the method with 1 argument.
    /// </summary>
    /// <typeparam name="T1">The type of the 1st argument.</typeparam>
    /// <param name="argument1">The 1st argument.</param>
    /// <param name="argument1Text">The text of the 1st argument.</param>
    public void Enter<T1>(T1 argument1, [CallerArgumentExpression(nameof(argument1))] string argument1Text = "")
        => this.Logger.Write(this.LogEventLevel, $"{this.MethodName}({argument1Text}: {{argument1}})", argument1);

#pragma warning disable SA1618 // Generic type parameters should be documented
#pragma warning disable SA1611 // Element parameters should be documented

    /// <summary>
    /// Logs the entering of the method with 2 arguments.
    /// </summary>
    public void Enter<T1, T2>(
        T1 argument1,
        T2 argument2,
        [CallerArgumentExpression(nameof(argument1))] string argument1Text = "",
        [CallerArgumentExpression(nameof(argument2))] string argument2Text = "")
        => this.Logger.Write(
            this.LogEventLevel,
            $"{this.MethodName}({argument1Text}: {{argument1}}, {argument2Text}: {{argument2}})",
            argument1,
            argument2);

    /// <summary>
    /// Logs the entering of the method with 3 arguments.
    /// </summary>
    public void Enter<T1, T2, T3>(
        T1 argument1,
        T2 argument2,
        T3 argument3,
        [CallerArgumentExpression(nameof(argument1))] string argument1Text = "",
        [CallerArgumentExpression(nameof(argument2))] string argument2Text = "",
        [CallerArgumentExpression(nameof(argument3))] string argument3Text = "")
        => this.Logger.Write(
            this.LogEventLevel,
            $"{this.MethodName}({argument1Text}: {{argument1}}, {argument2Text}: {{argument2}}, {argument3Text}: {{argument3}})",
            argument1,
            argument2,
            argument3);

    /// <summary>
    /// Logs the entering of the method with 4 arguments.
    /// </summary>
    public void Enter<T1, T2, T3, T4>(
        T1 argument1,
        T2 argument2,
        T3 argument3,
        T4 argument4,
        [CallerArgumentExpression(nameof(argument1))] string argument1Text = "",
        [CallerArgumentExpression(nameof(argument2))] string argument2Text = "",
        [CallerArgumentExpression(nameof(argument3))] string argument3Text = "",
        [CallerArgumentExpression(nameof(argument4))] string argument4Text = "")
        => this.Logger.Write(
            this.LogEventLevel,
            $"{this.MethodName}({argument1Text}: {{argument1}}, {argument2Text}: {{argument2}}, {argument3Text}: {{argument3}}, {argument4Text}: {{argument4}})",
            argument1,
            argument2,
            argument3,
            argument4);

    /// <summary>
    /// Logs the entering of the method with 5 arguments.
    /// </summary>
    public void Enter<T1, T2, T3, T4, T5>(
        T1 argument1,
        T2 argument2,
        T3 argument3,
        T4 argument4,
        T5 argument5,
        [CallerArgumentExpression(nameof(argument1))] string argument1Text = "",
        [CallerArgumentExpression(nameof(argument2))] string argument2Text = "",
        [CallerArgumentExpression(nameof(argument3))] string argument3Text = "",
        [CallerArgumentExpression(nameof(argument4))] string argument4Text = "",
        [CallerArgumentExpression(nameof(argument5))] string argument5Text = "")
        => this.Logger.Write(
            this.LogEventLevel,
            $"{this.MethodName}({argument1Text}: {{argument1}}, {argument2Text}: {{argument2}}, {argument3Text}: {{argument3}}, {argument4Text}: {{argument4}}, {argument5Text}: {{argument5}})",
            argument1,
            argument2,
            argument3,
            argument4,
            argument5);

#pragma warning restore SA1618 // Generic type parameters should be documented
#pragma warning restore SA1611 // Element parameters should be documented

    /// <summary>
    /// Logs the exiting of the method.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="result">The result.</param>
    /// <returns>The result for directly chaining it to the return statement.</returns>
    public TResult Exit<TResult>(TResult result)
    {
        this.Logger.Write(this.LogEventLevel, this.MethodName + "() -> {result}", result);
        return result;
    }

    /// <summary>
    /// Logs the exiting of the method.
    /// </summary>
    public void Exit()
        => this.Logger.Write(this.LogEventLevel, this.MethodName + "() -> void");
}
