using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

Console.OutputEncoding = System.Text.Encoding.UTF8;
//Chang the folder path in here
const string ROOT_PATH = @"D:\MyWorking\AISLOUTIONS\.NET\Assignment 1";


Dictionary<string, string> GetDataFromFile()
{
    Dictionary<string, string> data = new Dictionary<string, string>();
    foreach (string file in Directory.EnumerateFiles(ROOT_PATH, "*.txt"))
    {
        string contents = File.ReadAllText(file);
        data.Add(Path.GetFileName(file), contents);
    }
    return data;
}
//1 Tìm các file có chứa từ "việt nam", copy vào 1 folder cùng tên
void FindAndCopy()
{
    string newFolder = Path.Combine(ROOT_PATH, "Task1_Vietnam");
    Directory.CreateDirectory(newFolder);
    Dictionary<string, string> data = GetDataFromFile();
    foreach(var fileName in data.Keys)
    {
        string contents = data.GetValueOrDefault(fileName, String.Empty);
        if(contents.Contains("Vietnam", StringComparison.OrdinalIgnoreCase))
        {
            string sourceFile = Path.Combine(ROOT_PATH, fileName);
            string destFile = Path.Combine(newFolder, fileName);
            File.Copy(sourceFile, destFile, true);
        }
    }
}

/*2. 
Thống kê tất cả các từ , ghi ra 1 file csv với format sau (sắp xếp theo số lần xuất hiện từ lớn tới nhỏ)
Nội dung từ, Số lần xuất hiện
*/
void Statistic()
{
    string newFolder = Path.Combine(ROOT_PATH, "Task2_Statistic");
    Directory.CreateDirectory((newFolder));
    Dictionary<string, string> data = GetDataFromFile();
    foreach(var fileName in data.Keys)
    {
        string newFile = Path.Combine(newFolder, $"Statistic of {fileName.Replace(".txt", ".csv")}");

        string[] words = Regex.Split(data[fileName], @"[\W]+");
        Dictionary<string, int> dict = new Dictionary<string, int>();
        foreach (var word in words)
        {
            if (dict.ContainsKey(word))
            {
                dict[word]++;
            }
            else
            {
                dict.Add(word, 1);
            }
        }
        dict = dict.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

        StringBuilder sb = new StringBuilder();
        sb.Append($"--------------------{fileName}--------------------\n");
        foreach (var key in dict.Keys)
        {
            sb.Append($"{key},{dict[key]}\n");
        }

        File.WriteAllText(newFile, sb.ToString(), Encoding.UTF8);
    }
}

/*
 3) Ghi ra 1 file csv tên các file và số từ trong file
- Tên file,Số từ
*/
void WriteFile()
{
    string newFolder = Path.Combine(ROOT_PATH, "Task3_CountWord");
    Directory.CreateDirectory((newFolder));

    string newFile = Path.Combine(newFolder, "CountWord.txt");
    Dictionary<string, string> data = GetDataFromFile();
    StringBuilder sb = new StringBuilder();
    sb.Append("Tên file, số từ\n");
    foreach (var fileName in data.Keys)
    {
        string[] words = Regex.Split(data[fileName], @"[\W\n]+");
        sb.Append($"{fileName}, {words.Length}\n");
    }
    File.WriteAllText(newFile, sb.ToString(), Encoding.UTF8);
}

/*4 Tìm tất cả các từ trong 1 file, từ này bắt đầu bằng chữ n,
có ít nhất 4 chữ cái và không kết thúc bằng chữ n */
void FindAllWord()
{
    string newFolder = Path.Combine(ROOT_PATH, "Task4_FindAllWord");
    Directory.CreateDirectory((newFolder));
    string newFile = Path.Combine(newFolder, "WordToFind.txt");

    Dictionary<string, string> data = GetDataFromFile();
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append("-------------------Result-------------------\n");
    foreach (var fileName in data.Keys)
    {
        stringBuilder.Append($"---------{fileName}---------\n");
        string[] words = Regex.Split(data[fileName], @"[\W\n]+");
        string regex = @"^[nN][\w-]{2,}[^nN]$";
        foreach (var word in words)
        {
            if(Regex.IsMatch(word, regex))
            {
                stringBuilder.Append(word + "\n");
            }
        }
    }
    File.WriteAllText(newFile, stringBuilder.ToString(), Encoding.UTF8);
}

/* 5.Tìm tất cả các số xuất hiện trong các file (số là số nguyên số thực. VD: -5, 10, 0, 0.5, 103.35...). 
 Lưu danh sách các số này ra 1 file .txt (mỗi số 1 dòng). */
void FindAllNum()
{
    string newFolder = Path.Combine(ROOT_PATH, "Task5_FindAllNum");
    Directory.CreateDirectory((newFolder));
    string newFile = Path.Combine(newFolder, "NumToFind.txt");

    Dictionary<string, string> data = GetDataFromFile();
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append("-------------------Result-------------------\n");

    foreach (var fileName in data.Keys)
    {
        stringBuilder.Append($"---------{fileName}---------\n");
        var pattern = @"^(-?[1-9]+\d*([.]\d+)?)$|^(-?0[.]\d*[1-9]+)$|^0$|^0.0$";
        string[] words = Regex.Split(data[fileName], @"[^.\-\d]+");

        foreach (string word in words) 
        {
            var format = Regex.Replace(word, @"^[\-][\-]+", "-");
            if(Regex.IsMatch(format, pattern))
            {
                stringBuilder.Append(format + "\n");
            }
        }
    }
    File.WriteAllText(newFile, stringBuilder.ToString(), Encoding.UTF8);
}


//6) Liệt kê tất cả các chữ cái xuất hiện trong 1 file, có phân biệt hoa thường
void LetterCount()
{
    string newFolder = Path.Combine(ROOT_PATH, "Task6_LetterCount");
    Directory.CreateDirectory((newFolder));
    string newFile = Path.Combine(newFolder, "LetterCount.txt");

    StringBuilder sb = new StringBuilder();
    Dictionary<string, string> data = GetDataFromFile();
    foreach (var fileName in data.Keys)
    {
        Dictionary<char, int> dict = new Dictionary<char, int>();
        foreach (char c in data[fileName].ToCharArray())
        {
            if (Char.IsLetter(c))
            {
                if (dict.ContainsKey(c))
                {
                    dict[c]++;
                }
                else
                {
                    dict.Add(c, 1);
                }
            }
        }
        sb.Append($"--------------------{fileName}--------------------\n");
        sb.Append($"Letter, Count\n");
        foreach (var key in dict.Keys)
        {
            sb.Append($"{key},{dict[key]}\n");
        }
    }
    File.WriteAllText(newFile, sb.ToString(), Encoding.UTF8);
}


/* -------------------------------------------------------------------- */
FindAndCopy();      // 1
Statistic();        // 2
WriteFile();        // 3
FindAllWord();      // 4
FindAllNum();       // 5
LetterCount();      // 6