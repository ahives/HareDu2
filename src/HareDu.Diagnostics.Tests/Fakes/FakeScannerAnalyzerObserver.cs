// Copyright 2013-2020 Albert L. Hives
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
namespace HareDu.Analytics.Tests
{
    using System;
    using System.Linq;
    using Diagnostics;
    using Shouldly;

    public class FakeScannerAnalyzerObserver :
        IObserver<AnalyzerContext>
    {
        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(AnalyzerContext value)
        {
            value.Summary.ShouldNotBeNull();
            value.Summary.Count.ShouldBe(2);
            
            value.Summary.Any(x => x.Id == "Queue").ShouldBeTrue();

            var queueSummary = value.Summary
                .SingleOrDefault(x => x.Id == "Queue");
            queueSummary.ShouldNotBeNull();
            queueSummary.Healthy.Total.ShouldBe<uint>(26);
            Decimal.Round(queueSummary.Healthy.Percentage, 2).ShouldBe(46.43M);
            queueSummary.Unhealthy.Total.ShouldBe<uint>(30);
            Decimal.Round(queueSummary.Unhealthy.Percentage, 2).ShouldBe(53.57M);
            queueSummary.Warning.Total.ShouldBe<uint>(0);
            queueSummary.Warning.Percentage.ShouldBe(0);
            queueSummary.Inconclusive.Total.ShouldBe<uint>(0);
            queueSummary.Inconclusive.Percentage.ShouldBe(0);
            
            value.Summary.Any(x => x.Id == "Exchange").ShouldBeTrue();

            var exchangeSummary = value.Summary
                .SingleOrDefault(x => x.Id == "Exchange");
            exchangeSummary.ShouldNotBeNull();
            exchangeSummary.Healthy.Total.ShouldBe<uint>(0);
            exchangeSummary.Healthy.Percentage.ShouldBe(0);
            exchangeSummary.Unhealthy.Total.ShouldBe<uint>(1);
            exchangeSummary.Unhealthy.Percentage.ShouldBe(100);
            exchangeSummary.Warning.Total.ShouldBe<uint>(0);
            exchangeSummary.Warning.Percentage.ShouldBe(0);
            exchangeSummary.Inconclusive.Total.ShouldBe<uint>(0);
            exchangeSummary.Inconclusive.Percentage.ShouldBe(0);
        }
    }
}