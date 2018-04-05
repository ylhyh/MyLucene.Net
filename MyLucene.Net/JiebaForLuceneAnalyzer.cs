using JiebaNet.Segmenter;
using Lucene.Net.Analysis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLucene.Net
{
    /// <summary>
    /// 基于LuceneNet扩展的JieBa分析器
    /// </summary>
    public class JiebaForLuceneAnalyzer : Analyzer
    {
        protected static readonly ISet<string> DefaultStopWords = StopAnalyzer.ENGLISH_STOP_WORDS_SET;

        private static ISet<string> StopWords;

        static JiebaForLuceneAnalyzer()
        {
            StopWords = new HashSet<string>();
            var stopWordsFile = Path.GetFullPath(JiebaNet.Analyser.ConfigManager.StopWordsFile);
            if (File.Exists(stopWordsFile))
            {
                var lines = File.ReadAllLines(stopWordsFile);
                foreach (var line in lines)
                {
                    StopWords.Add(line.Trim());
                }
            }
            else
            {
                StopWords = DefaultStopWords;
            }
        }

        public override TokenStream TokenStream(string fieldName, TextReader reader)
        {
            var seg = new JiebaSegmenter();
            TokenStream result = new JiebaForLuceneTokenizer(seg, reader);
            result = new LowerCaseFilter(result);
            result = new StopFilter(true, result, StopWords);
            return result;
        }
    }
}
