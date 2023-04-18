#region Header
///Author:Mojiex
///Github:https://github.com/mojiex/Mojiex
///Create Time:2022/10/31
///Framework Description:This framework is developed based on unity2019LTS,Lower unity version may not supported.
#endregion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Mojiex.Math
{
    /// <summary>
    /// Just for Two-dimensional matrix
    /// </summary>
    public class Matrix<T>
    {
        private T[,] data;
        public T this[int index1, int index2]
        {
            get
            {
                return data[index1, index2];
            }
            set
            {
                data[index1, index2] = value;
            }
        }

        public int Row;
        public int Column;
        public int Size => Row * Column;

        public Matrix(int rank) : this(rank, rank) { }
        public Matrix(int row, int column)
        {
            data = new T[row, column];
            Row = row;
            Column = column;
        }

        public Matrix<T> FillData(T[] rowData)
        {
            for (int i = 0, k = 0; i < Row; i++)
            {
                for (int j = 0; j < Column; j++)
                {
                    if (k >= rowData.Length)
                    {
                        break;
                    }
                    data[i, j] = rowData[k];
                    k++;
                }
            }
            return this;
        }
        public T[] GetRow(int rowIndex)
        {
            T[] res = new T[Column];
            if (rowIndex >= Row)
            {
                MDebug.LogError($"The row you wanted do not exist.Target row {rowIndex},max row {Row}");
                return res;
            }
            for (int i = 0; i < Column; i++)
            {
                res[i] = data[rowIndex, i];
            }
            return res;
        }
        public T[] GetColumn(int columnIndex)
        {
            T[] res = new T[Row];
            if (columnIndex >= Row)
            {
                MDebug.LogError($"The column you wanted do not exist.Target column {columnIndex},max column {Column}");
                return res;
            }
            for (int i = 0; i < Row; i++)
            {
                res[i] = data[i, columnIndex];
            }
            return res;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || (!(obj is Matrix<T>)))
            {
                return false;
            }
            Matrix<T> matrix = obj as Matrix<T>;
            if (matrix.Column != Column || matrix.Row != Row)
            {
                return false;
            }

            for (int i = 0; i < Row; i++)
            {
                for (int j = 0; j < Column; j++)
                {
                    if (!matrix[i, j].Equals(this[i, j]))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public static bool operator ==(Matrix<T> lhs, Matrix<T> rhs)
        {
            return lhs.Equals(rhs);
        }
        public static bool operator !=(Matrix<T> lhs, Matrix<T> rhs)
        {
            return !lhs.Equals(rhs);
        }
    }
}