﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis.Operations.ControlFlow;

namespace Microsoft.CodeAnalysis.Operations.DataFlow.PointsToAnalysis
{
    using PointsToAnalysisData = IDictionary<AnalysisEntity, PointsToAbstractValue>;

    /// <summary>
    /// Result from execution of <see cref="PointsToAnalysis"/> on a basic block.
    /// It stores the PointsTo value for each <see cref="AnalysisEntity"/> at the start and end of the basic block.
    /// </summary>
    internal class PointsToBlockAnalysisResult : AbstractBlockAnalysisResult<PointsToAnalysisData, PointsToAbstractValue>
    {
        public PointsToBlockAnalysisResult(
            BasicBlock basicBlock,
            DataFlowAnalysisInfo<PointsToAnalysisData> blockAnalysisData,
            ImmutableDictionary<AnalysisEntity, PointsToAbstractValue> defaultPointsToValues)
            : base (basicBlock)
        {
            InputData = GetResult(blockAnalysisData.Input, defaultPointsToValues);
            OutputData = GetResult(blockAnalysisData.Output, defaultPointsToValues);
        }

        private static ImmutableDictionary<AnalysisEntity, PointsToAbstractValue> GetResult(PointsToAnalysisData analysisData, ImmutableDictionary<AnalysisEntity, PointsToAbstractValue> defaultPointsToValues)
        {
            if (analysisData == null || analysisData.Count == 0)
            {
                return defaultPointsToValues;
            }

            var builder = ImmutableDictionary.CreateBuilder<AnalysisEntity, PointsToAbstractValue>();
            builder.AddRange(analysisData);
            foreach (var kvp in defaultPointsToValues)
            {
                AnalysisEntity entity = kvp.Key;
                if (!builder.ContainsKey(entity))
                {
                    PointsToAbstractValue pointsToAbstractValue = kvp.Value;
                    builder.Add(entity, pointsToAbstractValue);
                }
            }

            return builder.ToImmutable();
        }

        public ImmutableDictionary<AnalysisEntity, PointsToAbstractValue> InputData { get; }
        public ImmutableDictionary<AnalysisEntity, PointsToAbstractValue> OutputData { get; }
    }
}
