﻿// <copyright file="TestOtlpExporter.cs" company="OpenTelemetry Authors">
// Copyright The OpenTelemetry Authors
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>

using OpenTelemetry;
using OpenTelemetry.Trace;

namespace Examples.Console
{
    internal static class TestOtlpExporter
    {
        internal static object Run(string endpoint, bool doClientContextPropagation)
        {
            return RunWithActivitySource(endpoint, doClientContextPropagation);
        }

        private static object RunWithActivitySource(string endpoint, bool doClientContextPropagation)
        {
            // Enable OpenTelemetry for the sources "Samples.SampleServer" and "Samples.SampleClient"
            // and use OTLP exporter.
            using var openTelemetry = Sdk.CreateTracerProviderBuilder()
                    .AddSource("Samples.SampleClient", "Samples.SampleServer")
                    .AddOtlpExporter(opt => opt.Endpoint = endpoint)
                    .Build();

            // The above line is required only in Applications
            // which decide to use OpenTelemetry.
            using (var sample = new InstrumentationWithActivitySource())
            {
                sample.Start(doClientContextPropagation: doClientContextPropagation);

                System.Console.WriteLine("Traces are being created and exported" +
                    "to OTLP in the background." +
                    "Press ENTER to stop.");
                System.Console.ReadLine();
            }

            return null;
        }
    }
}
