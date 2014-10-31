using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xml_valid
{
    class frames
    {
        //Массив границ
        private int[,] frame= new int[0,2];

        //Добавляем две границы
        public void addFrame(int start, int end)
        {
            Resize(ref frame, frame.GetLength(0) + 1, 2);
            frame[frame.GetLength(0) - 1, 0] = start;
            frame[frame.GetLength(0) - 1, 1] = end;
        }

        //Добавляем одну границу
        public void addFrame(int line)
        {
            Resize(ref frame, frame.GetLength(0) + 1, 2);
            frame[frame.GetLength(0) - 1, 0] = line;
            frame[frame.GetLength(0) - 1, 1] = line;
        }

        //Проверка нахождения в границах
        public bool isInFrames(int th)
        {
            for (int i = 0; i < frame.GetLength(0); i++)
            {
                if (frame[i,0] <= th && frame[i,1] >= th) return true;
            }
            return false;
        }

        //Очистка границ
        public void clear()
        {
            Resize(ref frame, 0, 2);
        }

        //Изменение размера двумерного массива
        private void Resize<T>(ref T[,] arr, int a, int b)
        {
            T[,] tmp = new T[a, b];
            int с = arr.GetLength(0);
            int d = arr.GetLength(1);
            for (int i = 0; i < a; i++)
            {
                for (int j = 0; j < b; j++)
                {
                    if (i < с && j < d)
                        tmp[i, j] = arr[i, j];
                    else
                        tmp[i, j] = default(T);
                }
            }
            arr = tmp;
        }
    }
}
