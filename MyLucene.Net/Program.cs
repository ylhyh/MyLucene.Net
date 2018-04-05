using Lucene.Net.Analysis.Standard;
using Lucene.Net.Store;
using Lucene.Net.Index;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Lucene.Net.Index.IndexWriter;
using Lucene.Net.Documents;
using System.IO;
using Lucene.Net.Search;
using Lucene.Net.QueryParsers;
using System.Threading;

namespace MyLucene.Net
{
    class Program
    {
        static string content = "全文数据库是全文检索系统的主要构成部分。所谓全文数据库是将一个完整的信息源的全部内容转化为计算机可以识别、处理的信息单元而形成的数据集合。全文数据库不仅存储了信息，而且还有对全文数据进行词、字、段落等更深层次的编辑、加工的功能，而且所有全文数据库无一不是海量信息数据库。";

        static void Main(string[] args)
        {
            //var analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30);
            var analyzer = new JiebaForLuceneAnalyzer();

            using (var writer = new IndexWriter(Directory, analyzer, MaxFieldLength.UNLIMITED))
            {
                writer.DeleteAll();

                Document doc1 = new Document();
                Field f10 = new Field("Name", "Doc1", Field.Store.YES, Field.Index.NOT_ANALYZED);
                Field f11 = new Field("SolutionDomain", "HBB", Field.Store.YES, Field.Index.NOT_ANALYZED);
                Field f12 = new Field("Content", content, Field.Store.YES, Field.Index.ANALYZED);
                Field f13 = new Field("ModifyTime", DateTools.DateToString(DateTime.Now,DateTools.Resolution.MILLISECOND), Field.Store.YES, Field.Index.NOT_ANALYZED);
                doc1.Add(f10);
                doc1.Add(f11);
                doc1.Add(f12);
                doc1.Add(f13);
                writer.AddDocument(doc1);

                Thread.Sleep(2000);

                Document doc2 = new Document();
                Field f20 = new Field("Name", "Doc2", Field.Store.YES, Field.Index.NOT_ANALYZED);
                Field f21 = new Field("SolutionDomain", "HBB", Field.Store.YES, Field.Index.NOT_ANALYZED);
                Field f22 = new Field("Content", content, Field.Store.YES, Field.Index.ANALYZED);
                Field f23 = new Field("ModifyTime", DateTools.DateToString(DateTime.Now,DateTools.Resolution.MILLISECOND), Field.Store.YES, Field.Index.NOT_ANALYZED);
                doc2.Add(f20);
                doc2.Add(f21);
                doc2.Add(f22);
                doc2.Add(f23);
                writer.AddDocument(doc2);

                Thread.Sleep(2000);

                Document doc3 = new Document();
                Field f30 = new Field("Name", "Doc3", Field.Store.YES, Field.Index.NOT_ANALYZED);
                Field f31 = new Field("SolutionDomain", "SOC", Field.Store.YES, Field.Index.NOT_ANALYZED);
                Field f32 = new Field("Content", content, Field.Store.YES, Field.Index.ANALYZED);
                Field f33 = new Field("ModifyTime", DateTools.DateToString(DateTime.Now,DateTools.Resolution.MILLISECOND), Field.Store.YES, Field.Index.NOT_ANALYZED);
                doc3.Add(f30);
                doc3.Add(f31);
                doc3.Add(f32);
                doc3.Add(f33);
                writer.AddDocument(doc3);

                Thread.Sleep(2000);

                Document doc4 = new Document();
                Field f40 = new Field("Name", "Doc4", Field.Store.YES, Field.Index.NOT_ANALYZED);
                Field f41 = new Field("SolutionDomain", "SOC", Field.Store.YES, Field.Index.NOT_ANALYZED);
                Field f42 = new Field("Content", content, Field.Store.YES, Field.Index.ANALYZED);
                Field f43 = new Field("ModifyTime", DateTools.DateToString(DateTime.Now,DateTools.Resolution.MILLISECOND), Field.Store.YES, Field.Index.NOT_ANALYZED);
                doc4.Add(f40);
                doc4.Add(f41);
                doc4.Add(f42);
                doc4.Add(f43);
                writer.AddDocument(doc4);

                writer.Commit();
                writer.Optimize();
                writer.Dispose();
            }

            string keyword = "全文";
            using (var searcher = new IndexSearcher(Directory, true))
            {
                QueryParser parser = new QueryParser(Lucene.Net.Util.Version.LUCENE_30, "Content", analyzer);
                Query query = parser.Parse(keyword);
                Filter filter = new QueryWrapperFilter(new TermQuery(new Term("SolutionDomain", "SOC")));
                Sort sort = new Sort(new SortField("ModifyTime", SortField.STRING, true));
                TopDocs result = searcher.Search(query, filter, 1000, sort);

                foreach (var r in result.ScoreDocs)
                {
                    Console.WriteLine(searcher.Doc(r.Doc).GetField("Name").StringValue);
                }

                Console.ReadLine();
            }

            analyzer.Close();
            analyzer.Dispose();
        }

        private static string _luceneDir = @"D:\LuceneIndex";
        private static FSDirectory __directory;
        private static FSDirectory Directory
        {
            get
            {
                if (__directory == null)
                {
                    __directory = FSDirectory.Open(_luceneDir);
                }
                else
                {
                    __directory.EnsureOpen();
                }

                //if (IndexWriter.IsLocked(_directoryTemp))
                //{
                //    IndexWriter.Unlock(_directoryTemp);
                //}

                //var lockFilePath = Path.Combine(_luceneDir, "write.lock");

                //if (File.Exists(lockFilePath)) File.Delete(lockFilePath);

                return __directory;
            }
        }
    }
}
