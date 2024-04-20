/*
    文件名：Program.cs
    作者：毕宿五
    创建日期：2024/04/20
    功能：简易png生成器，可以自定义生成png图片的大小、形状、颜色、文本等，主要用于生成UI界面的临时资源
*/

using System;
using System.Drawing;
using System.Text;

#pragma warning disable CA1416
public class Program
{
    public static void Main()
    {
        string? input = string.Empty;
        PngData data = new PngData();
        Console.InputEncoding = Encoding.Unicode;

        Console.WriteLine("请按下述提示输入png的基础信息, 输入错误将使用默认值。");

        // 1.选择形状
        Console.WriteLine("请选择形状(默认矩形):1.矩形 2.圆形");
        input = Console.ReadLine();
        data.IsCircle = input switch
        {
            "1" => false,
            "2" => true,
            _ => false
        };

        // 2.输入宽度
        Console.WriteLine("请输入宽度(默认120):");
        input = Console.ReadLine();
        data.width = int.TryParse(input, out int width) ? width : 120;

        // 3.输入高度
        Console.WriteLine("请输入高度(默认120):");
        input = Console.ReadLine();
        data.height = int.TryParse(input, out int height) ? height : 120;

        // 4.选择图形颜色
        Console.WriteLine("请选择图形颜色(默认黑色):1.黑色 2.红色 3.绿色 4.蓝色 5.白色");
        input = Console.ReadLine();
        data.BgColor = input switch
        {
            "1" => Brushes.Black,
            "2" => Brushes.Red,
            "3" => Brushes.Green,
            "4" => Brushes.Blue,
            "5" => Brushes.White,
            _ => Brushes.Black
        };

        // 5.选择文本颜色
        Console.WriteLine("请选择文本颜色(默认白色):1.黑色 2.红色 3.绿色 4.蓝色 5.白色");
        input = Console.ReadLine();
        data.TextColor = input switch
        {
            "1" => Brushes.Black,
            "2" => Brushes.Red,
            "3" => Brushes.Green,
            "4" => Brushes.Blue,
            "5" => Brushes.White,
            _ => Brushes.White
        };

        // 6.输入文本
        Console.WriteLine("请输入文本(默认“测试图标”):");
        input = Console.ReadLine();
        data.Text = string.IsNullOrWhiteSpace(input) ? "测试图标" : input;

        // 7.生成png
        SpawnPng.PngGenerator(data);
    }
}

/// <summary>
/// png数据结构
/// </summary>
public struct PngData
{
    public int width;               // 宽度
    public int height;              // 高度
    public bool IsCircle;           // 是否是圆形
    public Brush BgColor;           // 背景颜色
    public Brush TextColor;         // 文本颜色
    public string Text;             // 文本内容
}

/// <summary>
/// png生成器
/// </summary>
public class SpawnPng
{
    public const string FONT = "微软雅黑";               // 字体
    public const int FONT_SIZE = 20;                    // 字体大小
    public const string PATH = "D:/Cache/TempIcon/";    // 生成路径

    public static void PngGenerator(PngData data)
    {
        using Bitmap bitmap = new Bitmap(data.width, data.height);
        using Graphics graphics = Graphics.FromImage(bitmap);

        // 透明背景并绘制指定颜色的图形
        graphics.Clear(Color.Transparent);
        if (data.IsCircle)
        {
            graphics.FillEllipse(data.BgColor, 0, 0, data.width, data.height);
        }
        else
        {
            graphics.FillRectangle(data.BgColor, 0, 0, data.width, data.height);
        }

        // 在居中的位置绘制文本
        using Font font = new Font(FONT, FONT_SIZE);
        SizeF size = graphics.MeasureString(data.Text, font);
        graphics.DrawString(data.Text, font, data.TextColor, (data.width - size.Width) / 2, (data.height - size.Height) / 2);

        // 如果目录不存在则创建目录
        if (!System.IO.Directory.Exists(PATH))
        {
            System.IO.Directory.CreateDirectory(PATH);
        }

        // 保存图片
        bitmap.Save($"{PATH}{data.Text}.png", System.Drawing.Imaging.ImageFormat.Png);
        Console.WriteLine($"生成成功！保存路径：{PATH}{data.Text}.png");
        Console.WriteLine("按任意键退出...");
        Console.ReadKey();
    }
}
#pragma warning restore CA1416