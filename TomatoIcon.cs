using System;
using System.Drawing;
using System.IO;

namespace WawaPomodoro
{
    public static class TomatoIcon
    {
        public static Icon CreateTomatoIcon()
        {
            // 创建一个32x32的位图
            Bitmap bitmap = new Bitmap(32, 32);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                // 清除背景
                g.Clear(Color.Transparent);
                
                // 番茄主体 - 红色圆形
                using (SolidBrush redBrush = new SolidBrush(Color.FromArgb(255, 65, 54)))
                {
                    g.FillEllipse(redBrush, 2, 4, 28, 26);
                }
                
                // 番茄顶部 - 绿色叶子
                using (SolidBrush greenBrush = new SolidBrush(Color.FromArgb(46, 204, 64)))
                {
                    // 左叶
                    Point[] leftLeaf = {
                        new Point(14, 4),
                        new Point(8, 0),
                        new Point(10, 6)
                    };
                    g.FillPolygon(greenBrush, leftLeaf);
                    
                    // 右叶
                    Point[] rightLeaf = {
                        new Point(18, 4),
                        new Point(24, 0),
                        new Point(22, 6)
                    };
                    g.FillPolygon(greenBrush, rightLeaf);
                    
                    // 中间茎
                    g.FillRectangle(greenBrush, 15, 1, 2, 5);
                }
                
                // 番茄高光 - 增加一点立体感
                using (SolidBrush highlightBrush = new SolidBrush(Color.FromArgb(100, 255, 255, 255)))
                {
                    g.FillEllipse(highlightBrush, 8, 10, 8, 8);
                }
            }
            
            // 将位图转换为图标
            IntPtr hIcon = bitmap.GetHicon();
            Icon icon = Icon.FromHandle(hIcon);
            
            return icon;
        }
    }
}