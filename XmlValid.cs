using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Windows.Forms;

namespace xml_valid
{
    class XmlValid : MainForm
    {
        //Массив ошибок
        private static string[] errors = new string[0];
        private static int[] closers = new int[0];
        private static frames frame = new frames();

        //Конструктор класса
        public XmlValid(ref RichTextBox field)
        {
            string pattern = "(<\\s*[A-я]\\w+[\\w\\s=\":\\?\\/\\\\.-]*[^\\/]>)|(<\\/\\w+>)";
            MatchCollection mc = Regex.Matches(field.Text, pattern);
            try
            {
                attributes(ref field, mc);
            }
            catch (Exception e)
            {
                field.Text += "\n"+e.Message;
            }
            paint(ref field);

            if (mc.Count == 0) return;

            root(ref field, mc[0].Value);
            validate(ref mc, 0, mc.Count - 1,ref field);

            frame.addFrame(mc[0].Index,mc[mc.Count-1].Index);
            other(ref field);

        }
        
        //Основная валидация
        private static void validate(ref MatchCollection match, int start, int end,  ref RichTextBox field)
        {
            if (start > end) return;
            if (frame.isInFrames(match[start].Index))
            {
                validate(ref match, start + 1, end, ref field);
                return;
            }

            string line = match[start].Value;

            if (Regex.IsMatch(line, "<\\/\\w+>"))
            {
                foreach (int i in closers)
                {
                    if (start == i) return;
                }

                findedError(lineErr(ref field, match[start].Index), "Лишний закрывающий тег " + match[start].Value);
                validate(ref match, start + 1, end, ref field);
                return;
            }

            int first = Regex.Match(line, "[A-zА-я]").Index;
            string newLine = "</" + line.Substring(first, ((line.IndexOf(" ", first) != -1) ? line.IndexOf(" ", first) : line.IndexOf(">")) - first) + ">";
            for (int i = start+1; i <= end; i++)
            {
                string temp = match[i].Value;
                if (newLine == temp) 
                {
                    Array.Resize(ref closers, closers.Length + 1);
                    closers[closers.Length - 1] = i;

                    if ((start + 1) == i)
                    {
                        validate(ref match, start + 2, end,ref field);
                        return;
                    }else
                    {
                        validate(ref match, start + 1, i-1,ref field);
                    }
                    if (end - i > 2) validate(ref match, i + 1, end,ref field);
                    return;
                }
            }
            findedError(lineErr(ref field, match[start].Index), "Не найден парный элемент для " + newLine.Replace("/",""));
            validate(ref match, start+1, end,ref field);
        }

        //Корень
        private static void root(ref RichTextBox field,string line)
        {
            string eLine = line.Substring(0, ((line.IndexOf(" ") != -1) ? line.IndexOf(" ") : line.IndexOf(">")));
            if ((Regex.Matches(field.Text, eLine + " ").Count + Regex.Matches(field.Text, eLine + ">").Count)>1)
            {
                findedError(lineErr(ref field, field.Text.IndexOf(line)), "Корневой тег должен быть один");
            }
        }

        //Комментарии
        private static int comments(ref RichTextBox field)
        {
            int i = 0;
            while (i != -1)
            {
                int first = field.Text.IndexOf("<!--", i);
                int last = field.Text.IndexOf("-->", first + 4);
                if (first == -1)
                {
                    int next = field.Text.IndexOf("<!--", first + 4);
                    if (last != -1 && last < next) 
                    {
                        findedError(lineErr(ref field, last), "Лишний закрывающий тег комментария");
                    }
                    return 0;
                }
                if (last == -1)
                {
                    fill(ref field, first, field.Text.Length, Color.Green);
                    findedError(lineErr(ref field, first), "Не найден закрывающий тег комментария");
                    frame.addFrame(first, field.Text.Length);

                    return lineErr(ref field, first);
                }
                frame.addFrame(first,last);
                fill(ref field, first, last - first + 4, Color.Green);

                i = last + 3;
            }
            return 0;
        }

        //Доктайп
        private static void doctype(ref RichTextBox field)
        {
            int i = 0;
            while (i != -1)
            {
                int first = field.Text.IndexOf("<!DOCTYPE", i);
                int last = field.Text.IndexOf("]>", first + 2);
                if (first == -1) return;
                if (last == -1)
                {
                    fill(ref field, first, field.Text.Length, Color.Gray);
                    findedError(lineErr(ref field, first), "DOCTYPE проблемы");
                    frame.addFrame(first, field.Text.Length);

                    return;
                }
                frame.addFrame(first, last);
                fill(ref field, first, last - first + 2, Color.Gray);

                i = last + 3;
            }
            return;
        }

        //Поиск лишнего текста
        private static void other(ref RichTextBox field)
        {
            string pattern = "^([\\w@.\\sА-я#!\\?\\/]+)";
            int s = 0;

            for (int i = 0; i < field.Lines.Length;i++ )
            {
                s += field.Lines[i].Length;
                if (!frame.isInFrames(s) && Regex.IsMatch(field.Text,pattern))
                {
                    findedError(i, "Лишний текст в XML документе");
                }
            }
        }

        //Аттрибуты
        public static void attributes(ref RichTextBox field, MatchCollection match)
        {
            string pattern = "\\w[\\w:]*\\s*=\\s*\".+\"";
            string openFull = "<\\w+[\\w\\s=\":\\?\\/\\\\.-]*[^\\/]>";
            string tmp;

            foreach (Match ma in match)
            {
                if(!Regex.IsMatch(ma.Value,openFull)) continue;
                tmp = Regex.Replace(ma.Value, pattern,"");

                if (tmp.IndexOf(" ") == -1) continue;

                for (int i = tmp.IndexOf(" "); i <tmp.LastIndexOf(">") ; i++)
                {
                    if (!Regex.IsMatch(tmp[i]+"","\\s"))
                    {
                        findedError(lineErr(ref field, match[i].Index), "Проблемы с атрибутами в теге "+tmp);
                        break;
                    }
                } 
                tmp = "";
            }
        }

        //Перекраска
        private static void paint(ref RichTextBox field)
        {
            //Всё в чёрное
            fill(ref field, 0,  field.Text.Length, Color.Black);

            //Теги
            string pattern = "(<\\s*[A-zА-я]\\w+\\s+)|(<\\s*[A-zА-я]\\w+>)|(</\\w+>)|(\">)|(/>)";
            Regex regExp = new Regex(pattern);

            foreach (Match ma in regExp.Matches(field.Text))
            {
                fill(ref field, ma.Index, ma.Length, Color.Blue);
            }

            //Первая строка
            if (firstLine(field.Text.Substring(0, (field.Text.IndexOf("\n")>0?field.Text.IndexOf("\n"):field.Text.Length)),ref field))
            {
                frame.addFrame(0, (field.Text.IndexOf("\n") > 0 ? field.Text.IndexOf("\n") : field.Text.Length));

                fill(ref field, 0, 2, Color.Red);
                fill(ref field, 2, 3, Color.Blue);
                fill(ref field, field.Text.LastIndexOf("?>"), 2, Color.Red);
            }

            string att = "\\w[\\w:]*\\s*=\\s*\".+\"";
            foreach (Match ma in Regex.Matches(field.Text, att))
            {
                fill(ref field, ma.Index, ma.Length, Color.Red);
            }

            //Кавычки
            int stop = comments(ref field);            
            int i = 0;
            while (i != -1)
            {
                int first = field.Text.IndexOf("\"", i);
                int last = field.Text.IndexOf("\"", first + 1);
                if (first == -1 || (first > stop && stop>0)) break;
                if (last == -1)
                {
                    fill(ref field, first, field.Text.Length, Color.Purple);
                    findedError(lineErr(ref field, first), "Не найдена закрывающая кавычка");
                    frame.addFrame(first, field.Text.Length);

                    break;
                }
                fill(ref field, first, last - first + 1, Color.Purple);
                i = last + 1;
            }
            //Dtd
            doctype(ref field);
        }

        //Закрашивалка
        public static void fill(ref RichTextBox field, int start, int end, Color color){
            field.SelectionStart = start;
            field.SelectionLength = end;
            field.SelectionColor = color;
        }

        //Первая строчка
        private static bool firstLine(string line, ref RichTextBox field)
        {
            //Проверка первой троки на валидность
            string pattern = "<\\?xml\\s+version=(\"|\')\\d\\.\\d\\1(\\s+encoding=(\"|\')(utf-8|windows-1251|)\\3)?\\s*";
            string ending = "\\?>";
            Regex regExp = new Regex(pattern);

            if (regExp.IsMatch(line.ToLower()))
            {
                for (int i = 0; i < field.Lines.Length; i++)
                {
                    if (Regex.IsMatch(field.Lines[i].ToLower(), ending)) return true;

                    if (Regex.IsMatch(field.Lines[i], "^\\s*\\w+"))
                    {
                        findedError(0, "Не описан заголовок XML документа");
                        return false;
                    }
                }
            }

            if (!regExp.IsMatch(line.ToLower()))
            {
                findedError(0, "Не описан заголовок XML документа");
                return false;
            }
            if (regExp.Matches(line.ToLower()).Count > 1)
            {
                findedError(0, "Заголовок документа должен быть только один");
                return false;
            }

            

            return true;
        }

        //Добавление ошибки
        private static void findedError(int line, string message)
        {
            Array.Resize(ref errors, errors.Length + 1);
            errors[errors.Length - 1] =  "Reaction:  " + message+", Line:   " + (line+1);
        }  

        //Массив ошибок
        public string[] Errors
        {
            get { return errors; }
        }

        //Принудительная очистка ошибок
        public static void errorsClear()
        {
            Array.Resize(ref errors, 0);
            Array.Resize(ref closers, 0);
            frame.clear();
        }

        //Подсчёт строки по индексу
        public static int lineErr(ref RichTextBox field, int m)
        {
            int s = 0;
            for (int i = 0; i < field.Lines.Length; i++)
            {
                s += field.Lines[i].Length;
                if (s > m) return i;
            }
            return field.Lines.Length + 1;
        }
    }
}
