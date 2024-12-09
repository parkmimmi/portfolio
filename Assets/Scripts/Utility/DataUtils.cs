using System.Collections.Generic;
using System.Linq;

namespace DataUtils
{
    public class CSVParser
    {
        // 행
        public static string[] CSVToLineArr(string str)
        {
            str = str.Replace("\r", "");
            string[] lineArr = str.Split('\n');

            for (int i = 0; i < lineArr.Length; i++)
            {
                if (!string.IsNullOrEmpty(lineArr[i]))
                {
                    lineArr[i] = lineArr[i].Replace("\\n", "\n");       // csv data 텍스트 내 \n 처리
                }
            }

            return lineArr;
        }

        // 행
        public static List<string> CSVToLine(string _str)
        {
            _str = _str.Replace("\r", "");
            string[] lineArr = _str.Split('\n');

            List<string> lineList = lineArr.ToList();


            for (int i = 0; i < lineList.Count; i++)
            {
                if (!string.IsNullOrEmpty(lineList[i]))
                {
                    lineList[i] = lineList[i].Replace("\\n", "\n");
                }
                else
                {
                    lineList.RemoveAt(i);   // 빈 행 삭제
                }
            }
            lineList.RemoveAt(0); // 인덱스 삭제

            return lineList;
        }

        // 열
        public static string[] LineToColumnArr(string line)
        {
            string[] columnArr = line.Split(',');
            return columnArr;
        }

        // 열
        public static List<string> LineToColumn(string _line)
        {
            string[] columnArr = _line.Split(',');
            return columnArr.ToList();
        }
    }


    [System.Serializable]
    public class TreeNode<T>
    {
        public T Data { get; set; }

        public List<TreeNode<T>> Children { get; set; } = new List<TreeNode<T>>();

        public int Depth { get; private set; }
    }
}

