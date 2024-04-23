/*
    文件名：Main.cs
    作者：毕宿五
    创建日期：2024/04/20
    功能：简易png生成器，可以自定义生成png图片的大小、形状、颜色、文本等，主要用于生成UI界面的临时资源
    修改日志：2024/04/23 => 1.增加批量生成功能 2.增加了多种颜色 3.文本防溢出处理
*/

using System.Drawing;
using System.Text;

#pragma warning disable CA1416
public class Program
{
    public static void Main()
    {
        Console.InputEncoding = Encoding.Unicode;
        string? input = string.Empty;

        // 是否批量生成
        Console.WriteLine("是否批量生成？(默认否):(Y/N)");
        input = Console.ReadLine();

        if (input?.ToUpper() == "Y")
        {
            // 批量生成
            Console.WriteLine("请输入生成数量(默认1):");
            input = Console.ReadLine();
            int count = int.TryParse(input, out int num) ? num : 1;
            PngData data = GetPngData();
            string baseText = data.Text;
            for (int i = 0; i < count; i++)
            {
                data.Text = $"{baseText}{i}";
                SpawnPng.PngGenerator(data);
            }
        }
        else
        {
            // 单个生成
            SpawnPng.PngGenerator(GetPngData());
        }

        Console.WriteLine("按任意键退出...");
        Console.ReadKey();
    }

    /// <summary>
    /// 依照控制台输入获取png数据
    /// </summary>
    /// <returns>png数据</returns>
    public static PngData GetPngData()
    {
        string? input = string.Empty;
        PngData data = new PngData();
        Console.WriteLine("请按下述提示输入png的基础信息, 输入错误将使用默认值。");

        // 1.选择形状
        Console.WriteLine("请选择形状(默认矩形):1-矩形 2-圆形");
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
        Console.WriteLine("请选择图形颜色(默认黑):1-黑 2-白 3-红 4-绿 5-蓝 6-黄 7-粉 8-灰 9-橙");
        input = Console.ReadLine();
        data.BgColor = input switch
        {
            "1" => Brushes.Black,
            "2" => Brushes.White,
            "3" => Brushes.Red,
            "4" => Brushes.Green,
            "5" => Brushes.Blue,
            "6" => Brushes.Yellow,
            "7" => Brushes.Pink,
            "8" => Brushes.Gray,
            "9" => Brushes.Orange,
            _ => Brushes.Black
        };

        // 5.选择文本颜色
        Console.WriteLine("请选择文本颜色(默认白):1-黑 2-白 3-红 4-绿 5-蓝 6-黄 7-粉 8-灰 9-橙");
        input = Console.ReadLine();
        data.TextColor = input switch
        {
            "1" => Brushes.Black,
            "2" => Brushes.White,
            "3" => Brushes.Red,
            "4" => Brushes.Green,
            "5" => Brushes.Blue,
            "6" => Brushes.Yellow,
            "7" => Brushes.Pink,
            "8" => Brushes.Gray,
            "9" => Brushes.Orange,
            _ => Brushes.White
        };

        // 6.输入文本
        Console.WriteLine("请输入文本(默认“测试图标”):");
        input = Console.ReadLine();
        data.Text = string.IsNullOrWhiteSpace(input) ? "测试图标" : input;

        return data;
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
    public const string FONT = "宋体";                  // 字体
    public const int FONT_SIZE = 20;                    // 默认字体大小
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

        // 在居中的位置绘制文本, 如果字体大小超出图片大小则缩小字体
        Font font = new Font(FONT, FONT_SIZE);
        SizeF size = graphics.MeasureString(data.Text, font);
        int i = 1;
        while (size.Width > data.width || size.Height > data.height)
        {
            font.Dispose();
            font = new Font(FONT, FONT_SIZE - i++);
            size = graphics.MeasureString(data.Text, font);
        }
        graphics.DrawString(data.Text, font, data.TextColor, (data.width - size.Width) / 2, (data.height - size.Height) / 2);

        // 如果目录不存在则创建目录
        if (!Directory.Exists(PATH))
        {
            Directory.CreateDirectory(PATH);
        }

        // 保存图片
        bitmap.Save($"{PATH}{data.Text}.png", System.Drawing.Imaging.ImageFormat.Png);
        Console.WriteLine($"生成成功！保存路径：{PATH}{data.Text}.png");
    }
}
#pragma warning restore CA1416