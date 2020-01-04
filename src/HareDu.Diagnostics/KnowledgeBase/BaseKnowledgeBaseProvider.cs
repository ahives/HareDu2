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
namespace HareDu.Diagnostics.KnowledgeBase
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public abstract class BaseKnowledgeBaseProvider :
        IKnowledgeBaseProvider
    {
        protected readonly List<KnowledgeBaseArticle> _articles;

        protected BaseKnowledgeBaseProvider()
        {
            _articles = new List<KnowledgeBaseArticle>();

            Load();
        }

        protected abstract void Load();

        public bool TryGet(string identifier, DiagnosticStatus diagnosticStatus, out KnowledgeBaseArticle article)
        {
            if (_articles.Exists(x => x.Identifier == identifier))
            {
                try
                {
                    article = _articles.Single(x => x.Identifier == identifier && x.DiagnosticStatus == diagnosticStatus);
                    return true;
                }
                catch (Exception e)
                {
                }
            }

            article = null;
            return false;
        }

        public bool TryGet(string identifier, out IReadOnlyList<KnowledgeBaseArticle> articles)
        {
            if (_articles.Exists(x => x.Identifier == identifier))
            {
                try
                {
                    articles = _articles.Where(x => x.Identifier == identifier).ToList();
                    return true;
                }
                catch (Exception e)
                {
                }
            }

            articles = null;
            return false;
        }
    }
}