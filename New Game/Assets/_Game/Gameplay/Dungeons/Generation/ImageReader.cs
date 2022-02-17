using System;
using System.Drawing;

public static class ImageReader {
    public static Color[,] ReadImage(String path) {
        Bitmap img = new Bitmap(path);

        Color[,] colors = new Color[img.Width, img.Height];
        for (int y = 0; y < img.Height; y++) {
            for (int x = 0; x < img.Width; x++) {
                colors[x, y] = img.GetPixel(x, y);
            }
        }
        
        return colors;
    }
}