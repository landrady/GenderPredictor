using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NameBayes
{
    public class Trainner
    {
        private string[][] _data;

        private int _classIndex;

        public Trainner(string[][] data, int classIndex)
        {
            _data = data;
            _classIndex = classIndex;
        }

        private int[] _totalValue;
        private Dictionary<string, Dictionary<string, int>>[] _compiledCount;
        private Dictionary<string, int> _classCount;

        public void Load(bool ignoreFirst)
        {
            int startRow = ignoreFirst ? 1 : 0;
            int[] total = new int[_data[0].Length];
            Dictionary<string, Dictionary<string,int>>[] compiled = new Dictionary<string, Dictionary<string,int>>[_data[0].Length];
            _classCount = new Dictionary<string, int>();
            for (int i = startRow; i < _data.Length; i++)
            {
                for (int j = 0; j < _data[i].Length; j++)
                {
                    if (compiled[j] == null)
                        compiled[j] = new Dictionary<string, Dictionary<string,int>>();

                    if (!compiled[j].ContainsKey(_data[i][j]))
                    {
                        var classes = new Dictionary<string, int>();
                        classes.Add(_data[i][_classIndex],1);
                        compiled[j].Add(_data[i][j], classes);
                    }
                    else
                    {
                        if (!compiled[j][_data[i][j]].ContainsKey(_data[i][_classIndex]))
                        {
                            compiled[j][_data[i][j]].Add(_data[i][_classIndex], 1);
                        }
                        compiled[j][_data[i][j]][_data[i][_classIndex]]++;
                    }

                    if (!_classCount.ContainsKey(_data[i][_classIndex]))
                    {
                        _classCount.Add(_data[i][_classIndex], 0);
                    }
                    _classCount[_data[i][_classIndex]]++;
                    total[j]++;
                }
            }
            _totalValue = total;
            _compiledCount = compiled;
        }

        public double PartialProbability(string[] data, bool withSmoothing, int xClasses)
        {
            if (data.Length != _compiledCount.Length)
                throw new Exception("Tamanhos diferentes");

            double result = 0;
            int totalCases = _classCount.Sum(x => x.Value);
            int totalToUse = _classCount.ContainsKey(data[_classIndex]) ? _classCount[data[_classIndex]] : 0;
            double p0 = (totalToUse * 1.0) / (totalCases);
            int smooth = withSmoothing ? 1 : 0;
            int k = withSmoothing ? xClasses : 0;
            for (int i = 0; i < _compiledCount.Length; i++)
            {
                if (i != _classIndex)
                {
                    double cValue = _compiledCount[i].ContainsKey(data[i]) && _compiledCount[i][data[i]].ContainsKey(data[_classIndex]) ?
                        _compiledCount[i][data[i]][data[_classIndex]] : 0;
                    double p = (cValue + smooth) /   ((totalToUse + k) * 1.0);
                    result = result + Math.Log(p);
                }
            }
            return Math.Exp(result);
        }

        public string Classify(string[] data, bool withSmoothing, int xClasses)
        {
            double evidence = 0;
            double higher = 0;
            string result = string.Empty;

            Dictionary<string, double> scores = new Dictionary<string, double>();
            foreach (var item in _compiledCount[_classIndex])
            {
                data[_classIndex] = item.Key;
                double partProb = PartialProbability(data, withSmoothing, xClasses);
                evidence += partProb;
                scores.Add(item.Key, partProb);
            }

            foreach (var item in scores)
            {
                double current = item.Value / evidence;
                if(current > higher)
                {
                    higher = current;
                    result = item.Key;
                }
            }

            return result;
        }


    }
}
