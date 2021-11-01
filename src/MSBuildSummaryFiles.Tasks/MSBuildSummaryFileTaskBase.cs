// Copyright (c) Microsoft Corporation.
//
// Licensed under the MIT license.

using Microsoft.Build.Utilities;
using System;
using System.Diagnostics;

namespace MSBuildSummaryFiles.Tasks
{
    /// <summary>
    /// Represents a base class for tasks in this library.
    /// </summary>
    public abstract class MSBuildSummaryFileTaskBase : Task
    {
        static MSBuildSummaryFileTaskBase()
        {
            if (string.Equals(Environment.GetEnvironmentVariable("DEBUG_SUMMARYFILE_TASK"), bool.TrueString, StringComparison.OrdinalIgnoreCase))
            {
                Debugger.Launch();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MSBuildSummaryFileTaskBase" /> class.
        /// </summary>
        protected MSBuildSummaryFileTaskBase()
        : base(Strings.ResourceManager)
        {
        }

        /// <inheritdoc cref="Task.Execute" />
        public override bool Execute()
        {
            return !Log.HasLoggedErrors;
        }
    }
}